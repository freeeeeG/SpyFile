using System;
using Characters.Abilities.Triggers;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A94 RID: 2708
	public class OperationByTriggerComponent : AbilityComponent<OperationByTrigger>
	{
		// Token: 0x06003811 RID: 14353 RVA: 0x000A5690 File Offset: 0x000A3890
		public override void Initialize()
		{
			this._ability.trigger = this._triggerComponent;
			this._ability.operations = this._operations.components;
			base.Initialize();
		}

		// Token: 0x04002CB5 RID: 11445
		[SerializeField]
		[TriggerComponent.SubcomponentAttribute]
		private TriggerComponent _triggerComponent;

		// Token: 0x04002CB6 RID: 11446
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operations;
	}
}
