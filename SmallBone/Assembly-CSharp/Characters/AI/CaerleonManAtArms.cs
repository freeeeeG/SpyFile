using System;
using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200104C RID: 4172
	public sealed class CaerleonManAtArms : AIController
	{
		// Token: 0x06005084 RID: 20612 RVA: 0x000F2944 File Offset: 0x000F0B44
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x000F296C File Offset: 0x000F0B6C
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this._idle.CRun(this);
			yield return this.Combat();
			yield break;
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x000F297B File Offset: 0x000F0B7B
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && this.character.movement.controller.isGrounded)
				{
					if (this._tackle.CanUse())
					{
						this.stopTrigger = this._tackleCollider;
						if (base.FindClosestPlayerBody(this._tackleCollider))
						{
							yield return this._tackle.CRun(this);
							this.stopTrigger = this._attackCollider;
						}
						else if (base.FindClosestPlayerBody(this._attackCollider))
						{
							yield return this._attack.CRun(this);
						}
						else
						{
							yield return this._chase.CRun(this);
						}
					}
					else
					{
						this.stopTrigger = this._attackCollider;
						if (base.FindClosestPlayerBody(this._attackCollider))
						{
							yield return this._attack.CRun(this);
						}
						else
						{
							yield return this._chase.CRun(this);
							if (this._attack.result == Characters.AI.Behaviours.Behaviour.Result.Success)
							{
								yield return this._attack.CRun(this);
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x040040BC RID: 16572
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040040BD RID: 16573
		[Wander.SubcomponentAttribute(true)]
		[SerializeField]
		private Wander _wander;

		// Token: 0x040040BE RID: 16574
		[SerializeField]
		[Chase.SubcomponentAttribute(true)]
		private Chase _chase;

		// Token: 0x040040BF RID: 16575
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x040040C0 RID: 16576
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _tackle;

		// Token: 0x040040C1 RID: 16577
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x040040C2 RID: 16578
		[SerializeField]
		private Collider2D _attackCollider;

		// Token: 0x040040C3 RID: 16579
		[SerializeField]
		private Collider2D _tackleCollider;
	}
}
