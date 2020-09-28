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
            // Utilise la compétence ayant la plus grosse valeur d'attaque avec un coût en point d'action inférieur aux points d'action restant et ayant un cooldown restant de 0
            Ability ability = this.Abilities.Where(i => (i.Cooldown == 0) && (i.ActionPoint <= this.ActionPoints)).OrderByDescending(i => Math.Abs(i.Value)).FirstOrDefault();

            if(ability != null)
            {
                TurnAction action = new TurnAction
                {
                    Action = ability.DoAbility,
                    ActionName = $"L'adversaire utilise la compétence {ability.Name}",
                    Source = this,
                    Target = target
                };
                return (action);
            }
            // Si aucune compétence n'est utilisable
            else
            {
                UI.Display($"{this.Name} passe son tour !");
                return (null);
            }
        }
    }
}
