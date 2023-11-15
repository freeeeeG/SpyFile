using System;

namespace flanne
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public struct LocalizedString
	{
		// Token: 0x060004DB RID: 1243 RVA: 0x000186E2 File Offset: 0x000168E2
		public LocalizedString(string key)
		{
			this.key = key;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000186EB File Offset: 0x000168EB
		public static implicit operator LocalizedString(string key)
		{
			return new LocalizedString(key);
		}

		// Token: 0x040002CD RID: 717
		public string key;
	}
}
