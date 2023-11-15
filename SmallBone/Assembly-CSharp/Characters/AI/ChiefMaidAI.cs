using System;
using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001062 RID: 4194
	public sealed class ChiefMaidAI : AIController
	{
		// Token: 0x060050FD RID: 20733 RVA: 0x000F3BE6 File Offset: 0x000F1DE6
		private void Awake()
		{
			this._attackPoint.parent = null;
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x000F3BF4 File Offset: 0x000F1DF4
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x000F3C1C File Offset: 0x000F1E1C
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.Combat();
			yield break;
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x000F3C2B File Offset: 0x000F1E2B
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
					if (base.FindClosestPlayerBody(this._impactBellTrigger) && this._impactBell.CanUse())
					{
						this.ShiftAttackPoint();
						yield return this._impactBell.CRun(this);
					}
					if (base.FindClosestPlayerBody(this._keepDistanceCollider) && this._keepDistance.CanUseBackMove())
					{
						this.character.movement.moveBackward = true;
						yield return this._keepDistance.CRun(this);
						this.character.movement.moveBackward = false;
					}
				}
			}
			yield break;
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x000F3C3A File Offset: 0x000F1E3A
		private void ShiftAttackPoint()
		{
			this._attackPoint.position = base.target.transform.position;
		}

		// Token: 0x04004118 RID: 16664
		[SerializeField]
		private Collider2D _spawnCollider;

		// Token: 0x04004119 RID: 16665
		[SerializeField]
		private Collider2D _keepDistanceCollider;

		// Token: 0x0400411A RID: 16666
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400411B RID: 16667
		[SerializeField]
		[Subcomponent(typeof(KeepDistance))]
		private KeepDistance _keepDistance;

		// Token: 0x0400411C RID: 16668
		[Subcomponent(typeof(SpawnEnemy))]
		[SerializeField]
		private SpawnEnemy _spawnEnemy;

		// Token: 0x0400411D RID: 16669
		[Space]
		[Header("ImpectBell")]
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _impactBell;

		// Token: 0x0400411E RID: 16670
		[SerializeField]
		private Collider2D _impactBellTrigger;

		// Token: 0x0400411F RID: 16671
		[SerializeField]
		private Transform _attackPoint;
	}
}
