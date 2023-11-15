using System;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EF9 RID: 3833
	public class ToTargetOpposition : Policy
	{
		// Token: 0x06004B16 RID: 19222 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x000DCF58 File Offset: 0x000DB158
		public override Vector2 GetPosition()
		{
			Character character = this._ai.character;
			Character target = this._ai.target;
			if (target == null)
			{
				return character.transform.position;
			}
			if (!this._onPlatform)
			{
				return target.transform.position;
			}
			Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			Vector3 center = bounds.center;
			float x = this.CalculateX(target, ref bounds, center);
			float y = this.CalculateY(target, bounds);
			x = this.ClampX(character, x, bounds);
			return new Vector2(x, y);
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x000DD000 File Offset: 0x000DB200
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

		// Token: 0x06004B19 RID: 19225 RVA: 0x000DD08E File Offset: 0x000DB28E
		private float CalculateY(Character target, Bounds platform)
		{
			if (!this._onPlatform)
			{
				return target.transform.position.y;
			}
			return platform.max.y;
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x000DD0B8 File Offset: 0x000DB2B8
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

		// Token: 0x04003A4E RID: 14926
		[SerializeField]
		private AIController _ai;

		// Token: 0x04003A4F RID: 14927
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x04003A50 RID: 14928
		[SerializeField]
		private bool _randomX;
	}
}
