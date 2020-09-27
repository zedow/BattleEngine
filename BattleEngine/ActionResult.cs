using System;
using System.Collections.Generic;
using System.Text;

namespace BattleEngine
{
    public class ActionResult
    {
        public string DisplayText;

        public ActionResult(string message)
        {
            DisplayText = message;
        }

        public ActionResult() { }
    }
}
