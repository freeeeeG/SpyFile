using System;
using Characters.Player;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F27 RID: 3879
	public class DespawnMinions : CharacterOperation
	{
		// Token: 0x06004B9C RID: 19356 RVA: 0x000DE978 File Offset: 0x000DCB78
		public override void Run(Character owner)
		{
			MinionLeader minionLeader = owner.playerComponents.minionLeader;
			if (minionLeader == null)
			{
				return;
			}
			minionLeader.DespawnAll(this._keyPrefab);
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x000DE9A1 File Offset: 0x000DCBA1
		private void Despawn(Minion minion)
		{
			minion.Despawn();
		}

		// Token: 0x04003ADF RID: 15071
		[SerializeField]
		private Minion _keyPrefab;
	}
}
