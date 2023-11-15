using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001096 RID: 4246
	public sealed class DarkQuartzOgre : AIController
	{
		// Token: 0x06005239 RID: 21049 RVA: 0x000F6CE4 File Offset: 0x000F4EE4
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._attack,
				this._chase,
				this._counterAttack,
				this._idle
			};
		}

		// Token: 0x0600523A RID: 21050 RVA: 0x000F6D44 File Offset: 0x000F4F44
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			this.character.health.onTookDamage += new TookDamageDelegate(this.Health_onTookDamage);
			this.character.health.onDie += delegate()
			{
				this.character.health.onTookDamage -= new TookDamageDelegate(this.Health_onTookDamage);
			};
		}

		// Token: 0x0600523B RID: 21051 RVA: 0x000F6DB0 File Offset: 0x000F4FB0
		private void Health_onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (this._counter)
			{
				return;
			}
			if (this.character.health.dead || base.dead || this.character.health.percent <= damageDealt)
			{
				this._counter = true;
				return;
			}
			if (this._attack.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
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

		// Token: 0x0600523C RID: 21052 RVA: 0x000F6E25 File Offset: 0x000F5025
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this.Combat();
			yield break;
		}

		// Token: 0x0600523D RID: 21053 RVA: 0x000F6E34 File Offset: 0x000F5034
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && !base.stuned)
				{
					if (this._counter && this.character.health.currentHealth > 0.0 && !base.dead)
					{
						yield return this._counterAttack.CRun(this);
						this._counter = false;
					}
					if (base.FindClosestPlayerBody(this._attackTrigger) != null)
					{
						yield return this._attack.CRun(this);
					}
					else
					{
						yield return this._chase.CRun(this);
						if (this._chase.result == Characters.AI.Behaviours.Behaviour.Result.Success)
						{
							yield return this._attack.CRun(this);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x04004200 RID: 16896
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004201 RID: 16897
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x04004202 RID: 16898
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x04004203 RID: 16899
		[Subcomponent(typeof(Chase))]
		[SerializeField]
		private Chase _chase;

		// Token: 0x04004204 RID: 16900
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _counterAttack;

		// Token: 0x04004205 RID: 16901
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x04004206 RID: 16902
		[SerializeField]
		private Collider2D _attackTrigger;

		// Token: 0x04004207 RID: 16903
		[SerializeField]
		[Range(0f, 1f)]
		private float _counterChance;

		// Token: 0x04004208 RID: 16904
		private bool _counter;
	}
}
