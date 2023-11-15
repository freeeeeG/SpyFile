using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001470 RID: 5232
	[TaskDescription("Allows multiple action tasks to be added to a single node.")]
	[TaskIcon("{SkinColor}StackedActionIcon.png")]
	public sealed class Move : Action
	{
		// Token: 0x06006612 RID: 26130 RVA: 0x00126EEC File Offset: 0x001250EC
		public override TaskStatus OnUpdate()
		{
			Vector2 value = this._direction.Value;
			Character value2 = this._actor.Value;
			if (this._direction.Value != value)
			{
				this._direction.SetValue(value);
			}
			value2.movement.Move(value);
			return TaskStatus.Success;
		}

		// Token: 0x0400520A RID: 21002
		[SerializeField]
		private SharedVector2 _direction;

		// Token: 0x0400520B RID: 21003
		[SerializeField]
		private SharedCharacter _actor;
	}
}
