using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200146D RID: 5229
	[TaskDescription("Treu일 경우 캐릭터가 움직일때도 계속 뒤를 봅니다.원래대로 돌리려면 backward를 false로 바꿔줘야합니다.")]
	[TaskIcon("{SkinColor}TurnOnEdge.png")]
	public sealed class LookBackward : Action
	{
		// Token: 0x0600660C RID: 26124 RVA: 0x00126DEC File Offset: 0x00124FEC
		public override TaskStatus OnUpdate()
		{
			Character value = this._owner.Value;
			if (value == null)
			{
				return TaskStatus.Failure;
			}
			value.movement.moveBackward = this._backward;
			return TaskStatus.Success;
		}

		// Token: 0x04005203 RID: 20995
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005204 RID: 20996
		[SerializeField]
		private bool _backward;
	}
}
