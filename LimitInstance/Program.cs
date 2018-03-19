using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitInstance
{
    class LimitedInstances<T>
    {
        private Stack<T> stack;

        public LimitedInstances(int limit)
        {
            stack = new Stack<T>();
        }

        public T GetInstance()
        {
            var t = stack.Pop();
            return t;
        }
    }
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
    //startstarttypingtypingherehere
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
