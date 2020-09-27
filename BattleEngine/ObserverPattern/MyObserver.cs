using System;
using System.Collections.Generic;
using System.Text;

namespace BattleEngine.ObserverPattern
{
    public interface MyObserver<T> where T : MyObservable<T>
    {
        void Update(MyObservable<T> observed);
    }
}
