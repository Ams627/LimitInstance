namespace LimitInstance
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    class LimitedInstances<T> where T:class
    {
        private readonly Stack<T> stack;
        private readonly Semaphore semaphore;
        private readonly object thisLock = new object();
        private readonly int limit;

        public LimitedInstances(int limit)
        {
            this.limit = limit;
            stack = new Stack<T>();
            semaphore = new Semaphore(0, limit);
        }
        public T GetInstance()
        {
            T t = null;
            semaphore.WaitOne();
            lock (thisLock)
            {
                t = stack.Pop();
            }
            return t;
        }

        public void AddInstance(T t)
        {
            lock (thisLock)
            {
                if (stack.Count < limit)
                {
                    stack.Push(t);
                    semaphore.Release();
                }
            }
        }
    }
}