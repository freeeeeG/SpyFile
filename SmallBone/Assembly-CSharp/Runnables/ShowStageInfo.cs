using System;
using Level;
using Scenes;
using Services;
using Singletons;

namespace Runnables
{
	// Token: 0x02000330 RID: 816
	public class ShowStageInfo : Runnable
	{
		// Token: 0x06000F94 RID: 3988 RVA: 0x0002F394 File Offset: 0x0002D594
		public override void Run()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			Scene<GameBase>.instance.uiManager.stageName.Show(currentChapter.chapterName, currentChapter.stageTag, currentChapter.stageName);
		}
	}
}
