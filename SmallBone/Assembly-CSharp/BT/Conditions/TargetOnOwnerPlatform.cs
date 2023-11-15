using System;
using Characters;
using UnityEngine;

namespace BT.Conditions
{
	// Token: 0x02001432 RID: 5170
	public class TargetOnOwnerPlatform : Condition
	{
		// Token: 0x0600656F RID: 25967 RVA: 0x001258E8 File Offset: 0x00123AE8
		protected override bool Check(Context context)
		{
			Character character = context.Get<Character>(Key.Target);
			Character character2 = context.Get<Character>(Key.OwnerCharacter);
			if (character == null || character2 == null)
			{
				return false;
			}
			UnityEngine.Object lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
			Collider2D lastStandingCollider2 = character2.movement.controller.collisionState.lastStandingCollider;
			return !(lastStandingCollider != lastStandingCollider2);
		}
	}
}
