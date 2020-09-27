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
            BattleStates state = BattleStates.START;
            PartyCharacter berserker = new PartyCharacter();
            Foe monster = new Foe();

            string userKey = "";

            Ability ability1 = new Ability(3, "Fendoir","Coup d'épée infligeant une terrible blessure");

            BattleEngine Engine = new BattleEngine();


            Engine.StartTurn();
            Console.WriteLine("Pour Attaquer, appuyez sur A");
            userKey = Console.ReadLine();
            if (userKey == "A")
            {
            }

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
