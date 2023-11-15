using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using PhysicsUtils;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x02001385 RID: 4997
	public class WreckDestroy : Behaviour
	{
		// Token: 0x06006292 RID: 25234 RVA: 0x0011F3DD File Offset: 0x0011D5DD
		static WreckDestroy()
		{
			WreckDestroy._wreckOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x06006293 RID: 25235 RVA: 0x0011F404 File Offset: 0x0011D604
		private void Awake()
		{
			this._readyOperations.Initialize();
			this._attackOperations.Initialize();
			this._endOperations.Initialize();
			this._hitOperations.Initialize();
		}

		// Token: 0x06006294 RID: 25236 RVA: 0x0011F432 File Offset: 0x0011D632
		public void Ready(Character character)
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(character);
		}

		// Token: 0x06006295 RID: 25237 RVA: 0x0011F451 File Offset: 0x0011D651
		public void Attack(Character character)
		{
			this._attackOperations.gameObject.SetActive(true);
			this._attackOperations.Run(character);
		}

		// Token: 0x06006296 RID: 25238 RVA: 0x0011F470 File Offset: 0x0011D670
		public void End(Character character)
		{
			character.status.unstoppable.Detach(character);
			this._endOperations.gameObject.SetActive(true);
			this._endOperations.Run(character);
		}

		// Token: 0x06006297 RID: 25239 RVA: 0x0011F4A1 File Offset: 0x0011D6A1
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this.DestroyWreck(controller.character);
			this._hitOperations.gameObject.SetActive(true);
			this._hitOperations.Run(controller.character);
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06006298 RID: 25240 RVA: 0x0011F4B7 File Offset: 0x0011D6B7
		private List<ChimeraWreck> GetChimeraWrecks()
		{
			WreckDestroy._wreckOverlapper.contactFilter.SetLayerMask(1024);
			return WreckDestroy._wreckOverlapper.OverlapCollider(this._wreckFindRange).GetComponents<ChimeraWreck>(true);
		}

		// Token: 0x06006299 RID: 25241 RVA: 0x0011F4E8 File Offset: 0x0011D6E8
		public void DestroyWreck(Character character)
		{
			List<ChimeraWreck> chimeraWrecks = this.GetChimeraWrecks();
			for (int i = 0; i < chimeraWrecks.Count; i++)
			{
				chimeraWrecks[i].DestroyProp(character);
			}
		}

		// Token: 0x04004F7F RID: 20351
		[Header("Ready")]
		[SerializeField]
		private OperationInfos _readyOperations;

		// Token: 0x04004F80 RID: 20352
		[SerializeField]
		[Header("Attack")]
		private OperationInfos _attackOperations;

		// Token: 0x04004F81 RID: 20353
		[Header("End")]
		[SerializeField]
		private OperationInfos _endOperations;

		// Token: 0x04004F82 RID: 20354
		[Header("Hit")]
		[SerializeField]
		private OperationInfos _hitOperations;

		// Token: 0x04004F83 RID: 20355
		[SerializeField]
		private Collider2D _wreckFindRange;

		// Token: 0x04004F84 RID: 20356
		private static readonly NonAllocOverlapper _wreckOverlapper = new NonAllocOverlapper(100);
	}
}
