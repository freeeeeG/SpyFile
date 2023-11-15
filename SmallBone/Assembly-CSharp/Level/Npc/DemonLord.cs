using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x0200059A RID: 1434
	public class DemonLord : InteractiveObject
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001C3B RID: 7227 RVA: 0x00057632 File Offset: 0x00055832
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/name", NpcType.DemonLord));
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001C3C RID: 7228 RVA: 0x00057649 File Offset: 0x00055849
		public string greeting
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/greeting", NpcType.DemonLord)).Random<string>();
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001C3D RID: 7229 RVA: 0x00057665 File Offset: 0x00055865
		public string[] chat
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/chat", NpcType.DemonLord)).Random<string[]>();
			}
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x00057684 File Offset: 0x00055884
		public override void InteractWith(Character character)
		{
			this._lineText.gameObject.SetActive(false);
			this._npcConversation.name = this.displayName;
			this._npcConversation.portrait = this._portrait;
			this._npcConversation.skippable = true;
			base.StartCoroutine(this.<InteractWith>g__CRun|10_0());
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000576DD File Offset: 0x000558DD
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000576F4 File Offset: 0x000558F4
		private void Chat()
		{
			base.StartCoroutine(this.<Chat>g__CRun|12_0());
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x00057703 File Offset: 0x00055903
		private void Close()
		{
			this._npcConversation.visible = false;
			LetterBox.instance.Disappear(0.4f);
			this._lineText.gameObject.SetActive(true);
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x00057731 File Offset: 0x00055931
		[CompilerGenerated]
		private IEnumerator <InteractWith>g__CRun|10_0()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.OpenChatSelector(new Action(this.Chat), new Action(this.Close));
			this._npcConversation.body = this.greeting;
			yield return this._npcConversation.CType();
			yield break;
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x00057740 File Offset: 0x00055940
		[CompilerGenerated]
		private IEnumerator <Chat>g__CRun|12_0()
		{
			yield return this._npcConversation.CConversation(this.chat);
			this.Close();
			yield break;
		}

		// Token: 0x0400181F RID: 6175
		private const NpcType _type = NpcType.DemonLord;

		// Token: 0x04001820 RID: 6176
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x04001821 RID: 6177
		[SerializeField]
		private NpcLineText _lineText;

		// Token: 0x04001822 RID: 6178
		private NpcConversation _npcConversation;
	}
}
