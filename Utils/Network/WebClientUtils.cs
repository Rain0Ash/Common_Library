// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Common_Library.Utils
{
    public static class WebClientUtils
    {
        public static async Task<String> DownloadStringTaskAsync(this WebClient webClient, String address,
            CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (cancellationToken.Register(webClient.CancelAsync))
                {
                    return await webClient.DownloadStringTaskAsync(address).ConfigureAwait(true);
                }
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public static async Task<Byte[]> DownloadDataTaskAsync(this WebClient webClient, String address,
            CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (cancellationToken.Register(webClient.CancelAsync))
                {
                    return await webClient.DownloadDataTaskAsync(address).ConfigureAwait(true);
                }
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
    }
}