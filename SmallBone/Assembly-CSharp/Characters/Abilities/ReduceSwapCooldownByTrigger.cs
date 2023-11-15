using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AA3 RID: 2723
	[Serializable]
	public class ReduceSwapCooldownByTrigger : Ability
	{
		// Token: 0x0600383C RID: 14396 RVA: 0x00089C49 File Offset: 0x00087E49
		public ReduceSwapCooldownByTrigger()
		{
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x000A5DC5 File Offset: 0x000A3FC5
		public ReduceSwapCooldownByTrigger(ITrigger trigger)
		{
			this.trigger = trigger;
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x000A5DD4 File Offset: 0x000A3FD4
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ReduceSwapCooldownByTrigger.Instance(owner, this);
		}

		// Token: 0x04002CCE RID: 11470
		public ITrigger trigger;

		// Token: 0x04002CCF RID: 11471
		[SerializeField]
		private float _timeToReduce;

		// Token: 0x02000AA4 RID: 2724
		public class Instance : AbilityInstance<ReduceSwapCooldownByTrigger>
		{
			// Token: 0x0600383F RID: 14399 RVA: 0x000A5DDD File Offset: 0x000A3FDD
			public Instance(Character owner, ReduceSwapCooldownByTrigger ability) : base(owner, ability)
			{
			}

			// Token: 0x06003840 RID: 14400 RVA: 0x000A5DE7 File Offset: 0x000A3FE7
			protected override void OnAttach()
			{
				this.ability.trigger.Attach(this.owner);
				this.ability.trigger.onTriggered += this.OnTriggered;
			}

			// Token: 0x06003841 RID: 14401 RVA: 0x000A5E1B File Offset: 0x000A401B
			protected override void OnDetach()
			{
				this.ability.trigger.Detach();
				this.ability.trigger.onTriggered -= this.OnTriggered;
			}

			// Token: 0x06003842 RID: 14402 RVA: 0x000A5E49 File Offset: 0x000A4049
			private void OnTriggered()
			{
				this.owner.playerComponents.inventory.weapon.ReduceSwapCooldown(this.ability._timeToReduce);
			}

			// Token: 0x04002CD0 RID: 11472
			private float _remainCooldownTime;
		}
	}
}
