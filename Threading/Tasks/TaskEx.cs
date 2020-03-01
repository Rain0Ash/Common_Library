// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable AsyncConverter.AsyncMethodNamingHighlighting

namespace Common_Library.Threading.Tasks
{
    public static class TaskEx
    {
        /// <summary>
        /// Blocks while condition is true (until = false) or false (until = true) or timeout occurs.
        /// </summary>
        /// <param name="condition">The condition that will perpetuate the block.</param>
        /// <param name="frequency">The frequency at which the condition will be check, in milliseconds.</param>
        /// <param name="timeout">Timeout in milliseconds.</param>
        /// <param name="until">While false</param>
        /// <param name="token">Cancellation token</param>
        /// <exception cref="TimeoutException"></exception>
        /// <returns></returns>
        public static async Task Wait(Func<Boolean> condition, Int32 frequency = 25, Int32 timeout = -1, Boolean until = false, CancellationToken token = default)
        {
            Task waitTask = Task.Run(async () =>
            {
                while (until != condition())
                { 
                    await Task.Delay(frequency, token).ConfigureAwait(true);
                }
            }, token);

            if (await Task.WhenAny(waitTask, Task.Delay(timeout, token)).ConfigureAwait(true) != waitTask)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                
                throw new TimeoutException();
            }
        }

        public static Task WaitHandleAsync(WaitHandle handle, Int32 timeout = Timeout.Infinite)
        {
            TaskCompletionSource<Object> task = new TaskCompletionSource<Object>();

            void Callback(Object state, Boolean timedOut) {
                if (timedOut)
                {
                    task.TrySetCanceled();
                }
                else
                {
                    task.TrySetResult(default);
                }
            }

            ThreadPool.RegisterWaitForSingleObject(handle, Callback, default, timeout, true);

            return task.Task;
        }
    }
}