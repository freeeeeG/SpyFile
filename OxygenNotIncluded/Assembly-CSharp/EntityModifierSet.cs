using System;
using Database;

// Token: 0x02000A49 RID: 2633
public class EntityModifierSet : ModifierSet
{
	// Token: 0x06004F51 RID: 20305 RVA: 0x001C0610 File Offset: 0x001BE810
	public override void Initialize()
	{
		base.Initialize();
		this.DuplicantStatusItems = new DuplicantStatusItems(this.Root);
		this.ChoreGroups = new ChoreGroups(this.Root);
		base.LoadTraits();
	}

	// Token: 0x040033D8 RID: 13272
	public DuplicantStatusItems DuplicantStatusItems;

	// Token: 0x040033D9 RID: 13273
	public ChoreGroups ChoreGroups;
}
