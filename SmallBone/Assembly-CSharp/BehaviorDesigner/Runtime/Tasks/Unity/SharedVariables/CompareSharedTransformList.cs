using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001533 RID: 5427
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedTransformList : Conditional
	{
		// Token: 0x060068F4 RID: 26868 RVA: 0x0012EBF8 File Offset: 0x0012CDF8
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
			if (this.variable.Value.Count != this.compareTo.Value.Count)
			{
				return TaskStatus.Failure;
			}
			for (int i = 0; i < this.variable.Value.Count; i++)
			{
				if (this.variable.Value[i] != this.compareTo.Value[i])
				{
					return TaskStatus.Failure;
				}
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068F5 RID: 26869 RVA: 0x0012ECA8 File Offset: 0x0012CEA8
		public override void OnReset()
		{
			this.variable = null;
			this.compareTo = null;
		}

		// Token: 0x040054D7 RID: 21719
		[Tooltip("The first variable to compare")]
		public SharedTransformList variable;

		// Token: 0x040054D8 RID: 21720
		[Tooltip("The variable to compare to")]
		public SharedTransformList compareTo;
	}
}
