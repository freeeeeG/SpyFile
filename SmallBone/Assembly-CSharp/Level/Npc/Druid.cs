using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using Data;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x0200059D RID: 1437
	public class Druid : InteractiveObject
	{
		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001C51 RID: 7249 RVA: 0x00057889 File Offset: 0x00055A89
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/name", NpcType.Druid));
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001C52 RID: 7250 RVA: 0x000578A0 File Offset: 0x00055AA0
		public string greeting
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/greeting", NpcType.Druid)).Random<string>();
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001C53 RID: 7251 RVA: 0x000578BC File Offset: 0x00055ABC
		public string[] chat
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/chat", NpcType.Druid)).Random<string[]>();
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x000578D8 File Offset: 0x00055AD8
		public string changeProphecyLabel
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/ChangeProphecy/label", NpcType.Druid));
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001C55 RID: 7253 RVA: 0x000578EF File Offset: 0x00055AEF
		public string[] changeProphecyNoMoney
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/ChangeProphecy/NoMoney", NpcType.Druid)).Random<string[]>();
			}
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x0005790B File Offset: 0x00055B0B
		protected override void Awake()
		{
			base.Awake();
			if (!GameData.Progress.GetRescued(NpcType.Druid))
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00057927 File Offset: 0x00055B27
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00057940 File Offset: 0x00055B40
		public override void InteractWith(Character character)
		{
			this._lineText.gameObject.SetActive(false);
			this._npcConversation.name = this.displayName;
			this._npcConversation.portrait = this._portrait;
			this._npcConversation.skippable = true;
			base.StartCoroutine(this.<InteractWith>g__CRun|16_0());
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x00057999 File Offset: 0x00055B99
		private void Chat()
		{
			base.StartCoroutine(this.<Chat>g__CRun|17_0());
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000579A8 File Offset: 0x00055BA8
		private void Close()
		{
			this._npcConversation.visible = false;
			LetterBox.instance.Disappear(0.4f);
			this._lineText.gameObject.SetActive(true);
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x000579D6 File Offset: 0x00055BD6
		[CompilerGenerated]
		private IEnumerator <InteractWith>g__CRun|16_0()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.OpenChatSelector(new Action(this.Chat), new Action(this.Close));
			this._npcConversation.body = this.greeting;
			yield return this._npcConversation.CType();
			yield break;
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x000579E5 File Offset: 0x00055BE5
		[CompilerGenerated]
		private IEnumerator <Chat>g__CRun|17_0()
		{
			yield return this._npcConversation.CConversation(this.chat);
			this.Close();
			yield break;
		}

		// Token: 0x04001829 RID: 6185
		private const NpcType _type = NpcType.Druid;

		// Token: 0x0400182A RID: 6186
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x0400182B RID: 6187
		[SerializeField]
		private NpcLineText _lineText;

		// Token: 0x0400182C RID: 6188
		private NpcConversation _npcConversation;
	}
}
