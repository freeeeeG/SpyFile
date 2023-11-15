using System;
using Services;
using Singletons;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000211 RID: 529
	public class ResetGameScene : Event
	{
		// Token: 0x06000A90 RID: 2704 RVA: 0x0001CDE1 File Offset: 0x0001AFE1
		public override void Run()
		{
			Singleton<Service>.Instance.ResetGameScene();
		}
	}
}
