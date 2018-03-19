namespace LimitInstance
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    class LimitedInstances<T> where T : class, new()
    {
        private readonly Stack<T> stack;
        private readonly Semaphore semaphore;
        private readonly object thisLock = new object();

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
            T t = null;
            semaphore.WaitOne();
            lock (thisLock)
            {
                t = stack.Peek();
                if (t == null)
                {
                    Console.WriteLine($"NULL - stack size was {stack.Count}");
                }
                else
                {
                    stack.Pop();
                }
            }
            return t;
        }

        public void ReturnInstance(T t)
        {
            lock (thisLock)
            {
                stack.Push(t);
                semaphore.Release();
            }
        }
    }
}