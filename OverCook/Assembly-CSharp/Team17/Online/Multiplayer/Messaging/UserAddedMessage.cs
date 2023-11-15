using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008E6 RID: 2278
	public class UserAddedMessage : Serialisable
	{
		// Token: 0x06002C37 RID: 11319 RVA: 0x000CDE2B File Offset: 0x000CC22B
		public UserAddedMessage()
		{
			this.User = new UserData();
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06002C38 RID: 11320 RVA: 0x000CDE4A File Offset: 0x000CC24A
		// (set) Token: 0x06002C39 RID: 11321 RVA: 0x000CDE52 File Offset: 0x000CC252
		public uint UserIndex { get; private set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06002C3A RID: 11322 RVA: 0x000CDE5B File Offset: 0x000CC25B
		// (set) Token: 0x06002C3B RID: 11323 RVA: 0x000CDE63 File Offset: 0x000CC263
		public UserData User { get; private set; }

		// Token: 0x06002C3C RID: 11324 RVA: 0x000CDE6C File Offset: 0x000CC26C
		public void Initialise(uint _idx, User user)
		{
			this.UserIndex = _idx;
			this.User.Initialise(user);
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x000CDE81 File Offset: 0x000CC281
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write(this.UserIndex, this.kIdxBitCount);
			this.User.Serialise(writer);
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x000CDEA1 File Offset: 0x000CC2A1
		public bool Deserialise(BitStreamReader reader)
		{
			this.UserIndex = reader.ReadUInt32(this.kIdxBitCount);
			this.User.Deserialise(reader);
			return true;
		}

		// Token: 0x0400238B RID: 9099
		private int kIdxBitCount = GameUtils.GetRequiredBitCount(4);
	}
}
