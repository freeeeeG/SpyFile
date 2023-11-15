using System;
using Data;

namespace Level
{
	// Token: 0x020004D3 RID: 1235
	public interface IPurchasable
	{
		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600180A RID: 6154
		// (set) Token: 0x0600180B RID: 6155
		int price { get; set; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600180C RID: 6156
		// (set) Token: 0x0600180D RID: 6157
		GameData.Currency.Type priceCurrency { get; set; }
	}
}
