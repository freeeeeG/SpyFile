using System;
using Characters;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200149E RID: 5278
	[TaskDescription("Player의 현재 높이가 발판에서 얼만큼 떨어져 있는가")]
	public sealed class CanUseDarkHeroWallRaid : Conditional
	{
		// Token: 0x060066F7 RID: 26359 RVA: 0x00129B55 File Offset: 0x00127D55
		public override void OnAwake()
		{
			this._nonAllocCaster = new NonAllocCaster(1);
			this._nonAllocCaster.contactFilter.SetLayerMask(256);
		}

		// Token: 0x060066F8 RID: 26360 RVA: 0x00129B80 File Offset: 0x00127D80
		public override TaskStatus OnUpdate()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			Vector2 vector = player.collider.bounds.center - this._originTransform.Value.position;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			Character value = this._owner.Value;
			if (num < 0f)
			{
				num += 360f;
			}
			if (value.lookingDirection == Character.LookingDirection.Right)
			{
				if (num > 90f && num < 270f)
				{
					vector = Vector2.down;
				}
				else if (num > 0f && num < 90f)
				{
					vector = Vector2.right;
				}
			}
			else if (value.lookingDirection == Character.LookingDirection.Left)
			{
				if (num < 90f || num > 270f)
				{
					vector = Vector2.down;
				}
				else if (num > 90f && num < 180f)
				{
					vector = Vector2.left;
				}
			}
			this._nonAllocCaster.RayCast(this._originTransform.Value.transform.position, vector, 30f);
			Transform value2 = this._destinationTransform.Value;
			Bounds bounds = player.movement.controller.collisionState.lastStandingCollider.bounds;
			if (this._nonAllocCaster.results.Count == 0)
			{
				value2.position = new Vector2(bounds.center.x, bounds.max.y);
				return TaskStatus.Failure;
			}
			Vector2 point = this._nonAllocCaster.results[0].point;
			if (point.x >= value.transform.position.x)
			{
				point = new Vector2(point.x - value.collider.bounds.extents.x, point.y);
			}
			else
			{
				point = new Vector2(point.x + value.collider.bounds.extents.x, point.y);
			}
			value2.position = point;
			float num2 = point.y - bounds.max.y;
			CanUseDarkHeroWallRaid.Compare comparer = this._comparer;
			if (comparer != CanUseDarkHeroWallRaid.Compare.GreatherThan)
			{
				if (comparer != CanUseDarkHeroWallRaid.Compare.LessThan)
				{
					return TaskStatus.Failure;
				}
				if (num2 < this._height.Value)
				{
					return TaskStatus.Success;
				}
				return TaskStatus.Failure;
			}
			else
			{
				if (num2 > this._height.Value)
				{
					return TaskStatus.Success;
				}
				return TaskStatus.Failure;
			}
		}

		// Token: 0x040052D0 RID: 21200
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040052D1 RID: 21201
		[SerializeField]
		private SharedTransform _originTransform;

		// Token: 0x040052D2 RID: 21202
		[SerializeField]
		private SharedTransform _destinationTransform;

		// Token: 0x040052D3 RID: 21203
		[SerializeField]
		private SharedFloat _height;

		// Token: 0x040052D4 RID: 21204
		[SerializeField]
		private CanUseDarkHeroWallRaid.Compare _comparer;

		// Token: 0x040052D5 RID: 21205
		private NonAllocCaster _nonAllocCaster;

		// Token: 0x0200149F RID: 5279
		private enum Compare
		{
			// Token: 0x040052D7 RID: 21207
			GreatherThan,
			// Token: 0x040052D8 RID: 21208
			LessThan
		}
	}
}
