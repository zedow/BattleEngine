using System;
using System.Collections.Generic;
using System.Text;

namespace BattleEngine
{
    public class Character : MyObservable<Character>
    {
        public string Name;
        public float HP;
        public float CurrentHP;
        public int ActionPoints;
        public int MaxActionPoints;

        public List<Ability> Abilities;

        public Character()
        {
            Abilities = new List<Ability>();
            MaxActionPoints = ActionPoints;
        }

        public ActionResult ModifyHp(float value)
        {
            this.CurrentHP = value > 0f ? Math.Min(this.CurrentHP + value, this.HP) : Math.Max(this.CurrentHP + value, 0);
            NotifyObservers();
            if(this.CurrentHP == 0)
            {
                return (new ActionResult($"{Name} a péri au combat"));
            }
            else
            {
                return (new ActionResult(value > 0f ? $"{Name} récupère {value} HP" : $"{Name} a subit {Math.Abs(value)} points de dégâts"));
            }
        }
    }
}