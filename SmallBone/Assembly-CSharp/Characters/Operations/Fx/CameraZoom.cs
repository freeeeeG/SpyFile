using System;
using System.Collections;
using Scenes;
using Services;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F0D RID: 3853
	public class CameraZoom : CharacterOperation
	{
		// Token: 0x06004B42 RID: 19266 RVA: 0x000DD8BB File Offset: 0x000DBABB
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CZoom(owner.chronometer.animation));
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x000DD8D5 File Offset: 0x000DBAD5
		public override void Stop()
		{
			if (Service.quitting)
			{
				return;
			}
			Scene<GameBase>.instance.cameraController.Zoom(1f, this._restoreSpeed);
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x000DD8F9 File Offset: 0x000DBAF9
		private IEnumerator CZoom(Chronometer chronometer)
		{
			CameraController cameraController = Scene<GameBase>.instance.cameraController;
			cameraController.Zoom(this._percent, this._zoomSpeed);
			yield return chronometer.WaitForSeconds(this._duration);
			cameraController.Zoom(1f, this._restoreSpeed);
			yield break;
		}

		// Token: 0x04003A6E RID: 14958
		[SerializeField]
		private float _percent = 1f;

		// Token: 0x04003A6F RID: 14959
		[SerializeField]
		private float _zoomSpeed = 1f;

		// Token: 0x04003A70 RID: 14960
		[SerializeField]
		private float _restoreSpeed = 1f;

		// Token: 0x04003A71 RID: 14961
		[SerializeField]
		private float _duration;

		// Token: 0x04003A72 RID: 14962
		private float originalTrackSpeed;
	}
}
