using System;
using System.Collections;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001051 RID: 4177
	public sealed class EntAI : AIController
	{
		// Token: 0x0600509E RID: 20638 RVA: 0x000F2D91 File Offset: 0x000F0F91
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x000F2DB9 File Offset: 0x000F0FB9
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			while (!base.dead)
			{
				yield return this._wander.CRun(this);
				yield return this._idle.CRun(this);
				yield return this._chaseAndAttack.CRun(this);
			}
			yield break;
		}

		// Token: 0x040040D0 RID: 16592
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040040D1 RID: 16593
		[Wander.SubcomponentAttribute(true)]
		[SerializeField]
		private Wander _wander;

		// Token: 0x040040D2 RID: 16594
		[Subcomponent(typeof(ChaseAndAttack))]
		[SerializeField]
		private ChaseAndAttack _chaseAndAttack;

		// Token: 0x040040D3 RID: 16595
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;
	}
}
