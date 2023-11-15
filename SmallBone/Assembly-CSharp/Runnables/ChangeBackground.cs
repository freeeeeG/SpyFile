using System;
using Level;
using Scenes;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000304 RID: 772
	public sealed class ChangeBackground : Runnable
	{
		// Token: 0x06000F22 RID: 3874 RVA: 0x0002E5B8 File Offset: 0x0002C7B8
		public override void Run()
		{
			Map instance = Map.Instance;
			Scene<GameBase>.instance.ChangeBackgroundWithFade(this._background, instance.playerOrigin.y - instance.backgroundOrigin.y);
		}

		// Token: 0x04000C74 RID: 3188
		[SerializeField]
		private ParallaxBackground _background;
	}
}
