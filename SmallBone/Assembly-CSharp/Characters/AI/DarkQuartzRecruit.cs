using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001099 RID: 4249
	public sealed class DarkQuartzRecruit : AIController
	{
		// Token: 0x0600524C RID: 21068 RVA: 0x000F7088 File Offset: 0x000F5288
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chaseAndAttack,
				this._idle
			};
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x000F70C5 File Offset: 0x000F52C5
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x000F70ED File Offset: 0x000F52ED
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			while (!base.dead)
			{
				yield return this._chaseAndAttack.CRun(this);
			}
			yield break;
		}

		// Token: 0x0400420F RID: 16911
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004210 RID: 16912
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x04004211 RID: 16913
		[Subcomponent(typeof(ChaseAndAttack))]
		[SerializeField]
		private ChaseAndAttack _chaseAndAttack;

		// Token: 0x04004212 RID: 16914
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;
	}
}
