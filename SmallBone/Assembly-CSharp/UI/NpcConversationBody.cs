using System;
using System.Collections;
using FX;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UserInput;

namespace UI
{
	// Token: 0x020003BE RID: 958
	public class NpcConversationBody : Dialogue
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x000348FF File Offset: 0x00032AFF
		// (set) Token: 0x060011CD RID: 4557 RVA: 0x00034907 File Offset: 0x00032B07
		public bool skippable { get; set; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x00034910 File Offset: 0x00032B10
		// (set) Token: 0x060011CF RID: 4559 RVA: 0x00034918 File Offset: 0x00032B18
		public bool typing { get; private set; }

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00034921 File Offset: 0x00032B21
		// (set) Token: 0x060011D1 RID: 4561 RVA: 0x0003492E File Offset: 0x00032B2E
		public string text
		{
			get
			{
				return this._textMeshPro.text;
			}
			set
			{
				this._textMeshPro.text = value;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x00018EC5 File Offset: 0x000170C5
		public override bool closeWithPauseKey
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0003493C File Offset: 0x00032B3C
		protected override void OnEnable()
		{
			Dialogue.opened.Add(this);
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00034949 File Offset: 0x00032B49
		protected override void OnDisable()
		{
			Dialogue.opened.Remove(this);
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x00034957 File Offset: 0x00032B57
		public IEnumerator CType()
		{
			this.typing = true;
			this._textMeshPro.ForceMeshUpdate(false, false);
			int visibleCharacterCount = this._textMeshPro.textInfo.characterCount;
			this._textMeshPro.maxVisibleCharacters = 0;
			float interval = 0.04f * (1f / this._typeSpeed);
			int num;
			for (int index = 0; index < visibleCharacterCount; index = num + 1)
			{
				if (!this.typing)
				{
					yield break;
				}
				if (this._textMeshPro.text[index] != ' ')
				{
					PersistentSingleton<SoundManager>.Instance.PlaySound(this._systemTypeSoundInfo, Singleton<Service>.Instance.levelManager.player.transform.position);
				}
				TextMeshProUGUI textMeshPro = this._textMeshPro;
				num = textMeshPro.maxVisibleCharacters;
				textMeshPro.maxVisibleCharacters = num + 1;
				this._textMeshPro.havePropertiesChanged = false;
				float time = 0f;
				while (time < interval)
				{
					if (!this.typing)
					{
						yield break;
					}
					yield return null;
					time += Time.unscaledDeltaTime;
					if (this.skippable && (KeyMapper.Map.Attack.WasPressed || KeyMapper.Map.Jump.WasPressed || KeyMapper.Map.Submit.WasPressed || KeyMapper.Map.Cancel.WasPressed))
					{
						goto IL_1A8;
					}
				}
				num = index;
			}
			IL_1A8:
			this._textMeshPro.maxVisibleCharacters = visibleCharacterCount;
			this.typing = false;
			yield break;
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00034966 File Offset: 0x00032B66
		public IEnumerator CType(NpcConversation conversation)
		{
			this.typing = true;
			this._textMeshPro.ForceMeshUpdate(false, false);
			int visibleCharacterCount = this._textMeshPro.textInfo.characterCount;
			this._textMeshPro.maxVisibleCharacters = 0;
			float interval = 0.04f * (1f / this._typeSpeed);
			int num;
			for (int index = 0; index < visibleCharacterCount; index = num + 1)
			{
				if (!this.typing)
				{
					yield break;
				}
				if (!conversation.visible)
				{
					break;
				}
				if (this._textMeshPro.text[index] != ' ')
				{
					if (Singleton<Service>.Instance.levelManager.player != null)
					{
						PersistentSingleton<SoundManager>.Instance.PlaySound(this._typeSoundInfo, Singleton<Service>.Instance.levelManager.player.transform.position);
					}
					else
					{
						PersistentSingleton<SoundManager>.Instance.PlaySound(this._typeSoundInfo, Camera.main.transform.position);
					}
				}
				TextMeshProUGUI textMeshPro = this._textMeshPro;
				num = textMeshPro.maxVisibleCharacters;
				textMeshPro.maxVisibleCharacters = num + 1;
				this._textMeshPro.havePropertiesChanged = false;
				float time = 0f;
				while (time < interval)
				{
					if (!this.typing)
					{
						yield break;
					}
					yield return null;
					time += Time.unscaledDeltaTime;
					if (this.skippable && (KeyMapper.Map.Attack.WasPressed || KeyMapper.Map.Jump.WasPressed || KeyMapper.Map.Submit.WasPressed || KeyMapper.Map.Cancel.WasPressed))
					{
						goto IL_1F1;
					}
				}
				num = index;
			}
			IL_1F1:
			this._textMeshPro.maxVisibleCharacters = visibleCharacterCount;
			this.typing = false;
			yield break;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0003497C File Offset: 0x00032B7C
		public void StopType()
		{
			this._textMeshPro.maxVisibleCharacters = 0;
			this._textMeshPro.havePropertiesChanged = false;
			this.typing = false;
		}

		// Token: 0x04000EB8 RID: 3768
		private const float _lettersPerSecond = 25f;

		// Token: 0x04000EB9 RID: 3769
		private const float _intervalPerLetter = 0.04f;

		// Token: 0x04000EBA RID: 3770
		[SerializeField]
		private TextMeshProUGUI _textMeshPro;

		// Token: 0x04000EBB RID: 3771
		[SerializeField]
		private float _typeSpeed = 1f;

		// Token: 0x04000EBC RID: 3772
		[SerializeField]
		private SoundInfo _typeSoundInfo;

		// Token: 0x04000EBD RID: 3773
		[SerializeField]
		private SoundInfo _systemTypeSoundInfo;
	}
}
