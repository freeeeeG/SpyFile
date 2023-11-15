using System;
using Data;
using Platforms;
using Singletons;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000216 RID: 534
	public class SaveTutorialData : Event
	{
		// Token: 0x06000A9A RID: 2714 RVA: 0x0001CE38 File Offset: 0x0001B038
		public override void Run()
		{
			GameData.Generic.tutorial.End();
			GameData.Generic.tutorial.Save();
			PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
		}
	}
}
