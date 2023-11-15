using System;

namespace Database
{
	// Token: 0x02000D21 RID: 3361
	public class StateMachineCategories : ResourceSet<StateMachine.Category>
	{
		// Token: 0x06006A15 RID: 27157 RVA: 0x00294C48 File Offset: 0x00292E48
		public StateMachineCategories()
		{
			this.Ai = base.Add(new StateMachine.Category("Ai"));
			this.Monitor = base.Add(new StateMachine.Category("Monitor"));
			this.Chore = base.Add(new StateMachine.Category("Chore"));
			this.Misc = base.Add(new StateMachine.Category("Misc"));
		}

		// Token: 0x04004D1F RID: 19743
		public StateMachine.Category Ai;

		// Token: 0x04004D20 RID: 19744
		public StateMachine.Category Monitor;

		// Token: 0x04004D21 RID: 19745
		public StateMachine.Category Chore;

		// Token: 0x04004D22 RID: 19746
		public StateMachine.Category Misc;
	}
}
