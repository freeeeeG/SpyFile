using System;
using Characters.Abilities;

namespace Characters.Operations
{
	// Token: 0x02000DCE RID: 3534
	public class AttachCurseOfLight : TargetedCharacterOperation
	{
		// Token: 0x060046FF RID: 18175 RVA: 0x000CE115 File Offset: 0x000CC315
		public override void Run(Character owner, Character target)
		{
			if (target == null || !target.liveAndActive)
			{
				return;
			}
			if (target.playerComponents == null)
			{
				return;
			}
			target.playerComponents.savableAbilityManager.Apply(SavableAbilityManager.Name.Curse);
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}
	}
}
