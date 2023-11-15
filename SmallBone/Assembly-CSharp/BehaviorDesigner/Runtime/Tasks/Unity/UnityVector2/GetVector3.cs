using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014F8 RID: 5368
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Stores the Vector3 value of the Vector2.")]
	public class GetVector3 : Action
	{
		// Token: 0x06006827 RID: 26663 RVA: 0x0012CEB3 File Offset: 0x0012B0B3
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector3Variable.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x0012CED1 File Offset: 0x0012B0D1
		public override void OnReset()
		{
			this.vector3Variable = Vector2.zero;
			this.storeResult = Vector3.zero;
		}

		// Token: 0x04005411 RID: 21521
		[Tooltip("The Vector2 to get the Vector3 value of")]
		public SharedVector2 vector3Variable;

		// Token: 0x04005412 RID: 21522
		[Tooltip("The Vector3 value")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
