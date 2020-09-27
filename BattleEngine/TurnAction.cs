using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3
{
    public class TurnAction
    {
        public string ActionName;
        public Func<Character, Character, TurnAction, IEnumerable<ActionResult>> Action;
        public Character Source;
        public Character Target;

        public IEnumerable<ActionResult> DoAction()
        {
            return (Action(Source, Target, this));
        }
    }
}
