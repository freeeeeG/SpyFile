using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200152A RID: 5418
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedGameObject : Conditional
	{
		// Token: 0x060068D9 RID: 26841 RVA: 0x0012E77C File Offset: 0x0012C97C
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

		// Token: 0x060068DA RID: 26842 RVA: 0x0012E7F9 File Offset: 0x0012C9F9
		public override void OnReset()
		{
			this.variable = null;
			this.compareTo = null;
		}

		// Token: 0x040054C5 RID: 21701
		[Tooltip("The first variable to compare")]
		public SharedGameObject variable;

		// Token: 0x040054C6 RID: 21702
		[Tooltip("The variable to compare to")]
		public SharedGameObject compareTo;
	}
}
