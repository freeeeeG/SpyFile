using System;
using Characters;
using Characters.AI;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014DC RID: 5340
	[TaskDescription("적이 같은 플랫폼에 올 때 까지 주변을 돌아다닌다.")]
	[TaskIcon("{SkinColor}StackedActionIcon.png")]
	public sealed class RangeWander : Action
	{
		// Token: 0x060067D2 RID: 26578 RVA: 0x0012C1D8 File Offset: 0x0012A3D8
		public override void OnAwake()
		{
			this._characterValue = this._character.Value;
			this._stopTriggerValue = this._stopTrigger.Value;
			this._idleTimeRangeValue = this._idleTimeRange.Value;
			this._wanderDistanceRangeValue = this._wanderDistanceRange.Value;
			this._needDirectionUpdate = true;
		}

		// Token: 0x060067D3 RID: 26579 RVA: 0x0012C230 File Offset: 0x0012A430
		public override void OnStart()
		{
			this._center = this._characterValue.transform.position;
		}

		// Token: 0x060067D4 RID: 26580 RVA: 0x0012C248 File Offset: 0x0012A448
		public override TaskStatus OnUpdate()
		{
			if (this._characterValue.stunedOrFreezed)
			{
				return TaskStatus.Running;
			}
			if (this.CheckStopWander())
			{
				this._needDirectionUpdate = true;
				return TaskStatus.Success;
			}
			if (!Precondition.CanMove(this._characterValue))
			{
				this._needDirectionUpdate = true;
				return TaskStatus.Running;
			}
			if (this._needDirectionUpdate)
			{
				if (this._remainIdleTime > 0f)
				{
					this._remainIdleTime -= this._characterValue.chronometer.master.deltaTime;
					return TaskStatus.Running;
				}
				Collider2D lastStandingCollider = this._characterValue.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					return TaskStatus.Failure;
				}
				Bounds bounds = lastStandingCollider.bounds;
				float num = UnityEngine.Random.Range(this._wanderDistanceRangeValue.x, this._wanderDistanceRangeValue.y);
				if (this._rightDirection)
				{
					this._destinationX = Mathf.Min(this._center.x + num, bounds.max.x - this._characterValue.collider.bounds.size.x / 2f);
				}
				else
				{
					this._destinationX = Mathf.Max(this._center.x - num, bounds.min.x + this._characterValue.collider.bounds.size.x / 2f);
				}
				this._needDirectionUpdate = false;
				if (this.CheckReachedToDestination(1f))
				{
					this.Turn();
					return TaskStatus.Running;
				}
			}
			if (this.CheckReachedToDestination(0.5f))
			{
				this.Turn();
				return TaskStatus.Running;
			}
			this._characterValue.movement.MoveTo(new Vector2(this._destinationX, this._characterValue.transform.position.y));
			return TaskStatus.Running;
		}

		// Token: 0x060067D5 RID: 26581 RVA: 0x0012C40C File Offset: 0x0012A60C
		private bool CheckReachedToDestination(float distance)
		{
			return Mathf.Abs(this._characterValue.transform.position.x - this._destinationX) < distance;
		}

		// Token: 0x060067D6 RID: 26582 RVA: 0x0012C432 File Offset: 0x0012A632
		private void Turn()
		{
			this._remainIdleTime = UnityEngine.Random.Range(this._idleTimeRangeValue.x, this._idleTimeRangeValue.y);
			this._needDirectionUpdate = true;
			this._rightDirection = !this._rightDirection;
		}

		// Token: 0x060067D7 RID: 26583 RVA: 0x0012C46C File Offset: 0x0012A66C
		private bool CheckStopWander()
		{
			return Precondition.CanChase(this._characterValue, this._target.Value) || TargetFinder.FindClosestTarget(this._stopTriggerValue, this._characterValue.collider, this._targetLayer.Evaluate(this._characterValue.gameObject));
		}

		// Token: 0x040053B9 RID: 21433
		[SerializeField]
		private SharedCharacter _character;

		// Token: 0x040053BA RID: 21434
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x040053BB RID: 21435
		[SerializeField]
		private SharedVector2 _wanderDistanceRange;

		// Token: 0x040053BC RID: 21436
		[SerializeField]
		private SharedVector2 _idleTimeRange;

		// Token: 0x040053BD RID: 21437
		[SerializeField]
		private SharedCollider _stopTrigger;

		// Token: 0x040053BE RID: 21438
		[SerializeField]
		private TargetLayer _targetLayer = new TargetLayer(0, false, true, false, false);

		// Token: 0x040053BF RID: 21439
		private Character _characterValue;

		// Token: 0x040053C0 RID: 21440
		private Collider2D _stopTriggerValue;

		// Token: 0x040053C1 RID: 21441
		private Vector2 _idleTimeRangeValue;

		// Token: 0x040053C2 RID: 21442
		private Vector2 _wanderDistanceRangeValue;

		// Token: 0x040053C3 RID: 21443
		private Vector3 _center;

		// Token: 0x040053C4 RID: 21444
		private bool _rightDirection;

		// Token: 0x040053C5 RID: 21445
		private bool _needDirectionUpdate;

		// Token: 0x040053C6 RID: 21446
		private float _remainIdleTime;

		// Token: 0x040053C7 RID: 21447
		private float _destinationX;
	}
}
