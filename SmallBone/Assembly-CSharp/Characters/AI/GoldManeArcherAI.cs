using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001065 RID: 4197
	public sealed class GoldManeArcherAI : AIController
	{
		// Token: 0x0600510F RID: 20751 RVA: 0x000F3E50 File Offset: 0x000F2050
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._keepDistanceWithJump,
				this._keepDistanceWithMove,
				this._attack,
				this._idle
			};
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x000F3EBC File Offset: 0x000F20BC
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x000F3EE4 File Offset: 0x000F20E4
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this._idle.CRun(this);
			yield return this.Combat();
			yield break;
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x000F3EF3 File Offset: 0x000F20F3
		private IEnumerator Combat()
		{
			base.StartCoroutine(this.ProcessBackStep());
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && !base.stuned && this.character.movement.controller.isGrounded && this._keepDistanceWithJump.result != Characters.AI.Behaviours.Behaviour.Result.Doing && this._keepDistanceWithMove.result != Characters.AI.Behaviours.Behaviour.Result.Doing && this._attack.result != Characters.AI.Behaviours.Behaviour.Result.Doing && !(base.FindClosestPlayerBody(this._minimumCollider) != null))
				{
					if (base.FindClosestPlayerBody(this._attackCollider) != null)
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

		// Token: 0x06005113 RID: 20755 RVA: 0x000F3F02 File Offset: 0x000F2102
		private IEnumerator ProcessBackStep()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.FindClosestPlayerBody(this._minimumCollider) == null) && this._attack.result != Characters.AI.Behaviours.Behaviour.Result.Doing)
				{
					base.StopAllBehaviour();
					if (this._keepDistanceWithJump.CanUseBackStep())
					{
						yield return this._keepDistanceWithJump.CRun(this);
					}
					else if (this._keepDistanceWithMove.CanUseBackMove())
					{
						yield return this._keepDistanceWithMove.CRun(this);
						yield return this._attack.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x04004126 RID: 16678
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004127 RID: 16679
		[Wander.SubcomponentAttribute(true)]
		[SerializeField]
		private Wander _wander;

		// Token: 0x04004128 RID: 16680
		[Subcomponent(typeof(KeepDistance))]
		[SerializeField]
		private KeepDistance _keepDistanceWithJump;

		// Token: 0x04004129 RID: 16681
		[SerializeField]
		[Subcomponent(typeof(KeepDistance))]
		private KeepDistance _keepDistanceWithMove;

		// Token: 0x0400412A RID: 16682
		[SerializeField]
		[Subcomponent(typeof(HorizontalProjectileAttack))]
		private HorizontalProjectileAttack _attack;

		// Token: 0x0400412B RID: 16683
		[SerializeField]
		[Chase.SubcomponentAttribute(true)]
		private Chase _chase;

		// Token: 0x0400412C RID: 16684
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x0400412D RID: 16685
		[SerializeField]
		private Collider2D _minimumCollider;

		// Token: 0x0400412E RID: 16686
		[SerializeField]
		private Collider2D _attackCollider;
	}
}
