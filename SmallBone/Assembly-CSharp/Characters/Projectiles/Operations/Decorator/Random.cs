using System;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator
{
	// Token: 0x020007AE RID: 1966
	public class Random : Operation
	{
		// Token: 0x06002816 RID: 10262 RVA: 0x000794D4 File Offset: 0x000776D4
		public override void Run(IProjectile projectile)
		{
			this._toRandom.components.Random<Operation>().Run(projectile);
		}

		// Token: 0x04002258 RID: 8792
		[SerializeField]
		[Operation.SubcomponentAttribute]
		private Operation.Subcomponents _toRandom;
	}
}
