using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3.ObserverPattern
{
    public interface MyObserver<T> where T : MyObservable<T>
    {
        void Update(MyObservable<T> observed);
    }
}
