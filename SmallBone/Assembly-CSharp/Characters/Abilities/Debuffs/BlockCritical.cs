using System;

namespace Characters.Abilities.Debuffs
{
	// Token: 0x02000BA1 RID: 2977
	[Serializable]
	public sealed class BlockCritical : Ability
	{
		// Token: 0x06003D90 RID: 15760 RVA: 0x000B2FF1 File Offset: 0x000B11F1
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new BlockCritical.Instance(owner, this);
		}

		// Token: 0x02000BA2 RID: 2978
		public sealed class Instance : AbilityInstance<BlockCritical>
		{
			// Token: 0x06003D92 RID: 15762 RVA: 0x000B2FFA File Offset: 0x000B11FA
			public Instance(Character owner, BlockCritical ability) : base(owner, ability)
			{
			}

			// Token: 0x06003D93 RID: 15763 RVA: 0x000B3004 File Offset: 0x000B1204
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
			}

			// Token: 0x06003D94 RID: 15764 RVA: 0x000B3027 File Offset: 0x000B1227
			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				damage.SetGuaranteedCritical(int.MaxValue, false);
				return false;
			}

			// Token: 0x06003D95 RID: 15765 RVA: 0x000B3036 File Offset: 0x000B1236
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
			}
		}
	}
}
