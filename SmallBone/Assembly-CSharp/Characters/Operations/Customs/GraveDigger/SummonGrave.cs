using System;
using Characters.Abilities.Customs;
using UnityEngine;

namespace Characters.Operations.Customs.GraveDigger
{
	// Token: 0x0200100F RID: 4111
	public sealed class SummonGrave : CharacterOperation
	{
		// Token: 0x06004F61 RID: 20321 RVA: 0x000EEC58 File Offset: 0x000ECE58
		public override void Run(Character owner)
		{
			foreach (Transform transform in this._summonPoints)
			{
				this._passive.SpawnGrave(transform.position);
			}
		}

		// Token: 0x04003F92 RID: 16274
		[SerializeField]
		private GraveDiggerPassiveComponent _passive;

		// Token: 0x04003F93 RID: 16275
		[Space]
		[SerializeField]
		private Transform[] _summonPoints;
	}
}
