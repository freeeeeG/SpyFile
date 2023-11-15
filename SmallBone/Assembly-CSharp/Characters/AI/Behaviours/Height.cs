using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012C3 RID: 4803
	public class Height : Decorator
	{
		// Token: 0x06005F12 RID: 24338 RVA: 0x00116842 File Offset: 0x00114A42
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			Character character = controller.character;
			Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
			Height.Comparer comparer;
			if (!this._lastStandingCollider || lastStandingCollider == null)
			{
				Collider2D collider2D;
				if (!character.movement.TryGetClosestBelowCollider(out collider2D, this._terrainLayer, 100f))
				{
					base.result = Behaviour.Result.Fail;
					yield break;
				}
				comparer = this._comparer;
				if (comparer != Height.Comparer.Greater)
				{
					if (comparer == Height.Comparer.Lesser)
					{
						if (character.transform.position.y - collider2D.bounds.max.y < this._diff)
						{
							base.result = Behaviour.Result.Success;
						}
						else
						{
							base.result = Behaviour.Result.Fail;
						}
					}
				}
				else if (character.transform.position.y - collider2D.bounds.max.y >= this._diff)
				{
					base.result = Behaviour.Result.Success;
				}
				else
				{
					base.result = Behaviour.Result.Fail;
				}
			}
			comparer = this._comparer;
			if (comparer != Height.Comparer.Greater)
			{
				if (comparer == Height.Comparer.Lesser)
				{
					if (character.transform.position.y - lastStandingCollider.bounds.max.y < this._diff)
					{
						base.result = Behaviour.Result.Success;
					}
					else
					{
						base.result = Behaviour.Result.Fail;
					}
				}
			}
			else if (character.transform.position.y - lastStandingCollider.bounds.max.y >= this._diff)
			{
				base.result = Behaviour.Result.Success;
			}
			else
			{
				base.result = Behaviour.Result.Fail;
			}
			yield break;
		}

		// Token: 0x04004C59 RID: 19545
		[SerializeField]
		private Height.Comparer _comparer;

		// Token: 0x04004C5A RID: 19546
		[SerializeField]
		private float _diff;

		// Token: 0x04004C5B RID: 19547
		[SerializeField]
		private bool _lastStandingCollider;

		// Token: 0x04004C5C RID: 19548
		[SerializeField]
		private LayerMask _terrainLayer = Layers.groundMask;

		// Token: 0x020012C4 RID: 4804
		private enum Comparer
		{
			// Token: 0x04004C5E RID: 19550
			Greater,
			// Token: 0x04004C5F RID: 19551
			Lesser
		}
	}
}
