using System;
using System.Collections.Generic;
using System.Text;

namespace BattleEngine
{
    public class Ability
    {
        // Coût de la compétence en points d'action
        public int ActionPoint;

        public string Name;
        public string Description;

        // Valeur négative si compétence de type attaque, valeur positive si compétence de type soin
        public float Value;

        public Ability(int actionPoint, float value, string name, string description)
        {
            Value = value;
            ActionPoint = actionPoint;
            Name = name;
            Description = description;
        }

        // Fonction basée sur le template Action de la classe TurnAction
        public IEnumerable<ActionResult> DoAbility(Character source, Character target, TurnAction action)
        {
            List<ActionResult> results = new List<ActionResult>();
            results.Add(new ActionResult($"{source.Name} a utilisé la compétence {this.Name}"));

            // Si la valeur est négative, la compétence inflige des dêgats à la cible, si la valeur est positive, la compétence soigne la cible
            results.Add(new ActionResult(this.Value > 0f ? $"Se soignant de {this.Value} Hp" : $"Infligeant {this.Value * -1} sur {target.Name}"));
            results.Add(new ActionResult($"{source.Name} a perdu {this.ActionPoint} points d'action, {source.ActionPoints - this.ActionPoint} points d'action restant"));

            // Opération à effectuer sur la cible
            target.HP += this.Value;
            source.ActionPoints -= this.ActionPoint;

            // Retours des messages contenus dans la liste d'objets ActionResult qui seront affichés par le BattleEngine
            return (results);
        }
    }
}
