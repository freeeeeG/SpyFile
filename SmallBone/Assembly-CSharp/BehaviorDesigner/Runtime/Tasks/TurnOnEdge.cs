using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200148B RID: 5259
	[TaskIcon("{SkinColor}TurnOnEdge.png")]
	[TaskDescription("Allows multiple action tasks to be added to a single node.")]
	public sealed class TurnOnEdge : Action
	{
		// Token: 0x06006677 RID: 26231 RVA: 0x001287E8 File Offset: 0x001269E8
		public override TaskStatus OnUpdate()
		{
			Character value = this._owner.Value;
			Vector2 value2 = this._moveDirection.Value;
			value.movement.TurnOnEdge(ref value2);
			this._moveDirection.SetValue(value2);
			return TaskStatus.Success;
		}

		// Token: 0x04005283 RID: 21123
		[SerializeField]
		private SharedVector2 _moveDirection;

		// Token: 0x04005284 RID: 21124
		[SerializeField]
		private SharedCharacter _owner;
	}
}
