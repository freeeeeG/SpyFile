using System;
using Characters.Gear.Synergy.Inscriptions;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C6C RID: 3180
	[Serializable]
	public sealed class StatBonusCooldownable : CooldownAbility
	{
		// Token: 0x060040F9 RID: 16633 RVA: 0x000BCD96 File Offset: 0x000BAF96
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusCooldownable.Instance(owner, this);
		}

		// Token: 0x040031DC RID: 12764
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x02000C6D RID: 3181
		public sealed class Instance : CooldownAbility.CooldownAbilityInstance
		{
			// Token: 0x060040FB RID: 16635 RVA: 0x000BCD9F File Offset: 0x000BAF9F
			public Instance(Character owner, StatBonusCooldownable ability) : base(owner, ability)
			{
				this._ability = ability;
			}

			// Token: 0x060040FC RID: 16636 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x060040FD RID: 16637 RVA: 0x000BCDB0 File Offset: 0x000BAFB0
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this._remainCooldownTime < 0f)
				{
					this.OnAttachBuff();
				}
			}

			// Token: 0x060040FE RID: 16638 RVA: 0x000BCDCC File Offset: 0x000BAFCC
			protected override void OnAttachBuff()
			{
				base.OnAttachBuff();
				this.owner.stat.AttachValues(this._ability._stat);
			}

			// Token: 0x060040FF RID: 16639 RVA: 0x000BCDEF File Offset: 0x000BAFEF
			protected override void OnDetachBuff()
			{
				base.OnDetachBuff();
				this.owner.stat.DetachValues(this._ability._stat);
			}

			// Token: 0x06004100 RID: 16640 RVA: 0x000BCE12 File Offset: 0x000BB012
			protected override void OnDetach()
			{
				this.OnDetachBuff();
			}

			// Token: 0x040031DD RID: 12765
			private StatBonusCooldownable _ability;
		}
	}
}
