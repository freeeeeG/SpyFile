using System;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011C3 RID: 4547
	public sealed class BetweenTargetAndWall : Condition
	{
		// Token: 0x06005979 RID: 22905 RVA: 0x0010A480 File Offset: 0x00108680
		protected override bool Check(AIController controller)
		{
			Character target = controller.target;
			Character character = controller.character;
			Collider2D lastStandingCollider = controller.character.movement.controller.collisionState.lastStandingCollider;
			if (character.transform.position.x < lastStandingCollider.bounds.center.x)
			{
				return character.transform.position.x <= target.transform.position.x;
			}
			return character.transform.position.x >= target.transform.position.x;
		}
	}
}
