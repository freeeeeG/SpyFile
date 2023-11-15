using System;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Operations.Customs.GraveDigger
{
	// Token: 0x02001011 RID: 4113
	public sealed class SummonMinionFromGraves : CharacterOperation
	{
		// Token: 0x06004F6A RID: 20330 RVA: 0x000EEF00 File Offset: 0x000ED100
		public override void Run(Character owner)
		{
			this._graveContainer.SummonMinionFromGraves(this._minionPrefab, this._maxCount);
		}

		// Token: 0x04003F9C RID: 16284
		[SerializeField]
		private GraveDiggerGraveContainer _graveContainer;

		// Token: 0x04003F9D RID: 16285
		[SerializeField]
		[Space]
		private Minion _minionPrefab;

		// Token: 0x04003F9E RID: 16286
		[SerializeField]
		private int _maxCount;
	}
}
