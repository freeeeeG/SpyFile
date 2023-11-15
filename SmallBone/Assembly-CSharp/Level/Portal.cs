using System;
using Characters;
using UnityEngine;

namespace Level
{
	// Token: 0x02000506 RID: 1286
	public class Portal : MonoBehaviour
	{
		// Token: 0x06001956 RID: 6486 RVA: 0x0004F788 File Offset: 0x0004D988
		private void OnTriggerEnter2D(Collider2D collision)
		{
			Character component = collision.gameObject.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			this.Teleport(component);
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0004F7B4 File Offset: 0x0004D9B4
		private void OnTriggerStay2D(Collider2D collision)
		{
			Character component = collision.gameObject.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			this.Teleport(component);
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x0004F7E0 File Offset: 0x0004D9E0
		private void Teleport(Character character)
		{
			switch (this._direction)
			{
			case Portal.Direction.Up:
				if (character.movement.velocity.y >= 0f)
				{
					character.transform.position = new Vector3(character.transform.position.x, this._targetTransform.position.y);
					return;
				}
				break;
			case Portal.Direction.Down:
				if (character.movement.velocity.y <= 0f)
				{
					character.transform.position = new Vector3(character.transform.position.x, this._targetTransform.position.y);
					return;
				}
				break;
			case Portal.Direction.Left:
				if (character.movement.velocity.x <= 0f)
				{
					character.transform.position = new Vector3(this._targetTransform.position.x, character.transform.position.y);
					return;
				}
				break;
			case Portal.Direction.Right:
				if (character.movement.velocity.x >= 0f)
				{
					character.transform.position = new Vector3(this._targetTransform.position.x, character.transform.position.y);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x04001618 RID: 5656
		private const float _maxRemainTime = 1f;

		// Token: 0x04001619 RID: 5657
		[SerializeField]
		private Transform _targetTransform;

		// Token: 0x0400161A RID: 5658
		[SerializeField]
		private Portal.Direction _direction;

		// Token: 0x02000507 RID: 1287
		private enum Direction
		{
			// Token: 0x0400161C RID: 5660
			Up,
			// Token: 0x0400161D RID: 5661
			Down,
			// Token: 0x0400161E RID: 5662
			Left,
			// Token: 0x0400161F RID: 5663
			Right
		}
	}
}
