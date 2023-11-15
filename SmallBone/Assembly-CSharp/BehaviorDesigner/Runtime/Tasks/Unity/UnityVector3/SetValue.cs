using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014EF RID: 5359
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Sets the value of the Vector3.")]
	public class SetValue : Action
	{
		// Token: 0x0600680C RID: 26636 RVA: 0x0012CB75 File Offset: 0x0012AD75
		public override TaskStatus OnUpdate()
		{
			this.vector3Variable.Value = this.vector3Value.Value;
			return TaskStatus.Success;
		}

		// Token: 0x0600680D RID: 26637 RVA: 0x0012CB8E File Offset: 0x0012AD8E
		public override void OnReset()
		{
			this.vector3Value = Vector3.zero;
			this.vector3Variable = Vector3.zero;
		}

		// Token: 0x040053FB RID: 21499
		[Tooltip("The Vector3 to get the values of")]
		public SharedVector3 vector3Value;

		// Token: 0x040053FC RID: 21500
		[Tooltip("The Vector3 to set the values of")]
		public SharedVector3 vector3Variable;
	}
}
