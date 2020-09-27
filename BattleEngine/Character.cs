using System;
using System.Collections.Generic;
using System.Text;

namespace BattleEngine
{
    public class Character
    {
        public string Name;
        public int HP;

        public Character Target;

        protected List<Ability> Abilities;

        public Character()
        {
            Abilities = new List<Ability>();
            Target = null;
        }

        public void DoAbility(Ability ability)
        {
            
        }
    }
}
