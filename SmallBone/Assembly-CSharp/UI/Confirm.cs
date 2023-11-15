using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInput;

namespace UI
{
	// Token: 0x0200038D RID: 909
	public class Confirm : Dialogue
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool closeWithPauseKey
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00031288 File Offset: 0x0002F488
		public void Open(string text, Action action)
		{
			if (this._text != null)
			{
				this._text.text = text;
			}
			this._yes.onClick.RemoveAllListeners();
			this._yes.onClick.AddListener(delegate
			{
				if (this._elaspedTime > 0.3f)
				{
					this.Close();
					action();
				}
			});
			this._no.onClick.RemoveAllListeners();
			this._no.onClick.AddListener(delegate
			{
				if (this._elaspedTime > 0.3f)
				{
					this.Close();
				}
			});
			base.Open();
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00031324 File Offset: 0x0002F524
		public void Open(string text, Action onYes, Action onNo)
		{
			if (this._text != null)
			{
				this._text.text = text;
			}
			this._yes.onClick.RemoveAllListeners();
			this._yes.onClick.AddListener(delegate
			{
				if (this._elaspedTime > 0.3f)
				{
					this.Close();
					onYes();
				}
			});
			this._no.onClick.RemoveAllListeners();
			this._no.onClick.AddListener(delegate
			{
				if (this._elaspedTime > 0.3f)
				{
					this.Close();
					onNo();
				}
			});
			base.Open();
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x000313C4 File Offset: 0x0002F5C4
		protected override void OnEnable()
		{
			base.OnEnable();
			Chronometer.global.AttachTimeScale(this, 0f);
			this._elaspedTime = 0f;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x000313E7 File Offset: 0x0002F5E7
		protected override void OnDisable()
		{
			base.OnDisable();
			Chronometer.global.DetachTimeScale(this);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x000313FC File Offset: 0x0002F5FC
		protected override void Update()
		{
			EventSystem current = EventSystem.current;
			if (current.currentSelectedGameObject == null)
			{
				current.SetSelectedGameObject(this._lastSelectedGameObject);
			}
			else
			{
				this._lastSelectedGameObject = current.currentSelectedGameObject;
			}
			if (KeyMapper.Map.Pause.WasPressed)
			{
				this._no.onClick.Invoke();
			}
			this._elaspedTime += Time.unscaledDeltaTime;
		}

		// Token: 0x04000DA5 RID: 3493
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x04000DA6 RID: 3494
		[SerializeField]
		private Button _yes;

		// Token: 0x04000DA7 RID: 3495
		[SerializeField]
		private Button _no;

		// Token: 0x04000DA8 RID: 3496
		private float _elaspedTime;

		// Token: 0x04000DA9 RID: 3497
		private GameObject _lastSelectedGameObject;
	}
}
