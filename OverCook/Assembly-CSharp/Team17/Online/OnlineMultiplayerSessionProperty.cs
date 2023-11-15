using System;

namespace Team17.Online
{
	// Token: 0x0200095D RID: 2397
	public class OnlineMultiplayerSessionProperty : IOnlineMultiplayerSessionProperty
	{
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06002EED RID: 12013 RVA: 0x000DBC28 File Offset: 0x000DA028
		public string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06002EEE RID: 12014 RVA: 0x000DBC30 File Offset: 0x000DA030
		public uint Id
		{
			get
			{
				return this.m_id;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06002EEF RID: 12015 RVA: 0x000DBC38 File Offset: 0x000DA038
		public uint Index
		{
			get
			{
				return this.m_index;
			}
		}

		// Token: 0x0400257D RID: 9597
		public string m_name;

		// Token: 0x0400257E RID: 9598
		public uint m_id;

		// Token: 0x0400257F RID: 9599
		public uint m_index;
	}
}
