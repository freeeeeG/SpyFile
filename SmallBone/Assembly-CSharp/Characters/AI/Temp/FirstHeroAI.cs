using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Temp
{
	// Token: 0x0200121F RID: 4639
	public class FirstHeroAI : AIController
	{
		// Token: 0x06005AEA RID: 23274 RVA: 0x0010D43F File Offset: 0x0010B63F
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._advent,
				this._dash,
				this._energyBlast
			};
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x0010D47C File Offset: 0x0010B67C
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x0010D4A4 File Offset: 0x0010B6A4
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.DoAdvent();
			this._auraAction.Initialize(this._aura);
			base.StartCoroutine(this.DoRangeContinuousAttack());
			this._auraEffect.SetActive(true);
			while (!base.dead)
			{
				if (base.target == null)
				{
					yield return null;
				}
				else
				{
					yield return this.DoComoboAttack();
				}
			}
			yield break;
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x0010D4B3 File Offset: 0x0010B6B3
		private IEnumerator DoComoboAttack()
		{
			float num = base.target.transform.position.x - this.character.transform.position.x;
			this.character.lookingDirection = ((num > 0f) ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			yield return this.DoDash();
			yield return this._commboAttack1.CRun(this);
			yield return this.DoDash();
			yield return this._commboAttack2.CRun(this);
			yield return this.DoDash();
			yield return this._commboAttack3.CRun(this);
			yield break;
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x0010D4C2 File Offset: 0x0010B6C2
		private IEnumerator DoAdvent()
		{
			RaycastHit2D raycastHit2D;
			if (base.target.movement.TryBelowRayCast(this._adventLayerMask, out raycastHit2D, 20f))
			{
				this.character.transform.position = raycastHit2D.point;
			}
			else
			{
				this.character.transform.position = base.target.transform.position;
			}
			yield return this._advent.CRun(this);
			this._adventIdle.TryStart();
			while (this._adventIdle.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x0010D4D1 File Offset: 0x0010B6D1
		private IEnumerator DoDash()
		{
			yield return this._teleportInRangeWithFly.CRun(this);
			if (this.character.transform.position.x > base.target.transform.position.x)
			{
				this.character.lookingDirection = Character.LookingDirection.Left;
			}
			else
			{
				this.character.lookingDirection = Character.LookingDirection.Right;
			}
			base.destination = base.target.transform.position;
			yield break;
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x0010D4E0 File Offset: 0x0010B6E0
		private IEnumerator DoEnergyBlast()
		{
			yield return this.DoDash();
			yield return this._circularProjectileAttack.CRun(this);
			yield break;
		}

		// Token: 0x06005AF1 RID: 23281 RVA: 0x0010D4EF File Offset: 0x0010B6EF
		private IEnumerator DoRangeContinuousAttack()
		{
			while (!base.dead)
			{
				if (this._teleportInRangeWithFly.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
				{
					yield return null;
				}
				else
				{
					yield return Chronometer.global.WaitForSeconds(0.5f);
					if (this._auraAction.gameObject.activeSelf)
					{
						this._auraAction.TryStart();
					}
				}
			}
			yield break;
		}

		// Token: 0x04004958 RID: 18776
		[SerializeField]
		private LayerMask _adventLayerMask;

		// Token: 0x04004959 RID: 18777
		[SerializeField]
		private Characters.Actions.Action _adventIdle;

		// Token: 0x0400495A RID: 18778
		[SerializeField]
		private MotionTrail _motionTrail;

		// Token: 0x0400495B RID: 18779
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400495C RID: 18780
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _advent;

		// Token: 0x0400495D RID: 18781
		[Subcomponent(typeof(MoveToDestinationWithFly))]
		[SerializeField]
		private MoveToDestinationWithFly _dash;

		// Token: 0x0400495E RID: 18782
		[SerializeField]
		[Subcomponent(typeof(TeleportInRangeWithFly))]
		private TeleportInRangeWithFly _teleportInRangeWithFly;

		// Token: 0x0400495F RID: 18783
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _commboAttack1;

		// Token: 0x04004960 RID: 18784
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _commboAttack2;

		// Token: 0x04004961 RID: 18785
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _commboAttack3;

		// Token: 0x04004962 RID: 18786
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _energyBlast;

		// Token: 0x04004963 RID: 18787
		[SerializeField]
		[Subcomponent(typeof(CircularProjectileAttack))]
		private CircularProjectileAttack _circularProjectileAttack;

		// Token: 0x04004964 RID: 18788
		[SerializeField]
		private Character _aura;

		// Token: 0x04004965 RID: 18789
		[SerializeField]
		private Characters.Actions.Action _auraAction;

		// Token: 0x04004966 RID: 18790
		[SerializeField]
		private GameObject _auraEffect;
	}
}
