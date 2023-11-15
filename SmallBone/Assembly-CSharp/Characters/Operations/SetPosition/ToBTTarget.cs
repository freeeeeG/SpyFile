using System;
using BT;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EDF RID: 3807
	public class ToBTTarget : Policy
	{
		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06004AB3 RID: 19123 RVA: 0x000D9CB3 File Offset: 0x000D7EB3
		private Vector2 _default
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x000DA6C0 File Offset: 0x000D88C0
		public override Vector2 GetPosition()
		{
			Character character = this._bt.context.Get<Character>(Key.Target);
			if (character == null)
			{
				return base.transform.position;
			}
			if (!this._onPlatform)
			{
				return character.transform.position;
			}
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					character.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
					if (lastStandingCollider == null)
					{
						return this._default;
					}
				}
			}
			else
			{
				character.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
				if (lastStandingCollider == null)
				{
					return this._default;
				}
			}
			float x = character.transform.position.x;
			float y = lastStandingCollider.bounds.max.y;
			return new Vector2(x, y);
		}

		// Token: 0x040039D5 RID: 14805
		[SerializeField]
		private BehaviourTreeRunner _bt;

		// Token: 0x040039D6 RID: 14806
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x040039D7 RID: 14807
		[SerializeField]
		private bool _lastStandingCollider;
	}
}
