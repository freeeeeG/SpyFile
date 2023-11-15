using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using CutScenes;
using Data;
using GameResources;
using Runnables;
using Scenes;
using TMPro;
using UI;
using UnityEngine;
using UserInput;

namespace Level.Npc
{
	// Token: 0x020005A0 RID: 1440
	public class Dwarf : InteractiveObject
	{
		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001C6A RID: 7274 RVA: 0x00057B2D File Offset: 0x00055D2D
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/name", NpcType.Dwarf));
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x00057B44 File Offset: 0x00055D44
		public string greeting
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/greeting", NpcType.Dwarf)).Random<string>();
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001C6C RID: 7276 RVA: 0x00057B60 File Offset: 0x00055D60
		public string[] chat
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/chat", NpcType.Dwarf)).Random<string[]>();
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001C6D RID: 7277 RVA: 0x00057B7C File Offset: 0x00055D7C
		public int tryLevel
		{
			get
			{
				return this._tryLevel;
			}
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x00057B84 File Offset: 0x00055D84
		public override void InteractWith(Character character)
		{
			if (this._readyForLevelChangeTutorial)
			{
				this._levelChangeTutorial.Run();
				this._readyForLevelChangeTutorial = false;
				return;
			}
			this._lineText.gameObject.SetActive(false);
			this._npcConversation.name = this.displayName;
			this._npcConversation.portrait = this._portrait;
			this._npcConversation.skippable = true;
			base.StartCoroutine(this.<InteractWith>g__CRun|29_0());
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x00057BF8 File Offset: 0x00055DF8
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			this._tryLevel = GameData.HardmodeProgress.hardmodeLevel;
			ColorUtility.TryParseHtmlString("#7311BC", out this._activeColor);
			ColorUtility.TryParseHtmlString("#020005", out this._deactiveColor);
			this.UpdateHardmodeLevelText();
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x00057C4D File Offset: 0x00055E4D
		private void Chat()
		{
			base.StartCoroutine(this.<Chat>g__CRun|31_0());
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x00057C5C File Offset: 0x00055E5C
		private void Close()
		{
			this._npcConversation.visible = false;
			LetterBox.instance.Disappear(0.4f);
			this._lineText.gameObject.SetActive(true);
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x00057C8A File Offset: 0x00055E8A
		public void SetReadyForLevelChangeTutorial()
		{
			this._readyForLevelChangeTutorial = true;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x00057C93 File Offset: 0x00055E93
		public void SetNotReadyForLevelChangeTutorial()
		{
			this._readyForLevelChangeTutorial = false;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x00057C9C File Offset: 0x00055E9C
		private void Update()
		{
			if (!base.popupVisible)
			{
				return;
			}
			if (!GameData.Progress.cutscene.GetData(CutScenes.Key.darkMirror_FirstClear))
			{
				return;
			}
			if (GameData.HardmodeProgress.clearedLevel < 0)
			{
				return;
			}
			if (KeyMapper.Map.Up.WasPressed)
			{
				this.LevelUp();
				return;
			}
			if (KeyMapper.Map.Down.WasPressed)
			{
				this.LevelDown();
			}
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x00057CFC File Offset: 0x00055EFC
		private void LevelUp()
		{
			if (this._tryLevel == GameData.HardmodeProgress.maxLevel)
			{
				this._tryLevel = GameData.HardmodeProgress.maxLevel;
				GameData.HardmodeProgress.hardmodeLevel = this._tryLevel;
				this.UpdateHardmodeLevelText();
				return;
			}
			this._tryLevel++;
			if (this._tryLevel <= GameData.HardmodeProgress.clearedLevel + 1)
			{
				GameData.HardmodeProgress.hardmodeLevel = this._tryLevel;
			}
			this.UpdateHardmodeLevelText();
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x00057D61 File Offset: 0x00055F61
		private void LevelDown()
		{
			if (this._tryLevel == 0)
			{
				return;
			}
			this._tryLevel--;
			if (this._tryLevel <= GameData.HardmodeProgress.clearedLevel + 1)
			{
				GameData.HardmodeProgress.hardmodeLevel = this._tryLevel;
			}
			this.UpdateHardmodeLevelText();
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x00057D9C File Offset: 0x00055F9C
		private void UpdateHardmodeLevelText()
		{
			if (this._readyForLevelChangeTutorial)
			{
				this._levelChangeTutorial.Run();
				this._readyForLevelChangeTutorial = false;
				return;
			}
			if (!this._levelTextActive)
			{
				return;
			}
			this._levelAnimationCoroutineReference.Stop();
			this._levelAnimationCoroutineReference = this.StartCoroutineWithReference(this.CUpdateAnimation());
			this._activeLevelFrame.SetActive(this._tryLevel == GameData.HardmodeProgress.clearedLevel + 1);
			this._levelText.color = ((this._tryLevel <= GameData.HardmodeProgress.clearedLevel + 1) ? this._activeColor : this._deactiveColor);
			this._levelText.text = this._tryLevel.ToString();
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x00057E42 File Offset: 0x00056042
		private IEnumerator CUpdateAnimation()
		{
			this._levelText.gameObject.SetActive(false);
			this._animator.Play(this._putQuartz);
			this._levelChangeAudio.Play();
			yield return Chronometer.global.WaitForSeconds(0.3f);
			this._levelChangeAudio.Stop();
			this._animator.Play(this._Idle_2);
			this._levelText.gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x00057E51 File Offset: 0x00056051
		public void DeactivateLevelText()
		{
			base.StopAllCoroutines();
			this._levelTextActive = false;
			this._levelText.gameObject.SetActive(false);
			this._activeLevelFrame.gameObject.SetActive(false);
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x00057E84 File Offset: 0x00056084
		public void ActivateLevelText()
		{
			base.StopAllCoroutines();
			this._levelTextActive = true;
			this._levelText.gameObject.SetActive(true);
			this._tryLevel = GameData.HardmodeProgress.hardmodeLevel;
			this._activeLevelFrame.SetActive(this._tryLevel == GameData.HardmodeProgress.clearedLevel + 1);
			this._levelText.color = ((this._tryLevel <= GameData.HardmodeProgress.clearedLevel + 1) ? this._activeColor : this._deactiveColor);
			this._levelText.text = this._tryLevel.ToString();
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x00057F14 File Offset: 0x00056114
		public void SetLevelText(int _level)
		{
			base.StopAllCoroutines();
			this._tryLevel = _level;
			this._levelText.color = ((this._tryLevel <= GameData.HardmodeProgress.clearedLevel + 1) ? this._activeColor : this._deactiveColor);
			this._levelText.text = this._tryLevel.ToString();
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00057F6C File Offset: 0x0005616C
		public void OnDisable()
		{
			this.DeactivateLevelText();
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x00057F74 File Offset: 0x00056174
		public void DetachLevelChangeUI()
		{
			this._uiObjects = new GameObject[0];
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x00057F82 File Offset: 0x00056182
		public void AttachLevelChangeUI()
		{
			this._uiObjects = new GameObject[1];
			this._uiObjects[0] = this._uiLevelChange;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x00057FCD File Offset: 0x000561CD
		[CompilerGenerated]
		private IEnumerator <InteractWith>g__CRun|29_0()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.OpenChatSelector(new Action(this.Chat), new Action(this.Close));
			this._npcConversation.body = this.greeting;
			yield return this._npcConversation.CType();
			yield break;
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x00057FDC File Offset: 0x000561DC
		[CompilerGenerated]
		private IEnumerator <Chat>g__CRun|31_0()
		{
			yield return this._npcConversation.CConversation(this.chat);
			this.Close();
			yield break;
		}

		// Token: 0x04001833 RID: 6195
		private const NpcType _type = NpcType.Dwarf;

		// Token: 0x04001834 RID: 6196
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x04001835 RID: 6197
		[SerializeField]
		private NpcLineText _lineText;

		// Token: 0x04001836 RID: 6198
		[SerializeField]
		private Animator _animator;

		// Token: 0x04001837 RID: 6199
		[SerializeField]
		private bool _readyForLevelChangeTutorial;

		// Token: 0x04001838 RID: 6200
		[SerializeField]
		private Runnable _levelChangeTutorial;

		// Token: 0x04001839 RID: 6201
		[SerializeField]
		private bool _levelTextActive = true;

		// Token: 0x0400183A RID: 6202
		[SerializeField]
		private TMP_Text _levelText;

		// Token: 0x0400183B RID: 6203
		[SerializeField]
		private CustomAudioSource _levelChangeAudio;

		// Token: 0x0400183C RID: 6204
		[SerializeField]
		private GameObject _activeLevelFrame;

		// Token: 0x0400183D RID: 6205
		[SerializeField]
		private GameObject _uiLevelChange;

		// Token: 0x0400183E RID: 6206
		private readonly int _putQuartz = Animator.StringToHash("Put_DarkQuartz");

		// Token: 0x0400183F RID: 6207
		private readonly int _Idle_2 = Animator.StringToHash("Idle_2");

		// Token: 0x04001840 RID: 6208
		private NpcConversation _npcConversation;

		// Token: 0x04001841 RID: 6209
		private int _tryLevel;

		// Token: 0x04001842 RID: 6210
		private const string _activeColorCode = "#7311BC";

		// Token: 0x04001843 RID: 6211
		private const string _deactiveColorCode = "#020005";

		// Token: 0x04001844 RID: 6212
		private const float _levelAnimationDuration = 0.3f;

		// Token: 0x04001845 RID: 6213
		private CoroutineReference _levelAnimationCoroutineReference;

		// Token: 0x04001846 RID: 6214
		private Color _activeColor;

		// Token: 0x04001847 RID: 6215
		private Color _deactiveColor;
	}
}
