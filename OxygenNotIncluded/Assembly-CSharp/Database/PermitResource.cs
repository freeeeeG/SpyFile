using System;

namespace Database
{
	// Token: 0x02000D10 RID: 3344
	public abstract class PermitResource : Resource
	{
		// Token: 0x060069D8 RID: 27096 RVA: 0x00290EC0 File Offset: 0x0028F0C0
		public PermitResource(string id, string Name, string Desc, PermitCategory permitCategory, PermitRarity rarity) : base(id, Name)
		{
			DebugUtil.DevAssert(Name != null, "Name must be provided for permit with id \"" + id + "\" of type " + base.GetType().Name, null);
			DebugUtil.DevAssert(Desc != null, "Description must be provided for permit with id \"" + id + "\" of type " + base.GetType().Name, null);
			this.Description = Desc;
			this.Category = permitCategory;
			this.Rarity = rarity;
		}

		// Token: 0x060069D9 RID: 27097
		public abstract PermitPresentationInfo GetPermitPresentationInfo();

		// Token: 0x060069DA RID: 27098 RVA: 0x00290F36 File Offset: 0x0028F136
		public bool IsOwnable()
		{
			return this.Rarity != PermitRarity.Universal;
		}

		// Token: 0x060069DB RID: 27099 RVA: 0x00290F44 File Offset: 0x0028F144
		public bool IsUnlocked()
		{
			return !this.IsOwnable() || PermitItems.IsPermitUnlocked(this);
		}

		// Token: 0x04004C8C RID: 19596
		public string Description;

		// Token: 0x04004C8D RID: 19597
		public PermitCategory Category;

		// Token: 0x04004C8E RID: 19598
		public PermitRarity Rarity;
	}
}
