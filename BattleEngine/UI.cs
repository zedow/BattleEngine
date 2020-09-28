using BattleEngine.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleEngine
{
    public static class UI
    {
        public static void Display(string message)
        {
            Console.WriteLine(message);
        }

        public static void Seperate()
        {
            Console.WriteLine("");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("");
        }
    }
}
