using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001540 RID: 5440
	[TaskDescription("Sets the SharedRect variable to the specified object. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SetSharedRect : Action
	{
		// Token: 0x0600691B RID: 26907 RVA: 0x0012EFAD File Offset: 0x0012D1AD
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x0600691C RID: 26908 RVA: 0x0012EFC8 File Offset: 0x0012D1C8
		public override void OnReset()
		{
			this.targetValue = default(Rect);
			this.targetVariable = default(Rect);
		}

		// Token: 0x040054F2 RID: 21746
		[Tooltip("The value to set the SharedRect to")]
		public SharedRect targetValue;

		// Token: 0x040054F3 RID: 21747
		[Tooltip("The SharedRect to set")]
		[RequiredField]
		public SharedRect targetVariable;
	}
}
