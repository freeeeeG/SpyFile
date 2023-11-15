using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200152D RID: 5421
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	[TaskCategory("Unity/SharedVariable")]
	public class CompareSharedObject : Conditional
	{
		// Token: 0x060068E2 RID: 26850 RVA: 0x0012E918 File Offset: 0x0012CB18
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

		// Token: 0x060068E3 RID: 26851 RVA: 0x0012E995 File Offset: 0x0012CB95
		public override void OnReset()
		{
			this.variable = null;
			this.compareTo = null;
		}

		// Token: 0x040054CB RID: 21707
		[Tooltip("The first variable to compare")]
		public SharedObject variable;

		// Token: 0x040054CC RID: 21708
		[Tooltip("The variable to compare to")]
		public SharedObject compareTo;
	}
}
