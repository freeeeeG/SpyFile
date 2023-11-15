using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
	// Token: 0x02001504 RID: 5380
	[TaskCategory("Unity/Transform")]
	[TaskDescription("Stores the position of the Transform. Returns Success.")]
	public class GetCharacterPosition : Action
	{
		// Token: 0x0600684A RID: 26698 RVA: 0x0012D4D0 File Offset: 0x0012B6D0
		public override void OnStart()
		{
			this._targetTransform = this._target.Value.transform;
		}

		// Token: 0x0600684B RID: 26699 RVA: 0x0012D4E8 File Offset: 0x0012B6E8
		public override TaskStatus OnUpdate()
		{
			if (this._targetTransform == null)
			{
				return TaskStatus.Failure;
			}
			this._storeValue.Value = this._targetTransform.position;
			return TaskStatus.Success;
		}

		// Token: 0x0400543C RID: 21564
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x0400543D RID: 21565
		[SerializeField]
		private SharedVector2 _storeValue;

		// Token: 0x0400543E RID: 21566
		private Transform _targetTransform;
	}
}
