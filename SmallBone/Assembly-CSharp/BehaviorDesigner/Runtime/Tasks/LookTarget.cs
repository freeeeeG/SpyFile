using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200146F RID: 5231
	[TaskDescription("Allows multiple action tasks to be added to a single node.")]
	[TaskIcon("{SkinColor}TurnOnEdge.png")]
	public sealed class LookTarget : Action
	{
		// Token: 0x06006610 RID: 26128 RVA: 0x00126E6C File Offset: 0x0012506C
		public override TaskStatus OnUpdate()
		{
			Character value = this._owner.Value;
			Character value2 = this._target.Value;
			if (value2 == null)
			{
				return TaskStatus.Failure;
			}
			if (value2.transform.position.x > value.transform.position.x)
			{
				this._moveDirection.SetValue(Vector2.right);
			}
			else
			{
				this._moveDirection.SetValue(Vector2.left);
			}
			return TaskStatus.Success;
		}

		// Token: 0x04005207 RID: 20999
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005208 RID: 21000
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x04005209 RID: 21001
		[SerializeField]
		private SharedVector2 _moveDirection;
	}
}
