using System;
using System.Collections.Generic;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008E8 RID: 2280
	public class UsersChangedMessage : Serialisable
	{
		// Token: 0x06002C44 RID: 11332 RVA: 0x000CE1C0 File Offset: 0x000CC5C0
		public void Initialise(FastList<User> users)
		{
			this.m_Users.Clear();
			for (int i = 0; i < users.Count; i++)
			{
				User user = users._items[i];
				this.AddUser(user);
			}
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x000CE1FF File Offset: 0x000CC5FF
		public void Initialise(UserData userData)
		{
			this.m_Users.Clear();
			this.m_Users.Add(userData);
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x000CE218 File Offset: 0x000CC618
		public void AddUser(User user)
		{
			UserData userData = new UserData();
			userData.Initialise(user);
			this.m_Users.Add(userData);
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x000CE240 File Offset: 0x000CC640
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((uint)this.m_Users.Count, 3);
			for (int i = 0; i < this.m_Users.Count; i++)
			{
				UserData userData = this.m_Users._items[i];
				userData.Serialise(writer);
			}
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x000CE290 File Offset: 0x000CC690
		public bool Deserialise(BitStreamReader reader)
		{
			this.m_Users.Clear();
			uint num = reader.ReadUInt32(3);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				UserData userData = new UserData();
				userData.Deserialise(reader);
				this.m_Users.Add(userData);
				num2++;
			}
			return true;
		}

		// Token: 0x0400239C RID: 9116
		public FastList<UserData> m_Users = new FastList<UserData>(4);
	}
}
