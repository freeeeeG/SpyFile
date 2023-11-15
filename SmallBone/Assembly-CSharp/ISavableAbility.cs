using System;
using Characters.Abilities;

// Token: 0x0200001B RID: 27
public interface ISavableAbility
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600005C RID: 92
	// (set) Token: 0x0600005D RID: 93
	float remainTime { get; set; }

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600005E RID: 94
	// (set) Token: 0x0600005F RID: 95
	float stack { get; set; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000060 RID: 96
	IAbility ability { get; }
}
