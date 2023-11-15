using System;

namespace Characters.Abilities
{
	// Token: 0x02000993 RID: 2451
	public abstract class AbilityInstance<T> : AbilityInstance where T : Ability
	{
		// Token: 0x06003499 RID: 13465 RVA: 0x0009B252 File Offset: 0x00099452
		public AbilityInstance(Character owner, T ability) : base(owner, ability)
		{
			this.ability = ability;
		}

		// Token: 0x04002A71 RID: 10865
		public new readonly T ability;
	}
}
