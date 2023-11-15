using System;
using Scenes;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x020001FF RID: 511
	public class ChangeCameraTrackSpeed : Event
	{
		// Token: 0x06000A6B RID: 2667 RVA: 0x0001C9D7 File Offset: 0x0001ABD7
		public override void Run()
		{
			Scene<GameBase>.instance.cameraController.trackSpeed = this._cameraTrackSpeed;
		}

		// Token: 0x0400087F RID: 2175
		[SerializeField]
		private float _cameraTrackSpeed = 3f;
	}
}
