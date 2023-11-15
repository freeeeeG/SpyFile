using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010CF RID: 4303
	public sealed class HolyKnightsRecruitAI : AIController
	{
		// Token: 0x0600538B RID: 21387 RVA: 0x000FA7B0 File Offset: 0x000F89B0
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._counterAttack,
				this._comboAttack
			};
			this.character.health.onTookDamage += new TookDamageDelegate(this.TryCounterAttack);
		}

		// Token: 0x0600538C RID: 21388 RVA: 0x000FA820 File Offset: 0x000F8A20
		private void OnDestroy()
		{
			this._idleClipAfterWander = null;
		}

		// Token: 0x0600538D RID: 21389 RVA: 0x000FA82C File Offset: 0x000F8A2C
		private void TryCounterAttack(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (this._counter)
			{
				return;
			}
			if (!this._counterAttack.CanUse())
			{
				return;
			}
			if (this.character.health.dead || base.dead || this.character.health.percent <= damageDealt)
			{
				this._counter = true;
				return;
			}
			if (this._comboAttack.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				return;
			}
			if (!MMMaths.Chance(this._counterChance))
			{
				return;
			}
			base.StopAllBehaviour();
			this._counter = true;
		}

		// Token: 0x0600538E RID: 21390 RVA: 0x000FA8AF File Offset: 0x000F8AAF
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600538F RID: 21391 RVA: 0x000FA8D7 File Offset: 0x000F8AD7
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x000FA8E6 File Offset: 0x000F8AE6
		private IEnumerator CCombat()
		{
			yield return this._wander.CRun(this);
			this._characterAnimation.SetIdle(this._idleClipAfterWander);
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && !base.stuned)
				{
					if (this._counter && this.character.health.currentHealth > 0.0 && !base.dead)
					{
						yield return this._counterAttack.CRun(this);
						this._counter = false;
					}
					if (base.FindClosestPlayerBody(this._comboAttackTrigger) != null)
					{
						yield return this._comboAttack.CRun(this);
					}
					else
					{
						yield return this._chase.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x0400431C RID: 17180
		[Header("Behaviours")]
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400431D RID: 17181
		[Subcomponent(typeof(Wander))]
		[SerializeField]
		private Wander _wander;

		// Token: 0x0400431E RID: 17182
		[Subcomponent(typeof(Chase))]
		[SerializeField]
		private Chase _chase;

		// Token: 0x0400431F RID: 17183
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _counterAttack;

		// Token: 0x04004320 RID: 17184
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _comboAttack;

		// Token: 0x04004321 RID: 17185
		[Space]
		[Header("Tools")]
		[SerializeField]
		private CharacterAnimation _characterAnimation;

		// Token: 0x04004322 RID: 17186
		[SerializeField]
		private AnimationClip _idleClipAfterWander;

		// Token: 0x04004323 RID: 17187
		[SerializeField]
		private Collider2D _comboAttackTrigger;

		// Token: 0x04004324 RID: 17188
		[SerializeField]
		[Range(0f, 1f)]
		private float _counterChance;

		// Token: 0x04004325 RID: 17189
		private bool _counter;
	}
}
