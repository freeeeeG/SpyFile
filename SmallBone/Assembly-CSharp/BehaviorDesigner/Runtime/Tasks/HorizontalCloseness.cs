using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014BD RID: 5309
	[TaskDescription("Target과의 수평거리가 distance 보다 작은가")]
	public class HorizontalCloseness : Conditional
	{
		// Token: 0x06006743 RID: 26435 RVA: 0x0012AF33 File Offset: 0x00129133
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
			this._distanceValue = this._distance.Value;
		}

		// Token: 0x06006744 RID: 26436 RVA: 0x0012AF58 File Offset: 0x00129158
		public override TaskStatus OnUpdate()
		{
			Character value = this._target.Value;
			if (value == null)
			{
				return TaskStatus.Failure;
			}
			if (Mathf.Abs(this._ownerValue.transform.position.x - value.transform.position.x) < this._distanceValue)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		// Token: 0x04005359 RID: 21337
		[SerializeField]
		[Tooltip("행동 주체")]
		private SharedCharacter _owner;

		// Token: 0x0400535A RID: 21338
		[Tooltip("타겟")]
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x0400535B RID: 21339
		[Tooltip("거리")]
		[SerializeField]
		private SharedFloat _distance;

		// Token: 0x0400535C RID: 21340
		private Character _ownerValue;

		// Token: 0x0400535D RID: 21341
		private float _distanceValue;
	}
}
