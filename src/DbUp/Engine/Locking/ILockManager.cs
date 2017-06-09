namespace DbUp.Engine.Locking
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILockManager
    {
        /// <summary>
        /// Lock so Database to ensure that only one update is running hat the same time
        /// </summary>
        void Lock();

        /// <summary>
        /// Unlock the database, so that new updates can peformed
        /// </summary>
        void UnLock();

    }
}