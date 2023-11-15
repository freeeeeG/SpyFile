using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001532 RID: 5426
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedTransform : Conditional
	{
		// Token: 0x060068F1 RID: 26865 RVA: 0x0012EB68 File Offset: 0x0012CD68
		public override TaskStatus OnUpdate()
		{
			if (this.variable.Value == null && this.compareTo.Value != null)
			{
				return TaskStatus.Failure;
			}
			if (this.variable.Value == null && this.compareTo.Value == null)
			{
				return TaskStatus.Success;
			}
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068F2 RID: 26866 RVA: 0x0012EBE5 File Offset: 0x0012CDE5
		public override void OnReset()
		{
			this.variable = null;
			this.compareTo = null;
		}

		// Token: 0x040054D5 RID: 21717
		[Tooltip("The first variable to compare")]
		public SharedTransform variable;

		// Token: 0x040054D6 RID: 21718
		[Tooltip("The variable to compare to")]
		public SharedTransform compareTo;
	}
}
