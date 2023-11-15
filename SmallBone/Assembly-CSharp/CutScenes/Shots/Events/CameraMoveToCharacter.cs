using System;
using Runnables;
using Scenes;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x020001FD RID: 509
	public class CameraMoveToCharacter : Event
	{
		// Token: 0x06000A67 RID: 2663 RVA: 0x0001C9A4 File Offset: 0x0001ABA4
		public override void Run()
		{
			Scene<GameBase>.instance.cameraController.StartTrack(this._target.character.transform);
		}

		// Token: 0x0400087D RID: 2173
		[SerializeField]
		private Target _target;
	}
}
