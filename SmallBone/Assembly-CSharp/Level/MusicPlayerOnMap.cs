using System;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004FD RID: 1277
	public class MusicPlayerOnMap : MonoBehaviour
	{
		// Token: 0x06001933 RID: 6451 RVA: 0x0004F0B0 File Offset: 0x0004D2B0
		private void Start()
		{
			if (Singleton<Service>.Instance.levelManager.currentChapter.type == Chapter.Type.Chapter5)
			{
				return;
			}
			if (this._musicInfo.audioClip != null)
			{
				PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._musicInfo);
				return;
			}
			PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0004F103 File Offset: 0x0004D303
		private void OnDestroy()
		{
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(Singleton<Service>.Instance.levelManager.currentChapter.currentStage.music, 1f, true, true, false);
		}

		// Token: 0x040015F8 RID: 5624
		[SerializeField]
		private MusicInfo _musicInfo;
	}
}
