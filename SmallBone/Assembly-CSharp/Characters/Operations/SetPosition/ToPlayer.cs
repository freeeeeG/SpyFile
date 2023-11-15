using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EF2 RID: 3826
	public class ToPlayer : Policy
	{
		// Token: 0x06004AF8 RID: 19192 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x000DC450 File Offset: 0x000DA650
		public override Vector2 GetPosition()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (!this._onPlatform)
			{
				return player.transform.position;
			}
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = player.movement.controller.collisionState.lastStandingCollider;
			}
			else if (!player.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, this._belowRayLength))
			{
				return player.transform.position;
			}
			float x = player.transform.position.x;
			float y = lastStandingCollider.bounds.max.y;
			return new Vector2(x, y);
		}

		// Token: 0x04003A2D RID: 14893
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x04003A2E RID: 14894
		[SerializeField]
		private bool _lastStandingCollider;

		// Token: 0x04003A2F RID: 14895
		[SerializeField]
		private float _belowRayLength = 100f;
	}
}
