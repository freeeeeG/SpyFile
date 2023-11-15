using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200146E RID: 5230
	[TaskIcon("{SkinColor}TurnOnEdge.png")]
	[TaskDescription("캐릭터를 바라본다.")]
	public sealed class LookCharacter : Action
	{
		// Token: 0x0600660E RID: 26126 RVA: 0x00126E24 File Offset: 0x00125024
		public override TaskStatus OnUpdate()
		{
			Character value = this._owner.Value;
			Character value2 = this._target.Value;
			if (value2 == null)
			{
				return TaskStatus.Failure;
			}
			value.ForceToLookAt(value2.transform.position.x);
			return TaskStatus.Success;
		}

		// Token: 0x04005205 RID: 20997
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005206 RID: 20998
		[SerializeField]
		private SharedCharacter _target;
	}
}
