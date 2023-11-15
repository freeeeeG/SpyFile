using System;

namespace Characters.Abilities
{
	// Token: 0x02000A4B RID: 2635
	public interface IAbility
	{
		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06003744 RID: 14148
		float duration { get; }

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06003745 RID: 14149
		int iconPriority { get; }

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06003746 RID: 14150
		bool removeOnSwapWeapon { get; }

		// Token: 0x06003747 RID: 14151
		void Initialize();

		// Token: 0x06003748 RID: 14152
		IAbilityInstance CreateInstance(Character owner);
	}
}
