using System;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FCA RID: 4042
	public sealed class MultiDecoy : CharacterOperation
	{
		// Token: 0x06004E40 RID: 20032 RVA: 0x000EA2BC File Offset: 0x000E84BC
		public override void Run(Character owner)
		{
			int childCount = this._spawnPointContainer.childCount;
			this.Shuffle();
			for (int i = 0; i < childCount - 1; i++)
			{
				OperationInfos operationInfos = this._decoy.Spawn().operationInfos;
				operationInfos.transform.position = this._spawnPointContainer.GetChild(i).position;
				operationInfos.Run(owner);
			}
			owner.transform.position = this._spawnPointContainer.GetChild(childCount - 1).position;
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x000EA33C File Offset: 0x000E853C
		private void Shuffle()
		{
			foreach (object obj in this._spawnPointContainer)
			{
				((Transform)obj).SetSiblingIndex(UnityEngine.Random.Range(0, this._spawnPointContainer.childCount - 1));
			}
		}

		// Token: 0x04003E47 RID: 15943
		[SerializeField]
		private OperationRunner _decoy;

		// Token: 0x04003E48 RID: 15944
		[SerializeField]
		private Transform _spawnPointContainer;
	}
}
