using System;
using Scenes;

namespace CutScenes.Shots.Events
{
	// Token: 0x0200020F RID: 527
	public sealed class RenderEndingCut : Event
	{
		// Token: 0x06000A8C RID: 2700 RVA: 0x0001CD9E File Offset: 0x0001AF9E
		public override void Run()
		{
			Scene<GameBase>.instance.cameraController.RenderEndingScene();
		}
	}
}
