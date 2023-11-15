using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A50 RID: 2640
	[Serializable]
	public class IgnoreSkillCooldown : Ability
	{
		// Token: 0x06003754 RID: 14164 RVA: 0x000A3246 File Offset: 0x000A1446
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new IgnoreSkillCooldown.Instance(owner, this);
		}

		// Token: 0x04002C0D RID: 11277
		[Range(1f, 100f)]
		[SerializeField]
		private int _possibility;

		// Token: 0x02000A51 RID: 2641
		public class Instance : AbilityInstance<IgnoreSkillCooldown>
		{
			// Token: 0x06003755 RID: 14165 RVA: 0x000A324F File Offset: 0x000A144F
			public Instance(Character owner, IgnoreSkillCooldown ability) : base(owner, ability)
			{
			}

			// Token: 0x06003756 RID: 14166 RVA: 0x000A3259 File Offset: 0x000A1459
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.OnStartAction;
			}

			// Token: 0x06003757 RID: 14167 RVA: 0x000A3272 File Offset: 0x000A1472
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.OnStartAction;
			}

			// Token: 0x06003758 RID: 14168 RVA: 0x000A328C File Offset: 0x000A148C
			private void OnStartAction(Characters.Actions.Action action)
			{
				if (!MMMaths.PercentChance(this.ability._possibility) || action.type != Characters.Actions.Action.Type.Skill || action.cooldown.time == null || action.cooldown.usedByStreak)
				{
					return;
				}
				action.cooldown.time.remainTime = Mathf.Min(0.2f, action.cooldown.time.cooldownTime);
			}
		}
	}
}
