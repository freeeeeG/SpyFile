using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001308 RID: 4872
	public class Confusing : Behaviour
	{
		// Token: 0x0600604F RID: 24655 RVA: 0x00119C90 File Offset: 0x00117E90
		private void Start()
		{
			this._childs = new List<Behaviour>
			{
				this._moveForDuration
			};
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x00119CA9 File Offset: 0x00117EA9
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Collider2D platform = character.movement.controller.collisionState.lastStandingCollider;
			while (platform == null)
			{
				yield return null;
				platform = character.movement.controller.collisionState.lastStandingCollider;
			}
			int count = UnityEngine.Random.Range((int)this._turnCount.x, (int)this._turnCount.y);
			base.result = Behaviour.Result.Doing;
			for (;;)
			{
				int num = count;
				count = num - 1;
				if (num <= 0 || base.result != Behaviour.Result.Doing)
				{
					break;
				}
				float duration = UnityEngine.Random.Range(this._speedBonusDuration.x, this._speedBonusDuration.y);
				Vector2 vector;
				if (Mathf.Abs(character.transform.position.x - platform.bounds.max.x) < 1f)
				{
					vector = Vector2.left;
				}
				else if (Mathf.Abs(character.transform.position.x - platform.bounds.min.x) < 1f)
				{
					vector = Vector2.right;
				}
				else if (MMMaths.RandomBool())
				{
					vector = Vector2.right;
				}
				else
				{
					vector = Vector2.left;
				}
				if (this.allowBackward && MMMaths.RandomBool())
				{
					character.movement.moveBackward = true;
				}
				character.stat.DetachTimedValues(Confusing._movementspeedBonus);
				character.stat.AttachTimedValues(Confusing._movementspeedBonus, duration, null);
				this._moveForDuration.direction = vector;
				character.movement.move = vector;
				yield return this._moveForDuration.CRun(controller);
				character.movement.moveBackward = false;
			}
			character.stat.DetachTimedValues(Confusing._movementspeedBonus);
			yield break;
		}

		// Token: 0x04004D8C RID: 19852
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2 _speedBonusDuration;

		// Token: 0x04004D8D RID: 19853
		[MinMaxSlider(0f, 10f)]
		[SerializeField]
		private Vector2 _turnCount;

		// Token: 0x04004D8E RID: 19854
		[Move.SubcomponentAttribute(true)]
		[SerializeField]
		private MoveForDuration _moveForDuration;

		// Token: 0x04004D8F RID: 19855
		private static Stat.Values _movementspeedBonus = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.MovementSpeed, 1.0)
		});

		// Token: 0x04004D90 RID: 19856
		[SerializeField]
		private bool allowBackward = true;
	}
}
