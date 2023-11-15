using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200148A RID: 5258
	[TaskIcon("{SkinColor}TurnOnEdge.png")]
	[TaskDescription("뒤를 바라본다.")]
	public sealed class TurnAround : Action
	{
		// Token: 0x06006674 RID: 26228 RVA: 0x001287B3 File Offset: 0x001269B3
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
		}

		// Token: 0x06006675 RID: 26229 RVA: 0x001287C6 File Offset: 0x001269C6
		public override TaskStatus OnUpdate()
		{
			this._ownerValue.ForceToLookAt((this._ownerValue.lookingDirection == Character.LookingDirection.Left) ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			return TaskStatus.Success;
		}

		// Token: 0x04005281 RID: 21121
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005282 RID: 21122
		private Character _ownerValue;
	}
}
