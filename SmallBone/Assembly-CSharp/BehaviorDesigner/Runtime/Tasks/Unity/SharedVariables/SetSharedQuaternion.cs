using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200153F RID: 5439
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Sets the SharedQuaternion variable to the specified object. Returns Success.")]
	public class SetSharedQuaternion : Action
	{
		// Token: 0x06006918 RID: 26904 RVA: 0x0012EF72 File Offset: 0x0012D172
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006919 RID: 26905 RVA: 0x0012EF8B File Offset: 0x0012D18B
		public override void OnReset()
		{
			this.targetValue = Quaternion.identity;
			this.targetVariable = Quaternion.identity;
		}

		// Token: 0x040054F0 RID: 21744
		[Tooltip("The value to set the SharedQuaternion to")]
		public SharedQuaternion targetValue;

		// Token: 0x040054F1 RID: 21745
		[Tooltip("The SharedQuaternion to set")]
		[RequiredField]
		public SharedQuaternion targetVariable;
	}
}
