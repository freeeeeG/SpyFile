using System;
using Characters.Controllers;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000975 RID: 2421
	public class DirectionConstraint : Constraint
	{
		// Token: 0x06003421 RID: 13345 RVA: 0x0009A56B File Offset: 0x0009876B
		public override bool Pass()
		{
			return DirectionConstraint.Pass(this._action.owner, this._direcion);
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x0009A584 File Offset: 0x00098784
		public static bool Pass(Character character, DirectionConstraint.Direction direction)
		{
			PlayerInput component = character.GetComponent<PlayerInput>();
			if (component == null)
			{
				return false;
			}
			DirectionConstraint.Direction direction2 = (DirectionConstraint.Direction)0;
			if (component.direction.y > 0.66f)
			{
				direction2 |= DirectionConstraint.Direction.Up;
			}
			if (component.direction.y < -0.66f)
			{
				direction2 |= DirectionConstraint.Direction.Down;
			}
			if (component.direction.x < -0.66f)
			{
				direction2 |= DirectionConstraint.Direction.Left;
			}
			if (component.direction.x > 0.66f)
			{
				direction2 |= DirectionConstraint.Direction.Right;
			}
			return (direction & direction2) == direction;
		}

		// Token: 0x04002A2C RID: 10796
		public const float threshold = 0.66f;

		// Token: 0x04002A2D RID: 10797
		[SerializeField]
		[EnumFlag]
		protected DirectionConstraint.Direction _direcion;

		// Token: 0x02000976 RID: 2422
		[Flags]
		public enum Direction
		{
			// Token: 0x04002A2F RID: 10799
			Up = 1,
			// Token: 0x04002A30 RID: 10800
			Down = 2,
			// Token: 0x04002A31 RID: 10801
			Left = 4,
			// Token: 0x04002A32 RID: 10802
			Right = 8
		}
	}
}
