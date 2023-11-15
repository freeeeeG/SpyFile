using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x0200137C RID: 4988
	public class VenomBreath : Behaviour
	{
		// Token: 0x06006258 RID: 25176 RVA: 0x0011EC99 File Offset: 0x0011CE99
		private void Awake()
		{
			this._readyOperations.Initialize();
			this._fireOperations.Initialize();
			this._endOperations.Initialize();
		}

		// Token: 0x06006259 RID: 25177 RVA: 0x0011ECBC File Offset: 0x0011CEBC
		public void Ready(Character character)
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(character);
		}

		// Token: 0x0600625A RID: 25178 RVA: 0x0011ECDB File Offset: 0x0011CEDB
		public void End(Character character)
		{
			this._endOperations.gameObject.SetActive(true);
			this._endOperations.Run(character);
		}

		// Token: 0x0600625B RID: 25179 RVA: 0x0011ECFA File Offset: 0x0011CEFA
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._fireOperations.gameObject.SetActive(true);
			this._fireOperations.Run(controller.character);
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x04004F50 RID: 20304
		[Header("Ready")]
		[SerializeField]
		private OperationInfos _readyOperations;

		// Token: 0x04004F51 RID: 20305
		[Header("Fire")]
		[SerializeField]
		private OperationInfos _fireOperations;

		// Token: 0x04004F52 RID: 20306
		[Header("End")]
		[SerializeField]
		private OperationInfos _endOperations;
	}
}
