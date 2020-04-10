using System;

namespace Common_Library.Network.IPC.Messaging
{
	public class TinyMessageReceivedEventArgs : EventArgs
	{
		public Byte[] Message { get; }

		public TinyMessageReceivedEventArgs(Byte[] message)
		{
			Message = message;
		}
	}
}
