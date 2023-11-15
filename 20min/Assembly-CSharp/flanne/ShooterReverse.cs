using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000111 RID: 273
	public class ShooterReverse : Shooter
	{
		// Token: 0x060007A9 RID: 1961 RVA: 0x00021104 File Offset: 0x0001F304
		public override void Shoot(ProjectileRecipe recipe, Vector2 pointDirection, int numProjectiles, float spread, float inaccuracy)
		{
			pointDirection *= -1f;
			base.Shoot(recipe, pointDirection, numProjectiles, spread, inaccuracy);
		}
	}
}
