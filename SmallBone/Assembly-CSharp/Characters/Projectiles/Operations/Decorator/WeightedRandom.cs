using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator
{
	// Token: 0x020007B6 RID: 1974
	public class WeightedRandom : Operation
	{
		// Token: 0x06002837 RID: 10295 RVA: 0x000798C6 File Offset: 0x00077AC6
		public override void Run(IProjectile projectile)
		{
			this._toRandom.RunWeightedRandom(projectile);
		}

		// Token: 0x04002277 RID: 8823
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationWithWeight))]
		private OperationWithWeight.Subcomponents _toRandom;
	}
}
