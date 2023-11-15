using System;
using System.Collections;
using UnityEngine;
using UserInput;

namespace SkulStories
{
	// Token: 0x02000105 RID: 261
	public class NarrationBody : MonoBehaviour
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x0000FB6F File Offset: 0x0000DD6F
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x0000FB77 File Offset: 0x0000DD77
		public bool isClear
		{
			get
			{
				return this._isClear;
			}
			set
			{
				this._isClear = value;
				this.skippable = value;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x0000FB87 File Offset: 0x0000DD87
		// (set) Token: 0x0600050D RID: 1293 RVA: 0x0000FB8F File Offset: 0x0000DD8F
		public bool skippable { get; set; }

		// Token: 0x0600050E RID: 1294 RVA: 0x0000FB98 File Offset: 0x0000DD98
		public IEnumerator CShow(ShowTexts sequence, string text)
		{
			this._isClear = false;
			base.StartCoroutine(this.CWaitInput());
			ShowTexts.Type type = sequence.type;
			if (type != ShowTexts.Type.SplitText)
			{
				if (type == ShowTexts.Type.IntactText)
				{
					yield return this.CShowIntactText(text);
				}
			}
			else
			{
				yield return this.CShowSplitText(text);
			}
			yield break;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0000FBB5 File Offset: 0x0000DDB5
		public IEnumerator CShowIntactText(string text)
		{
			yield return this._textInfo.CFadeInText(text);
			this.isClear = true;
			yield break;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0000FBCB File Offset: 0x0000DDCB
		public void PlaceText(string[] texts)
		{
			this.skippable = false;
			this._textInfo.InitializeVisibleBuilder();
			this._textInfo.AddText(texts);
			this._textInfo.InsertText();
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0000FBF6 File Offset: 0x0000DDF6
		public IEnumerator CShowSplitText(string text)
		{
			this._textInfo.InitializeInvisibleBuilder();
			this._textInfo.CheckContainsText(text);
			this._textInfo.ReplaceText();
			this._textInfo.AppendInvisibleText();
			yield return this._textInfo.CFadeInText();
			this._textInfo.AppendVisibleText();
			if (this._textInfo.Condition())
			{
				this.isClear = true;
			}
			yield break;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0000FC0C File Offset: 0x0000DE0C
		public IEnumerator CWaitInput()
		{
			yield return Chronometer.global.WaitForSeconds(0.5f);
			do
			{
				yield return null;
			}
			while (!KeyMapper.Map.Attack.WasPressed && !KeyMapper.Map.Jump.WasPressed && !KeyMapper.Map.Submit.WasPressed);
			if (!this._isClear)
			{
				this.ShowText();
			}
			yield break;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0000FC1B File Offset: 0x0000DE1B
		private void ShowText()
		{
			this._textInfo.Show();
			this.isClear = true;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0000FC2F File Offset: 0x0000DE2F
		public void Clear()
		{
			this._isClear = true;
			this._textInfo.Clear();
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0000FC43 File Offset: 0x0000DE43
		private void OnDisable()
		{
			this.Clear();
		}

		// Token: 0x040003DE RID: 990
		[SerializeField]
		private NarrationText _textInfo;

		// Token: 0x040003DF RID: 991
		private bool _isClear;
	}
}
