using System;
using System.Collections.Generic;
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
           foreach(TurnAction action in actions)
           {
                Actions.Add(action);
           }
        }

        public void DoTurn()
        {
            foreach(TurnAction action in Actions)
            {
                foreach(ActionResult result in action.DoAction())
                {
                    Results.Add(result);
                }
            }
        }

        public IEnumerable<ActionResult> GetTurnResults()
        {
            return (Results);
        }
    }
}
