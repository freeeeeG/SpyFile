using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200152C RID: 5420
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	[TaskCategory("Unity/SharedVariable")]
	public class CompareSharedInt : Conditional
	{
		// Token: 0x060068DF RID: 26847 RVA: 0x0012E8CC File Offset: 0x0012CACC
		public override TaskStatus OnUpdate()
		{
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068E0 RID: 26848 RVA: 0x0012E8FC File Offset: 0x0012CAFC
		public override void OnReset()
		{
			this.variable = 0;
			this.compareTo = 0;
		}

		// Token: 0x040054C9 RID: 21705
		[Tooltip("The first variable to compare")]
		public SharedInt variable;

		// Token: 0x040054CA RID: 21706
		[Tooltip("The variable to compare to")]
		public SharedInt compareTo;
	}
}
