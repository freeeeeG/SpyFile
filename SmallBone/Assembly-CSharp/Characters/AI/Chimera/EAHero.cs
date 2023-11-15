using System;
using System.Collections;
using Characters.Actions;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Characters.AI.Chimera
{
	// Token: 0x02001256 RID: 4694
	public class EAHero : MonoBehaviour
	{
		// Token: 0x06005D0F RID: 23823 RVA: 0x00111F82 File Offset: 0x00110182
		private void OnEnable()
		{
			this._texts = Localization.GetLocalizedStringArray(EAHero._textKey);
		}

		// Token: 0x06005D10 RID: 23824 RVA: 0x00111F94 File Offset: 0x00110194
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			LetterBox.instance.Disappear(0.4f);
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
			if (Scene<GameBase>.instance.cameraController == null || Singleton<Service>.Instance.levelManager.player == null)
			{
				return;
			}
			Scene<GameBase>.instance.cameraController.StartTrack(Singleton<Service>.Instance.levelManager.player.transform);
		}

		// Token: 0x06005D11 RID: 23825 RVA: 0x00112047 File Offset: 0x00110247
		public IEnumerator Landing()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = null;
			npcConversation.skippable = true;
			npcConversation.name = Localization.GetLocalizedString(EAHero._nameKey);
			this._landingFreeze.TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[0]
			});
			this._landing.TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[1]
			});
			yield break;
		}

		// Token: 0x06005D12 RID: 23826 RVA: 0x00112056 File Offset: 0x00110256
		public IEnumerator Standing()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = null;
			npcConversation.skippable = true;
			npcConversation.name = Localization.GetLocalizedString(EAHero._nameKey);
			this._stand.TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[2]
			});
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[3]
			});
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[4]
			});
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[5]
			});
			yield break;
		}

		// Token: 0x06005D13 RID: 23827 RVA: 0x00112065 File Offset: 0x00110265
		public IEnumerator Teleport()
		{
			this.SetTeleportDestinationToPlayer();
			this._teleportStart.TryStart();
			this._spriteRenderer.enabled = false;
			while (this._teleportStart.running)
			{
				yield return null;
			}
			yield return Chronometer.global.WaitForSeconds(this._teleportDelay);
			this._character.ForceToLookAt(Character.LookingDirection.Right);
			this._teleportEnd.TryStart();
			this._spriteRenderer.enabled = true;
			while (this._teleportEnd.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005D14 RID: 23828 RVA: 0x00112074 File Offset: 0x00110274
		public IEnumerator FlyIdle()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = null;
			npcConversation.skippable = true;
			npcConversation.name = Localization.GetLocalizedString(EAHero._nameKey);
			this._fly.TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[6]
			});
			npcConversation.Done();
			yield break;
		}

		// Token: 0x06005D15 RID: 23829 RVA: 0x00112083 File Offset: 0x00110283
		public IEnumerator Attack()
		{
			this._attack.TryStart();
			yield return null;
			yield break;
		}

		// Token: 0x06005D16 RID: 23830 RVA: 0x00112094 File Offset: 0x00110294
		public void SetTeleportDestinationToPlayer()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			this._teleportDestination.position = player.transform.position;
		}

		// Token: 0x04004A99 RID: 19097
		private static readonly string _nameKey = "CutScene/name/FirstHero";

		// Token: 0x04004A9A RID: 19098
		private static readonly string _textKey = "CutScene/EAEnding/FirstHero/0";

		// Token: 0x04004A9B RID: 19099
		[SerializeField]
		private Character _character;

		// Token: 0x04004A9C RID: 19100
		[SerializeField]
		private float _teleportDelay;

		// Token: 0x04004A9D RID: 19101
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04004A9E RID: 19102
		[SerializeField]
		private Transform _teleportDestination;

		// Token: 0x04004A9F RID: 19103
		[SerializeField]
		private Characters.Actions.Action _landingFreeze;

		// Token: 0x04004AA0 RID: 19104
		[SerializeField]
		private Characters.Actions.Action _landing;

		// Token: 0x04004AA1 RID: 19105
		[SerializeField]
		private Characters.Actions.Action _stand;

		// Token: 0x04004AA2 RID: 19106
		[SerializeField]
		private Characters.Actions.Action _fly;

		// Token: 0x04004AA3 RID: 19107
		[SerializeField]
		private Characters.Actions.Action _teleportStart;

		// Token: 0x04004AA4 RID: 19108
		[SerializeField]
		private Characters.Actions.Action _teleportEnd;

		// Token: 0x04004AA5 RID: 19109
		[SerializeField]
		private Characters.Actions.Action _attack;

		// Token: 0x04004AA6 RID: 19110
		private string[] _texts;

		// Token: 0x02001257 RID: 4695
		private enum Text
		{
			// Token: 0x04004AA8 RID: 19112
			LandingFreeze_01,
			// Token: 0x04004AA9 RID: 19113
			Landing_01,
			// Token: 0x04004AAA RID: 19114
			Standing_01,
			// Token: 0x04004AAB RID: 19115
			Standing_02,
			// Token: 0x04004AAC RID: 19116
			Standing_03,
			// Token: 0x04004AAD RID: 19117
			Standing_04,
			// Token: 0x04004AAE RID: 19118
			Fly_01
		}
	}
}
