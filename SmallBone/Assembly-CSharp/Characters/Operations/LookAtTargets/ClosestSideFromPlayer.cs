using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000EFE RID: 3838
	public sealed class ClosestSideFromPlayer : Target
	{
		// Token: 0x06004B24 RID: 19236 RVA: 0x000DD294 File Offset: 0x000DB494
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (player == null)
			{
				return character.lookingDirection;
			}
			Collider2D lastStandingCollider = player.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null && !player.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f))
			{
				return character.lookingDirection;
			}
			if (player.transform.position.x > lastStandingCollider.bounds.center.x)
			{
				if (!this._farthest)
				{
					return Character.LookingDirection.Right;
				}
				return Character.LookingDirection.Left;
			}
			else
			{
				if (!this._farthest)
				{
					return Character.LookingDirection.Left;
				}
				return Character.LookingDirection.Right;
			}
		}

		// Token: 0x04003A56 RID: 14934
		[SerializeField]
		private bool _farthest;
	}
}
