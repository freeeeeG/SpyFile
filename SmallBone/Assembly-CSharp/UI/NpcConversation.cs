using System;
using System.Collections;
using Data;
using GameResources;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI
{
	// Token: 0x020003B8 RID: 952
	public class NpcConversation : MonoBehaviour
	{
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x00034268 File Offset: 0x00032468
		// (set) Token: 0x06001191 RID: 4497 RVA: 0x00034275 File Offset: 0x00032475
		public new string name
		{
			get
			{
				return this._name.text;
			}
			set
			{
				this._name.text = value;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x00034283 File Offset: 0x00032483
		// (set) Token: 0x06001193 RID: 4499 RVA: 0x00034290 File Offset: 0x00032490
		public string body
		{
			get
			{
				return this._body.text;
			}
			set
			{
				this._body.text = value;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x0003429E File Offset: 0x0003249E
		// (set) Token: 0x06001195 RID: 4501 RVA: 0x000342AB File Offset: 0x000324AB
		public Sprite portrait
		{
			get
			{
				return this._portrait.sprite;
			}
			set
			{
				this._portrait.sprite = value;
				this._portraitContainer.SetActive(value != null);
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x000342CB File Offset: 0x000324CB
		// (set) Token: 0x06001197 RID: 4503 RVA: 0x000342D8 File Offset: 0x000324D8
		public bool visible
		{
			get
			{
				return this._container.activeSelf;
			}
			set
			{
				if (this._container.activeSelf == value)
				{
					return;
				}
				if (!value)
				{
					this._currencyBalanceDisplay.gameObject.SetActive(false);
					this._contentSelector.Close();
					this._witchContent.SetActive(false);
				}
				this._container.SetActive(value);
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x0003432B File Offset: 0x0003252B
		// (set) Token: 0x06001199 RID: 4505 RVA: 0x00034338 File Offset: 0x00032538
		public bool skippable
		{
			get
			{
				return this._body.skippable;
			}
			set
			{
				this._body.skippable = value;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x00034346 File Offset: 0x00032546
		public GameObject witchContent
		{
			get
			{
				return this._witchContent;
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0003434E File Offset: 0x0003254E
		private void Awake()
		{
			this._enter.enabled = false;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0003435C File Offset: 0x0003255C
		private void OnDisable()
		{
			this._currencyBalanceDisplay.gameObject.SetActive(false);
			this._contentSelector.Close();
			this._witchContent.SetActive(false);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00034386 File Offset: 0x00032586
		public void OpenChatSelector(Action onChat, Action onCancel)
		{
			this._contentSelector.Open(Localization.GetLocalizedString("npc/conversation/chat"), onChat, Localization.GetLocalizedString("npc/conversation/cancel"), onCancel);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x000343A9 File Offset: 0x000325A9
		public void OpenConfirmSelector(Action onYes, Action onNo)
		{
			this._contentSelector.Open(Localization.GetLocalizedString("label/confirm/yes"), onYes, Localization.GetLocalizedString("label/confirm/no"), onNo);
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x000343CC File Offset: 0x000325CC
		public void OpenContentSelector(string contentLabel, Action onContent, Action onChat, Action onCancel)
		{
			this._contentSelector.Open(contentLabel, onContent, Localization.GetLocalizedString("npc/conversation/chat"), onChat, Localization.GetLocalizedString("npc/conversation/cancel"), onCancel);
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x000343F2 File Offset: 0x000325F2
		public void OpenContentSelector(string contentLabel, Action onContent, string cancelLabel, Action onCancel)
		{
			this._contentSelector.Open(contentLabel, onContent, cancelLabel, onCancel);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00034404 File Offset: 0x00032604
		public void OpenCurrencyBalancePanel(GameData.Currency.Type type)
		{
			this._currencyBalanceDisplay.gameObject.SetActive(true);
			this._currencyBalanceDisplay.SetType(type);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00034423 File Offset: 0x00032623
		public void CloseCurrencyBalancePanel()
		{
			this._currencyBalanceDisplay.gameObject.SetActive(false);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00034436 File Offset: 0x00032636
		public void Talk(string nameKey, string textKey)
		{
			this.TalkRaw(Localization.GetLocalizedString(nameKey), Localization.GetLocalizedString(textKey));
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0003444A File Offset: 0x0003264A
		public void TalkRaw(string name, string text)
		{
			this._name.text = name;
			this._body.text = text;
			this.visible = true;
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0003446B File Offset: 0x0003266B
		public IEnumerator CTalk(string nameKey, string textKey)
		{
			this.portrait = null;
			this.skippable = true;
			this.name = Localization.GetLocalizedString(nameKey);
			this.body = Localization.GetLocalizedString(textKey);
			if (string.IsNullOrWhiteSpace(this.body))
			{
				yield break;
			}
			yield return this.CType();
			yield return this.CWaitInput();
			yield break;
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00034488 File Offset: 0x00032688
		public IEnumerator CTalkRaw(string name, string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				yield break;
			}
			this.portrait = null;
			this.skippable = true;
			this.name = name;
			this.body = text;
			yield return this.CType();
			yield return this.CWaitInput();
			yield break;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x000344A5 File Offset: 0x000326A5
		public void Conversation(params string[] texts)
		{
			base.StartCoroutine(this.CConversation(texts));
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x000344B5 File Offset: 0x000326B5
		public IEnumerator CConversation(params string[] texts)
		{
			if (this._body.typing)
			{
				this.body = string.Empty;
				this._body.StopType();
				yield return null;
			}
			foreach (string body in texts)
			{
				this.body = body;
				yield return this.CType();
				yield return this.CWaitInput();
			}
			string[] array = null;
			this.visible = false;
			yield break;
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x000344CB File Offset: 0x000326CB
		public void Type()
		{
			base.StartCoroutine(this.CType());
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x000344DA File Offset: 0x000326DA
		public IEnumerator CType()
		{
			this.visible = true;
			if (this._body.typing)
			{
				this._body.StopType();
				yield return null;
			}
			yield return this._body.CType(this);
			yield break;
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x000344E9 File Offset: 0x000326E9
		public IEnumerator CWaitInput()
		{
			this._enter.enabled = true;
			do
			{
				yield return null;
			}
			while (!KeyMapper.Map.Attack.WasPressed && !KeyMapper.Map.Jump.WasPressed && !KeyMapper.Map.Submit.WasPressed && !KeyMapper.Map.Cancel.WasPressed);
			this._enter.enabled = false;
			yield break;
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x000344F8 File Offset: 0x000326F8
		public void Done()
		{
			this.visible = false;
		}

		// Token: 0x04000E99 RID: 3737
		[SerializeField]
		private NpcName _name;

		// Token: 0x04000E9A RID: 3738
		[SerializeField]
		private NpcConversationBody _body;

		// Token: 0x04000E9B RID: 3739
		[SerializeField]
		private Image _enter;

		// Token: 0x04000E9C RID: 3740
		[SerializeField]
		private GameObject _container;

		// Token: 0x04000E9D RID: 3741
		[SerializeField]
		private Image _portrait;

		// Token: 0x04000E9E RID: 3742
		[SerializeField]
		private GameObject _portraitContainer;

		// Token: 0x04000E9F RID: 3743
		[SerializeField]
		private ContentSelector _contentSelector;

		// Token: 0x04000EA0 RID: 3744
		[SerializeField]
		private CurrencyBalanceDisplay _currencyBalanceDisplay;

		// Token: 0x04000EA1 RID: 3745
		[SerializeField]
		private GameObject _witchContent;
	}
}
