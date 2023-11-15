using System;
using Characters.Gear.Weapons.Rockstar;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FEF RID: 4079
	public class SummonRockstarAmp : CharacterOperation
	{
		// Token: 0x06004ECD RID: 20173 RVA: 0x000ECA81 File Offset: 0x000EAC81
		public override void Run(Character owner)
		{
			this._rockstarAmp.InstantiateAmp(this._spawnPosition.position, this._flipX);
		}

		// Token: 0x04003EEA RID: 16106
		[SerializeField]
		private Amp _rockstarAmp;

		// Token: 0x04003EEB RID: 16107
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04003EEC RID: 16108
		[SerializeField]
		private bool _flipX;
	}
}
