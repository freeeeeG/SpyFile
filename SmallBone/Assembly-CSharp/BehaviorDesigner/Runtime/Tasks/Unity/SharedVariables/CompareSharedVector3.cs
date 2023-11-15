using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001535 RID: 5429
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	[TaskCategory("Unity/SharedVariable")]
	public class CompareSharedVector3 : Conditional
	{
		// Token: 0x060068FA RID: 26874 RVA: 0x0012ED0C File Offset: 0x0012CF0C
		public override TaskStatus OnUpdate()
		{
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068FB RID: 26875 RVA: 0x0012ED3C File Offset: 0x0012CF3C
		public override void OnReset()
		{
			this.variable = Vector3.zero;
			this.compareTo = Vector3.zero;
		}

		// Token: 0x040054DB RID: 21723
		[Tooltip("The first variable to compare")]
		public SharedVector3 variable;

		// Token: 0x040054DC RID: 21724
		[Tooltip("The variable to compare to")]
		public SharedVector3 compareTo;
	}
}
