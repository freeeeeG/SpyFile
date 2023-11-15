using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200104F RID: 4175
	public sealed class CaerleonRecruitAI : AIController
	{
		// Token: 0x06005094 RID: 20628 RVA: 0x000F2C6D File Offset: 0x000F0E6D
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chaseAndAttack
			};
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x000F2C9E File Offset: 0x000F0E9E
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x000F2CC6 File Offset: 0x000F0EC6
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

		// Token: 0x040040CA RID: 16586
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040040CB RID: 16587
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x040040CC RID: 16588
		[SerializeField]
		[Subcomponent(typeof(ChaseAndAttack))]
		private ChaseAndAttack _chaseAndAttack;
	}
}
