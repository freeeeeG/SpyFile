using System;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000208 RID: 520
	public class SetPortalUsed : Event
	{
		// Token: 0x06000A7D RID: 2685 RVA: 0x0001CC40 File Offset: 0x0001AE40
		public override void Run()
		{
			Singleton<Service>.Instance.levelManager.skulPortalUsed = this._portalUsed;
		}

		// Token: 0x04000896 RID: 2198
		[SerializeField]
		private bool _portalUsed;
	}
}
