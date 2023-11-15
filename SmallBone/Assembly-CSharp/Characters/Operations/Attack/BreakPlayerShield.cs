using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F70 RID: 3952
	public sealed class BreakPlayerShield : Operation
	{
		// Token: 0x06004CAF RID: 19631 RVA: 0x000E376C File Offset: 0x000E196C
		public override void Run()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (player == null)
			{
				return;
			}
			if (player.health.shield.amount > 0.0)
			{
				Vector2 v = new Vector2(player.collider.bounds.center.x, player.collider.bounds.max.y);
				Singleton<Service>.Instance.floatingTextSpawner.SpawnBreakShield(v, "#F2F2F2");
			}
			while (player.health.shield.amount > 0.0)
			{
				player.health.shield.Consume(player.health.shield.amount + 1.0);
			}
		}
	}
}
