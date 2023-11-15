using System;
using Characters.Movements;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000789 RID: 1929
	public class MoveOwnerToProjectile : Operation
	{
		// Token: 0x06002795 RID: 10133 RVA: 0x00076F0C File Offset: 0x0007510C
		public override void Run(IProjectile projectile)
		{
			Movement movement = projectile.owner.movement;
			if (movement.controller.Teleport(projectile.transform.position, -projectile.firedDirection, 5f))
			{
				movement.verticalVelocity = 0f;
			}
		}
	}
}
