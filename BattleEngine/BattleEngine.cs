using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3
{
    public class BattleEngine
    {
        public List<TurnAction> Actions;
        public List<ActionResult> Results;

        public BattleEngine()
        {
            Actions = new List<TurnAction>();
            Results = new List<ActionResult>();
        }

        public void StartTurn()
        {
            Actions.Clear();
            Results.Clear();
        }

        public void AppendAction(params TurnAction[] actions)
        {
            // Codez ici !
        }

        public void DoTurn()
        {
            // Codez ici !
        }

        public IEnumerable<ActionResult> GetTurnResults()
        {
            return (Results);
        }
    }
}
