using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.Gear.Items.Customs
{
	// Token: 0x02000900 RID: 2304
	public sealed class ArchbishopsBibleMovement : MonoBehaviour
	{
		// Token: 0x06003121 RID: 12577 RVA: 0x000930EB File Offset: 0x000912EB
		public void StartMove(OperationInfos operationInfos)
		{
			this._owner = operationInfos.owner;
			this.FindTarget();
			this.FindPlatform();
			this._chaseReference.Stop();
			this._chaseReference = this.StartCoroutineWithReference(this.CChase());
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x00093122 File Offset: 0x00091322
		private void FindTarget()
		{
			this._target = TargetFinder.FindClosestTarget(this._findRange, this._collider, this._targetLayer.Evaluate(base.gameObject));
			this._remainFindTime = 1f;
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x00093158 File Offset: 0x00091358
		private void FindPlatform()
		{
			if (this._target != null)
			{
				this._platform = this._target.movement.controller.collisionState.lastStandingCollider;
				if (this._platform == null)
				{
					this._target.movement.TryGetClosestBelowCollider(out this._platform, Layers.footholdMask, 100f);
					return;
				}
			}
			else
			{
				this._platform = this._owner.movement.controller.collisionState.lastStandingCollider;
				if (this._platform == null)
				{
					this._owner.movement.TryGetClosestBelowCollider(out this._platform, Layers.footholdMask, 100f);
				}
			}
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x00093214 File Offset: 0x00091414
		private void MoveToTarget()
		{
			float deltaTime = Chronometer.global.deltaTime;
			Vector2 vector = new Vector2(this._target.transform.position.x, this._platform.bounds.max.y);
			if (vector.x >= this._platform.bounds.max.x)
			{
				vector.x = this._platform.bounds.max.x - this._collider.bounds.extents.x;
			}
			if (vector.x <= this._platform.bounds.min.x)
			{
				vector.x = this._platform.bounds.min.x + this._collider.bounds.extents.x;
			}
			base.transform.position = Vector3.Lerp(base.transform.position, vector, deltaTime * this._speed);
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x00093339 File Offset: 0x00091539
		private IEnumerator CChase()
		{
			for (;;)
			{
				yield return null;
				this._remainFindTime -= Chronometer.global.deltaTime;
				if (this._remainFindTime <= 0f)
				{
					this.FindTarget();
				}
				if (!(this._target == null) && !this._target.health.dead)
				{
					this.MoveToTarget();
				}
			}
			yield break;
		}

		// Token: 0x0400286D RID: 10349
		[SerializeField]
		private float _speed;

		// Token: 0x0400286E RID: 10350
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x0400286F RID: 10351
		[SerializeField]
		private Collider2D _findRange;

		// Token: 0x04002870 RID: 10352
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x04002871 RID: 10353
		private Character _owner;

		// Token: 0x04002872 RID: 10354
		private Character _target;

		// Token: 0x04002873 RID: 10355
		private CoroutineReference _chaseReference;

		// Token: 0x04002874 RID: 10356
		private float _remainFindTime;

		// Token: 0x04002875 RID: 10357
		private const float _findInterval = 1f;

		// Token: 0x04002876 RID: 10358
		private Collider2D _platform;
	}
}
