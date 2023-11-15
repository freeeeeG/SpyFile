using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A98 RID: 2712
	public sealed class OperationByTriggersComponent : AbilityComponent<OperationByTriggers>
	{
		// Token: 0x0600381F RID: 14367 RVA: 0x000A5A13 File Offset: 0x000A3C13
		public override void Initialize()
		{
			this._ability.operations = this._operations.components;
			base.Initialize();
		}

		// Token: 0x04002CC0 RID: 11456
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operations;
	}
}
