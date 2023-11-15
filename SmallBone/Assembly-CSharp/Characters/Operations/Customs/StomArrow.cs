using System;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FCD RID: 4045
	public sealed class StomArrow : CharacterOperation
	{
		// Token: 0x06004E4E RID: 20046 RVA: 0x000EA67C File Offset: 0x000E887C
		private void Awake()
		{
			this._numbers = new int[this._spawnPointContainer.childCount];
			for (int i = 0; i < this._spawnPointContainer.childCount; i++)
			{
				this._numbers[i] = i;
			}
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x000EA6C0 File Offset: 0x000E88C0
		public override void Run(Character owner)
		{
			this._numbers.Shuffle<int>();
			for (int i = 0; i < this._spawnPointContainer.childCount - this._emptyCount; i++)
			{
				int index = this._numbers[i];
				Vector3 position = this._spawnPointContainer.GetChild(index).position;
				OperationInfos operationInfos = this._operationRunner.Spawn().operationInfos;
				operationInfos.transform.position = position;
				operationInfos.Run(owner);
			}
		}

		// Token: 0x04003E56 RID: 15958
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		private OperationRunner _operationRunner;

		// Token: 0x04003E57 RID: 15959
		[SerializeField]
		private Transform _spawnPointContainer;

		// Token: 0x04003E58 RID: 15960
		[SerializeField]
		private int _emptyCount = 2;

		// Token: 0x04003E59 RID: 15961
		private int[] _numbers;
	}
}
