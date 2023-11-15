using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001546 RID: 5446
	[TaskDescription("Sets the SharedVector4 variable to the specified object. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SetSharedVector4 : Action
	{
		// Token: 0x0600692D RID: 26925 RVA: 0x0012F11B File Offset: 0x0012D31B
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x0600692E RID: 26926 RVA: 0x0012F134 File Offset: 0x0012D334
		public override void OnReset()
		{
			this.targetValue = Vector4.zero;
			this.targetVariable = Vector4.zero;
		}

		// Token: 0x040054FE RID: 21758
		[Tooltip("The value to set the SharedVector4 to")]
		public SharedVector4 targetValue;

		// Token: 0x040054FF RID: 21759
		[RequiredField]
		[Tooltip("The SharedVector4 to set")]
		public SharedVector4 targetVariable;
	}
}
