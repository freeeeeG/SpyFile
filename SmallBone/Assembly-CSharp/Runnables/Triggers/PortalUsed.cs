using System;
using Services;
using Singletons;

namespace Runnables.Triggers
{
	// Token: 0x02000354 RID: 852
	public class PortalUsed : Trigger
	{
		// Token: 0x06000FF1 RID: 4081 RVA: 0x0002FC2C File Offset: 0x0002DE2C
		protected override bool Check()
		{
			return Singleton<Service>.Instance.levelManager.skulPortalUsed;
		}
	}
}
