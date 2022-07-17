// https://docs.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim?view=net-6.0

namespace SLSAK.Extensions;

public static class ReaderWriterLockSlimExtensions
{

    /// <summary>
    /// Wraps the provided action in a read lock.
    /// </summary>
    public static void SafeRead(this ReaderWriterLockSlim rwls, Action readOperation)
    {
        rwls.EnterReadLock();
        try
        {
            readOperation();
        }
        finally
        {
            rwls.ExitReadLock();
        }
    }

    /// <summary>
    /// Wraps the provided function in a read lock.
    /// </summary>
    public static TResult SafeRead<TResult>(this ReaderWriterLockSlim rwls, Func<TResult> readOperation)
    {
        rwls.EnterReadLock();
        try
        {
            return readOperation();
        }
        finally
        {
            rwls.ExitReadLock();
        }
    }

    /// <summary>
    /// Wraps the provided action in a write lock.
    /// </summary>
    public static void SafeWrite(this ReaderWriterLockSlim rwls, Action writeOperation)
    {
        rwls.EnterWriteLock();
        try
        {
            writeOperation();
        }
        finally
        {
            rwls.ExitWriteLock();
        }
    }

    /// <summary>
    /// Wraps the provided action in a write lock.
    /// </summary>
    public static void SafeWrite<T>(this ReaderWriterLockSlim rwls, Action<T> writeOperation, T input)
    {
        rwls.EnterWriteLock();
        try
        {
            writeOperation(input);
        }
        finally
        {
            rwls.ExitWriteLock();
        }
    }

}