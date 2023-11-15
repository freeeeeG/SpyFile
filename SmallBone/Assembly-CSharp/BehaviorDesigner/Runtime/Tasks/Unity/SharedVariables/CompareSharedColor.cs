using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001528 RID: 5416
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedColor : Conditional
	{
		// Token: 0x060068D3 RID: 26835 RVA: 0x0012E6D4 File Offset: 0x0012C8D4
		public override TaskStatus OnUpdate()
		{
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068D4 RID: 26836 RVA: 0x0012E704 File Offset: 0x0012C904
		public override void OnReset()
		{
			this.variable = Color.black;
			this.compareTo = Color.black;
		}

		// Token: 0x040054C1 RID: 21697
		[Tooltip("The first variable to compare")]
		public SharedColor variable;

		// Token: 0x040054C2 RID: 21698
		[Tooltip("The variable to compare to")]
		public SharedColor compareTo;
	}
}
