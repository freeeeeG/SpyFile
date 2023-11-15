using System;

namespace Characters.Operations
{
	// Token: 0x02000E19 RID: 3609
	public class EndWeaponPolymorph : CharacterOperation
	{
		// Token: 0x06004809 RID: 18441 RVA: 0x000D1C30 File Offset: 0x000CFE30
		public override void Run(Character target)
		{
			target.playerComponents.inventory.weapon.Unpolymorph();
		}
	}
}
