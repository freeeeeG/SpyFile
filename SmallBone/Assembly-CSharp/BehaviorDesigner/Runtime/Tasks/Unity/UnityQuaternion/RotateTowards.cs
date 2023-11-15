using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x0200156F RID: 5487
	[TaskDescription("Stores the quaternion after a rotation.")]
	[TaskCategory("Unity/Quaternion")]
	public class RotateTowards : Action
	{
		// Token: 0x060069C5 RID: 27077 RVA: 0x00130427 File Offset: 0x0012E627
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.RotateTowards(this.fromQuaternion.Value, this.toQuaternion.Value, this.maxDeltaDegrees.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069C6 RID: 27078 RVA: 0x0013045C File Offset: 0x0012E65C
		public override void OnReset()
		{
			this.fromQuaternion = (this.toQuaternion = (this.storeResult = Quaternion.identity));
			this.maxDeltaDegrees = 0f;
		}

		// Token: 0x04005588 RID: 21896
		[Tooltip("The from rotation")]
		public SharedQuaternion fromQuaternion;

		// Token: 0x04005589 RID: 21897
		[Tooltip("The to rotation")]
		public SharedQuaternion toQuaternion;

		// Token: 0x0400558A RID: 21898
		[Tooltip("The maximum degrees delta")]
		public SharedFloat maxDeltaDegrees;

		// Token: 0x0400558B RID: 21899
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
