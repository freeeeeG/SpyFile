using System;
using GameResources;
using UnityEngine;
using UserInput;

namespace UI
{
	// Token: 0x020003B5 RID: 949
	public class NpcContent : MonoBehaviour
	{
		// Token: 0x06001189 RID: 4489 RVA: 0x00034088 File Offset: 0x00032288
		private void OpenContent()
		{
			this._npcConversation.body = this._content;
			this._npcConversation.skippable = false;
			this._npcConversation.Type();
			this._contentSelector.gameObject.SetActive(false);
			this._contents[this._type].gameObject.SetActive(true);
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x000340EA File Offset: 0x000322EA
		private void Chat()
		{
			this._npcConversation.Conversation(this._chatScripts);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x000340FD File Offset: 0x000322FD
		private void Update()
		{
			if (this._contentContainer.activeSelf && KeyMapper.Map.Cancel.WasPressed)
			{
				this.Close();
			}
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00034124 File Offset: 0x00032324
		public void Open(NpcContent.Type type, string key)
		{
			this._type = type;
			this._name = Localization.GetLocalizedString(key + "/name");
			this._chatScripts = Localization.GetLocalizedStringArrays("npc/" + key + "/chat").Random<string[]>();
			this._greeting = Localization.GetLocalizedString(key + "/greeting");
			this._content = Localization.GetLocalizedString(key + "/content");
			string localizedString = Localization.GetLocalizedString(key + "/contentLabel");
			this._contentContainer.SetActive(true);
			this._npcConversation.name = this._name;
			this._npcConversation.body = this._greeting;
			this._npcConversation.skippable = false;
			this._npcConversation.Type();
			this._npcConversation.OpenContentSelector(localizedString, new Action(this.OpenContent), new Action(this.Chat), new Action(this.Close));
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00034220 File Offset: 0x00032420
		public void Close()
		{
			this._npcConversation.Done();
			this._contentSelector.Close();
			this._contents[this._type].gameObject.SetActive(false);
			this._contentContainer.SetActive(false);
		}

		// Token: 0x04000E8A RID: 3722
		[SerializeField]
		private NpcConversation _npcConversation;

		// Token: 0x04000E8B RID: 3723
		[SerializeField]
		private NpcContent.TypeGameObjectArray _contents;

		// Token: 0x04000E8C RID: 3724
		[SerializeField]
		private GameObject _contentContainer;

		// Token: 0x04000E8D RID: 3725
		[SerializeField]
		private ContentSelector _contentSelector;

		// Token: 0x04000E8E RID: 3726
		private NpcContent.Type _type;

		// Token: 0x04000E8F RID: 3727
		private string _name;

		// Token: 0x04000E90 RID: 3728
		private string[] _chatScripts;

		// Token: 0x04000E91 RID: 3729
		private string _greeting;

		// Token: 0x04000E92 RID: 3730
		private string _content;

		// Token: 0x020003B6 RID: 950
		[Serializable]
		public class TypeGameObjectArray : EnumArray<NpcContent.Type, GameObject>
		{
		}

		// Token: 0x020003B7 RID: 951
		public enum Type
		{
			// Token: 0x04000E94 RID: 3732
			Witch,
			// Token: 0x04000E95 RID: 3733
			WitchCat,
			// Token: 0x04000E96 RID: 3734
			Druid,
			// Token: 0x04000E97 RID: 3735
			Orge,
			// Token: 0x04000E98 RID: 3736
			Fox
		}
	}
}
