using System;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F0B RID: 3851
	public class CameraShake : CharacterOperation
	{
		// Token: 0x06004B3E RID: 19262 RVA: 0x000DD838 File Offset: 0x000DBA38
		public override void Run(Character owner)
		{
			Scene<GameBase>.instance.cameraController.shake.Attach(this, this._amount, this._duration);
			if (this._vibrateController)
			{
				Singleton<Service>.Instance.controllerVibation.vibration.Attach(this, this._amount, this._duration);
			}
		}

		// Token: 0x04003A6A RID: 14954
		[SerializeField]
		private float _amount;

		// Token: 0x04003A6B RID: 14955
		[SerializeField]
		private float _duration;

		// Token: 0x04003A6C RID: 14956
		[SerializeField]
		private bool _vibrateController = true;
	}
}
