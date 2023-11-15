using System;

namespace Characters.Operations.Customs.GlacialSkull
{
	// Token: 0x02001012 RID: 4114
	public class AddFreezeRemainHitStack : CharacterOperation
	{
		// Token: 0x06004F6C RID: 20332 RVA: 0x000EEF19 File Offset: 0x000ED119
		public override void Run(Character owner)
		{
			owner.status.freeze.AddRemainHitStack();
		}
	}
}
