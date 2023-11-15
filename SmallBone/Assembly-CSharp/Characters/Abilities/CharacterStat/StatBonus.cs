using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C3F RID: 3135
	[Serializable]
	public class StatBonus : Ability
	{
		// Token: 0x06004051 RID: 16465 RVA: 0x00089C49 File Offset: 0x00087E49
		public StatBonus()
		{
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x000BAD67 File Offset: 0x000B8F67
		public StatBonus(Stat.Values stat)
		{
			this._stat = stat;
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x000BAD76 File Offset: 0x000B8F76
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonus.Instance(owner, this);
		}

		// Token: 0x04003170 RID: 12656
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x02000C40 RID: 3136
		public class Instance : AbilityInstance<StatBonus>
		{
			// Token: 0x06004054 RID: 16468 RVA: 0x000BAD7F File Offset: 0x000B8F7F
			public Instance(Character owner, StatBonus ability) : base(owner, ability)
			{
			}

			// Token: 0x06004055 RID: 16469 RVA: 0x000BAD89 File Offset: 0x000B8F89
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this.ability._stat);
			}

			// Token: 0x06004056 RID: 16470 RVA: 0x000BADA6 File Offset: 0x000B8FA6
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this.ability._stat);
			}
		}
	}
}
