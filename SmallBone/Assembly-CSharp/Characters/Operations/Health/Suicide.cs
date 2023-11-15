using System;
using UnityEngine;

namespace Characters.Operations.Health
{
	// Token: 0x02000E8F RID: 3727
	public class Suicide : CharacterOperation
	{
		// Token: 0x060049BE RID: 18878 RVA: 0x000D7672 File Offset: 0x000D5872
		public override void Run(Character owner)
		{
			if (this._force)
			{
				owner.health.Kill();
				return;
			}
			owner.health.TryKill();
		}

		// Token: 0x040038F9 RID: 14585
		[SerializeField]
		private bool _force = true;
	}
}
