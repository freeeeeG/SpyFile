using System;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B6B RID: 2923
	public abstract class CharacterStatusAbility
	{
		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06003AA3 RID: 15011
		// (remove) Token: 0x06003AA4 RID: 15012
		public abstract event CharacterStatus.OnTimeDelegate onAttachEvents;

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06003AA5 RID: 15013
		// (remove) Token: 0x06003AA6 RID: 15014
		public abstract event CharacterStatus.OnTimeDelegate onRefreshEvents;

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06003AA7 RID: 15015
		// (remove) Token: 0x06003AA8 RID: 15016
		public abstract event CharacterStatus.OnTimeDelegate onDetachEvents;

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06003AA9 RID: 15017 RVA: 0x000AD390 File Offset: 0x000AB590
		// (set) Token: 0x06003AAA RID: 15018 RVA: 0x000AD398 File Offset: 0x000AB598
		public StatusEffect.EffectHandler effectHandler { get; set; }

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06003AAB RID: 15019 RVA: 0x000AD3A1 File Offset: 0x000AB5A1
		// (set) Token: 0x06003AAC RID: 15020 RVA: 0x000AD3A9 File Offset: 0x000AB5A9
		public Character attacker { get; set; }

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06003AAD RID: 15021 RVA: 0x000AD3B2 File Offset: 0x000AB5B2
		// (set) Token: 0x06003AAE RID: 15022 RVA: 0x000AD3BA File Offset: 0x000AB5BA
		public float durationMultiplier { get; set; } = 1f;

		// Token: 0x04002E81 RID: 11905
		public CharacterStatus.OnTimeDelegate onAttached;

		// Token: 0x04002E82 RID: 11906
		public CharacterStatus.OnTimeDelegate onRefreshed;

		// Token: 0x04002E83 RID: 11907
		public CharacterStatus.OnTimeDelegate onDetached;
	}
}
