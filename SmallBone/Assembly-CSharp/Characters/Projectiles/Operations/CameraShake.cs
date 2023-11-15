using System;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000772 RID: 1906
	public class CameraShake : Operation
	{
		// Token: 0x0600275C RID: 10076 RVA: 0x000761D4 File Offset: 0x000743D4
		public override void Run(IProjectile projectile)
		{
			Scene<GameBase>.instance.cameraController.shake.Attach(this, this._amount, this._duration);
			if (this._vibrateController)
			{
				Singleton<Service>.Instance.controllerVibation.vibration.Attach(this, this._amount, this._duration);
			}
		}

		// Token: 0x04002178 RID: 8568
		[SerializeField]
		private float _amount;

		// Token: 0x04002179 RID: 8569
		[SerializeField]
		private float _duration;

		// Token: 0x0400217A RID: 8570
		[SerializeField]
		private bool _vibrateController = true;
	}
}
