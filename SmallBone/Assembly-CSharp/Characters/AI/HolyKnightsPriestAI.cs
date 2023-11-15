using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010CC RID: 4300
	public sealed class HolyKnightsPriestAI : AIController
	{
		// Token: 0x06005379 RID: 21369 RVA: 0x000FA470 File Offset: 0x000F8670
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._heal,
				this._holyLight
			};
			this.character.health.onTookDamage += new TookDamageDelegate(this.TryCounterAttack);
		}

		// Token: 0x0600537A RID: 21370 RVA: 0x000FA4C8 File Offset: 0x000F86C8
		private void TryCounterAttack(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (this._counter)
			{
				return;
			}
			if (!this._holyLight.CanUse())
			{
				return;
			}
			if (this.character.health.dead || base.dead || this.character.health.percent <= damageDealt)
			{
				this._counter = true;
				return;
			}
			if (this._holyLight.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				return;
			}
			if (!MMMaths.Chance(this._counterChance))
			{
				return;
			}
			base.StopAllBehaviour();
			this._counter = true;
		}

		// Token: 0x0600537B RID: 21371 RVA: 0x000FA54B File Offset: 0x000F874B
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x000FA573 File Offset: 0x000F8773
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x000FA582 File Offset: 0x000F8782
		private IEnumerator CCombat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && !base.stuned)
				{
					if (this._counter && this.character.health.currentHealth > 0.0 && !base.dead)
					{
						yield return this._holyLight.CRun(this);
						this._counter = false;
					}
					if (base.FindClosestPlayerBody(this._keepDistanceTrigger) != null && this._keepDistance.CanUseBackMove())
					{
						yield return this._keepDistance.CRun(this);
					}
					else if (this._heal.CanUse())
					{
						yield return this._heal.CRun(this);
					}
					else if (this._holyLight.CanUse())
					{
						yield return this._holyLight.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x0400430F RID: 17167
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		[Header("Behaviours")]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004310 RID: 17168
		[Subcomponent(typeof(KeepDistance))]
		[SerializeField]
		private KeepDistance _keepDistance;

		// Token: 0x04004311 RID: 17169
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _heal;

		// Token: 0x04004312 RID: 17170
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _holyLight;

		// Token: 0x04004313 RID: 17171
		[SerializeField]
		[Space]
		[Header("Tools")]
		private Collider2D _keepDistanceTrigger;

		// Token: 0x04004314 RID: 17172
		[Range(0f, 1f)]
		[SerializeField]
		private float _counterChance;

		// Token: 0x04004315 RID: 17173
		private bool _counter;
	}
}
