using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014DE RID: 5342
	[TaskDescription("Clamps the magnitude of the Vector3.")]
	[TaskCategory("Unity/Vector3")]
	public class ClampMagnitude : Action
	{
		// Token: 0x060067DC RID: 26588 RVA: 0x0012C541 File Offset: 0x0012A741
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.ClampMagnitude(this.vector3Variable.Value, this.maxLength.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060067DD RID: 26589 RVA: 0x0012C56A File Offset: 0x0012A76A
		public override void OnReset()
		{
			this.vector3Variable = Vector3.zero;
			this.storeResult = Vector3.zero;
			this.maxLength = 0f;
		}

		// Token: 0x040053CB RID: 21451
		[Tooltip("The Vector3 to clamp the magnitude of")]
		public SharedVector3 vector3Variable;

		// Token: 0x040053CC RID: 21452
		[Tooltip("The max length of the magnitude")]
		public SharedFloat maxLength;

		// Token: 0x040053CD RID: 21453
		[RequiredField]
		[Tooltip("The clamp magnitude resut")]
		public SharedVector3 storeResult;
	}
}
