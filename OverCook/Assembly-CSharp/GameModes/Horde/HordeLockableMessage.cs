using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

namespace GameModes.Horde
{
	// Token: 0x020007D0 RID: 2000
	public class HordeLockableMessage : Serialisable
	{
		// Token: 0x06002673 RID: 9843 RVA: 0x000B7390 File Offset: 0x000B5790
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write(this.m_locked);
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000B739E File Offset: 0x000B579E
		public bool Deserialise(BitStreamReader reader)
		{
			this.m_locked = reader.ReadBit();
			return true;
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x000B73AD File Offset: 0x000B57AD
		public static void Lock(ref HordeLockableMessage message)
		{
			message.m_locked = true;
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000B73B7 File Offset: 0x000B57B7
		public static void Unlock(ref HordeLockableMessage message)
		{
			message.m_locked = false;
		}

		// Token: 0x04001E80 RID: 7808
		public bool m_locked = true;
	}
}
