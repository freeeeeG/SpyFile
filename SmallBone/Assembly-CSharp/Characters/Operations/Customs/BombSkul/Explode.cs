using System;
using Characters.Abilities.Customs;
using UnityEngine;

namespace Characters.Operations.Customs.BombSkul
{
	// Token: 0x02001023 RID: 4131
	public class Explode : Operation
	{
		// Token: 0x06004FA7 RID: 20391 RVA: 0x000F0133 File Offset: 0x000EE333
		public override void Run()
		{
			this._bombSkulPassive.Explode();
		}

		// Token: 0x04003FF8 RID: 16376
		[SerializeField]
		private BombSkulPassiveComponent _bombSkulPassive;
	}
}
