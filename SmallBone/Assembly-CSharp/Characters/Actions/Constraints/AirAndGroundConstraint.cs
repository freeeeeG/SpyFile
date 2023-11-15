using System;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x0200096F RID: 2415
	public class AirAndGroundConstraint : Constraint
	{
		// Token: 0x0600340D RID: 13325 RVA: 0x0009A21F File Offset: 0x0009841F
		public override bool Pass()
		{
			return AirAndGroundConstraint.Pass(this._action.owner, this._state);
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x0009A238 File Offset: 0x00098438
		public static bool Pass(Character character, AirAndGroundConstraint.State state)
		{
			return (character.movement.controller.isGrounded && (state == AirAndGroundConstraint.State.Ground || (state == AirAndGroundConstraint.State.Terrain && character.movement.controller.onTerrain) || (state == AirAndGroundConstraint.State.Platform && character.movement.controller.onPlatform))) || (!character.movement.controller.isGrounded && (state == AirAndGroundConstraint.State.Air || (state == AirAndGroundConstraint.State.JumpUp && character.movement.controller.velocity.y > 0f) || (state == AirAndGroundConstraint.State.Fall && character.movement.controller.velocity.y < 0f)));
		}

		// Token: 0x04002A1E RID: 10782
		[SerializeField]
		protected AirAndGroundConstraint.State _state;

		// Token: 0x02000970 RID: 2416
		public enum State
		{
			// Token: 0x04002A20 RID: 10784
			Ground,
			// Token: 0x04002A21 RID: 10785
			Terrain,
			// Token: 0x04002A22 RID: 10786
			Platform,
			// Token: 0x04002A23 RID: 10787
			Air,
			// Token: 0x04002A24 RID: 10788
			JumpUp,
			// Token: 0x04002A25 RID: 10789
			Fall
		}
	}
}
