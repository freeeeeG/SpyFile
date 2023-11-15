using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010D2 RID: 4306
	public sealed class HolyKnightsWizardAI : AIController
	{
		// Token: 0x0600539E RID: 21406 RVA: 0x000FAAFC File Offset: 0x000F8CFC
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._chaseTeleport,
				this._homingRangeAttack,
				this._radialRangeAttack
			};
			this.character.health.onTookDamage += new TookDamageDelegate(this.TryCounterAttack);
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x000FAB60 File Offset: 0x000F8D60
		private void TryCounterAttack(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (this._counter)
			{
				return;
			}
			if (!this._radialRangeAttack.CanUse())
			{
				return;
			}
			if (this.character.health.dead || base.dead || this.character.health.percent <= damageDealt)
			{
				this._counter = true;
				return;
			}
			if (this._radialRangeAttack.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
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

		// Token: 0x060053A0 RID: 21408 RVA: 0x000FABE3 File Offset: 0x000F8DE3
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060053A1 RID: 21409 RVA: 0x000FAC0B File Offset: 0x000F8E0B
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x060053A2 RID: 21410 RVA: 0x000FAC1A File Offset: 0x000F8E1A
		private IEnumerator CCombat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && !base.stuned)
				{
					if (this._counter && this.character.health.currentHealth > 0.0 && !base.dead)
					{
						yield return this._radialRangeAttack.CRun(this);
						this._counter = false;
					}
					if (base.FindClosestPlayerBody(this._attackTrigger) != null)
					{
						if (!this._radialRangeAttack.CanUse())
						{
							yield return this._homingRangeAttack.CRun(this);
						}
						else if (MMMaths.RandomBool())
						{
							yield return this._homingRangeAttack.CRun(this);
						}
						else
						{
							yield return this._radialRangeAttack.CRun(this);
						}
					}
					else
					{
						yield return this._chaseTeleport.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x0400432C RID: 17196
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		[Header("Behaviours")]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400432D RID: 17197
		[Subcomponent(typeof(ChaseTeleport))]
		[SerializeField]
		private ChaseTeleport _chaseTeleport;

		// Token: 0x0400432E RID: 17198
		[SerializeField]
		[Subcomponent(typeof(CircularProjectileAttack))]
		private CircularProjectileAttack _homingRangeAttack;

		// Token: 0x0400432F RID: 17199
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _radialRangeAttack;

		// Token: 0x04004330 RID: 17200
		[Header("Tools")]
		[Space]
		[SerializeField]
		private Collider2D _attackTrigger;

		// Token: 0x04004331 RID: 17201
		[SerializeField]
		[Range(0f, 1f)]
		private float _counterChance;

		// Token: 0x04004332 RID: 17202
		private bool _counter;
	}
}
