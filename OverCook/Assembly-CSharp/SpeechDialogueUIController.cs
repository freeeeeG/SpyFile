using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000B4F RID: 2895
public class SpeechDialogueUIController : HoverIconUIController
{
	// Token: 0x06003ACE RID: 15054 RVA: 0x0011818C File Offset: 0x0011658C
	public void Setup(string _dialogueTag, bool _autoPrint = true)
	{
		this.m_localisedDialogue = Localization.Get(_dialogueTag, new LocToken[0]);
		MatchCollection matchCollection = Regex.Matches(this.m_localisedDialogue, "<.+/>");
		this.m_tags = new SpeechDialogueUIController.Tag[matchCollection.Count];
		for (int i = 0; i < matchCollection.Count; i++)
		{
			this.m_tags[i] = new SpeechDialogueUIController.Tag(matchCollection[i].Index, matchCollection[i].Value);
		}
		Array.Sort<SpeechDialogueUIController.Tag>(this.m_tags, (SpeechDialogueUIController.Tag x, SpeechDialogueUIController.Tag y) => x.StartIndex.CompareTo(y.StartIndex));
		this.m_text.SetNewLocalizationTag(string.Empty);
		this.m_localisedDialogueInfo = new StringInfo(this.m_localisedDialogue);
		this.m_autoPrint = _autoPrint;
		this.m_printage = 0f;
	}

	// Token: 0x06003ACF RID: 15055 RVA: 0x00118264 File Offset: 0x00116664
	public override void LateUpdate()
	{
		if (this.m_autoPrint)
		{
			this.m_letterTimer += TimeManager.GetDeltaTime(base.gameObject);
			this.m_printage = Mathf.Clamp01(this.m_letterTimer / this.m_perLetterDelay / (float)this.CollapsedLength());
		}
		this.m_text.SetNewLocalizationTag(this.m_localisedDialogueInfo.SubstringByTextElements(0, this.GetLetterCount(this.m_printage)));
		base.LateUpdate();
	}

	// Token: 0x06003AD0 RID: 15056 RVA: 0x001182DD File Offset: 0x001166DD
	public void SetPrintage(float _printage)
	{
		this.m_printage = _printage;
	}

	// Token: 0x06003AD1 RID: 15057 RVA: 0x001182E6 File Offset: 0x001166E6
	public bool IsPrinting()
	{
		return this.GetCollapsedLetterCount(this.m_printage) < this.CollapsedLength();
	}

	// Token: 0x06003AD2 RID: 15058 RVA: 0x001182FC File Offset: 0x001166FC
	public void SkipPrinting()
	{
		this.m_letterTimer = (float)(this.m_localisedDialogueInfo.LengthInTextElements + 1) * this.m_perLetterDelay;
	}

	// Token: 0x06003AD3 RID: 15059 RVA: 0x0011831C File Offset: 0x0011671C
	private int GetLetterCount(float _printage)
	{
		int num = this.GetCollapsedLetterCount(_printage);
		for (int i = 0; i < this.m_tags.Length; i++)
		{
			if (num > this.m_tags[i].StartIndex)
			{
				num += this.m_tags[i].Text.Length - 1;
			}
		}
		return num;
	}

	// Token: 0x06003AD4 RID: 15060 RVA: 0x00118378 File Offset: 0x00116778
	private int GetCollapsedLetterCount(float _printage)
	{
		int value = Mathf.FloorToInt(_printage * (float)this.CollapsedLength());
		return Mathf.Clamp(value, 0, this.CollapsedLength());
	}

	// Token: 0x06003AD5 RID: 15061 RVA: 0x001183A4 File Offset: 0x001167A4
	private int CollapsedLength()
	{
		int num = 0;
		for (int i = 0; i < this.m_tags.Length; i++)
		{
			num += this.m_tags[i].Text.Length - 1;
		}
		return this.m_localisedDialogueInfo.LengthInTextElements - num;
	}

	// Token: 0x04002FB5 RID: 12213
	[SerializeField]
	private T17Text m_text;

	// Token: 0x04002FB6 RID: 12214
	[SerializeField]
	private float m_perLetterDelay = 0.05f;

	// Token: 0x04002FB7 RID: 12215
	private SpeechDialogueUIController.Tag[] m_tags = new SpeechDialogueUIController.Tag[0];

	// Token: 0x04002FB8 RID: 12216
	private string m_localisedDialogue = string.Empty;

	// Token: 0x04002FB9 RID: 12217
	private bool m_autoPrint = true;

	// Token: 0x04002FBA RID: 12218
	private float m_printage;

	// Token: 0x04002FBB RID: 12219
	private float m_letterTimer;

	// Token: 0x04002FBC RID: 12220
	private StringInfo m_localisedDialogueInfo;

	// Token: 0x02000B50 RID: 2896
	private class Tag
	{
		// Token: 0x06003AD7 RID: 15063 RVA: 0x00118403 File Offset: 0x00116803
		public Tag(int _index, string _text)
		{
			this.StartIndex = _index;
			this.Text = _text;
		}

		// Token: 0x04002FBE RID: 12222
		public int StartIndex;

		// Token: 0x04002FBF RID: 12223
		public string Text;
	}
}
