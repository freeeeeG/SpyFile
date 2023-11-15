using System;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator
{
	// Token: 0x020007AA RID: 1962
	public class Chance : Operation
	{
		// Token: 0x0600280D RID: 10253 RVA: 0x00079368 File Offset: 0x00077568
		public override void Run(IProjectile projectile)
		{
			if (MMMaths.Chance(this._successChance))
			{
				if (this._onSuccess == null)
				{
					return;
				}
				this._onSuccess.Run(projectile);
				return;
			}
			else
			{
				if (this._onFail == null)
				{
					return;
				}
				this._onFail.Run(projectile);
				return;
			}
		}

		// Token: 0x04002251 RID: 8785
		[Range(0f, 1f)]
		[SerializeField]
		private float _successChance = 0.5f;

		// Token: 0x04002252 RID: 8786
		[Operation.SubcomponentAttribute]
		[SerializeField]
		private Operation _onSuccess;

		// Token: 0x04002253 RID: 8787
		[Operation.SubcomponentAttribute]
		[SerializeField]
		private Operation _onFail;
	}
}
