using System;
using Characters.Actions;
using Characters.Operations;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008B4 RID: 2228
	public sealed class Spoils : InscriptionInstance
	{
		// Token: 0x06002F71 RID: 12145 RVA: 0x0008E285 File Offset: 0x0008C485
		protected override void Initialize()
		{
			this._flagOperations.Initialize();
			this._flagOperations.gameObject.SetActive(false);
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x0008E2A3 File Offset: 0x0008C4A3
		public override void Attach()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.Disable;
			base.character.onStartAction += this.OnStartAction;
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x0008E2D7 File Offset: 0x0008C4D7
		private void Disable()
		{
			this._flagOperations.gameObject.SetActive(false);
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x0008E2EA File Offset: 0x0008C4EA
		public override void Detach()
		{
			this.Disable();
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.Disable;
			base.character.onStartAction -= this.OnStartAction;
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x0008E324 File Offset: 0x0008C524
		private void OnStartAction(Characters.Actions.Action action)
		{
			if (action.type != Characters.Actions.Action.Type.Swap)
			{
				return;
			}
			if (this.keyword.step < 1)
			{
				return;
			}
			this._flagOperations.gameObject.SetActive(false);
			this._flagOperations.gameObject.SetActive(true);
			Vector3 vector = base.transform.position;
			RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.down, 12f, Layers.groundMask);
			if (hit)
			{
				vector = hit.point;
			}
			this._flagOperations.transform.position = vector;
			this._flagOperations.Run(base.character);
		}

		// Token: 0x04002725 RID: 10021
		[Header("2세트 효과")]
		[SerializeField]
		private OperationInfos _flagOperations;
	}
}
