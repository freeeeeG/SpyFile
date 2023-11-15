using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001530 RID: 5424
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedRect : Conditional
	{
		// Token: 0x060068EB RID: 26859 RVA: 0x0012EABC File Offset: 0x0012CCBC
		public override TaskStatus OnUpdate()
		{
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068EC RID: 26860 RVA: 0x0012EAEC File Offset: 0x0012CCEC
		public override void OnReset()
		{
			this.variable = default(Rect);
			this.compareTo = default(Rect);
		}

		// Token: 0x040054D1 RID: 21713
		[Tooltip("The first variable to compare")]
		public SharedRect variable;

		// Token: 0x040054D2 RID: 21714
		[Tooltip("The variable to compare to")]
		public SharedRect compareTo;
	}
}
