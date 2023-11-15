using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011E2 RID: 4578
	public sealed class Barrier : MonoBehaviour
	{
		// Token: 0x060059DF RID: 23007 RVA: 0x0010B25E File Offset: 0x0010945E
		public void Spawn()
		{
			if (this._onSpawnOperations == null)
			{
				return;
			}
			this._onSpawnOperations.gameObject.SetActive(true);
			this._onSpawnOperations.Run(this._owner);
		}

		// Token: 0x060059E0 RID: 23008 RVA: 0x0010B291 File Offset: 0x00109491
		public void Crack()
		{
			if (this._onCrackOperations == null)
			{
				return;
			}
			this._onCrackOperations.gameObject.SetActive(true);
			this._onCrackOperations.Run(this._owner);
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x0010B2C4 File Offset: 0x001094C4
		public void Despawn()
		{
			if (this._onDespawnOperations == null)
			{
				return;
			}
			this._onDespawnOperations.gameObject.SetActive(true);
			this._onDespawnOperations.Run(this._owner);
		}

		// Token: 0x04004893 RID: 18579
		[SerializeField]
		private Character _owner;

		// Token: 0x04004894 RID: 18580
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onSpawnOperations;

		// Token: 0x04004895 RID: 18581
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onCrackOperations;

		// Token: 0x04004896 RID: 18582
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onDespawnOperations;
	}
}
