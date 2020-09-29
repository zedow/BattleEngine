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

    public class BattleManager : MyObserver<Character>
    {
        public BattleStates state;
        public PartyCharacter Character;
        public List<Foe> Opponents;

        public BattleEngine Engine = new BattleEngine();

        public BattleManager()
        {
            Opponents = new List<Foe>();
        }
        public List<Character> BattleCharacters()
        {
            List<Character> list = new List<Character>();
            list.AddRange(Opponents);
            list.Add(Character);
            return (list);
        }
        public void Update(MyObservable<Character> observed)
        {
            Character character = (Character)observed;
            if(character.CurrentHP <= 0)
            {
                if(character == Character)
                {
                    state = BattleStates.LOSE;
                }
                else
                {
                    if(Opponents.Where(i => i.CurrentHP > 0).ToList().Count == 0)
                    {
                        state = BattleStates.WIN;
                    }
                }
            }
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
            // Déclaration du Battle Manager qui surveille l'état des personnages participants à la bataille
            BattleManager manager = new BattleManager();
            manager.state = BattleStates.START;


            // Mise en place des compétences utilisables
            List<Ability> gameAbilities = new List<Ability>
            {
                new Ability(2, -10f, "Griffure", "Coup de griffes acérées perforant les armures", 0),
                new Ability(3, -50f, "Fendoir", "Coup d'épée infligeant une terrible blessure", 0),
                new Ability(8, -30f, "Hachoir", "Coup d'épée infligeant une terrible blessure", 3),
                new Ability(5, -25f, "Tourbillon", "Le tourbillon est une technique mortelle de combat", 2),
                new Ability(4,-19f,"Coup circulaire","Terrible coup de faux circulaire",1),
            };

            // Mise en place du personnage jouable
            manager.Character = new PartyCharacter
            {
                Name = "Geralt of Rivia",
                HP = 100f,
                CurrentHP = 100f,
                ActionPoints = 8,
                MaxActionPoints = 8,
                Abilities = new List<Ability> { gameAbilities.ElementAt(1), gameAbilities.ElementAt(3) },
            };

            // Mise en place des monstres que le joueur peut combattre
            List<Foe> monsters = new List<Foe>
            {
                new Foe
                {
                    Name = "Noyeur",
                    HP = 35f,
                    CurrentHP = 35f,
                    ActionPoints = 8,
                    MaxActionPoints = 8,
                    Abilities = new List<Ability> { gameAbilities.ElementAt(0), gameAbilities.ElementAt(2) },
                },
                new Foe
                {
                    Name = "Spectre",
                    HP = 50f,
                    CurrentHP = 50f,
                    ActionPoints = 8,
                    MaxActionPoints = 8,
                    Abilities = new List<Ability> { gameAbilities.ElementAt(4) },
                }
            };

            // Ajout des monstres à la bataille
            manager.Opponents.AddRange(new List<Foe> { monsters.ElementAt(0), monsters.ElementAt(1) });

            // Initialisation de l'observation des personnages par le StateManager
            manager.BattleCharacters().ForEach(i => i.Observe(manager));

            // Booléan définissant si la bataille est en cours ou terminée
            bool battle = true;

            // Fonctionnement du jeu
            while(battle)
            {
                // Comportement de l'application selon l'état de la bataille
                switch (manager.state)
                {
                    // Si la bataille commence
                    case BattleStates.START:
                        Console.WriteLine("La bataille commence");
                        manager.state = BattleStates.PLAYERTURN;
                        break;

                    // Si c'est le tour du joueur
                    case BattleStates.PLAYERTURN:
                        manager.Engine.StartTurn();
                        manager.BattleCharacters().ForEach(i => UI.Display($"{i.Name} : Points d'action  restant : {i.ActionPoints}, HP restant : {i.CurrentHP} sur { i.HP }"));
                        UI.Seperate();
                        // Choix de l'action à effectuer pour le joueur
                        UI.Display("Quelle action voulez-vous effectuer ?");
                        UI.Display("Appuyez sur A pour attaquer, Q pour abandonner et P pour passer votre tour");

                        // Demande à l'utilisateur d'entrer une valeur tant que a valeur ne correspond pas à une option possible
                        string value = Console.ReadLine();
                        while (value != "A" && value != "Q" && value != "P")
                        {
                            UI.Display("Veuillez indiquer une valeur valide");
                            value = Console.ReadLine();
                        }
                        switch (value)
                        {
                            case "A":

                                UI.Display("Quelle compétence voulez-vous utiliser ?");

                                // Affichage de toutes les compétences du personnage et de leur index
                                for (int i = 0; i < manager.Character.Abilities.Count; i++)
                                {
                                    UI.Display($"{i} : {manager.Character.Abilities.ElementAt(i).Name} | Coûte {manager.Character.Abilities.ElementAt(i).ActionPoint} points d'action | Délai avant réutilisation {manager.Character.Abilities.ElementAt(i).currentCooldown} tours");
                                }

                                // Initialisation de l'index à 99 (signifiant que l'index est dans l'attente de recevoir une valeur)
                                int ability_index = 99;

                                // Demande à l'utilisateur d'indiquer l'index de la compétence à utiliser tant qu'il n'est pas renseigné ou tant que l'index renseigné est incorrect
                                while (ability_index >= manager.Character.Abilities.Count && ability_index != 100)
                                {
                                    switch (ability_index)
                                    {
                                        case 99:
                                            UI.Display("Veuillez renseigner le numéro de la compétence à utiliser (entrez 100 pour passer le tour)");
                                            break;
                                        case 98:
                                            UI.Display("Index incorrect, Veuillez réessayer (entrez 100 pour passer le tour)");
                                            break;
                                        case 97:
                                            UI.Display("Points d'action insuffisants pour utiliser cette compétence (entrez 100 pour passer le tour)");
                                            break;
                                        case 96:
                                            UI.Display("Compétence inutilisable (entrez 100 pour passer le tour)");
                                            break;
                                    }
                                    string input = Console.ReadLine();
                                    try
                                    {
                                        ability_index = Math.Abs(Int32.Parse(input));
                                        if (manager.Character.Abilities.ElementAt(ability_index).ActionPoint > manager.Character.ActionPoints && ability_index != 100)
                                        {
                                            // Initialisation de l'index à 97 (signifiant que la compétence choisie n'est pas réalisable)
                                            ability_index = 97;
                                        }
                                        else if (manager.Character.Abilities.ElementAt(ability_index).currentCooldown != 0)
                                        {
                                            // Initialisation de l'index à 96 (signifiant que la compétence choisie est en cooldown)
                                            ability_index = 96;
                                        }
                                    }
                                    catch
                                    {
                                        // Initialisation de l'index à 98 (signifiant que l'index choisi n'est pas valide)
                                        if (ability_index != 100)
                                        {
                                            ability_index = 98;
                                        }
                                    }
                                }
                                if (ability_index != 100)
                                {
                                    UI.Display("Veuillez renseigner l'index de votre cible parmi les adversaires suivant :");
                                    for (int i = 0; i < manager.Opponents.Count; i++)
                                    {
                                        UI.Display($"{i} : {manager.Opponents.ElementAt(i).Name} / {manager.Opponents.ElementAt(i).CurrentHP} Hp restants");
                                    }

                                    // Fonction permettant de choisir une cible, tant que la cible choisie est hors combat, la demande recommence
                                    int target = -1;
                                    while (target == -1)
                                    {
                                        target = UI.CheckIndexInput<Foe>(manager.Opponents);
                                        if (manager.Opponents.ElementAt(target).CurrentHP == 0)
                                        {
                                            target = -1;
                                            UI.Display("Veuillez choisir une autre cible");
                                        }
                                    }

                                    // Si l'index de la cible choisie est 100, l'utilisateur souhaite passer son tour
                                    if (target == 100)
                                    {
                                        UI.Display("Vous passez votre tour");
                                    }
                                    else
                                    {
                                        // Ajoute l'action de la compétence à utiliser aux actions du tour sur l'Engine
                                        TurnAction currentAction = new TurnAction
                                        {
                                            Action = manager.Character.Abilities.ElementAt(ability_index).DoAbility,
                                            Source = manager.Character,
                                            Target = manager.Opponents.ElementAt(target),
                                            ActionName = $"Utilise le sort {manager.Character.Abilities.ElementAt(ability_index).Name}"
                                        };
                                        manager.Engine.AppendAction(currentAction);
                                    }


                                }
                                else
                                {
                                    UI.Display("Vous passez votre tour");
                                }
                                manager.state = BattleStates.ENEMYTURN;
                                break;

                            case "Q":
                                UI.Display("Vous avez décidé d'abandonner");
                                manager.state = BattleStates.LOSE;
                                break;

                            case "P":
                                UI.Display("Vous passez votre tour");
                                manager.state = BattleStates.ENEMYTURN;
                                break;
                        }
                        manager.Engine.DoTurn();
                        manager.Engine.GetTurnResults().ToList().ForEach(i => UI.Display(i.DisplayText));
                        manager.Character.Abilities.ForEach(i => i.ReduceCd());
                        UI.Seperate();
                        break;

                    // Si c'est le tour de l'adversaire
                    case BattleStates.ENEMYTURN:
                        manager.Engine.StartTurn();
                        UI.Display("Tour de l'adversaire");
                        List<TurnAction> actions = new List<TurnAction>();
                        manager.Opponents.ForEach(i => actions.Add(i.DoTurn(manager.Character)));
                            
                        foreach(TurnAction action in actions)
                        {
                            if(action != null)
                            {
                                manager.Engine.AppendAction(action);
                            }
                        }

                        manager.Engine.DoTurn();
                        manager.Engine.GetTurnResults().ToList().ForEach(i => UI.Display(i.DisplayText));

                        manager.Opponents.ForEach(i => i.Abilities.ForEach(a => a.ReduceCd()));

                        // Redonne 3 points d'action aux personnages participants à la bataille
                        AddActionsPoint(manager.BattleCharacters());

                        if (manager.state != BattleStates.LOSE && manager.state != BattleStates.WIN)
                        {
                            manager.state = BattleStates.PLAYERTURN;
                        }
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
