using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EDB RID: 3803
	public class ToBDTargetOpposition : Policy
	{
		// Token: 0x06004AA5 RID: 19109 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x000DA114 File Offset: 0x000D8314
		public override Vector2 GetPosition()
		{
			SharedCharacter sharedCharacter = this._tree.GetVariable(this._ownerValueName) as SharedCharacter;
			SharedCharacter sharedCharacter2 = this._tree.GetVariable(this._targetValueName) as SharedCharacter;
			if (sharedCharacter2 == null)
			{
				return base.transform.position;
			}
			Character value = sharedCharacter2.Value;
			Character value2 = sharedCharacter.Value;
			if (!this._onPlatform)
			{
				return value.transform.position;
			}
			Bounds bounds = value.movement.controller.collisionState.lastStandingCollider.bounds;
			Vector3 center = bounds.center;
			float x = this.CalculateX(value, ref bounds, center);
			float y = this.CalculateY(value, bounds);
			x = this.ClampX(value2, x, bounds);
			return new Vector2(x, y);
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x000DA1E0 File Offset: 0x000D83E0
		private float ClampX(Character owner, float x, Bounds platform)
		{
			if (x <= platform.min.x + owner.collider.size.x)
			{
				x = platform.min.x + owner.collider.size.x;
			}
			else if (x >= platform.max.x - owner.collider.size.x)
			{
				x = platform.max.x - owner.collider.size.x;
			}
			return x;
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x000DA26E File Offset: 0x000D846E
		private float CalculateY(Character target, Bounds platform)
		{
			if (!this._onPlatform)
			{
				return target.transform.position.y;
			}
			return platform.max.y;
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x000DA298 File Offset: 0x000D8498
		private float CalculateX(Character target, ref Bounds platform, Vector3 center)
		{
			float result;
			if (target.transform.position.x > center.x)
			{
				result = (this._randomX ? UnityEngine.Random.Range(platform.min.x, platform.center.x) : platform.min.x);
			}
			else
			{
				result = (this._randomX ? UnityEngine.Random.Range(platform.center.x, platform.max.x) : platform.max.x);
			}
			return result;
		}

		// Token: 0x040039C0 RID: 14784
		[SerializeField]
		private BehaviorTree _tree;

		// Token: 0x040039C1 RID: 14785
		[SerializeField]
		private string _ownerValueName = "Owner";

		// Token: 0x040039C2 RID: 14786
		[SerializeField]
		private string _targetValueName = "Target";

		// Token: 0x040039C3 RID: 14787
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x040039C4 RID: 14788
		[SerializeField]
		private bool _randomX;
	}
}
