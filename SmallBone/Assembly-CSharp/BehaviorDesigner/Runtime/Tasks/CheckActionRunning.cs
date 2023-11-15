using System;
using Characters.Actions;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014A0 RID: 5280
	public class CheckActionRunning : Conditional
	{
		// Token: 0x060066FA RID: 26362 RVA: 0x00129DEF File Offset: 0x00127FEF
		public override void OnAwake()
		{
			this._actionValue = this._action.Value;
		}

		// Token: 0x060066FB RID: 26363 RVA: 0x00129E02 File Offset: 0x00128002
		public override TaskStatus OnUpdate()
		{
			if (!this._actionValue.running)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x040052D9 RID: 21209
		[SerializeField]
		private SharedCharacterAction _action;

		// Token: 0x040052DA RID: 21210
		private Characters.Actions.Action _actionValue;
	}
}
