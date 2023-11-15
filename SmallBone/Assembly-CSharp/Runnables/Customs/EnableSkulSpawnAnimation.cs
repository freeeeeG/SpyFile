using System;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables.Customs
{
	// Token: 0x0200037F RID: 895
	public sealed class EnableSkulSpawnAnimation : Runnable
	{
		// Token: 0x06001061 RID: 4193 RVA: 0x00030712 File Offset: 0x0002E912
		public override void Run()
		{
			Singleton<Service>.Instance.levelManager.skulSpawnAnimaionEnable = this._enable;
		}

		// Token: 0x04000D64 RID: 3428
		[SerializeField]
		private bool _enable;
	}
}
