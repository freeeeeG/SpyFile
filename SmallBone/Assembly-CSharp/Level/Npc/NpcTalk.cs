using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using GameResources;
using Scenes;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Npc
{
	// Token: 0x020005B2 RID: 1458
	public class NpcTalk : InteractiveObject
	{
		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001CDC RID: 7388 RVA: 0x00058B46 File Offset: 0x00056D46
		private string displayName
		{
			get
			{
				return Localization.GetLocalizedString(this._displayNameKey);
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001CDD RID: 7389 RVA: 0x00058B53 File Offset: 0x00056D53
		private string greeting
		{
			get
			{
				return Localization.GetLocalizedStringArray(this._greetingKey).Random<string>();
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001CDE RID: 7390 RVA: 0x00058B65 File Offset: 0x00056D65
		private string[] chat
		{
			get
			{
				return Localization.GetLocalizedStringArrays(this._chatKey).Random<string[]>();
			}
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x00058B77 File Offset: 0x00056D77
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x00058B90 File Offset: 0x00056D90
		public override void InteractWith(Character character)
		{
			LetterBox.instance.Appear(0.4f);
			this._npcConversation.name = this.displayName;
			this._npcConversation.portrait = null;
			this._npcConversation.body = this.greeting;
			this._npcConversation.skippable = false;
			this._npcConversation.Type();
			this._npcConversation.OpenChatSelector(new Action(this.Chat), new Action(this.Close));
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x00058C14 File Offset: 0x00056E14
		private void Chat()
		{
			this._npcConversation.skippable = true;
			base.StartCoroutine(this.<Chat>g__CRun|13_0());
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x00058C2F File Offset: 0x00056E2F
		private void Close()
		{
			this._npcConversation.visible = false;
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x00058C4C File Offset: 0x00056E4C
		[CompilerGenerated]
		private IEnumerator <Chat>g__CRun|13_0()
		{
			this._onChat.Invoke();
			yield return this._npcConversation.CConversation(this.chat);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x0400188F RID: 6287
		[SerializeField]
		private string _displayNameKey;

		// Token: 0x04001890 RID: 6288
		[SerializeField]
		private string _greetingKey;

		// Token: 0x04001891 RID: 6289
		[SerializeField]
		private string _chatKey;

		// Token: 0x04001892 RID: 6290
		[SerializeField]
		private UnityEvent _onChat;

		// Token: 0x04001893 RID: 6291
		private NpcConversation _npcConversation;
	}
}
