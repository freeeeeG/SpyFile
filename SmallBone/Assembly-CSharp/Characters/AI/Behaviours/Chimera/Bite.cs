using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x0200136E RID: 4974
	public class Bite : Behaviour
	{
		// Token: 0x060061F4 RID: 25076 RVA: 0x0011E111 File Offset: 0x0011C311
		private void Awake()
		{
			this._readyOperations.Initialize();
			this._attackOperations.Initialize();
			this._endOperations.Initialize();
			this._terrainHitOperations.Initialize();
		}

		// Token: 0x060061F5 RID: 25077 RVA: 0x0011E13F File Offset: 0x0011C33F
		public void Ready(Character character)
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(character);
		}

		// Token: 0x060061F6 RID: 25078 RVA: 0x0011E15E File Offset: 0x0011C35E
		public void Hit(Character character)
		{
			this._terrainHitOperations.gameObject.SetActive(true);
			this._terrainHitOperations.Run(character);
		}

		// Token: 0x060061F7 RID: 25079 RVA: 0x0011E17D File Offset: 0x0011C37D
		public void End(Character character)
		{
			this._endOperations.gameObject.SetActive(true);
			this._endOperations.Run(character);
		}

		// Token: 0x060061F8 RID: 25080 RVA: 0x0011E19C File Offset: 0x0011C39C
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._attackOperations.gameObject.SetActive(true);
			this._attackOperations.Run(controller.character);
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x060061F9 RID: 25081 RVA: 0x0011E1B2 File Offset: 0x0011C3B2
		public bool CanUse(AIController controller)
		{
			return controller.FindClosestPlayerBody(this._trigger) != null;
		}

		// Token: 0x04004F03 RID: 20227
		[SerializeField]
		private Collider2D _trigger;

		// Token: 0x04004F04 RID: 20228
		[SerializeField]
		[Header("Ready")]
		private OperationInfos _readyOperations;

		// Token: 0x04004F05 RID: 20229
		[SerializeField]
		[Header("Attack")]
		private OperationInfos _attackOperations;

		// Token: 0x04004F06 RID: 20230
		[Header("Hit")]
		[SerializeField]
		private OperationInfos _terrainHitOperations;

		// Token: 0x04004F07 RID: 20231
		[Header("End")]
		[SerializeField]
		private OperationInfos _endOperations;
	}
}
