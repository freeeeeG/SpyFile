using System;
using Characters.Abilities.Triggers;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AA5 RID: 2725
	public class ReduceSwapCooldownByTriggerComponent : AbilityComponent<ReduceSwapCooldownByTrigger>
	{
		// Token: 0x06003843 RID: 14403 RVA: 0x000A5E70 File Offset: 0x000A4070
		public override void Initialize()
		{
			this._ability.trigger = this._triggerComponent;
			base.Initialize();
		}

		// Token: 0x04002CD1 RID: 11473
		[SerializeField]
		[TriggerComponent.SubcomponentAttribute]
		private TriggerComponent _triggerComponent;
	}
}
