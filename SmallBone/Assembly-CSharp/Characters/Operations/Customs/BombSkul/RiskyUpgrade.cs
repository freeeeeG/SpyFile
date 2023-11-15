using System;
using Characters.Abilities.Customs;
using UnityEngine;

namespace Characters.Operations.Customs.BombSkul
{
	// Token: 0x02001024 RID: 4132
	public class RiskyUpgrade : Operation
	{
		// Token: 0x06004FA9 RID: 20393 RVA: 0x000F0140 File Offset: 0x000EE340
		public override void Run()
		{
			this._bombSkulPassive.RiskyUpgrade();
		}

		// Token: 0x04003FF9 RID: 16377
		[SerializeField]
		private BombSkulPassiveComponent _bombSkulPassive;
	}
}
