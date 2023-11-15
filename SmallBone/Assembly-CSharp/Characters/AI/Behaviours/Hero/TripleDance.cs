using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013A4 RID: 5028
	public class TripleDance : Behaviour
	{
		// Token: 0x0600632D RID: 25389 RVA: 0x00120988 File Offset: 0x0011EB88
		private void Awake()
		{
			this._throwGhost.Initialize();
			this._slashGhost.Initialize();
		}

		// Token: 0x0600632E RID: 25390 RVA: 0x001209A0 File Offset: 0x0011EBA0
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._jumpAndThrow.CRun(controller);
			this._throwGhost.gameObject.SetActive(true);
			this._throwGhost.Run(controller.character);
			yield return this._lightMove.CRun(controller);
			yield return this._slash.CRun(controller);
			this._slashGhost.gameObject.SetActive(true);
			this._slashGhost.Run(controller.character);
			yield return this._lightMove.CRun(controller);
			yield return this._slash.CRun(controller);
			this._slashGhost.gameObject.SetActive(true);
			this._slashGhost.Run(controller.character);
			yield return this._lightMove.CRun(controller);
			yield return this._strike.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004FFB RID: 20475
		[SerializeField]
		private Behaviour _jumpAndThrow;

		// Token: 0x04004FFC RID: 20476
		[SerializeField]
		private TripleDanceMove _lightMove;

		// Token: 0x04004FFD RID: 20477
		[SerializeField]
		private Behaviour _slash;

		// Token: 0x04004FFE RID: 20478
		[SerializeField]
		private Behaviour _strike;

		// Token: 0x04004FFF RID: 20479
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfos))]
		private OperationInfos _throwGhost;

		// Token: 0x04005000 RID: 20480
		[UnityEditor.Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _slashGhost;
	}
}
