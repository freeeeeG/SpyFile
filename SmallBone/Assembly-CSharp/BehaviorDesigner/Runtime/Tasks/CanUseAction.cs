using System;
using Characters.Actions;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200149D RID: 5277
	[TaskIcon("Assets/Behavior Designer/Icon/CanUseAction.png")]
	public class CanUseAction : Conditional
	{
		// Token: 0x060066F4 RID: 26356 RVA: 0x00129B30 File Offset: 0x00127D30
		public override void OnAwake()
		{
			this._actionValue = this._action.Value;
		}

		// Token: 0x060066F5 RID: 26357 RVA: 0x00129B43 File Offset: 0x00127D43
		public override TaskStatus OnUpdate()
		{
			if (!this._actionValue.canUse)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x040052CE RID: 21198
		[SerializeField]
		private SharedCharacterAction _action;

		// Token: 0x040052CF RID: 21199
		private Characters.Actions.Action _actionValue;
	}
}
