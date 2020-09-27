using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleEngine
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
            Actions.AddRange(actions);
        }

        public void DoTurn()
        {
            Actions.ForEach(i => Results.AddRange( i.DoAction()));
        }

        public IEnumerable<ActionResult> GetTurnResults()
        {
            return (Results);
        }
    }
}
