using System;
using Characters.Abilities.Customs;
using UnityEngine;

namespace Characters.Operations.Customs.BombSkul
{
	// Token: 0x02001022 RID: 4130
	public class AddDamageStack : Operation
	{
		// Token: 0x06004FA5 RID: 20389 RVA: 0x000F0120 File Offset: 0x000EE320
		public override void Run()
		{
			this._bombSkulPassive.AddDamageStack(this._damageStacks);
		}

		// Token: 0x04003FF6 RID: 16374
		[SerializeField]
		private BombSkulPassiveComponent _bombSkulPassive;

		// Token: 0x04003FF7 RID: 16375
		[SerializeField]
		private int _damageStacks;
	}
}
