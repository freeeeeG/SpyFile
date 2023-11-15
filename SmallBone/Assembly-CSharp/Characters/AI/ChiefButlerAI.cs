using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200105F RID: 4191
	public class ChiefButlerAI : AIController
	{
		// Token: 0x060050EA RID: 20714 RVA: 0x000F3910 File Offset: 0x000F1B10
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._counterAttack,
				this._spawnEnemy
			};
			this.character.health.onTookDamage += new TookDamageDelegate(this.TryCounterAttack);
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x000F3980 File Offset: 0x000F1B80
		private void TryCounterAttack(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (this._counter)
			{
				return;
			}
			if (!this._counterAttack.CanUse())
			{
				return;
			}
			if (this._spawnEnemy.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				return;
			}
			base.StopAllBehaviour();
			this._counter = true;
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x000F39B5 File Offset: 0x000F1BB5
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x000F39DD File Offset: 0x000F1BDD
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.Combat();
			yield break;
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x000F39EC File Offset: 0x000F1BEC
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null))
				{
					if (base.FindClosestPlayerBody(this._spawnCollider))
					{
						yield return this._spawnEnemy.CRun(this);
					}
					if (this._counter && this.character.health.currentHealth > 0.0 && !base.dead)
					{
						yield return this._counterAttack.CRun(this);
						this._counter = false;
					}
					this.LookTarget();
				}
			}
			yield break;
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x000F39FC File Offset: 0x000F1BFC
		private void LookTarget()
		{
			float targetX = base.target.transform.position.x - this.character.transform.position.x;
			this.character.ForceToLookAt(targetX);
		}

		// Token: 0x0400410B RID: 16651
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		[Header("Behaviours")]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400410C RID: 16652
		[Subcomponent(typeof(Wander))]
		[SerializeField]
		private Wander _wander;

		// Token: 0x0400410D RID: 16653
		[Subcomponent(typeof(Chase))]
		[SerializeField]
		private Chase _chase;

		// Token: 0x0400410E RID: 16654
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _counterAttack;

		// Token: 0x0400410F RID: 16655
		[SerializeField]
		[Subcomponent(typeof(SpawnEnemy))]
		private SpawnEnemy _spawnEnemy;

		// Token: 0x04004110 RID: 16656
		[SerializeField]
		[Header("Spawn")]
		private Collider2D _spawnCollider;

		// Token: 0x04004111 RID: 16657
		private bool _counter;
	}
}
