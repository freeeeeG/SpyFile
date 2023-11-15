using System;
using UnityEngine;

// Token: 0x02000366 RID: 870
public class DisplayTimeUIController : DisplayIntUIController
{
	// Token: 0x17000239 RID: 569
	// (get) Token: 0x060010A4 RID: 4260 RVA: 0x0005FD54 File Offset: 0x0005E154
	// (set) Token: 0x060010A3 RID: 4259 RVA: 0x0005FD24 File Offset: 0x0005E124
	public override int Value
	{
		get
		{
			return this.m_value;
		}
		set
		{
			this.m_value = Mathf.Clamp(Mathf.CeilToInt((float)value), 0, 5999);
			this.m_textUI.text = this.m_value.ToTimeString();
		}
	}

	// Token: 0x04000CD4 RID: 3284
	private const int k_timeMax = 5999;
}
