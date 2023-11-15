using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001545 RID: 5445
	[TaskDescription("Sets the SharedVector3 variable to the specified object. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SetSharedVector3 : Action
	{
		// Token: 0x0600692A RID: 26922 RVA: 0x0012F0E0 File Offset: 0x0012D2E0
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x0600692B RID: 26923 RVA: 0x0012F0F9 File Offset: 0x0012D2F9
		public override void OnReset()
		{
			this.targetValue = Vector3.zero;
			this.targetVariable = Vector3.zero;
		}

		// Token: 0x040054FC RID: 21756
		[Tooltip("The value to set the SharedVector3 to")]
		public SharedVector3 targetValue;

		// Token: 0x040054FD RID: 21757
		[RequiredField]
		[Tooltip("The SharedVector3 to set")]
		public SharedVector3 targetVariable;
	}
}
