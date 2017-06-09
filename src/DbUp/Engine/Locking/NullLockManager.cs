using System;

namespace DbUp.Engine.Locking
{
    public class NullLockManager : ILockManager
    {
        public void Lock()
        {
            return;
        }

        public void UnLock()
        {
            return;
        }
    }
}