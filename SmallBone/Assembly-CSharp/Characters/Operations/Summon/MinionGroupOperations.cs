using System;
using Characters.Player;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F28 RID: 3880
	public class MinionGroupOperations : CharacterOperation
	{
		// Token: 0x06004B9F RID: 19359 RVA: 0x000DE9AC File Offset: 0x000DCBAC
		public override void Run(Character owner)
		{
			MinionLeader minionLeader = owner.playerComponents.minionLeader;
			if (minionLeader == null)
			{
				return;
			}
			minionLeader.Commands(this._keyPrefab, new MinionLeader.MinionCommands(this.CommandTo));
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x000DE9E1 File Offset: 0x000DCBE1
		private void CommandTo(Minion minion)
		{
			minion.StartCoroutine(this._operations.CRun(minion.character));
		}

		// Token: 0x04003AE0 RID: 15072
		[SerializeField]
		private Minion _keyPrefab;

		// Token: 0x04003AE1 RID: 15073
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;
	}
}
