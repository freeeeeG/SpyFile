using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001037 RID: 4151
	public class EliteSoundController : MonoBehaviour
	{
		// Token: 0x06005010 RID: 20496 RVA: 0x000F18EC File Offset: 0x000EFAEC
		private void Awake()
		{
			if (this._elite == null)
			{
				this._elite = base.GetComponent<AIController>();
			}
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x000F1908 File Offset: 0x000EFB08
		private void Start()
		{
			this._elite.onFind += delegate()
			{
				PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._musicInfo);
			};
		}

		// Token: 0x04004068 RID: 16488
		[SerializeField]
		private MusicInfo _musicInfo;

		// Token: 0x04004069 RID: 16489
		[SerializeField]
		private AIController _elite;
	}
}
