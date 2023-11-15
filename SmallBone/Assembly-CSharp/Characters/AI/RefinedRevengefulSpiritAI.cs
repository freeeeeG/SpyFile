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
	// Token: 0x020010A6 RID: 4262
	public sealed class RefinedRevengefulSpiritAI : AIController
	{
		// Token: 0x06005293 RID: 21139 RVA: 0x000F7C95 File Offset: 0x000F5E95
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._flyChase,
				this._attack
			};
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x000F7CC6 File Offset: 0x000F5EC6
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.Dash());
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x000F7CFB File Offset: 0x000F5EFB
		protected override IEnumerator CProcess()
		{
			this.character.movement.config.acceleration = this._attackAccelerationFar;
			this.character.movement.MoveHorizontal(UnityEngine.Random.rotation * Vector3.forward);
			yield return base.CPlayStartOption();
			if (!this._introSkip)
			{
				this._intro.TryStart();
				yield return this._idle.CRun(this);
			}
			while (!base.dead)
			{
				yield return this.Combat();
			}
			yield break;
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x000F7D0A File Offset: 0x000F5F0A
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null))
				{
					if (base.FindClosestPlayerBody(this._attackTrigger) != null)
					{
						yield return this._attack.CRun(this);
					}
					else
					{
						yield return this._flyChase.CRun(this);
						yield return this._attack.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x000F7D19 File Offset: 0x000F5F19
		private IEnumerator Dash()
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

		// Token: 0x0400424C RID: 16972
		[SerializeField]
		private bool _introSkip;

		// Token: 0x0400424D RID: 16973
		[SerializeField]
		private Collider2D _attackTrigger;

		// Token: 0x0400424E RID: 16974
		[SerializeField]
		private Collider2D _dashTrigger;

		// Token: 0x0400424F RID: 16975
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004250 RID: 16976
		[SerializeField]
		[Subcomponent(typeof(FlyChase))]
		private FlyChase _flyChase;

		// Token: 0x04004251 RID: 16977
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x04004252 RID: 16978
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x04004253 RID: 16979
		[SerializeField]
		private Characters.Actions.Action _intro;

		// Token: 0x04004254 RID: 16980
		[SerializeField]
		[Range(0f, 100f)]
		private float _attackAccelerationNear;

		// Token: 0x04004255 RID: 16981
		[SerializeField]
		[Range(0f, 100f)]
		private float _attackAccelerationFar;
	}
}
