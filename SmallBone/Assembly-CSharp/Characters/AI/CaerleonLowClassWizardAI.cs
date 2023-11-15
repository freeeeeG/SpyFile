using System;
using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001049 RID: 4169
	public sealed class CaerleonLowClassWizardAI : AIController
	{
		// Token: 0x06005074 RID: 20596 RVA: 0x000F272C File Offset: 0x000F092C
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x000F2754 File Offset: 0x000F0954
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.Combat();
			yield break;
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x000F2763 File Offset: 0x000F0963
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && this.character.movement.controller.isGrounded)
				{
					if (base.FindClosestPlayerBody(this._attackCollider))
					{
						yield return this._circularProjectileAttack.CRun(this);
						yield return this._essentialIdleAfterAttack.CRun(this);
					}
					else if (base.target.movement.controller.isGrounded && this._chaseTeleport.CanUse())
					{
						yield return this._chaseTeleport.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x040040B1 RID: 16561
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040040B2 RID: 16562
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private CircularProjectileAttack _circularProjectileAttack;

		// Token: 0x040040B3 RID: 16563
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _essentialIdleAfterAttack;

		// Token: 0x040040B4 RID: 16564
		[SerializeField]
		[Subcomponent(typeof(ChaseTeleport))]
		private ChaseTeleport _chaseTeleport;

		// Token: 0x040040B5 RID: 16565
		[SerializeField]
		private Collider2D _attackCollider;
	}
}
