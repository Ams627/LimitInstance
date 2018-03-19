using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LimitInstance
{
    class LimitedInstances<T> where T : new()
    {
        private readonly Stack<T> stack;
        private readonly Semaphore semaphore; 

        public LimitedInstances(int limit)
        {
            stack = new Stack<T>();
            semaphore = new Semaphore(limit, limit);
            for (var i = 0; i < limit; i++)
            {
                stack.Push(new T());
            }
        }
        public T GetInstance()
        {
            semaphore.WaitOne();
            var t = stack.Pop();
            return t;
        }

        public void ReturnInstance(T t)
        {
            stack.Push(t);
            semaphore.Release();
        }
    }

    class X
    {
        private static int i;
        static X()
        {
            i = 0;
        }

        public X()
        {
            i++;
        }

        public void Print()
        {
            Console.WriteLine($"i is {i}");
        }
    }
    internal class Program
    {
        private static IEnumerable<int> GetInts()
        {
            for (var i = 0; i < 1000_000; i++)
            {
                yield return i;
            }
        }
        private static void Main(string[] args)
        {
            try
            {
                var random = new Random();
                var limited = new LimitedInstances<X>(8);
                Parallel.ForEach(GetInts(), i =>
                {
                    var x = limited.GetInstance();
                    var rnd = random.Next() % 10;
                    Thread.Sleep(rnd);
                });
            }
            catch (Exception ex)
            {
                var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var progname = Path.GetFileNameWithoutExtension(codeBase);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
