using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AC2 RID: 2754
	[Serializable]
	public sealed class DamageAttributeChange : Ability
	{
		// Token: 0x060038A6 RID: 14502 RVA: 0x000A7058 File Offset: 0x000A5258
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new DamageAttributeChange.Instance(owner, this);
		}

		// Token: 0x04002D10 RID: 11536
		[SerializeField]
		private MotionTypeBoolArray _motionType;

		// Token: 0x02000AC3 RID: 2755
		public sealed class Instance : AbilityInstance<DamageAttributeChange>
		{
			// Token: 0x060038A8 RID: 14504 RVA: 0x000A7061 File Offset: 0x000A5261
			public Instance(Character owner, DamageAttributeChange ability) : base(owner, ability)
			{
			}

			// Token: 0x060038A9 RID: 14505 RVA: 0x000A706B File Offset: 0x000A526B
			protected override void OnAttach()
			{
				Stat stat = this.owner.stat;
				stat.IsChangeAttribute = (Func<HitInfo, bool>)Delegate.Combine(stat.IsChangeAttribute, new Func<HitInfo, bool>(this.CheckMotion));
			}

			// Token: 0x060038AA RID: 14506 RVA: 0x000A7099 File Offset: 0x000A5299
			private bool CheckMotion(HitInfo hitInfo)
			{
				return this.ability._motionType[hitInfo.motionType];
			}

			// Token: 0x060038AB RID: 14507 RVA: 0x000A70B6 File Offset: 0x000A52B6
			protected override void OnDetach()
			{
				Stat stat = this.owner.stat;
				stat.IsChangeAttribute = (Func<HitInfo, bool>)Delegate.Remove(stat.IsChangeAttribute, new Func<HitInfo, bool>(this.CheckMotion));
			}
		}
	}
}
