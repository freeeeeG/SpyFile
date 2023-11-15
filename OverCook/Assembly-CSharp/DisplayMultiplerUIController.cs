using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B29 RID: 2857
public class DisplayMultiplerUIController : MonoBehaviour
{
	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x060039D8 RID: 14808 RVA: 0x001133C6 File Offset: 0x001117C6
	// (set) Token: 0x060039D7 RID: 14807 RVA: 0x001133B7 File Offset: 0x001117B7
	public virtual int FirstValue
	{
		get
		{
			return this.m_first;
		}
		set
		{
			this.m_first = value;
			this.RefreshText();
		}
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x060039DA RID: 14810 RVA: 0x001133DD File Offset: 0x001117DD
	// (set) Token: 0x060039D9 RID: 14809 RVA: 0x001133CE File Offset: 0x001117CE
	public virtual int SecondValue
	{
		get
		{
			return this.m_second;
		}
		set
		{
			this.m_second = value;
			this.RefreshText();
		}
	}

	// Token: 0x060039DB RID: 14811 RVA: 0x001133E8 File Offset: 0x001117E8
	private void RefreshText()
	{
		int num = this.m_first * this.m_second;
		this.m_textUI.text = string.Concat(new string[]
		{
			this.m_first.ToString(),
			" x ",
			this.m_second.ToString(),
			" = ",
			num.ToString()
		});
	}

	// Token: 0x060039DC RID: 14812 RVA: 0x00113461 File Offset: 0x00111861
	protected virtual void Awake()
	{
		if (this.m_textUI == null)
		{
			this.m_textUI = base.gameObject.RequireComponent<Text>();
		}
		this.RefreshText();
	}

	// Token: 0x04002EBF RID: 11967
	[SerializeField]
	[AssignComponentRecursive(Editorbility.NonEditable)]
	private Text m_textUI;

	// Token: 0x04002EC0 RID: 11968
	[SerializeField]
	private int m_first;

	// Token: 0x04002EC1 RID: 11969
	[SerializeField]
	private int m_second = 20;
}
