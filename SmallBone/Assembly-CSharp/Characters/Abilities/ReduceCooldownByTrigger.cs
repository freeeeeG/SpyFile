using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AA0 RID: 2720
	[Serializable]
	public class ReduceCooldownByTrigger : Ability
	{
		// Token: 0x06003831 RID: 14385 RVA: 0x000A5BEC File Offset: 0x000A3DEC
		public ReduceCooldownByTrigger()
		{
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x000A5BFB File Offset: 0x000A3DFB
		public ReduceCooldownByTrigger(ITrigger trigger)
		{
			this.trigger = trigger;
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x000A5C11 File Offset: 0x000A3E11
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ReduceCooldownByTrigger.Instance(owner, this);
		}

		// Token: 0x04002CC8 RID: 11464
		public ITrigger trigger;

		// Token: 0x04002CC9 RID: 11465
		[SerializeField]
		private float _timeToReduce;

		// Token: 0x04002CCA RID: 11466
		[SerializeField]
		private bool _skill = true;

		// Token: 0x04002CCB RID: 11467
		[SerializeField]
		private bool _dash;

		// Token: 0x02000AA1 RID: 2721
		public class Instance : AbilityInstance<ReduceCooldownByTrigger>
		{
			// Token: 0x06003834 RID: 14388 RVA: 0x000A5C1A File Offset: 0x000A3E1A
			public Instance(Character owner, ReduceCooldownByTrigger ability) : base(owner, ability)
			{
			}

			// Token: 0x17000BC4 RID: 3012
			// (get) Token: 0x06003835 RID: 14389 RVA: 0x000A5C24 File Offset: 0x000A3E24
			public override float iconFillAmount
			{
				get
				{
					if (this.ability.trigger.cooldownTime <= 0f)
					{
						return base.iconFillAmount;
					}
					return this.ability.trigger.remainCooldownTime / this.ability.trigger.cooldownTime;
				}
			}

			// Token: 0x06003836 RID: 14390 RVA: 0x000A5C70 File Offset: 0x000A3E70
			protected override void OnAttach()
			{
				this.ability.trigger.Attach(this.owner);
				this.ability.trigger.onTriggered += this.OnTriggered;
			}

			// Token: 0x06003837 RID: 14391 RVA: 0x000A5CA4 File Offset: 0x000A3EA4
			protected override void OnDetach()
			{
				this.ability.trigger.Detach();
				this.ability.trigger.onTriggered -= this.OnTriggered;
			}

			// Token: 0x06003838 RID: 14392 RVA: 0x000A5CD2 File Offset: 0x000A3ED2
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this.ability.trigger.UpdateTime(deltaTime);
			}

			// Token: 0x06003839 RID: 14393 RVA: 0x000A5CEC File Offset: 0x000A3EEC
			private void OnTriggered()
			{
				foreach (Characters.Actions.Action action in this.owner.actions)
				{
					if (action.cooldown.time != null)
					{
						bool flag = this.ability._skill && action.type == Characters.Actions.Action.Type.Skill;
						bool flag2 = this.ability._dash && action.type == Characters.Actions.Action.Type.Dash;
						if (flag || flag2)
						{
							action.cooldown.time.remainTime -= this.ability._timeToReduce;
						}
					}
				}
			}

			// Token: 0x04002CCC RID: 11468
			private float _remainCooldownTime;
		}
	}
}
