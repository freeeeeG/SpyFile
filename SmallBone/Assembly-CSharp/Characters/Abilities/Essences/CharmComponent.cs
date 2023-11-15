using System;
using Characters.Gear.Quintessences;
using UnityEngine;

namespace Characters.Abilities.Essences
{
	// Token: 0x02000BDB RID: 3035
	public sealed class CharmComponent : AbilityComponent<Charm>
	{
		// Token: 0x06003E72 RID: 15986 RVA: 0x000B5913 File Offset: 0x000B3B13
		private void Awake()
		{
			this._ability.SetAttacker(this._essece.owner);
		}

		// Token: 0x04003033 RID: 12339
		[SerializeField]
		private Quintessence _essece;
	}
}
