using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x02001647 RID: 5703
	[TaskDescription("Target이 가까운지 체크")]
	public class IsReachToTarget : Conditional
	{
		// Token: 0x06006CCD RID: 27853 RVA: 0x00137167 File Offset: 0x00135367
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
		}

		// Token: 0x06006CCE RID: 27854 RVA: 0x0013717C File Offset: 0x0013537C
		public override TaskStatus OnUpdate()
		{
			Character value = this._target.Value;
			float num = this._ownerValue.stat.GetInterpolatedMovementSpeed() * this._ownerValue.chronometer.animation.deltaTime;
			if (this._ownerValue == null || value == null)
			{
				return TaskStatus.Failure;
			}
			if (this._ownerValue.transform.position.x + num >= value.transform.position.x && this._ownerValue.transform.position.x - num <= value.transform.position.x)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		// Token: 0x04005896 RID: 22678
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005897 RID: 22679
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x04005898 RID: 22680
		private Character _ownerValue;
	}
}
