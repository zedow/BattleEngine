using System;
using System.Collections.Generic;
using System.Text;
using BattleEngine.ObserverPattern;

namespace BattleEngine
{
    public class MyObservable<T> where T : MyObservable<T>
    {
        private List<MyObserver<T>> Observers;

        public MyObservable()
        {
            Observers = new List<MyObserver<T>>();
        }

        public void Observe(MyObserver<T> observer)
        {
            Observers.Add(observer);
        }

        protected void NotifyObservers()
        {
            Observers.ForEach(i => i.Update(this));
        }
    }
}
