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
                        battleCharacters.ForEach(i => Console.WriteLine($"{i.Name} : Points d'action  restant : {i.ActionPoints}, HP restant : {i.HP}"));
                        Console.WriteLine("Quelle action voulez-vous effectuer ?");
                        Console.WriteLine("Appuyez sur A pour attaquer et Q pour abandonner");
                        string value = Console.ReadLine();
                        switch (value)
                        {
                            case "A":
                                Engine.AppendAction(action);
                                break;

                            case "Q":
                                Console.WriteLine("Vous avez décidé d'abandonner");
                                state = BattleStates.LOSE;
                                break;
                        }
                        Engine.DoTurn();
                        Engine.GetTurnResults().ToList().ForEach(i => Console.WriteLine(i.DisplayText));
                        break;

                    // Si c'est le tour de l'adversaire
                    case BattleStates.ENEMYTURN:
                        Engine.StartTurn();
                        Console.WriteLine("Tour de l'adversaire");
                        Engine.DoTurn();
                        Engine.GetTurnResults().ToList().ForEach(i => Console.WriteLine(i.DisplayText));
                        break;

                    // Si le joueur a gagné la bataille
                    case BattleStates.WIN:
                        Console.WriteLine("Vous avez gagné la bataille !");
                        battle = false;
                        break;

                    // Si le joueur a perdu la bataille
                    case BattleStates.LOSE:
                        Console.WriteLine("Vous avez perdu la bataille");
                        battle = false;
                        break;
                }
            }
            Console.ReadLine();
        }
    }
}
