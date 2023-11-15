using System;

namespace Characters.Operations.Health
{
	// Token: 0x02000E88 RID: 3720
	public sealed class Kill : CharacterOperation
	{
		// Token: 0x060049AA RID: 18858 RVA: 0x000D7314 File Offset: 0x000D5514
		public override void Run(Character owner)
		{
			this.Run(owner, owner);
		}

		// Token: 0x060049AB RID: 18859 RVA: 0x000D731E File Offset: 0x000D551E
		public override void Run(Character owner, Character target)
		{
			target.health.Kill();
		}
	}
}
