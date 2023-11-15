using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001121 RID: 4385
	public sealed class DarkOgre : AIController
	{
		// Token: 0x06005546 RID: 21830 RVA: 0x000FE94F File Offset: 0x000FCB4F
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chaseAndAttack
			};
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x000FE980 File Offset: 0x000FCB80
		public void StartCombat()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005548 RID: 21832 RVA: 0x000FE9A8 File Offset: 0x000FCBA8
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			while (!base.dead)
			{
				yield return this._wander.CRun(this);
				yield return this._chaseAndAttack.CRun(this);
			}
			yield break;
		}

		// Token: 0x04004454 RID: 17492
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004455 RID: 17493
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x04004456 RID: 17494
		[Subcomponent(typeof(ChaseAndAttack))]
		[SerializeField]
		private ChaseAndAttack _chaseAndAttack;
	}
}
