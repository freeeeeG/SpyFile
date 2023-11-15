using System;
using Scenes;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x0200020B RID: 523
	public class GameFadeIn : Event
	{
		// Token: 0x06000A83 RID: 2691 RVA: 0x0001CC90 File Offset: 0x0001AE90
		public override void Run()
		{
			GameBase instance = Scene<GameBase>.instance;
			base.StartCoroutine(instance.gameFadeInOut.CFadeIn(this._speed));
		}

		// Token: 0x04000899 RID: 2201
		[SerializeField]
		private float _speed;
	}
}
