// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Net;

namespace Common_Library.Types.Network
{
    public class WebClientExtended : WebClient
    {
        public Boolean HeadOnly { get; set; }
        
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            
            if (HeadOnly && req.Method == "GET")
            {
                req.Method = "HEAD";
            }
            
            return req;
        }
    }
}