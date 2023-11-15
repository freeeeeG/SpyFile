using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001053 RID: 4179
	public sealed class FireCaerleonLowClassWizardAI : AIController
	{
		// Token: 0x060050A7 RID: 20647 RVA: 0x000F2EAC File Offset: 0x000F10AC
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._circularProjectileAttack,
				this._essentialIdleAfterAttack,
				this._chaseTeleport,
				this._escapeTeleport
			};
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x000F2F00 File Offset: 0x000F1100
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x000F2F28 File Offset: 0x000F1128
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.Combat();
			yield break;
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x000F2F37 File Offset: 0x000F1137
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && this._escapeTeleport.result != Characters.AI.Behaviours.Behaviour.Result.Doing && this._chaseTeleport.result != Characters.AI.Behaviours.Behaviour.Result.Doing)
				{
					if (base.FindClosestPlayerBody(this._attackCollider))
					{
						yield return this._circularProjectileAttack.CRun(this);
						yield return this._essentialIdleAfterAttack.CRun(this);
					}
					else if (base.target.movement.controller.isGrounded && !this._noEscape && this._chaseTeleport.CanUse())
					{
						yield return this._chaseTeleport.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x040040D7 RID: 16599
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040040D8 RID: 16600
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private CircularProjectileAttack _circularProjectileAttack;

		// Token: 0x040040D9 RID: 16601
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _essentialIdleAfterAttack;

		// Token: 0x040040DA RID: 16602
		[SerializeField]
		[Subcomponent(typeof(ChaseTeleport))]
		private ChaseTeleport _chaseTeleport;

		// Token: 0x040040DB RID: 16603
		[SerializeField]
		[Subcomponent(typeof(Teleport))]
		private Teleport _escapeTeleport;

		// Token: 0x040040DC RID: 16604
		[SerializeField]
		private Collider2D _attackCollider;

		// Token: 0x040040DD RID: 16605
		[SerializeField]
		private bool _noEscape;
	}
}
