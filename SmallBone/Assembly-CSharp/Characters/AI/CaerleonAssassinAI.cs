using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001045 RID: 4165
	public sealed class CaerleonAssassinAI : AIController
	{
		// Token: 0x0600505C RID: 20572 RVA: 0x000F2224 File Offset: 0x000F0424
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._chaseTelport,
				this._idle
			};
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x000F2278 File Offset: 0x000F0478
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x000F22A0 File Offset: 0x000F04A0
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			while (!base.dead)
			{
				yield return this._wander.CRun(this);
				yield return this._idle.CRun(this);
				base.StartCoroutine(this.ChaseWithJump());
				yield return this.Combat();
			}
			yield break;
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x000F22AF File Offset: 0x000F04AF
		private IEnumerator ChaseWithJump()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && this._chaseTelport.CanUse() && this._chaseTelport.result != Characters.AI.Behaviours.Behaviour.Result.Doing && base.target.movement.controller.isGrounded && !(base.FindClosestPlayerBody(this._teleportTrigger) == null) && !(base.target.movement.controller.collisionState.lastStandingCollider == this.character.movement.controller.collisionState.lastStandingCollider))
				{
					base.StopAllBehaviour();
					this.character.movement.moveBackward = false;
					this._chaseTeleportAttack = true;
					yield return this._chaseTelport.CRun(this);
					this.character.ForceToLookAt(base.target.transform.position.x);
					yield return this.character.chronometer.master.WaitForSeconds(0.3f);
					yield return this._attack.CRun(this);
					yield return this._confusing.CRun(this);
					this._chaseTeleportAttack = false;
				}
			}
			yield break;
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x000F22BE File Offset: 0x000F04BE
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (base.target == null)
				{
					break;
				}
				if (this.character.movement.controller.isGrounded && this._chaseTelport.result != Characters.AI.Behaviours.Behaviour.Result.Doing && !this._chaseTeleportAttack)
				{
					if (base.FindClosestPlayerBody(this._attackCollider))
					{
						yield return this._attack.CRun(this);
						yield return this._confusing.CRun(this);
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

		// Token: 0x0400409E RID: 16542
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400409F RID: 16543
		[SerializeField]
		[Wander.SubcomponentAttribute(true)]
		private Wander _wander;

		// Token: 0x040040A0 RID: 16544
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private ActionAttack _attack;

		// Token: 0x040040A1 RID: 16545
		[SerializeField]
		[Subcomponent(typeof(Confusing))]
		private Confusing _confusing;

		// Token: 0x040040A2 RID: 16546
		[Chase.SubcomponentAttribute(true)]
		[SerializeField]
		private Chase _chase;

		// Token: 0x040040A3 RID: 16547
		[Subcomponent(typeof(ChaseTeleport))]
		[SerializeField]
		private ChaseTeleport _chaseTelport;

		// Token: 0x040040A4 RID: 16548
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x040040A5 RID: 16549
		[SerializeField]
		private Collider2D _attackCollider;

		// Token: 0x040040A6 RID: 16550
		[SerializeField]
		private Collider2D _teleportTrigger;

		// Token: 0x040040A7 RID: 16551
		private bool _chaseTeleportAttack;
	}
}
