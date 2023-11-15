using System;
using System.Collections;
using UI.Pause;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace SkulStories
{
	// Token: 0x02000101 RID: 257
	public class Narration : MonoBehaviour
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0000F81C File Offset: 0x0000DA1C
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0000F824 File Offset: 0x0000DA24
		public bool skipped { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0000F82D File Offset: 0x0000DA2D
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0000F83A File Offset: 0x0000DA3A
		public bool sceneVisible
		{
			get
			{
				return this._sceneContainer.activeSelf;
			}
			set
			{
				if (this._sceneContainer.activeSelf == value)
				{
					return;
				}
				this._sceneContainer.SetActive(value);
				this.Initialize();
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0000F85D File Offset: 0x0000DA5D
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x0000F86A File Offset: 0x0000DA6A
		public bool textVisible
		{
			get
			{
				return this._textContainer.activeSelf;
			}
			set
			{
				if (this._textContainer.activeSelf == value)
				{
					return;
				}
				this._textContainer.SetActive(value);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000F887 File Offset: 0x0000DA87
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x0000F894 File Offset: 0x0000DA94
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000F8A2 File Offset: 0x0000DAA2
		public Image blackScreen
		{
			get
			{
				return this._blackScreen;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000F8AA File Offset: 0x0000DAAA
		public void Initialize()
		{
			base.StopAllCoroutines();
			this.textVisible = false;
			this.skippable = false;
			this.ResetPauseType();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000F8C6 File Offset: 0x0000DAC6
		public IEnumerator CShowText(ShowTexts sequence, string text)
		{
			if (this.skippable || !this.sceneVisible)
			{
				yield break;
			}
			this.ShowText();
			yield return this._body.CShow(sequence, text);
			this.Clear();
			yield break;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000F8E3 File Offset: 0x0000DAE3
		public void CombineTexts(string[] texts)
		{
			if (this.skippable || !this.sceneVisible)
			{
				return;
			}
			this.ShowText();
			this._body.PlaceText(texts);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000F908 File Offset: 0x0000DB08
		private void ShowText()
		{
			base.StartCoroutine(this.CRun());
			this.textVisible = true;
			this._enter.enabled = false;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000F92A File Offset: 0x0000DB2A
		public IEnumerator CWaitInput()
		{
			yield return Chronometer.global.WaitForSeconds(0.5f);
			do
			{
				yield return null;
			}
			while (!KeyMapper.Map.Attack.WasPressed && !KeyMapper.Map.Jump.WasPressed && !KeyMapper.Map.Submit.WasPressed);
			if (this.skippable)
			{
				this.Skip();
			}
			yield break;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000F939 File Offset: 0x0000DB39
		private void Clear()
		{
			if (this._body.isClear)
			{
				this._enter.enabled = true;
				base.StartCoroutine(this.CWaitInput());
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000F961 File Offset: 0x0000DB61
		private IEnumerator CRun()
		{
			while (!this.skippable)
			{
				yield return null;
			}
			this.Clear();
			yield break;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000F970 File Offset: 0x0000DB70
		private void Skip()
		{
			this.skipped = true;
			this.textVisible = false;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000F980 File Offset: 0x0000DB80
		private void ResetPauseType()
		{
			if (this.sceneVisible)
			{
				this._pauseEventSystem.PushEvent(this._pauseEvent);
				return;
			}
			this._pauseEventSystem.PopEvent();
		}

		// Token: 0x040003CB RID: 971
		[SerializeField]
		private GameObject _sceneContainer;

		// Token: 0x040003CC RID: 972
		[SerializeField]
		private GameObject _textContainer;

		// Token: 0x040003CD RID: 973
		[SerializeField]
		private NarrationBody _body;

		// Token: 0x040003CE RID: 974
		[SerializeField]
		private Image _blackScreen;

		// Token: 0x040003CF RID: 975
		[SerializeField]
		private Image _enter;

		// Token: 0x040003D0 RID: 976
		[SerializeField]
		[PauseEvent.SubcomponentAttribute]
		private PauseEvent _pauseEvent;

		// Token: 0x040003D1 RID: 977
		[SerializeField]
		private PauseEventSystem _pauseEventSystem;
	}
}
