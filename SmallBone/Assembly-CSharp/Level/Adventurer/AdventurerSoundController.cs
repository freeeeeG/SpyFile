using System;
using Characters;
using Characters.Player;
using CutScenes;
using Data;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Adventurer
{
	// Token: 0x02000692 RID: 1682
	public class AdventurerSoundController : MonoBehaviour
	{
		// Token: 0x0600219F RID: 8607 RVA: 0x00065050 File Offset: 0x00063250
		private void OnTriggerEnter2D(Collider2D collision)
		{
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			if (component.type != Character.Type.Player)
			{
				return;
			}
			this.PlaySound();
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x00065080 File Offset: 0x00063280
		private void PlaySound()
		{
			if (!GameData.Progress.cutscene.GetData(CutScenes.Key.rookieHero))
			{
				PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
				return;
			}
			WeaponInventory weapon = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
			if (weapon.Has("RockStar") || weapon.Has("RockStar_2"))
			{
				PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._rockstarMusicInfo);
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._musicInfo);
		}

		// Token: 0x04001CAF RID: 7343
		[SerializeField]
		private MusicInfo _musicInfo;

		// Token: 0x04001CB0 RID: 7344
		[SerializeField]
		private MusicInfo _rockstarMusicInfo;

		// Token: 0x04001CB1 RID: 7345
		[GetComponent]
		[SerializeField]
		private Collider2D _trigger;
	}
}
