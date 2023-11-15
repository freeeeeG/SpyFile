using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001076 RID: 4214
	public sealed class GoldmaneSpearMan : AIController
	{
		// Token: 0x06005177 RID: 20855 RVA: 0x000F4FE4 File Offset: 0x000F31E4
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._chaseAndAttack,
				this._upperAttack,
				this._wander,
				this._idle
			};
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x000F5038 File Offset: 0x000F3238
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x000F5060 File Offset: 0x000F3260
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			base.StartCoroutine(this.ProcessForUpperAttack());
			while (!base.dead)
			{
				yield return this._wander.CRun(this);
				yield return this._idle.CRun(this);
				yield return this.Combat();
			}
			yield break;
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x000F506F File Offset: 0x000F326F
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && this.character.movement.controller.isGrounded && this._upperAttack.result != Characters.AI.Behaviours.Behaviour.Result.Doing)
				{
					yield return this._chaseAndAttack.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x000F507E File Offset: 0x000F327E
		private IEnumerator ProcessForUpperAttack()
		{
			while (!base.dead)
			{
				yield return null;
				if (this._upperAttack.CanUse() && this._chaseAndAttack.attack.result != Characters.AI.Behaviours.Behaviour.Result.Doing && !(base.FindClosestPlayerBody(this._upperAttackCollider) == null))
				{
					base.StopAllBehaviour();
					yield return this.DoUpperAttack();
				}
			}
			yield break;
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x000F508D File Offset: 0x000F328D
		private IEnumerator DoUpperAttack()
		{
			yield return this._upperAttack.CRun(this);
			yield break;
		}

		// Token: 0x0400417C RID: 16764
		[SerializeField]
		private Collider2D _upperAttackCollider;

		// Token: 0x0400417D RID: 16765
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400417E RID: 16766
		[SerializeField]
		[Wander.SubcomponentAttribute(true)]
		private Wander _wander;

		// Token: 0x0400417F RID: 16767
		[Subcomponent(typeof(ChaseAndAttack))]
		[SerializeField]
		private ChaseAndAttack _chaseAndAttack;

		// Token: 0x04004180 RID: 16768
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _upperAttack;

		// Token: 0x04004181 RID: 16769
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;
	}
}
