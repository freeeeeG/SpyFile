using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010FE RID: 4350
	public sealed class TrolleyMaid : AIController
	{
		// Token: 0x0600548F RID: 21647 RVA: 0x000FCF82 File Offset: 0x000FB182
		private void Awake()
		{
			this._onRush.Initialize();
		}

		// Token: 0x06005490 RID: 21648 RVA: 0x000FCF8F File Offset: 0x000FB18F
		private void OnDestroy()
		{
			this._rushWalk = null;
		}

		// Token: 0x06005491 RID: 21649 RVA: 0x000FCF98 File Offset: 0x000FB198
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005492 RID: 21650 RVA: 0x000FCFC0 File Offset: 0x000FB1C0
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			while (this.platformBounds == null)
			{
				if (this.character.movement.controller.collisionState.lastStandingCollider)
				{
					this.platformBounds = new Bounds?(this.character.movement.controller.collisionState.lastStandingCollider.bounds);
					break;
				}
				yield return null;
			}
			yield return this.CRunFirstRush();
			while (!base.dead)
			{
				if (base.stuned)
				{
					yield return null;
				}
				else
				{
					this.platformBounds = new Bounds?(this.character.movement.controller.collisionState.lastStandingCollider.bounds);
					yield return this.CRush();
					yield return this._turn.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x06005493 RID: 21651 RVA: 0x000FCFCF File Offset: 0x000FB1CF
		private IEnumerator CRunFirstRush()
		{
			this.<CRunFirstRush>g__SetDestination|15_0();
			this._charactrerAnimation.SetWalk(this._rushWalk);
			yield return this._rushReady.CRun(this);
			base.StartCoroutine(this._onRush.CRun(this.character));
			if (base.stuned)
			{
				yield break;
			}
			yield return this._rush.CRun(this);
			this._onRush.StopAll();
			if (base.stuned)
			{
				yield break;
			}
			yield return this._turn.CRun(this);
			yield break;
		}

		// Token: 0x06005494 RID: 21652 RVA: 0x000FCFDE File Offset: 0x000FB1DE
		private IEnumerator CRush()
		{
			this.<CRush>g__SetDestination|16_0();
			base.StartCoroutine(this._onRush.CRun(this.character));
			yield return this._rush.CRun(this);
			this._onRush.StopAll();
			yield break;
		}

		// Token: 0x06005496 RID: 21654 RVA: 0x000FD000 File Offset: 0x000FB200
		[CompilerGenerated]
		private void <CRunFirstRush>g__SetDestination|15_0()
		{
			Vector3 position = base.target.transform.position;
			Bounds bounds = this.character.collider.bounds;
			float num = bounds.max.x - bounds.center.x;
			float num2 = bounds.center.x - bounds.min.x;
			if (this.character.transform.position.x < position.x)
			{
				base.destination = new Vector2(this.platformBounds.Value.max.x - num - this._distanceFromEdgeToTurn, this.platformBounds.Value.max.y);
				return;
			}
			base.destination = new Vector2(this.platformBounds.Value.min.x - num2 + this._distanceFromEdgeToTurn, this.platformBounds.Value.max.y);
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x000FD110 File Offset: 0x000FB310
		[CompilerGenerated]
		private void <CRush>g__SetDestination|16_0()
		{
			Bounds bounds = this.character.collider.bounds;
			float num = bounds.max.x - bounds.center.x;
			float num2 = bounds.center.x - bounds.min.x;
			if (this.character.transform.position.x < this.platformBounds.Value.center.x)
			{
				base.destination = new Vector2(this.platformBounds.Value.max.x - num - this._distanceFromEdgeToTurn, this.platformBounds.Value.max.y);
				return;
			}
			base.destination = new Vector2(this.platformBounds.Value.min.x + num2 + this._distanceFromEdgeToTurn, this.platformBounds.Value.max.y);
		}

		// Token: 0x040043E0 RID: 17376
		[Tooltip("Turn Action의 이동량과 연관있음, 땅 끝에서 떨어진다면 값을 키울 것")]
		[SerializeField]
		private float _distanceFromEdgeToTurn = 2f;

		// Token: 0x040043E1 RID: 17377
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040043E2 RID: 17378
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x040043E3 RID: 17379
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _rushReady;

		// Token: 0x040043E4 RID: 17380
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _rush;

		// Token: 0x040043E5 RID: 17381
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _turn;

		// Token: 0x040043E6 RID: 17382
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x040043E7 RID: 17383
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onRush;

		// Token: 0x040043E8 RID: 17384
		[SerializeField]
		private CharacterAnimation _charactrerAnimation;

		// Token: 0x040043E9 RID: 17385
		[SerializeField]
		private AnimationClip _rushWalk;

		// Token: 0x040043EA RID: 17386
		private Bounds? platformBounds;
	}
}
