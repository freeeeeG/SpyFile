using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001534 RID: 5428
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	[TaskCategory("Unity/SharedVariable")]
	public class CompareSharedVector2 : Conditional
	{
		// Token: 0x060068F7 RID: 26871 RVA: 0x0012ECB8 File Offset: 0x0012CEB8
		public override TaskStatus OnUpdate()
		{
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068F8 RID: 26872 RVA: 0x0012ECE8 File Offset: 0x0012CEE8
		public override void OnReset()
		{
			this.variable = Vector2.zero;
			this.compareTo = Vector2.zero;
		}

		// Token: 0x040054D9 RID: 21721
		[Tooltip("The first variable to compare")]
		public SharedVector2 variable;

		// Token: 0x040054DA RID: 21722
		[Tooltip("The variable to compare to")]
		public SharedVector2 compareTo;
	}
}
