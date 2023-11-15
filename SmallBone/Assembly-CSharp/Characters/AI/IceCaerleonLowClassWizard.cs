using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001059 RID: 4185
	public sealed class IceCaerleonLowClassWizard : AIController
	{
		// Token: 0x060050C8 RID: 20680 RVA: 0x000F32E7 File Offset: 0x000F14E7
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._icestom,
				this._essentialIdleAfterAttack,
				this._chaseTeleport
			};
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x000F3324 File Offset: 0x000F1524
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x000F334C File Offset: 0x000F154C
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			while (!base.dead)
			{
				yield return this.Combat();
			}
			yield break;
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x000F335B File Offset: 0x000F155B
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && this.character.movement.controller.isGrounded && this._chaseTeleport.result != Characters.AI.Behaviours.Behaviour.Result.Doing)
				{
					if (base.FindClosestPlayerBody(this._escapeTrigger))
					{
						yield return this._icestom.CRun(this);
					}
					else if (base.FindClosestPlayerBody(this._attackCollider))
					{
						yield return this._icestom.CRun(this);
					}
					else if (base.target.movement.controller.isGrounded && !this._noEscape && this._chaseTeleport.CanUse())
					{
						yield return this._chaseTeleport.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x040040EE RID: 16622
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040040EF RID: 16623
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _icestom;

		// Token: 0x040040F0 RID: 16624
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _essentialIdleAfterAttack;

		// Token: 0x040040F1 RID: 16625
		[Subcomponent(typeof(ChaseTeleport))]
		[SerializeField]
		private ChaseTeleport _chaseTeleport;

		// Token: 0x040040F2 RID: 16626
		[SerializeField]
		private Collider2D _attackCollider;

		// Token: 0x040040F3 RID: 16627
		[SerializeField]
		private Collider2D _escapeTrigger;

		// Token: 0x040040F4 RID: 16628
		[SerializeField]
		private bool _noEscape;
	}
}
