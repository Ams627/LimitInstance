using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitInstance
{
    class X
    {
        private static int i;
        private int instance;
        static X()
        {
            i = 0;
        }

        public X()
        {
            instance = i++;
        }

        public void Print()
        {
            //Console.WriteLine($"instance is {instance}");
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
                    x.Print();
                    var rnd = random.Next() % 10;
//                    Thread.Sleep(rnd);
                    limited.ReturnInstance(x);
                });
                Console.WriteLine("finished");
            }
            catch (Exception ex)
            {
                var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var progname = Path.GetFileNameWithoutExtension(codeBase);
                Console.Error.WriteLine(progname + ": Error: " + ex.ToString());
            }

        }
    }
}
