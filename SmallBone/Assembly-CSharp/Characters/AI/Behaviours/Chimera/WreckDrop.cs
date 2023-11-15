using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x02001387 RID: 4999
	public class WreckDrop : Behaviour
	{
		// Token: 0x060062A1 RID: 25249 RVA: 0x0011F5A2 File Offset: 0x0011D7A2
		private void Awake()
		{
			this._outReadyOperations.Initialize();
			this._outJumpOperations.Initialize();
			this._inReadyOperations.Initialize();
			this._inWreckDropFireOperations.Initialize();
			this._inLandingOperations.Initialize();
		}

		// Token: 0x060062A2 RID: 25250 RVA: 0x0011F5DB File Offset: 0x0011D7DB
		public void OutReady(Character character)
		{
			this._outReadyOperations.gameObject.SetActive(true);
			this._outReadyOperations.Run(character);
			character.status.unstoppable.Attach(character);
		}

		// Token: 0x060062A3 RID: 25251 RVA: 0x0011F60B File Offset: 0x0011D80B
		public void OutJump(Character character)
		{
			this._outJumpOperations.gameObject.SetActive(true);
			this._outJumpOperations.Run(character);
		}

		// Token: 0x060062A4 RID: 25252 RVA: 0x0011F62A File Offset: 0x0011D82A
		public void InSign(Character character)
		{
			this._inSignOperations.gameObject.SetActive(true);
			this._inSignOperations.Run(character);
		}

		// Token: 0x060062A5 RID: 25253 RVA: 0x0011F649 File Offset: 0x0011D849
		public void InReady(Character character)
		{
			this._inReadyOperations.gameObject.SetActive(true);
			this._inReadyOperations.Run(character);
		}

		// Token: 0x060062A6 RID: 25254 RVA: 0x0011F668 File Offset: 0x0011D868
		public void InWreckDrop(Character character)
		{
			this._inWreckDropFireOperations.gameObject.SetActive(true);
			this._inWreckDropFireOperations.Run(character);
		}

		// Token: 0x060062A7 RID: 25255 RVA: 0x0011F687 File Offset: 0x0011D887
		public void InLanding(Character character)
		{
			this._inLandingOperations.gameObject.SetActive(true);
			this._inLandingOperations.Run(character);
		}

		// Token: 0x060062A8 RID: 25256 RVA: 0x0011F6A6 File Offset: 0x0011D8A6
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			base.StartCoroutine(this.CoolDown(controller.character.chronometer.animation));
			this._inWreckDropFireOperations.gameObject.SetActive(true);
			this._inWreckDropFireOperations.Run(controller.character);
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x060062A9 RID: 25257 RVA: 0x0011F6BC File Offset: 0x0011D8BC
		private IEnumerator CoolDown(Chronometer chronometer)
		{
			this._coolDown = false;
			yield return chronometer.WaitForSeconds(this._coolTime);
			this._coolDown = true;
			yield break;
		}

		// Token: 0x060062AA RID: 25258 RVA: 0x0011F6D2 File Offset: 0x0011D8D2
		public bool CanUse(Character character)
		{
			return this._coolDown && character.health.percent < (double)this._triggerHealthPercent;
		}

		// Token: 0x04004F89 RID: 20361
		[SerializeField]
		private float _coolTime;

		// Token: 0x04004F8A RID: 20362
		[SerializeField]
		[Range(0f, 1f)]
		private float _triggerHealthPercent;

		// Token: 0x04004F8B RID: 20363
		[SerializeField]
		[Header("Out")]
		private OperationInfos _outReadyOperations;

		// Token: 0x04004F8C RID: 20364
		[SerializeField]
		private OperationInfos _outJumpOperations;

		// Token: 0x04004F8D RID: 20365
		[SerializeField]
		[Header("In")]
		private OperationInfos _inSignOperations;

		// Token: 0x04004F8E RID: 20366
		[SerializeField]
		private OperationInfos _inReadyOperations;

		// Token: 0x04004F8F RID: 20367
		[SerializeField]
		private OperationInfos _inWreckDropFireOperations;

		// Token: 0x04004F90 RID: 20368
		[SerializeField]
		private OperationInfos _inLandingOperations;

		// Token: 0x04004F91 RID: 20369
		private bool _coolDown = true;
	}
}
