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

        public static int CheckIndexInput<T>(List<T> list) 
        {
            int index = 99;
            bool goodIndex = false;
            while (!goodIndex && index != 100)
            {
                string input = Console.ReadLine();
                try
                {
                    index = Int32.Parse(input);
                    if(index >= list.Count || index < 0)
                    {
                        UI.Display("Index incorrect, Veuillez réessayer (entrez 100 pour passer le tour)");
                    }
                    else
                    {
                        goodIndex = true;
                    }
                }
                catch
                {
                    index = 99;
                }
            }
            return index;
        }
    }
}
