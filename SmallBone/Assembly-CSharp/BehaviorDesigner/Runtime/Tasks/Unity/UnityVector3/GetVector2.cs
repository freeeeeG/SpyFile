using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014E6 RID: 5350
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Stores the Vector2 value of the Vector3.")]
	public class GetVector2 : Action
	{
		// Token: 0x060067F4 RID: 26612 RVA: 0x0012C75F File Offset: 0x0012A95F
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector3Variable.Value;
			return TaskStatus.Success;
		}

		// Token: 0x060067F5 RID: 26613 RVA: 0x0012C77D File Offset: 0x0012A97D
		public override void OnReset()
		{
			this.vector3Variable = Vector3.zero;
			this.storeResult = Vector2.zero;
		}

		// Token: 0x040053DB RID: 21467
		[Tooltip("The Vector3 to get the Vector2 value of")]
		public SharedVector3 vector3Variable;

		// Token: 0x040053DC RID: 21468
		[RequiredField]
		[Tooltip("The Vector2 value")]
		public SharedVector2 storeResult;
	}
}
