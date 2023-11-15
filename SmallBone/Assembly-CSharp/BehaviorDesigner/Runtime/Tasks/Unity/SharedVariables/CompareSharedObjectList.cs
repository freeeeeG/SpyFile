using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200152E RID: 5422
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedObjectList : Conditional
	{
		// Token: 0x060068E5 RID: 26853 RVA: 0x0012E9A8 File Offset: 0x0012CBA8
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

		// Token: 0x060068E6 RID: 26854 RVA: 0x0012EA58 File Offset: 0x0012CC58
		public override void OnReset()
		{
			this.variable = null;
			this.compareTo = null;
		}

		// Token: 0x040054CD RID: 21709
		[Tooltip("The first variable to compare")]
		public SharedObjectList variable;

		// Token: 0x040054CE RID: 21710
		[Tooltip("The variable to compare to")]
		public SharedObjectList compareTo;
	}
}
