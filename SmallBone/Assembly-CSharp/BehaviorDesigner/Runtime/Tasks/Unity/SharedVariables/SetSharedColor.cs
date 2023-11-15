using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001538 RID: 5432
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Sets the SharedColor variable to the specified object. Returns Success.")]
	public class SetSharedColor : Action
	{
		// Token: 0x06006903 RID: 26883 RVA: 0x0012EDE5 File Offset: 0x0012CFE5
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006904 RID: 26884 RVA: 0x0012EDFE File Offset: 0x0012CFFE
		public override void OnReset()
		{
			this.targetValue = Color.black;
			this.targetVariable = Color.black;
		}

		// Token: 0x040054E1 RID: 21729
		[Tooltip("The value to set the SharedColor to")]
		public SharedColor targetValue;

		// Token: 0x040054E2 RID: 21730
		[Tooltip("The SharedColor to set")]
		[RequiredField]
		public SharedColor targetVariable;
	}
}
