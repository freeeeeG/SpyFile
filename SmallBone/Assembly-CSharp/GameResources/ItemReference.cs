using System;
using Characters.Gear;
using Characters.Gear.Synergy.Inscriptions;

namespace GameResources
{
	// Token: 0x02000182 RID: 386
	[Serializable]
	public class ItemReference : GearReference
	{
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x000076D4 File Offset: 0x000058D4
		public override Gear.Type type
		{
			get
			{
				return Gear.Type.Item;
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00018568 File Offset: 0x00016768
		public new ItemRequest LoadAsync()
		{
			return new ItemRequest(this.path);
		}

		// Token: 0x040006A3 RID: 1699
		public Inscription.Key prefabKeyword1;

		// Token: 0x040006A4 RID: 1700
		public Inscription.Key prefabKeyword2;
	}
}
