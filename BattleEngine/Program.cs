using BattleEngine.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace BattleEngine
{
    public enum BattleStates
    {
        START,
        PLAYERTURN,
        ENEMYTURN,
        WIN,
        LOSE
    }

    public class BattleStatesManager : MyObserver<Character>
    {
        public void Update(MyObservable<Character> observed)
        {
            
        }
    }
    class Program
    {
        static void AddActionsPoint(List<Character> characters)
        {
            UI.Display("3 points d'action rendus aux personnages, tour suivant");
            UI.Seperate();
            characters.ForEach(i => i.ActionPoints = Math.Min(i.ActionPoints + 3, i.MaxActionPoints));
        }
        static void Main(string[] args)
        {
            // Déclaration du Battle Engine et des personnages joueurs
            BattleEngine Engine = new BattleEngine();
            BattleStatesManager manager = new BattleStatesManager();

            List<Character> battleCharacters = new List<Character>();

            BattleStates state = BattleStates.START;
            PartyCharacter berserker = new PartyCharacter
            {
                Name = "Geralt of Rivia",
                HP = 100f,
                CurrentHP = 100f,
                ActionPoints = 8,
                MaxActionPoints = 8,
            };

            Foe monster = new Foe
            {
                Name = "Noyeur",
                HP = 35f,
                CurrentHP = 35f,
                ActionPoints = 8,
                MaxActionPoints = 8,
            };
       
            battleCharacters.Add(berserker);
            battleCharacters.Add(monster);
            battleCharacters.ForEach(i => i.Observe(manager));

            // Mise en place des compétences utilisables
            Ability ability1 = new Ability(2, -10f, "Griffure", "Coup de griffes acérées perforant les armures");
            Ability ability2 = new Ability(3, -15f,"Fendoir","Coup d'épée infligeant une terrible blessure");
            Ability ability3 = new Ability(5, -25f, "Tourbillon", "Le tourbillon est une technique mortelle de combat");
            berserker.Abilities.Add(ability2);
            berserker.Abilities.Add(ability3);
            monster.Abilities.Add(ability1);

            // Booléan définissant si la bataille est en cours ou terminée
            bool battle = true;

            // Fonctionnement du jeu
            while(battle)
            {
                // Comportement de l'application selon l'état de la bataille
                switch (state)
                {
                    // Si la bataille commence
                    case BattleStates.START:
                        Console.WriteLine("La bataille commence");
                        state = BattleStates.PLAYERTURN;
                        break;

                    // Si c'est le tour du joueur
                    case BattleStates.PLAYERTURN:
                        Engine.StartTurn(); 
                        battleCharacters.ForEach(i => UI.Display($"{i.Name} : Points d'action  restant : {i.ActionPoints}, HP restant : {i.CurrentHP} sur { i.HP }"));
                        UI.Seperate();
                        // Choix de l'action à effectuer pour le joueur
                        UI.Display("Quelle action voulez-vous effectuer ?");
                        UI.Display("Appuyez sur A pour attaquer, Q pour abandonner et P pour passer votre tour");

                        // Demande à l'utilisateur d'entrer une valeur tant que a valeur ne correspond pas à une option possible
                        string value = Console.ReadLine();
                        while(value != "A" && value != "Q" && value != "P")
                        {
                            UI.Display("Veuillez indiquer une valeur valide");
                            value = Console.ReadLine();
                        }
                        switch (value)
                        {
                            case "A":

                                UI.Display("Quelle compétence voulez-vous utiliser ?");

                                // Affichage de toutes les compétences du personnage et de leur index
                                for (int i = 0; i < berserker.Abilities.Count; i++)
                                {
                                    UI.Display($"{i} : {berserker.Abilities.ElementAt(i).Name} | Coûte {berserker.Abilities.ElementAt(i).ActionPoint} points d'action");
                                }   
                                
                                // Initialisation de l'index à 99 (signifiant que l'index est dans l'attente de recevoir une valeur)
                                int ability_index = 99;

                                // Demande à l'utilisateur d'indiquer l'index de la compétence à utiliser tant qu'il n'est pas renseigné ou tant que l'index renseigné est incorrect
                                while(ability_index >= berserker.Abilities.Count && ability_index != 100)
                                {
                                    switch(ability_index)
                                    {
                                        case 99:
                                            UI.Display("Veuillez renseigner le numéro de la compétence à utiliser");
                                            break;
                                        case 98:
                                            UI.Display("Index incorrect, Veuillez réessayer");
                                            break;
                                        case 97:
                                            UI.Display("Points d'action insuffisants pour utiliser cette compétence (entrez 100 pour passer le tour)");
                                            break;
                                    }
                                    string input = Console.ReadLine();
                                    try
                                    {
                                        ability_index = Math.Abs(Int32.Parse(input));
                                        if (berserker.Abilities.ElementAt(ability_index).ActionPoint > berserker.ActionPoints && ability_index != 100)
                                        {
                                            // Initialisation de l'index à 97 (signifiant que la compétence choisie n'est pas réalisable)
                                            ability_index = 97;
                                        }
                                    }
                                    catch
                                    {
                                        // Initialisation de l'index à 98 (signifiant que l'index choisi n'est pas valide)
                                        if(ability_index != 100)
                                        {
                                            ability_index = 98;
                                        }
                                    }
                                }
                                if(ability_index != 100)
                                {
                                    // Ajoute l'action de la compétence à utiliser aux actions du tour sur l'Engine
                                    TurnAction currentAction = new TurnAction
                                    {
                                        Action = berserker.Abilities.ElementAt(ability_index).DoAbility,
                                        Source = berserker,
                                        Target = monster,
                                        ActionName = $"Utilise le sort {berserker.Abilities.ElementAt(ability_index).Name}"
                                    };
                                    Engine.AppendAction(currentAction);
                                }
                                else
                                {
                                    UI.Display("Vous passez votre tour");
                                }
                                state = BattleStates.ENEMYTURN;
                                break;

                            case "Q":
                                UI.Display("Vous avez décidé d'abandonner");
                                state = BattleStates.LOSE;
                                break;

                            case "P":
                                UI.Display("Vous passez votre tour");
                                state = BattleStates.ENEMYTURN;
                                break;
                        }
                        Engine.DoTurn();
                        Engine.GetTurnResults().ToList().ForEach(i => UI.Display(i.DisplayText));

                        UI.Seperate();
                        break;

                    // Si c'est le tour de l'adversaire
                    case BattleStates.ENEMYTURN:
                        Engine.StartTurn();
                        UI.Display("Tour de l'adversaire");
                        TurnAction action = monster.DoTurn(berserker);
                        if(action != null)
                        {
                            Engine.AppendAction(action);
                        }
                        Engine.DoTurn();
                        Engine.GetTurnResults().ToList().ForEach(i => UI.Display(i.DisplayText));

                        // Redonne 3 points d'action aux personnages participants à la bataille
                        AddActionsPoint(battleCharacters);

                        state = BattleStates.PLAYERTURN;
                        break;

                    // Si le joueur a gagné la bataille
                    case BattleStates.WIN:
                        UI.Display("Vous avez gagné la bataille !");
                        battle = false;
                        break;

                    // Si le joueur a perdu la bataille
                    case BattleStates.LOSE:
                        UI.Display("Vous avez perdu la bataille");
                        battle = false;
                        break;
                }
            }
            Console.ReadLine();
        }
    }
}
