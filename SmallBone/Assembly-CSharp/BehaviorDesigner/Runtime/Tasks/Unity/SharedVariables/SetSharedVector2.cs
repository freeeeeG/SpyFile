using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001544 RID: 5444
	[TaskDescription("Sets the SharedVector2 variable to the specified object. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SetSharedVector2 : Action
	{
		// Token: 0x06006927 RID: 26919 RVA: 0x0012F0A5 File Offset: 0x0012D2A5
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006928 RID: 26920 RVA: 0x0012F0BE File Offset: 0x0012D2BE
		public override void OnReset()
		{
			this.targetValue = Vector2.zero;
			this.targetVariable = Vector2.zero;
		}

		// Token: 0x040054FA RID: 21754
		[Tooltip("The value to set the SharedVector2 to")]
		public SharedVector2 targetValue;

		// Token: 0x040054FB RID: 21755
		[Tooltip("The SharedVector2 to set")]
		[RequiredField]
		public SharedVector2 targetVariable;
	}
}
