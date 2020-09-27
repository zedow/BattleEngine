using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleEngine
{
    public class Foe : Character
    {
        public TurnAction DoTurn(PartyCharacter target)
        {
            if(this.ActionPoints > this.Abilities.ElementAt(0).ActionPoint)
            {
                TurnAction action = new TurnAction
                {
                    Action = this.Abilities.ElementAt(0).DoAbility,
                    ActionName = "L'adversaire utilise une compétence",
                    Source = this,
                    Target = target
                };
                return (action);
            }
            else
            {
                UI.Display($"{this.Name} passe son tour !");
                return (null);
            }
        }
    }
}
