using System;
using System.Collections;
using Characters.AI.Behaviours;
using Runnables;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x02001145 RID: 4421
	public class YggdrasillElderEntAI : AIController
	{
		// Token: 0x06005604 RID: 22020 RVA: 0x00100281 File Offset: 0x000FE481
		private void Awake()
		{
			base.StartCoroutine(this.CProcess());
			this.character.health.onDie += this.OnDie;
		}

		// Token: 0x06005605 RID: 22021 RVA: 0x001002AC File Offset: 0x000FE4AC
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._behaviours.CRun(this);
			yield break;
		}

		// Token: 0x06005606 RID: 22022 RVA: 0x001002BC File Offset: 0x000FE4BC
		private void OnDie()
		{
			this.character.health.onDie -= this.OnDie;
			this.character.status.unstoppable.Attach(this);
			this.character.health.Heal(0.009999999776482582, false);
			this._onPhase2.Run();
			base.StopAllCoroutinesWithBehaviour();
			base.StartCoroutine(this.CReserveCleansing());
			base.StartCoroutine(this._phase2Sequence.CRun(this));
		}

		// Token: 0x06005607 RID: 22023 RVA: 0x00100347 File Offset: 0x000FE547
		private IEnumerator CReserveCleansing()
		{
			yield return null;
			this.character.status.RemoveAllStatus();
			this.character.status.unstoppable.Detach(this);
			yield break;
		}

		// Token: 0x06005608 RID: 22024 RVA: 0x00002191 File Offset: 0x00000391
		public void Update2PhaseFreezeEffect()
		{
		}

		// Token: 0x04004510 RID: 17680
		[Characters.AI.Behaviours.Behaviour.SubcomponentAttribute(true)]
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _behaviours;

		// Token: 0x04004511 RID: 17681
		[Characters.AI.Behaviours.Behaviour.SubcomponentAttribute(true)]
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _phase2Sequence;

		// Token: 0x04004512 RID: 17682
		[SerializeField]
		private Runnable _onPhase2;
	}
}
