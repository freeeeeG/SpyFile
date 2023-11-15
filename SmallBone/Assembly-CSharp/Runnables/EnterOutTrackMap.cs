using System;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000315 RID: 789
	public sealed class EnterOutTrackMap : Runnable
	{
		// Token: 0x06000F4B RID: 3915 RVA: 0x0002EB6A File Offset: 0x0002CD6A
		public override void Run()
		{
			Singleton<Service>.Instance.levelManager.EnterOutTrack(this._map, this._playerReset);
		}

		// Token: 0x04000CA0 RID: 3232
		[SerializeField]
		private bool _playerReset;

		// Token: 0x04000CA1 RID: 3233
		[SerializeField]
		private Map _map;
	}
}
