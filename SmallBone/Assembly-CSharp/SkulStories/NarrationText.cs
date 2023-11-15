using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x0200010C RID: 268
	public class NarrationText : MonoBehaviour
	{
		// Token: 0x06000540 RID: 1344 RVA: 0x00010168 File Offset: 0x0000E368
		private void Awake()
		{
			this._narrationList = new List<string>();
			this._visible = new StringBuilder(100);
			this._invisible = new StringBuilder(100);
			this._alpha = new StringBuilder(100);
			this._display = new StringBuilder(100);
			this._intact = new StringBuilder(100);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000101C1 File Offset: 0x0000E3C1
		public void InitializeVisibleBuilder()
		{
			this._visible.Clear();
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000101CF File Offset: 0x0000E3CF
		public void InitializeInvisibleBuilder()
		{
			this._invisible.Clear();
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x000101DD File Offset: 0x0000E3DD
		public IEnumerator CFadeInText(string text)
		{
			this._skippable = false;
			this._text.text = " " + text;
			this._intactText = this._text.text;
			for (int index = 0; index < 256; index += this._textSpeed)
			{
				this._alpha.AppendFormat("<alpha=#{0:X2}>", index);
				this._display.Append(this._alpha.ToString()).Append(" ").Append(text);
				this._text.text = this._display.ToString();
				yield return null;
				this._alpha.Clear();
				this._display.Clear();
				if (this._skippable)
				{
					break;
				}
			}
			yield break;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000101F4 File Offset: 0x0000E3F4
		public void AddText(string[] texts)
		{
			this._intact.Clear();
			foreach (string text in texts)
			{
				this._intact.Append(" ").Append(text);
				this._intactText = this._intact.ToString();
				this._narrationList.Add(" <alpha=#00>" + text);
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00010260 File Offset: 0x0000E460
		public void InsertText()
		{
			foreach (string value in this._narrationList)
			{
				this._text.text = this._text.text.Insert(this._text.text.Length, value);
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x000102D8 File Offset: 0x0000E4D8
		public void CheckContainsText(string text)
		{
			for (int i = 0; i < this._narrationList.Count; i++)
			{
				if (this._narrationList[i].Contains(text))
				{
					this._linesIndex = i;
				}
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00010316 File Offset: 0x0000E516
		public void ReplaceText()
		{
			this._narrationList[this._linesIndex] = this._narrationList[this._linesIndex].Replace("<alpha=#00>", "");
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001034C File Offset: 0x0000E54C
		public void AppendInvisibleText()
		{
			for (int i = 0; i < this._narrationList.Count; i++)
			{
				if (this._narrationList[i].Contains("<alpha=#00>"))
				{
					this._invisible.Append(this._narrationList[i]);
				}
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001039F File Offset: 0x0000E59F
		public IEnumerator CFadeInText()
		{
			this._skippable = false;
			string visible = this._visible.ToString();
			string currentText = this._narrationList[this._linesIndex];
			string invisible = this._invisible.ToString();
			for (int index = 0; index < 256; index += this._textSpeed)
			{
				this._alpha.AppendFormat("<alpha=#{0:X2}>", index);
				this._display.Append(visible).Append(this._alpha.ToString()).Append(currentText).Append(invisible);
				this._text.text = this._display.ToString();
				yield return null;
				this._alpha.Clear();
				this._display.Clear();
				if (this._skippable)
				{
					break;
				}
			}
			yield break;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000103AE File Offset: 0x0000E5AE
		public void AppendVisibleText()
		{
			if (this._skippable)
			{
				return;
			}
			this._visible.Append(this._narrationList[this._linesIndex]);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x000103D6 File Offset: 0x0000E5D6
		public bool Condition()
		{
			return this._linesIndex == this._narrationList.Count - 1;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x000103ED File Offset: 0x0000E5ED
		public void Show()
		{
			this._skippable = true;
			this._text.text = this._intactText;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00010407 File Offset: 0x0000E607
		public void Clear()
		{
			this._text.text = string.Empty;
			this._narrationList.Clear();
			this._skippable = true;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001042B File Offset: 0x0000E62B
		private void OnDisable()
		{
			this.Clear();
		}

		// Token: 0x04000400 RID: 1024
		[SerializeField]
		private TextMeshProUGUI _text;

		// Token: 0x04000401 RID: 1025
		[SerializeField]
		private int _textSpeed = 2;

		// Token: 0x04000402 RID: 1026
		private List<string> _narrationList;

		// Token: 0x04000403 RID: 1027
		private StringBuilder _visible;

		// Token: 0x04000404 RID: 1028
		private StringBuilder _invisible;

		// Token: 0x04000405 RID: 1029
		private StringBuilder _alpha;

		// Token: 0x04000406 RID: 1030
		private StringBuilder _display;

		// Token: 0x04000407 RID: 1031
		private StringBuilder _intact;

		// Token: 0x04000408 RID: 1032
		private int _linesIndex;

		// Token: 0x04000409 RID: 1033
		private string _intactText;

		// Token: 0x0400040A RID: 1034
		private bool _skippable;

		// Token: 0x0400040B RID: 1035
		private const int _colorIndex = 256;
	}
}
