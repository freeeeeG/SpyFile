using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D70 RID: 3440
	public class MummyGunDropPassiveComponent : AbilityComponent<MummyGunDropPassive>
	{
		// Token: 0x06004558 RID: 17752 RVA: 0x000C92FD File Offset: 0x000C74FD
		public void SupplyGunBySwap()
		{
			this._ability.SupplyGunBySwap();
		}
	}
}
