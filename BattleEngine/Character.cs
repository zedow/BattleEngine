using System;
using System.Collections.Generic;
using System.Text;

namespace BattleEngine
{
    public class Character : MyObservable<Character>
    {
        public string Name;
        public float HP;
        public int ActionPoints;
        public int MaxActionPoints;

        public List<Ability> Abilities;

        public Character()
        {
            Abilities = new List<Ability>();
            MaxActionPoints = ActionPoints;
        }

        public void Notify()
        {
            NotifyObservers();
        }
    }
}