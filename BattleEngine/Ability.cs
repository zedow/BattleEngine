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

        // Nombre de tours avant réutilisation de la compétence
        public int Cooldown;
        public int currentCooldown;

        public Ability(int actionPoint, float value, string name, string description,int cd)
        {
            Value = value;
            ActionPoint = actionPoint;
            Name = name;
            Description = description;
            Cooldown = cd;
            currentCooldown = 0;
        }

        // Fonction basée sur le template Action de la classe TurnAction
        public IEnumerable<ActionResult> DoAbility(Character source, Character target, TurnAction action)
        {
            currentCooldown = Cooldown;
            List<ActionResult> results = new List<ActionResult>();
            // Si le personnage a suffisament de points d'action pour utiliser la compétence
              
            results.Add(new ActionResult($"{source.Name} a utilisé la compétence {this.Name}"));

            results.Add(new ActionResult($"{source.Name} a perdu {this.ActionPoint} points d'action, {source.ActionPoints - this.ActionPoint} points d'action restant"));
            source.ActionPoints -= this.ActionPoint;

            // Si la valeur est négative, la compétence inflige des dêgats à la cible, si la valeur est positive, la compétence soigne la cible
            // Opération à effectuer sur la cible
            results.Add(target.ModifyHp(this.Value));
            
            // Retours des messages contenus dans la liste d'objets ActionResult qui seront affichés par le BattleEngine
            return (results);
        }

        public void ReduceCd()
        {
            if(currentCooldown > 0)
            {
                currentCooldown--;
            }
        }
    }
}
