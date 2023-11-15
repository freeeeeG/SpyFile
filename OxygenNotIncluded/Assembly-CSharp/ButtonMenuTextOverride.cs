using System;

// Token: 0x02000BFF RID: 3071
[Serializable]
public struct ButtonMenuTextOverride
{
	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x06006134 RID: 24884 RVA: 0x0023E43D File Offset: 0x0023C63D
	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.Text) && !string.IsNullOrEmpty(this.ToolTip);
		}
	}

	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x06006135 RID: 24885 RVA: 0x0023E466 File Offset: 0x0023C666
	public bool HasCancelText
	{
		get
		{
			return !string.IsNullOrEmpty(this.CancelText) && !string.IsNullOrEmpty(this.CancelToolTip);
		}
	}

	// Token: 0x04004235 RID: 16949
	public LocString Text;

	// Token: 0x04004236 RID: 16950
	public LocString CancelText;

	// Token: 0x04004237 RID: 16951
	public LocString ToolTip;

	// Token: 0x04004238 RID: 16952
	public LocString CancelToolTip;
}
