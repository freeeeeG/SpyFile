using System;
using System.Collections;
using Scenes;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002FA RID: 762
	public class MoveCamera : CRunnable
	{
		// Token: 0x06000EFA RID: 3834 RVA: 0x0002E124 File Offset: 0x0002C324
		public override IEnumerator CRun()
		{
			CameraController cameraController = Scene<GameBase>.instance.cameraController;
			cameraController.StopTrack();
			cameraController.StartTrack(this._target);
			float elapsed = 0f;
			Vector3 startPosition = Camera.main.transform.position;
			while (elapsed < this._curve.duration)
			{
				elapsed += Chronometer.global.deltaTime;
				Vector3 position = Vector3.Lerp(startPosition, this._target.position, this._curve.Evaluate(elapsed));
				position.z = Camera.main.transform.position.z;
				Camera.main.transform.position = position;
				yield return null;
			}
			Vector3 position2 = this._target.position;
			position2.z = Camera.main.transform.position.z;
			Camera.main.transform.position = position2;
			yield break;
		}

		// Token: 0x04000C53 RID: 3155
		[SerializeField]
		private Transform _target;

		// Token: 0x04000C54 RID: 3156
		[SerializeField]
		private Curve _curve;
	}
}
