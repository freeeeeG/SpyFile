using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001460 RID: 5216
	[TaskDescription("실행중인 액션을 취소합니다. character.CancelAction()")]
	public sealed class CancelCharacterAction : Action
	{
		// Token: 0x060065E5 RID: 26085 RVA: 0x001264C1 File Offset: 0x001246C1
		public override TaskStatus OnUpdate()
		{
			if (this._character == null)
			{
				return TaskStatus.Failure;
			}
			this._character.Value.CancelAction();
			return TaskStatus.Success;
		}

		// Token: 0x040051CE RID: 20942
		[SerializeField]
		private SharedCharacter _character;
	}
}
