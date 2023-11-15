using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010D5 RID: 4309
	public sealed class HolySpearManAI : AIController
	{
		// Token: 0x060053B0 RID: 21424 RVA: 0x000FAE60 File Offset: 0x000F9060
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._lightJavelin,
				this._attack,
				this._upperSlash
			};
			this.character.health.onTookDamage += new TookDamageDelegate(this.TryCounterAttack);
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x000FAEDC File Offset: 0x000F90DC
		private void OnDestroy()
		{
			this._idleClipAfterWander = null;
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x000FAEE8 File Offset: 0x000F90E8
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
			if (this._lightJavelin.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				return;
			}
			if (this._attack.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				return;
			}
			if (this._upperSlash.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				return;
			}
			if (this._counterAttack.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
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

		// Token: 0x060053B3 RID: 21427 RVA: 0x000FAF98 File Offset: 0x000F9198
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x000FAFC0 File Offset: 0x000F91C0
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x000FAFCF File Offset: 0x000F91CF
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
					if (base.FindClosestPlayerBody(this._attackTrigger) != null)
					{
						yield return this._attack.CRun(this);
					}
					else if (base.FindClosestPlayerBody(this._lightJavelinTrigger) != null && this._lightJavelin.CanUse())
					{
						yield return this._lightJavelin.CRun(this);
					}
					else
					{
						yield return this._chase.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x04004339 RID: 17209
		[Header("Behaviours")]
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400433A RID: 17210
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x0400433B RID: 17211
		[SerializeField]
		[Subcomponent(typeof(Chase))]
		private Chase _chase;

		// Token: 0x0400433C RID: 17212
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _counterAttack;

		// Token: 0x0400433D RID: 17213
		[SerializeField]
		[Subcomponent(typeof(CircularProjectileAttack))]
		private CircularProjectileAttack _lightJavelin;

		// Token: 0x0400433E RID: 17214
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;

		// Token: 0x0400433F RID: 17215
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _upperSlash;

		// Token: 0x04004340 RID: 17216
		[Header("Tools")]
		[Space]
		[SerializeField]
		private CharacterAnimation _characterAnimation;

		// Token: 0x04004341 RID: 17217
		[SerializeField]
		private AnimationClip _idleClipAfterWander;

		// Token: 0x04004342 RID: 17218
		[SerializeField]
		private Collider2D _attackTrigger;

		// Token: 0x04004343 RID: 17219
		[SerializeField]
		private Collider2D _lightJavelinTrigger;

		// Token: 0x04004344 RID: 17220
		[SerializeField]
		[Range(0f, 1f)]
		private float _counterChance;

		// Token: 0x04004345 RID: 17221
		private bool _counter;
	}
}
