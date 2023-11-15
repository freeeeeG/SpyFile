using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000202 RID: 514
	public class DestroyPlayer : Event
	{
		// Token: 0x06000A74 RID: 2676 RVA: 0x0001CA86 File Offset: 0x0001AC86
		public override void Run()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
			if (this._player == null)
			{
				return;
			}
			UnityEngine.Object.Destroy(this._player.gameObject);
		}

		// Token: 0x04000885 RID: 2181
		private Character _player;
	}
}
