using System;
using System.Collections.Generic;
using System.Linq;
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
    class Program
    {
        static void Main(string[] args)
        {
            // Déclaration du Battle Engine
            BattleEngine Engine = new BattleEngine();
            UI ui = new UI();

            List<Character> battleCharacters = new List<Character>();

            BattleStates state = BattleStates.START;
            PartyCharacter berserker = new PartyCharacter
            {
                Name = "Geralt of Rivia",
                HP = 100f,
                ActionPoints = 8,
            };

            Foe monster = new Foe
            {
                Name = "Noyeur",
                HP = 35f,
                ActionPoints = 8
            };

            battleCharacters.Add(berserker);
            battleCharacters.Add(monster);

            string userKey = "";

            Ability ability1 = new Ability(3, -15f,"Fendoir","Coup d'épée infligeant une terrible blessure");
            berserker.Abilities.Add(ability1);

           
            // Action d'attaque basique
            TurnAction action = new TurnAction
            {
                Action = berserker.Abilities.ElementAt(0).DoAbility,
                Source = berserker,
                Target = monster,
                ActionName = "Lancer un sort"
            };

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
                        battleCharacters.ForEach(i => ui.Display($"{i.Name} : Points d'action  restant : {i.ActionPoints}, HP restant : {i.HP}"));

                        // Choix de l'action à effectuer pour le joueur
                        ui.Display("Quelle action voulez-vous effectuer ?");
                        ui.Display("Appuyez sur A pour attaquer et Q pour abandonner");

                        string value = Console.ReadLine();
                        switch (value)
                        {
                            case "A":

                                ui.Display("Quelle compétence voulez-vous utiliser ?");

                                // Affichage de toutes les compétences du personnage et de leur index
                                for (int i = 0; i < berserker.Abilities.Count; i++)
                                {
                                    ui.Display($"{i} : {berserker.Abilities.ElementAt(i).Name}");
                                }   
                                
                                // Initialisation de l'inex à -1
                                int ability_index = 99;

                                // Demande à l'utilisateur d'indiquer l'index de la compétence à utiliser tant qu'il n'est pas renseigné ou tant que l'index renseigné est incorrect
                                while(ability_index >= berserker.Abilities.Count)
                                {
                                    switch(ability_index)
                                    {
                                        case 99:
                                            ui.Display("Veuillez renseigner le numéro de la compétence à utiliser");
                                            break;
                                        case 98:
                                            ui.Display("Index incorrect, Veuillez réessayer");
                                            break;
                                        case 97:
                                            ui.Display("Points d'action insuffisants pour utiliser cette compétence");
                                            break;
                                    }
                                    string input = Console.ReadLine();
                                    try
                                    {
                                        ability_index = Math.Abs(Int32.Parse(input));
                                        if (berserker.Abilities.ElementAt(ability_index).ActionPoint > berserker.ActionPoints)
                                        {
                                            ability_index = 97;
                                        }
                                    }
                                    catch
                                    {
                                        ability_index = 98;
                                    }
                                }
                                // Ajoute l'action de la compétence à utiliser aux actions du tour sur l'Engine
                                TurnAction currentAction = new TurnAction
                                {
                                    Action = berserker.Abilities.ElementAt(ability_index).DoAbility,
                                    Source = berserker,
                                    Target = monster,
                                    ActionName = $"Utilise le sort {berserker.Abilities.ElementAt(ability_index).Name}"
                                };
                                Engine.AppendAction(currentAction);
                                break;

                            case "Q":
                                ui.Display("Vous avez décidé d'abandonner");
                                state = BattleStates.LOSE;
                                break;
                        }
                        Engine.DoTurn();
                        Engine.GetTurnResults().ToList().ForEach(i => ui.Display(i.DisplayText));
                        break;

                    // Si c'est le tour de l'adversaire
                    case BattleStates.ENEMYTURN:
                        Engine.StartTurn();
                        ui.Display("Tour de l'adversaire");
                        Engine.DoTurn();
                        Engine.GetTurnResults().ToList().ForEach(i => ui.Display(i.DisplayText));
                        break;

                    // Si le joueur a gagné la bataille
                    case BattleStates.WIN:
                        ui.Display("Vous avez gagné la bataille !");
                        battle = false;
                        break;

                    // Si le joueur a perdu la bataille
                    case BattleStates.LOSE:
                        ui.Display("Vous avez perdu la bataille");
                        battle = false;
                        break;
                }
            }
            Console.ReadLine();
        }
    }
}
