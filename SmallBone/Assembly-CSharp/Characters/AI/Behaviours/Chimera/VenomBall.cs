using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x0200137A RID: 4986
	public class VenomBall : Behaviour
	{
		// Token: 0x0600624E RID: 25166 RVA: 0x0011EBE1 File Offset: 0x0011CDE1
		private void Awake()
		{
			this._fireOperation.Initialize();
		}

		// Token: 0x0600624F RID: 25167 RVA: 0x0011EBEE File Offset: 0x0011CDEE
		public void Ready(Character character)
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(character);
		}

		// Token: 0x06006250 RID: 25168 RVA: 0x0011EC0D File Offset: 0x0011CE0D
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._fireOperation.gameObject.SetActive(true);
			this._fireOperation.Run(controller.character);
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x04004F4A RID: 20298
		[SerializeField]
		[Header("Ready")]
		private OperationInfos _readyOperations;

		// Token: 0x04004F4B RID: 20299
		[Header("Fire")]
		[SerializeField]
		private OperationInfos _fireOperation;
	}
}
