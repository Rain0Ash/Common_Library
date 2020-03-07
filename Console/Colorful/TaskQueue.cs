// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common_Library.Colorful
{
#if !NET40
    public class TaskQueue
    {
        private readonly SemaphoreSlim _semaphore;

        public TaskQueue()
        {
            _semaphore = new SemaphoreSlim(1);
        }

        public async Task<T> EnqueueAsync<T>(Func<Task<T>> taskGenerator)
        {
            await _semaphore.WaitAsync().ConfigureAwait(true);
            try
            {
                return await taskGenerator().ConfigureAwait(true);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task EnqueueAsync(Func<Task> taskGenerator)
        {
            await _semaphore.WaitAsync().ConfigureAwait(true);
            try
            {
                await taskGenerator().ConfigureAwait(true);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
#endif
}