using System;
using Characters.Player;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x0200020D RID: 525
	public class PlayAdventurerMusic : Event
	{
		// Token: 0x06000A87 RID: 2695 RVA: 0x0001CD0C File Offset: 0x0001AF0C
		public override void Run()
		{
			WeaponInventory weapon = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
			if (weapon.Has("RockStar") || weapon.Has("RockStar_2"))
			{
				PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._rockstarMusicInfo);
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._musicInfo);
		}

		// Token: 0x0400089D RID: 2205
		[SerializeField]
		private MusicInfo _musicInfo;

		// Token: 0x0400089E RID: 2206
		[SerializeField]
		private MusicInfo _rockstarMusicInfo;
	}
}
