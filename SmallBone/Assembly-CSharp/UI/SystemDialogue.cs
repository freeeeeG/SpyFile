using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI
{
	// Token: 0x020003D3 RID: 979
	public sealed class SystemDialogue : MonoBehaviour
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x00019108 File Offset: 0x00017308
		// (set) Token: 0x06001233 RID: 4659 RVA: 0x000359C0 File Offset: 0x00033BC0
		public bool visible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				if (base.gameObject.activeSelf == value)
				{
					return;
				}
				base.gameObject.SetActive(value);
			}
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x000359DD File Offset: 0x00033BDD
		public IEnumerator CShow(string name, string body, bool skippable = true)
		{
			this._name.text = name;
			this._body.text = body;
			this._body.skippable = skippable;
			base.gameObject.SetActive(true);
			yield return this.CShow();
			yield break;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00035A04 File Offset: 0x00033C04
		public void Show(string name, string body, bool skippable = true)
		{
			this._name.text = name;
			this._body.text = body;
			this._body.skippable = skippable;
			base.gameObject.SetActive(true);
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			base.StartCoroutine(this.CShow());
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00035A5C File Offset: 0x00033C5C
		private IEnumerator CShow()
		{
			yield return this.CType();
			yield return this.CWaitInput();
			yield break;
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00035A6B File Offset: 0x00033C6B
		private IEnumerator CType()
		{
			if (this._body.typing)
			{
				this._body.StopType();
				yield return null;
			}
			yield return this._body.CType();
			yield break;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00035A7A File Offset: 0x00033C7A
		private IEnumerator CWaitInput()
		{
			this._enter.enabled = true;
			do
			{
				yield return null;
			}
			while (!KeyMapper.Map.Attack.WasPressed && !KeyMapper.Map.Jump.WasPressed && !KeyMapper.Map.Submit.WasPressed && !KeyMapper.Map.Cancel.WasPressed);
			this._enter.enabled = false;
			this.Done();
			yield break;
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00035A89 File Offset: 0x00033C89
		private void Done()
		{
			this.visible = false;
		}

		// Token: 0x04000F18 RID: 3864
		[SerializeField]
		private TextMeshProUGUI _name;

		// Token: 0x04000F19 RID: 3865
		[SerializeField]
		private NpcConversationBody _body;

		// Token: 0x04000F1A RID: 3866
		[SerializeField]
		private Image _enter;
	}
}
