using System;
using Scenes;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000339 RID: 825
	public sealed class Zoom : Runnable
	{
		// Token: 0x06000FAD RID: 4013 RVA: 0x0002F699 File Offset: 0x0002D899
		public override void Run()
		{
			Scene<GameBase>.instance.cameraController.Zoom(this._percent, this._speed);
		}

		// Token: 0x04000CE4 RID: 3300
		[SerializeField]
		[Range(0f, 10f)]
		private float _percent = 1f;

		// Token: 0x04000CE5 RID: 3301
		[SerializeField]
		private float _speed = 1f;
	}
}
