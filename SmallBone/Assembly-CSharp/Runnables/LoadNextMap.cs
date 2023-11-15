using System;
using Level;
using Services;
using Singletons;

namespace Runnables
{
	// Token: 0x02000329 RID: 809
	public sealed class LoadNextMap : Runnable
	{
		// Token: 0x06000F87 RID: 3975 RVA: 0x0002F1E3 File Offset: 0x0002D3E3
		public override void Run()
		{
			Singleton<Service>.Instance.levelManager.LoadNextMap(NodeIndex.Node1);
		}
	}
}
