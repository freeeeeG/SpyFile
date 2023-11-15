using System;
using Scenes;

namespace CutScenes.Shots.Events
{
	// Token: 0x0200020A RID: 522
	public class ForceToUpdateCameraPosition : Event
	{
		// Token: 0x06000A81 RID: 2689 RVA: 0x0001CC7E File Offset: 0x0001AE7E
		public override void Run()
		{
			Scene<GameBase>.instance.cameraController.UpdateCameraPosition();
		}
	}
}
