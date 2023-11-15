using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000792 RID: 1938
	public class RunCharacterOperation : Operation
	{
		// Token: 0x060027B1 RID: 10161 RVA: 0x000773C0 File Offset: 0x000755C0
		public override void Run(IProjectile projectile)
		{
			this._operation.Initialize();
			this._operation.Run(projectile.owner);
		}

		// Token: 0x040021C8 RID: 8648
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation _operation;
	}
}
