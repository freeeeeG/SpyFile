using System;
using Characters.Abilities.Triggers;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C43 RID: 3139
	public sealed class StatBonusAndTimeBonusByTriggerComponent : AbilityComponent<StatBonusAndTimeBonusByTrigger>
	{
		// Token: 0x0600405F RID: 16479 RVA: 0x000BAF20 File Offset: 0x000B9120
		public override void Initialize()
		{
			this._ability.trigger = this._triggerComponent;
			base.Initialize();
		}

		// Token: 0x04003176 RID: 12662
		[TriggerComponent.SubcomponentAttribute]
		[SerializeField]
		private TriggerComponent _triggerComponent;
	}
}
