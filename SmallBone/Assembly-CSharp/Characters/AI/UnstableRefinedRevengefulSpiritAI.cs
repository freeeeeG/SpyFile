using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010B0 RID: 4272
	public sealed class UnstableRefinedRevengefulSpiritAI : AIController
	{
		// Token: 0x060052CC RID: 21196 RVA: 0x000F847F File Offset: 0x000F667F
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._flyChase,
				this._attack,
				this._dash
			};
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x000F84BC File Offset: 0x000F66BC
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.CDash());
		}

		// Token: 0x060052CE RID: 21198 RVA: 0x000F84F1 File Offset: 0x000F66F1
		protected override IEnumerator CProcess()
		{
			this.character.movement.config.acceleration = this._attackAccelerationFar;
			this.character.movement.MoveHorizontal(UnityEngine.Random.rotation * Vector3.forward);
			yield return base.CPlayStartOption();
			if (!this._introSkip)
			{
				this._intro.TryStart();
			}
			yield return this._idle.CRun(this);
			while (!base.dead)
			{
				yield return this.CCombat();
			}
			yield break;
		}

		// Token: 0x060052CF RID: 21199 RVA: 0x000F8500 File Offset: 0x000F6700
		private IEnumerator CCombat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null))
				{
					if (base.FindClosestPlayerBody(this._dashTrigger) != null)
					{
						yield return this._dash.CRun(this);
						this.character.movement.config.keepMove = false;
						yield return Chronometer.global.WaitForSeconds(this._suicideDelay);
						yield return this._attack.CRun(this);
					}
					else
					{
						yield return this._flyChase.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x000F850F File Offset: 0x000F670F
		private IEnumerator CDash()
		{
			while (!base.dead)
			{
				if (base.FindClosestPlayerBody(this._dashTrigger) != null)
				{
					this.character.movement.config.acceleration = this._attackAccelerationNear;
				}
				else
				{
					this.character.movement.config.acceleration = this._attackAccelerationFar;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x04004278 RID: 17016
		[SerializeField]
		private bool _introSkip;

		// Token: 0x04004279 RID: 17017
		[SerializeField]
		private Collider2D _dashTrigger;

		// Token: 0x0400427A RID: 17018
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400427B RID: 17019
		[Subcomponent(typeof(FlyChase))]
		[SerializeField]
		private FlyChase _flyChase;

		// Token: 0x0400427C RID: 17020
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x0400427D RID: 17021
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _dash;

		// Token: 0x0400427E RID: 17022
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x0400427F RID: 17023
		[SerializeField]
		private Characters.Actions.Action _intro;

		// Token: 0x04004280 RID: 17024
		[SerializeField]
		[Range(0f, 100f)]
		private float _attackAccelerationNear;

		// Token: 0x04004281 RID: 17025
		[SerializeField]
		[Range(0f, 100f)]
		private float _attackAccelerationFar;

		// Token: 0x04004282 RID: 17026
		[SerializeField]
		private float _suicideDelay = 0.1f;
	}
}
