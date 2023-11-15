using System;
using Characters.Abilities.Triggers;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AA2 RID: 2722
	public class ReduceCooldownByTriggerComponent : AbilityComponent<ReduceCooldownByTrigger>
	{
		// Token: 0x0600383A RID: 14394 RVA: 0x000A5DA4 File Offset: 0x000A3FA4
		public override void Initialize()
		{
			this._ability.trigger = this._triggerComponent;
			base.Initialize();
		}

		// Token: 0x04002CCD RID: 11469
		[TriggerComponent.SubcomponentAttribute]
		[SerializeField]
		private TriggerComponent _triggerComponent;
	}
}
