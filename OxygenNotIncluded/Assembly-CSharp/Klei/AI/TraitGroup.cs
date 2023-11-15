using System;

namespace Klei.AI
{
	// Token: 0x02000E0D RID: 3597
	public class TraitGroup : ModifierGroup<Trait>
	{
		// Token: 0x06006E58 RID: 28248 RVA: 0x002B6E0D File Offset: 0x002B500D
		public TraitGroup(string id, string name, bool is_spawn_trait) : base(id, name)
		{
			this.IsSpawnTrait = is_spawn_trait;
		}

		// Token: 0x040052A2 RID: 21154
		public bool IsSpawnTrait;
	}
}
