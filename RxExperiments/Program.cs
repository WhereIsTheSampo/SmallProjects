using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reactive;
using System.Reactive.Linq;

namespace RxExperiments
{
    class Program
    {
        static void Main(String[] args)
        {
            //var observable = Observable.Return("return value");
            //var observable = Observable.Empty<String>();
            //var observable = Observable.Never<String>();
            //var observable = Observable.Throw<String>(new Exception("test exception"));


            observable.Subscribe(s => HandleNext(s), ex => HandleError(ex), HandleCompleted);

            Console.ReadLine();
        }

        private static void HandleNext(String item)
        {
            Console.WriteLine("Next: " + item.ToString());
        }

        private static void HandleError(Exception exception)
        {
            Console.WriteLine("Error: " + exception.ToString());
        }

        private static void HandleCompleted()
        {
            Console.WriteLine("Completed");
        }

    }
}
