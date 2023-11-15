using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using CutScenes;
using Data;
using FX;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x020005C6 RID: 1478
	public class Witch : InteractiveObject
	{
		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x00059DAC File Offset: 0x00057FAC
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/name", NpcType.Witch));
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001D6A RID: 7530 RVA: 0x00059DC3 File Offset: 0x00057FC3
		public string greeting
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/greeting", NpcType.Witch)).Random<string>();
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x00059DDF File Offset: 0x00057FDF
		public string[] chat
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/chat", NpcType.Witch)).Random<string[]>();
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x00059DFB File Offset: 0x00057FFB
		public string masteriesScript
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/Masteries", NpcType.Witch)).Random<string>();
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x00059E17 File Offset: 0x00058017
		public string masteriesLabel
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/Masteries/label", NpcType.Witch));
			}
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x00059E30 File Offset: 0x00058030
		protected override void Awake()
		{
			base.Awake();
			if (!GameData.Generic.normalEnding)
			{
				this._animator.Play(this._normalIdle);
				return;
			}
			if (GameData.Progress.cutscene.GetData(CutScenes.Key.dwarfEngineer_First))
			{
				this._animator.Play(this._hardmodeIdle);
				return;
			}
			this._animator.Play(this._hardmodeEmptyIdle);
			this._lineText.gameObject.SetActive(false);
			this._tutorialWitch.Deactivate();
			base.Deactivate();
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x00059EB2 File Offset: 0x000580B2
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x00059EC9 File Offset: 0x000580C9
		private void OnDisable()
		{
			if (Service.quitting)
			{
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._close, base.transform.position);
			LetterBox.instance.visible = false;
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x00059EFA File Offset: 0x000580FA
		public override void InteractWith(Character character)
		{
			base.StartCoroutine(this.<InteractWith>g__CRun|26_0());
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x00059F0C File Offset: 0x0005810C
		private void OpenContent()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._open, base.transform.position);
			this._npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.DarkQuartz);
			this._npcConversation.witchContent.SetActive(true);
			this._npcConversation.body = this.masteriesScript;
			this._npcConversation.skippable = false;
			this._npcConversation.Type();
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x00059F7A File Offset: 0x0005817A
		private void Chat()
		{
			this._npcConversation.skippable = true;
			base.StartCoroutine(this.<Chat>g__CRun|28_0());
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x00059F95 File Offset: 0x00058195
		private void Close()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._close, base.transform.position);
			this._npcConversation.visible = false;
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x0005A006 File Offset: 0x00058206
		[CompilerGenerated]
		private IEnumerator <InteractWith>g__CRun|26_0()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.name = this.displayName;
			this._npcConversation.portrait = this._portrait;
			this._npcConversation.body = this.greeting;
			this._npcConversation.skippable = false;
			this._npcConversation.Type();
			this._npcConversation.OpenContentSelector(this.masteriesLabel, new Action(this.OpenContent), new Action(this.Chat), new Action(this.Close));
			yield break;
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x0005A015 File Offset: 0x00058215
		[CompilerGenerated]
		private IEnumerator <Chat>g__CRun|28_0()
		{
			yield return this._npcConversation.CConversation(this.chat);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x040018EC RID: 6380
		private const NpcType _type = NpcType.Witch;

		// Token: 0x040018ED RID: 6381
		private readonly int _normalIdle = Animator.StringToHash("Idle_Human_Castle");

		// Token: 0x040018EE RID: 6382
		private readonly int _hardmodeIdle = Animator.StringToHash("Idle_Human_Castle2");

		// Token: 0x040018EF RID: 6383
		private readonly int _hardmodeEmptyIdle = Animator.StringToHash("Idle_NoHuman");

		// Token: 0x040018F0 RID: 6384
		[SerializeField]
		private Animator _animator;

		// Token: 0x040018F1 RID: 6385
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x040018F2 RID: 6386
		[SerializeField]
		private Sprite _portraitCat;

		// Token: 0x040018F3 RID: 6387
		[SerializeField]
		private GameObject _body;

		// Token: 0x040018F4 RID: 6388
		private NpcConversation _npcConversation;

		// Token: 0x040018F5 RID: 6389
		[SerializeField]
		private SoundInfo _open;

		// Token: 0x040018F6 RID: 6390
		[SerializeField]
		private SoundInfo _close;

		// Token: 0x040018F7 RID: 6391
		[SerializeField]
		private NpcLineText _lineText;

		// Token: 0x040018F8 RID: 6392
		[SerializeField]
		private InteractiveObject _tutorialWitch;
	}
}
