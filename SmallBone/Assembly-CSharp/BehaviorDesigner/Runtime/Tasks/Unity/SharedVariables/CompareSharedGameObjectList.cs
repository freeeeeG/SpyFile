using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200152B RID: 5419
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	[TaskCategory("Unity/SharedVariable")]
	public class CompareSharedGameObjectList : Conditional
	{
		// Token: 0x060068DC RID: 26844 RVA: 0x0012E80C File Offset: 0x0012CA0C
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

		// Token: 0x060068DD RID: 26845 RVA: 0x0012E8BC File Offset: 0x0012CABC
		public override void OnReset()
		{
			this.variable = null;
			this.compareTo = null;
		}

		// Token: 0x040054C7 RID: 21703
		[Tooltip("The first variable to compare")]
		public SharedGameObjectList variable;

		// Token: 0x040054C8 RID: 21704
		[Tooltip("The variable to compare to")]
		public SharedGameObjectList compareTo;
	}
}
