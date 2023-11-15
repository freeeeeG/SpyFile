using System;
using System.Collections;
using Level;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001315 RID: 4885
	public class MoveForDurationWithFly : Move
	{
		// Token: 0x06006082 RID: 24706 RVA: 0x0011AACF File Offset: 0x00118CCF
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			base.result = Behaviour.Result.Doing;
			base.StartCoroutine(base.CExpire(controller, this._duration));
			Vector2 direction = this.direction;
			Bounds bounds = Map.Instance.bounds;
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				character.movement.move = this.direction;
				this.ChangeDirectionIfBlocked(character, bounds);
			}
			this.idle.CRun(controller);
			yield break;
		}

		// Token: 0x06006083 RID: 24707 RVA: 0x0011AAE8 File Offset: 0x00118CE8
		private void ChangeDirectionIfBlocked(Character character, Bounds bounds)
		{
			if (character.transform.position.x + this.direction.x < bounds.min.x && character.lookingDirection == Character.LookingDirection.Left)
			{
				this.direction.x = 1f;
			}
			else if (character.transform.position.x + this.direction.x > bounds.max.x && character.lookingDirection == Character.LookingDirection.Right)
			{
				this.direction.x = -1f;
			}
			if (character.transform.position.y + this.direction.y < bounds.min.y)
			{
				this.direction.y = 1f;
				return;
			}
			if (character.transform.position.y + this.direction.y > bounds.max.y)
			{
				this.direction.y = -1f;
			}
		}

		// Token: 0x04004DC0 RID: 19904
		[MinMaxSlider(0f, 10f)]
		[SerializeField]
		private Vector2 _duration;
	}
}
