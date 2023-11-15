using System;
using System.Collections;
using Characters.Abilities;
using Characters.AI.Behaviours;
using Runnables.States;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010F6 RID: 4342
	public sealed class GiantMushroomEnt : AIController
	{
		// Token: 0x06005459 RID: 21593 RVA: 0x000FC682 File Offset: 0x000FA882
		private void Awake()
		{
			this._onGroggy.Initialize();
			this.SetNormalAnimationClip();
		}

		// Token: 0x0600545A RID: 21594 RVA: 0x000FC695 File Offset: 0x000FA895
		private void OnDestroy()
		{
			this._idleClip = null;
			this._walkClip = null;
			this._idleOnWeakClip = null;
			this._walkOnWeakClip = null;
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x000FC6B3 File Offset: 0x000FA8B3
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this.CCheckWeakable());
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x000FC6E8 File Offset: 0x000FA8E8
		private IEnumerator CCheckWeakable()
		{
			while (this._stateMachine.currentState != this._weakState)
			{
				yield return null;
			}
			base.StartCoroutine(this.CReturnToNormalState(this._weakStateTime));
			yield break;
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x000FC6F7 File Offset: 0x000FA8F7
		private IEnumerator CReturnToNormalState(float delay)
		{
			yield return this.CGroggy();
			yield return this.character.chronometer.master.WaitForSeconds(delay);
			yield return this.CRecover();
			yield break;
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x000FC70D File Offset: 0x000FA90D
		private IEnumerator CGroggy()
		{
			this._sequenceLock = true;
			this.character.CancelAction();
			this._attackOnWeak.StopPropagation();
			this._attack.StopPropagation();
			this.SetWeakAnimationClip();
			this.character.ability.Add(this._onGroggy.ability);
			yield return this._groggy.CRun(this);
			this._sequenceLock = false;
			yield break;
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x000FC71C File Offset: 0x000FA91C
		private IEnumerator CRecover()
		{
			this._sequenceLock = true;
			this._stateMachine.TransitTo(this._normalState);
			this.SetNormalAnimationClip();
			this.character.CancelAction();
			this._attackOnWeak.StopPropagation();
			this._attack.StopPropagation();
			yield return this._recover.CRun(this);
			this.character.ability.Remove(this._onGroggy.ability);
			this._platformCollider.enabled = true;
			this._sequenceLock = false;
			base.StartCoroutine(this.CCheckWeakable());
			yield break;
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x000FC72B File Offset: 0x000FA92B
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			while (!base.dead)
			{
				if (base.stuned || this._sequenceLock)
				{
					yield return null;
				}
				else if (this._stateMachine.currentState == this._weakState)
				{
					yield return this._attackOnWeak.CRun(this);
				}
				else
				{
					yield return this._attack.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x000FC73A File Offset: 0x000FA93A
		private void SetWeakAnimationClip()
		{
			this._charactrerAnimation.SetIdle(this._idleOnWeakClip);
			this._charactrerAnimation.SetWalk(this._walkOnWeakClip);
		}

		// Token: 0x06005462 RID: 21602 RVA: 0x000FC75E File Offset: 0x000FA95E
		private void SetNormalAnimationClip()
		{
			this._charactrerAnimation.SetIdle(this._idleClip);
			this._charactrerAnimation.SetWalk(this._walkClip);
		}

		// Token: 0x040043B3 RID: 17331
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040043B4 RID: 17332
		[Subcomponent(typeof(Wander))]
		[SerializeField]
		private Wander _wander;

		// Token: 0x040043B5 RID: 17333
		[SerializeField]
		[Subcomponent(typeof(ChaseAndAttack))]
		private ChaseAndAttack _attack;

		// Token: 0x040043B6 RID: 17334
		[Subcomponent(typeof(ChaseAndAttack))]
		[SerializeField]
		private ChaseAndAttack _attackOnWeak;

		// Token: 0x040043B7 RID: 17335
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _groggy;

		// Token: 0x040043B8 RID: 17336
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _recover;

		// Token: 0x040043B9 RID: 17337
		[SerializeField]
		private float _weakStateTime = 3f;

		// Token: 0x040043BA RID: 17338
		[SerializeField]
		private CharacterAnimation _charactrerAnimation;

		// Token: 0x040043BB RID: 17339
		[SerializeField]
		private AnimationClip _idleClip;

		// Token: 0x040043BC RID: 17340
		[SerializeField]
		private AnimationClip _walkClip;

		// Token: 0x040043BD RID: 17341
		[SerializeField]
		private AnimationClip _idleOnWeakClip;

		// Token: 0x040043BE RID: 17342
		[SerializeField]
		private AnimationClip _walkOnWeakClip;

		// Token: 0x040043BF RID: 17343
		[SerializeField]
		private State _normalState;

		// Token: 0x040043C0 RID: 17344
		[SerializeField]
		private State _weakState;

		// Token: 0x040043C1 RID: 17345
		[SerializeField]
		private StateMachine _stateMachine;

		// Token: 0x040043C2 RID: 17346
		[SerializeField]
		private Collider2D _platformCollider;

		// Token: 0x040043C3 RID: 17347
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _onGroggy;

		// Token: 0x040043C4 RID: 17348
		private bool _sequenceLock;
	}
}
