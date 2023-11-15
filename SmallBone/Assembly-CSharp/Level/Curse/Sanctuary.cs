using System;
using Characters;
using Characters.Abilities;

namespace Level.Curse
{
	// Token: 0x02000606 RID: 1542
	public class Sanctuary : InteractiveObject, ISanctuary
	{
		// Token: 0x06001EEC RID: 7916 RVA: 0x0005DCCB File Offset: 0x0005BECB
		public override void InteractWith(Character character)
		{
			this.RemoveCurse(character);
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x0005DCD4 File Offset: 0x0005BED4
		public void RemoveCurse(Character character)
		{
			character.playerComponents.savableAbilityManager.Remove(SavableAbilityManager.Name.Curse);
		}
	}
}
