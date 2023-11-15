using System;
using System.Collections;
using Level.Objects.DecorationCharacter.Movements;
using UnityEngine;

namespace Level.Objects.DecorationCharacter.Command
{
	// Token: 0x0200057E RID: 1406
	[Serializable]
	public class MoveCommand : ICommand
	{
		// Token: 0x06001BA0 RID: 7072 RVA: 0x00056054 File Offset: 0x00054254
		public IEnumerator CRun()
		{
			this._isRight = MMMaths.RandomBool();
			Collider2D lastStandingCollider = this._movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider != null)
			{
				Bounds bounds = lastStandingCollider.bounds;
				float value = this._moveRange.value;
				if (this._isRight)
				{
					this._destination = new Vector2(Mathf.Min(this._owner.transform.position.x + value, bounds.max.x - this._owner.collider.bounds.extents.x), 0f);
				}
				else
				{
					this._destination = new Vector2(Mathf.Max(this._owner.transform.position.x - value, bounds.min.x + this._owner.collider.bounds.extents.x), 0f);
				}
				while (Mathf.Abs(this._owner.transform.position.x - this._destination.x) > 1f)
				{
					this._movement.Move(this._isRight ? 0f : 3.1415927f);
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x040017BF RID: 6079
		private const float PI = 3.1415927f;

		// Token: 0x040017C0 RID: 6080
		[SerializeField]
		private DecorationCharacter _owner;

		// Token: 0x040017C1 RID: 6081
		[SerializeField]
		private DecorationCharacterMovement _movement;

		// Token: 0x040017C2 RID: 6082
		[SerializeField]
		private CustomFloat _moveRange;

		// Token: 0x040017C3 RID: 6083
		private bool _isRight = true;

		// Token: 0x040017C4 RID: 6084
		private Vector2 _destination;
	}
}
