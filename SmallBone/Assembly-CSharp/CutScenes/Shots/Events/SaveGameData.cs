using System;
using Data;
using Platforms;
using Singletons;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000214 RID: 532
	public class SaveGameData : Event
	{
		// Token: 0x06000A96 RID: 2710 RVA: 0x0001CE14 File Offset: 0x0001B014
		public override void Run()
		{
			GameData.Currency.SaveAll();
			GameData.Progress.SaveAll();
			PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
		}
	}
}
