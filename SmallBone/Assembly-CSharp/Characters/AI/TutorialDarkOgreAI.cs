using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200111E RID: 4382
	public sealed class TutorialDarkOgreAI : AIController
	{
		// Token: 0x0600553A RID: 21818 RVA: 0x000FE815 File Offset: 0x000FCA15
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chaseAndAttack
			};
		}

		// Token: 0x0600553B RID: 21819 RVA: 0x000FE846 File Offset: 0x000FCA46
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600553C RID: 21820 RVA: 0x000FE86E File Offset: 0x000FCA6E
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			for (;;)
			{
				yield return this._wander.CRun(this);
				yield return this._chaseAndAttack.CRun(this);
			}
			yield break;
		}

		// Token: 0x0400444D RID: 17485
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400444E RID: 17486
		[Subcomponent(typeof(Wander))]
		[SerializeField]
		private Wander _wander;

		// Token: 0x0400444F RID: 17487
		[SerializeField]
		[Subcomponent(typeof(ChaseAndAttack))]
		private ChaseAndAttack _chaseAndAttack;
	}
}
