using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001529 RID: 5417
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedFloat : Conditional
	{
		// Token: 0x060068D6 RID: 26838 RVA: 0x0012E728 File Offset: 0x0012C928
		public override TaskStatus OnUpdate()
		{
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068D7 RID: 26839 RVA: 0x0012E758 File Offset: 0x0012C958
		public override void OnReset()
		{
			this.variable = 0f;
			this.compareTo = 0f;
		}

		// Token: 0x040054C3 RID: 21699
		[Tooltip("The first variable to compare")]
		public SharedFloat variable;

		// Token: 0x040054C4 RID: 21700
		[Tooltip("The variable to compare to")]
		public SharedFloat compareTo;
	}
}
