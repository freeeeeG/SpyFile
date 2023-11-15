using System;
using Characters.Abilities.Weapons.Wizard;

namespace Characters.Operations.Customs.Skeleton_Mage
{
	// Token: 0x02001005 RID: 4101
	public sealed class FillUpMana : CharacterOperation
	{
		// Token: 0x06004F3F RID: 20287 RVA: 0x000EE5A4 File Offset: 0x000EC7A4
		public override void Run(Character owner)
		{
			WizardPassive.Instance instanceByInstanceType = owner.ability.GetInstanceByInstanceType<WizardPassive.Instance>();
			if (instanceByInstanceType == null)
			{
				return;
			}
			instanceByInstanceType.FillUp();
		}
	}
}
