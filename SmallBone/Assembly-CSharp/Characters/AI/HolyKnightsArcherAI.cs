using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010C1 RID: 4289
	public sealed class HolyKnightsArcherAI : AIController
	{
		// Token: 0x06005332 RID: 21298 RVA: 0x000F9760 File Offset: 0x000F7960
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._sniping,
				this._lightRain,
				this._backStepShot
			};
			this.character.health.onTookDamage += new TookDamageDelegate(this.TryCounterAttack);
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x000F97DC File Offset: 0x000F79DC
		private void OnDestroy()
		{
			this._idleClipAfterWander = null;
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x000F97E8 File Offset: 0x000F79E8
		private void TryCounterAttack(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (this._counter)
			{
				return;
			}
			if (!this._backStepShot.CanUse())
			{
				return;
			}
			if (this.character.health.dead || base.dead || this.character.health.percent <= damageDealt)
			{
				this._counter = true;
				return;
			}
			if (this._backStepShot.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				return;
			}
			if (this._lightRain.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				return;
			}
			if (this._sniping.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
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

		// Token: 0x06005335 RID: 21301 RVA: 0x000F9889 File Offset: 0x000F7A89
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x000F98B1 File Offset: 0x000F7AB1
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x000F98C0 File Offset: 0x000F7AC0
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
						yield return this._backStepShot.CRun(this);
						this._counter = false;
					}
					if (base.FindClosestPlayerBody(this._backstepShotTrigger) != null && this._backStepShot.CanUse())
					{
						yield return this._backStepShot.CRun(this);
					}
					else if (base.FindClosestPlayerBody(this._lightRainTrigger) != null && this._lightRain.CanUse())
					{
						yield return this._lightRain.CRun(this);
					}
					else if (base.FindClosestPlayerBody(this._snipingTrigger) != null && this._sniping.CanUse())
					{
						yield return this._sniping.CRun(this);
					}
					else
					{
						yield return this._chase.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x040042D0 RID: 17104
		[Header("Behaviours")]
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040042D1 RID: 17105
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x040042D2 RID: 17106
		[SerializeField]
		[Subcomponent(typeof(Chase))]
		private Chase _chase;

		// Token: 0x040042D3 RID: 17107
		[Subcomponent(typeof(HorizontalProjectileAttack))]
		[SerializeField]
		private HorizontalProjectileAttack _sniping;

		// Token: 0x040042D4 RID: 17108
		[SerializeField]
		[Subcomponent(typeof(HorizontalProjectileAttack))]
		private HorizontalProjectileAttack _lightRain;

		// Token: 0x040042D5 RID: 17109
		[SerializeField]
		[Subcomponent(typeof(HorizontalProjectileAttack))]
		private HorizontalProjectileAttack _backStepShot;

		// Token: 0x040042D6 RID: 17110
		[SerializeField]
		[Header("Tools")]
		[Space]
		private CharacterAnimation _characterAnimation;

		// Token: 0x040042D7 RID: 17111
		[SerializeField]
		private AnimationClip _idleClipAfterWander;

		// Token: 0x040042D8 RID: 17112
		[SerializeField]
		private Collider2D _snipingTrigger;

		// Token: 0x040042D9 RID: 17113
		[SerializeField]
		private Collider2D _lightRainTrigger;

		// Token: 0x040042DA RID: 17114
		[SerializeField]
		private Collider2D _backstepShotTrigger;

		// Token: 0x040042DB RID: 17115
		[Range(0f, 1f)]
		[SerializeField]
		private float _counterChance;

		// Token: 0x040042DC RID: 17116
		private bool _counter;
	}
}
