// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Common_Library.Utils
{
    public static class NetworkUtils
    {
        public const String LocalhostIP = "127.0.0.1";

        public static Boolean ValidateIPv4(String ip)
        {
            return !String.IsNullOrEmpty(ip) && ip.Count(c => c == '.') == 3 && IPAddress.TryParse(ip, out _);
        }

        public static Byte[] ConvertIP(String ip)
        {
            if (!ValidateIPv4(ip))
            {
                return null;
            }

            IPAddress result = IPAddress.Parse(ip);
            return result.GetAddressBytes().Reverse().ToArray();
        }

        public static Boolean CheckPort(Int32 port)
        {
            return ValidatePort(port) > 0;
        }

        public static Boolean CheckPort(String port)
        {
            return Int32.TryParse(port, out Int32 prt) && CheckPort(prt);
        }

        public static Int32 ValidatePort(Int32 port)
        {
            return port > 0 && port < 65536 ? port : 0;
        }

        public static Int32 ValidatePort(String port)
        {
            Int32.TryParse(port, out Int32 prt);
            return ValidatePort(prt);
        }

        public static async Task<Boolean> CheckPingAsync(String address)
        {
            try
            {
                PingReply reply = await new Ping().SendPingAsync(address, 2000).ConfigureAwait(true);
                if (reply == null)
                {
                    return false;
                }

                return reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
        }
    }
}