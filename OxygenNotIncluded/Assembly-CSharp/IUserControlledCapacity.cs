using System;

// Token: 0x02000C01 RID: 3073
public interface IUserControlledCapacity
{
	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x0600613D RID: 24893
	// (set) Token: 0x0600613E RID: 24894
	float UserMaxCapacity { get; set; }

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x0600613F RID: 24895
	float AmountStored { get; }

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x06006140 RID: 24896
	float MinCapacity { get; }

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x06006141 RID: 24897
	float MaxCapacity { get; }

	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x06006142 RID: 24898
	bool WholeValues { get; }

	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x06006143 RID: 24899
	LocString CapacityUnits { get; }
}
