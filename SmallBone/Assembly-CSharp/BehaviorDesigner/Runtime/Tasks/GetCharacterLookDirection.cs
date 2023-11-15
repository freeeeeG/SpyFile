using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001467 RID: 5223
	public class GetCharacterLookDirection : Action
	{
		// Token: 0x060065FD RID: 26109 RVA: 0x00126BD4 File Offset: 0x00124DD4
		public override TaskStatus OnUpdate()
		{
			Character value = this._owner.Value;
			if (value == null)
			{
				return TaskStatus.Failure;
			}
			this._lookingDirection.SetValue((value.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left);
			return TaskStatus.Success;
		}

		// Token: 0x040051F7 RID: 20983
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040051F8 RID: 20984
		[SerializeField]
		private SharedVector2 _lookingDirection;
	}
}
