using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014DD RID: 5341
	[TaskDescription("Returns the angle between two Vector3s.")]
	[TaskCategory("Unity/Vector3")]
	public class Angle : Action
	{
		// Token: 0x060067D9 RID: 26585 RVA: 0x0012C4E6 File Offset: 0x0012A6E6
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.Angle(this.firstVector3.Value, this.secondVector3.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060067DA RID: 26586 RVA: 0x0012C50F File Offset: 0x0012A70F
		public override void OnReset()
		{
			this.firstVector3 = Vector3.zero;
			this.secondVector3 = Vector3.zero;
			this.storeResult = 0f;
		}

		// Token: 0x040053C8 RID: 21448
		[Tooltip("The first Vector3")]
		public SharedVector3 firstVector3;

		// Token: 0x040053C9 RID: 21449
		[Tooltip("The second Vector3")]
		public SharedVector3 secondVector3;

		// Token: 0x040053CA RID: 21450
		[RequiredField]
		[Tooltip("The angle")]
		public SharedFloat storeResult;
	}
}
