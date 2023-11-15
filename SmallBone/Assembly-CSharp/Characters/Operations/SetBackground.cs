using System;
using Level;
using Scenes;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DFF RID: 3583
	public sealed class SetBackground : Operation
	{
		// Token: 0x060047A7 RID: 18343 RVA: 0x000D0458 File Offset: 0x000CE658
		public override void Run()
		{
			Map instance = Map.Instance;
			Scene<GameBase>.instance.SetBackground(this._background, instance.playerOrigin.y - instance.backgroundOrigin.y);
		}

		// Token: 0x040036A8 RID: 13992
		[SerializeField]
		private ParallaxBackground _background;
	}
}
