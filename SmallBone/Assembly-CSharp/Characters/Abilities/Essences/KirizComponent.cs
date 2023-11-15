using System;

namespace Characters.Abilities.Essences
{
	// Token: 0x02000BE7 RID: 3047
	public class KirizComponent : AbilityComponent<Kiriz>
	{
		// Token: 0x06003E9B RID: 16027 RVA: 0x000B5F3C File Offset: 0x000B413C
		public void SetAttacker(Character attacker)
		{
			this._ability.SetAttacker(attacker);
		}
	}
}
