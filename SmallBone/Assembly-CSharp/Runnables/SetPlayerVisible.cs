using System;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000316 RID: 790
	public class SetPlayerVisible : Runnable
	{
		// Token: 0x06000F4D RID: 3917 RVA: 0x0002EB87 File Offset: 0x0002CD87
		public override void Run()
		{
			Singleton<Service>.Instance.levelManager.player.playerComponents.visibility.SetVisible(this._visible);
		}

		// Token: 0x04000CA2 RID: 3234
		[SerializeField]
		private bool _visible;
	}
}
