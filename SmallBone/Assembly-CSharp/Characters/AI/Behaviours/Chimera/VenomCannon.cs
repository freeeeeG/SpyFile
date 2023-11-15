using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x0200137E RID: 4990
	public class VenomCannon : Behaviour
	{
		// Token: 0x06006263 RID: 25187 RVA: 0x0011ED88 File Offset: 0x0011CF88
		private void Awake()
		{
			for (int i = 0; i < this._fireOperations.Length; i++)
			{
				this._fireOperations[i].Initialize();
			}
		}

		// Token: 0x06006264 RID: 25188 RVA: 0x0011EDB5 File Offset: 0x0011CFB5
		public void Ready(Character character)
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(character);
		}

		// Token: 0x06006265 RID: 25189 RVA: 0x0011EDD4 File Offset: 0x0011CFD4
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._fireOperations[this._order].gameObject.SetActive(true);
			this._fireOperations[this._order].Run(controller.character);
			this._order++;
			this._order %= 4;
			if (this._order == 3)
			{
				this._fireOperations[this._order].gameObject.SetActive(true);
				this._fireOperations[this._order].Run(controller.character);
				this._order++;
				this._order %= 4;
			}
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x04004F57 RID: 20311
		[Header("Ready")]
		[SerializeField]
		private OperationInfos _readyOperations;

		// Token: 0x04004F58 RID: 20312
		[SerializeField]
		[Header("Fire")]
		private OperationInfos[] _fireOperations;

		// Token: 0x04004F59 RID: 20313
		private const int _maxOrder = 4;

		// Token: 0x04004F5A RID: 20314
		private int _order;
	}
}
