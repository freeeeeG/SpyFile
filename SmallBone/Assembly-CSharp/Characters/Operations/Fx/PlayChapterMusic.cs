using System;
using Services;
using Singletons;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F14 RID: 3860
	public sealed class PlayChapterMusic : Operation
	{
		// Token: 0x06004B5F RID: 19295 RVA: 0x0004F103 File Offset: 0x0004D303
		public override void Run()
		{
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(Singleton<Service>.Instance.levelManager.currentChapter.currentStage.music, 1f, true, true, false);
		}
	}
}
