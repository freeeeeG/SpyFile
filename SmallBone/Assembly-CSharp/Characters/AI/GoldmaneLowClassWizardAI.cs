using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001069 RID: 4201
	public sealed class GoldmaneLowClassWizardAI : AIController
	{
		// Token: 0x06005127 RID: 20775 RVA: 0x000F42A6 File Offset: 0x000F24A6
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._attack,
				this._essentialIdleAfterAttack,
				this._chaseTeleport
			};
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x000F42E3 File Offset: 0x000F24E3
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x000F430B File Offset: 0x000F250B
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x000F431A File Offset: 0x000F251A
		private IEnumerator CCombat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && this.character.movement.controller.isGrounded && this._chaseTeleport.result != Characters.AI.Behaviours.Behaviour.Result.Doing)
				{
					if (base.FindClosestPlayerBody(this._attackCollider))
					{
						yield return this._attack.CRun(this);
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

		// Token: 0x04004138 RID: 16696
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004139 RID: 16697
		[SerializeField]
		[Subcomponent(typeof(CircularProjectileAttack))]
		private CircularProjectileAttack _attack;

		// Token: 0x0400413A RID: 16698
		[SerializeField]
		[Subcomponent(typeof(ChaseTeleport))]
		private ChaseTeleport _chaseTeleport;

		// Token: 0x0400413B RID: 16699
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _essentialIdleAfterAttack;

		// Token: 0x0400413C RID: 16700
		[SerializeField]
		private Collider2D _attackCollider;

		// Token: 0x0400413D RID: 16701
		[SerializeField]
		private bool _noEscape;
	}
}
