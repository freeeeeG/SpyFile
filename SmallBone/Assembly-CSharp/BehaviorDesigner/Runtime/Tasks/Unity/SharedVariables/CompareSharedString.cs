using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001531 RID: 5425
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	[TaskCategory("Unity/SharedVariable")]
	public class CompareSharedString : Conditional
	{
		// Token: 0x060068EE RID: 26862 RVA: 0x0012EB21 File Offset: 0x0012CD21
		public override TaskStatus OnUpdate()
		{
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068EF RID: 26863 RVA: 0x0012EB43 File Offset: 0x0012CD43
		public override void OnReset()
		{
			this.variable = "";
			this.compareTo = "";
		}

		// Token: 0x040054D3 RID: 21715
		[Tooltip("The first variable to compare")]
		public SharedString variable;

		// Token: 0x040054D4 RID: 21716
		[Tooltip("The variable to compare to")]
		public SharedString compareTo;
	}
}
