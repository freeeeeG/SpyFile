using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001336 RID: 4918
	public class RangeWander : Wander
	{
		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x06006103 RID: 24835 RVA: 0x0011C45A File Offset: 0x0011A65A
		// (set) Token: 0x06006104 RID: 24836 RVA: 0x0011C462 File Offset: 0x0011A662
		public Vector2? center
		{
			get
			{
				return this._center;
			}
			set
			{
				this._center = value;
			}
		}

		// Token: 0x06006105 RID: 24837 RVA: 0x0011C46B File Offset: 0x0011A66B
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			base.result = Behaviour.Result.Doing;
			Bounds platformBounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			bool right = true;
			for (;;)
			{
				yield return null;
				if (this.CheckStopWander(controller))
				{
					break;
				}
				if (Precondition.CanMove(character))
				{
					if (this._center == null)
					{
						this._center = new Vector2?(character.transform.position);
					}
					float num = UnityEngine.Random.Range(this._range.x, this._range.y);
					float x;
					if (right)
					{
						x = Mathf.Min(this._center.Value.x + num, platformBounds.max.x);
					}
					else
					{
						x = Mathf.Max(this._center.Value.x - num, platformBounds.min.x);
					}
					controller.destination = new Vector2(x, 0f);
					yield return this._move.CRun(controller);
					right = !right;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06006106 RID: 24838 RVA: 0x0011C481 File Offset: 0x0011A681
		private bool CheckStopWander(AIController controller)
		{
			if (Precondition.CanChase(controller.character, controller.target))
			{
				base.result = Behaviour.Result.Done;
				return true;
			}
			if (controller.FindClosestPlayerBody(this._sightRange))
			{
				base.result = Behaviour.Result.Done;
				return true;
			}
			return false;
		}

		// Token: 0x04004E3F RID: 20031
		[SerializeField]
		[MinMaxSlider(1f, 10f)]
		private Vector2 _range;

		// Token: 0x04004E40 RID: 20032
		private Vector2? _center;
	}
}
