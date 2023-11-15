using System;
using Characters.Usables;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F5C RID: 3932
	public sealed class SummonLiquid : CharacterOperation
	{
		// Token: 0x06004C70 RID: 19568 RVA: 0x000E28FC File Offset: 0x000E0AFC
		public override void Run(Character owner)
		{
			if (this._liquidMaster == null)
			{
				this._liquid.Spawn(this._spawnPosition.position);
				return;
			}
			this._liquid.Spawn(this._spawnPosition.position, this._liquidMaster);
		}

		// Token: 0x04003C0D RID: 15373
		[SerializeField]
		private Liquid _liquid;

		// Token: 0x04003C0E RID: 15374
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04003C0F RID: 15375
		[SerializeField]
		[Space(5f)]
		[Header("Optional")]
		private LiquidMaster _liquidMaster;
	}
}
