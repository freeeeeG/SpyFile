using System;
using FX;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserInput;

namespace UI
{
	// Token: 0x02000384 RID: 900
	public class ContentSelector : Dialogue
	{
		// Token: 0x1700034F RID: 847
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x00018EC5 File Offset: 0x000170C5
		public override bool closeWithPauseKey
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x000309DC File Offset: 0x0002EBDC
		private void Awake()
		{
			UnityAction call = new UnityAction(base.Close);
			this._button1.onClick.AddListener(call);
			this._button2.onClick.AddListener(call);
			this._button3.onClick.AddListener(call);
			this._button1.onClick.AddListener(new UnityAction(this.InvokeButton1Click));
			this._button2.onClick.AddListener(new UnityAction(this.InvokeButton2Click));
			this._button3.onClick.AddListener(new UnityAction(this.InvokeButton3Click));
			Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.AddComponent<PlaySoundOnSelected>().soundInfo = this._selectSound;
			}
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00030AAB File Offset: 0x0002ECAB
		private void InvokeButton1Click()
		{
			this._onButton1Click();
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00030AB8 File Offset: 0x0002ECB8
		private void InvokeButton2Click()
		{
			this._onButton2Click();
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00030AC5 File Offset: 0x0002ECC5
		private void InvokeButton3Click()
		{
			this._onButton3Click();
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00030AD4 File Offset: 0x0002ECD4
		public void Open(string label1, Action action1, string cancelLabel, Action onCancel)
		{
			this._button3.gameObject.SetActive(false);
			this._button1Label.text = label1;
			this._button2Label.text = cancelLabel;
			this._onButton1Click = action1;
			this._onButton2Click = onCancel;
			this._onCancel = onCancel;
			base.Open();
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00030B28 File Offset: 0x0002ED28
		public void Open(string label1, Action action1, string label2, Action action2, string cancelLabel, Action onCancel)
		{
			this._button3.gameObject.SetActive(true);
			this._button1Label.text = label1;
			this._button2Label.text = label2;
			this._button3Label.text = cancelLabel;
			this._onButton1Click = action1;
			this._onButton2Click = action2;
			this._onButton3Click = onCancel;
			this._onCancel = onCancel;
			base.Open();
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00030B90 File Offset: 0x0002ED90
		private new void Update()
		{
			if (KeyMapper.Map.Cancel.WasPressed)
			{
				Action onCancel = this._onCancel;
				if (onCancel == null)
				{
					return;
				}
				onCancel();
			}
		}

		// Token: 0x04000D7C RID: 3452
		private Action _onButton1Click;

		// Token: 0x04000D7D RID: 3453
		private Action _onButton2Click;

		// Token: 0x04000D7E RID: 3454
		private Action _onButton3Click;

		// Token: 0x04000D7F RID: 3455
		private Action _onCancel;

		// Token: 0x04000D80 RID: 3456
		[SerializeField]
		private Button _button1;

		// Token: 0x04000D81 RID: 3457
		[SerializeField]
		private TMP_Text _button1Label;

		// Token: 0x04000D82 RID: 3458
		[SerializeField]
		private Button _button2;

		// Token: 0x04000D83 RID: 3459
		[SerializeField]
		private TMP_Text _button2Label;

		// Token: 0x04000D84 RID: 3460
		[SerializeField]
		private Button _button3;

		// Token: 0x04000D85 RID: 3461
		[SerializeField]
		private TMP_Text _button3Label;

		// Token: 0x04000D86 RID: 3462
		[SerializeField]
		private SoundInfo _selectSound;
	}
}
