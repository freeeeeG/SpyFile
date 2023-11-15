using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000365 RID: 869
[ExecutionDependency(typeof(LocalisedText))]
public class DisplayIntUIController : UIControllerBase
{
	// Token: 0x17000238 RID: 568
	// (get) Token: 0x060010A0 RID: 4256 RVA: 0x0005FCC5 File Offset: 0x0005E0C5
	// (set) Token: 0x0600109F RID: 4255 RVA: 0x0005FC54 File Offset: 0x0005E054
	public virtual int Value
	{
		get
		{
			return this.m_value;
		}
		set
		{
			this.m_value = value;
			string text = this.m_originalText.Replace(this.m_matchString, this.m_value.ToString());
			if (this.m_textUI as LocalisedText != null)
			{
				LocalisedText localisedText = this.m_textUI as LocalisedText;
				localisedText.literalText = text;
			}
			else
			{
				this.m_textUI.text = text;
			}
		}
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x0005FCD0 File Offset: 0x0005E0D0
	protected virtual void Awake()
	{
		if (this.m_textUI == null)
		{
			this.m_textUI = base.gameObject.RequireComponentRecursive<Text>();
		}
		this.m_originalText = this.m_textUI.text;
		this.Value = this.m_value;
	}

	// Token: 0x04000CD0 RID: 3280
	[SerializeField]
	[AssignComponentRecursive(Editorbility.NonEditable)]
	protected Text m_textUI;

	// Token: 0x04000CD1 RID: 3281
	[SerializeField]
	protected string m_matchString;

	// Token: 0x04000CD2 RID: 3282
	protected string m_originalText;

	// Token: 0x04000CD3 RID: 3283
	protected int m_value;
}
