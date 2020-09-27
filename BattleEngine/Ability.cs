using System;
using System.Collections.Generic;
using System.Text;

namespace BattleEngine
{
    public class Ability
    {
        public int ActionPoint;
        public string Name;
        public string Description;

        public Ability(int actionPoint, string name, string description)
        {
            ActionPoint = actionPoint;
            Name = name;
            Description = description;
        }
    }
}
