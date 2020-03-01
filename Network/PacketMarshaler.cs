// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com﻿﻿﻿﻿

using System;

namespace Common_Library.Network
{
    public abstract class PacketMarshaler
    {
        public virtual void Read(PacketStream stream) 
        {
            throw new NotImplementedException($"{GetType().FullName} doesn't inherit Read()");
        }

        public virtual PacketStream Write(PacketStream stream)
        {
            throw new NotImplementedException($"{GetType().FullName} doesn't inherit Read()");
        }
    }
}
