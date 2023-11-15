using System;
using Characters;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x020001FC RID: 508
	public class CameraMoveTo : Event
	{
		// Token: 0x06000A64 RID: 2660 RVA: 0x0001C946 File Offset: 0x0001AB46
		public override void Run()
		{
			Scene<GameBase>.instance.cameraController.StartTrack(this._trackPoint);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0001C960 File Offset: 0x0001AB60
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (player == null)
			{
				return;
			}
			Scene<GameBase>.instance.cameraController.StartTrack(player.transform);
		}

		// Token: 0x0400087C RID: 2172
		[SerializeField]
		private Transform _trackPoint;
	}
}
