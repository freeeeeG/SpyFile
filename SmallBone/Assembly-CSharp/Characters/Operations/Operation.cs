using System;

namespace Characters.Operations
{
	// Token: 0x02000E26 RID: 3622
	public abstract class Operation : CharacterOperation
	{
		// Token: 0x0600483F RID: 18495
		public abstract void Run();

		// Token: 0x06004840 RID: 18496 RVA: 0x000D22D1 File Offset: 0x000D04D1
		public override void Run(Character owner)
		{
			this.Run();
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x000D22D1 File Offset: 0x000D04D1
		public override void Run(Character owner, Character target)
		{
			this.Run();
		}
	}
}
