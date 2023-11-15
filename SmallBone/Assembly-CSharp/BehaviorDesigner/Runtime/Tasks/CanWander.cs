using System;
using Characters.AI;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014BC RID: 5308
	[TaskDescription("Wander를 할 수 있는가? 현재 땅 위에있지 않거나, 바닥 타일의 크기가 3보다 작을 경우 False")]
	public class CanWander : Conditional
	{
		// Token: 0x06006741 RID: 26433 RVA: 0x0012AF1C File Offset: 0x0012911C
		public override TaskStatus OnUpdate()
		{
			if (!Precondition.CanMove(this._character.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x04005358 RID: 21336
		[SerializeField]
		[Tooltip("행동 주체")]
		private SharedCharacter _character;
	}
}
