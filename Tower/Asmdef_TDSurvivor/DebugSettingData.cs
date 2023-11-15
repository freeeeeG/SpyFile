using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
[Serializable]
public class DebugSettingData
{
	// Token: 0x060005B9 RID: 1465 RVA: 0x00016B08 File Offset: 0x00014D08
	public DebugSettingData()
	{
		this.key = eDebugKey.TEMP_TEST;
		this.color = Color.white;
		this.colorHex = this.CalculateColorHex(this.color);
		this.prefix = "";
		this.isEnabled = true;
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x060005BA RID: 1466 RVA: 0x00016B88 File Offset: 0x00014D88
	// (set) Token: 0x060005BB RID: 1467 RVA: 0x00016B90 File Offset: 0x00014D90
	public eDebugKey Key
	{
		get
		{
			return this.key;
		}
		set
		{
			this.key = value;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060005BC RID: 1468 RVA: 0x00016B99 File Offset: 0x00014D99
	// (set) Token: 0x060005BD RID: 1469 RVA: 0x00016BC0 File Offset: 0x00014DC0
	public string Prefix
	{
		get
		{
			if (string.IsNullOrEmpty(this.prefix))
			{
				return this.key.ToString();
			}
			return this.prefix;
		}
		set
		{
			this.prefix = value;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060005BE RID: 1470 RVA: 0x00016BC9 File Offset: 0x00014DC9
	// (set) Token: 0x060005BF RID: 1471 RVA: 0x00016BD1 File Offset: 0x00014DD1
	public Color Color
	{
		get
		{
			return this.color;
		}
		set
		{
			this.color = value;
			this.colorHex = this.CalculateColorHex(this.color);
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00016BEC File Offset: 0x00014DEC
	public string ColorHex
	{
		get
		{
			if (string.IsNullOrEmpty(this.colorHex))
			{
				this.colorHex = this.CalculateColorHex(this.color);
			}
			return this.colorHex;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00016C13 File Offset: 0x00014E13
	// (set) Token: 0x060005C2 RID: 1474 RVA: 0x00016C1B File Offset: 0x00014E1B
	public bool IsEnabled
	{
		get
		{
			return this.isEnabled;
		}
		set
		{
			this.isEnabled = value;
		}
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00016C24 File Offset: 0x00014E24
	private string CalculateColorHex(Color _color)
	{
		return ColorUtility.ToHtmlStringRGB(_color);
	}

	// Token: 0x04000527 RID: 1319
	[SerializeField]
	private eDebugKey key = eDebugKey.TEMP_TEST;

	// Token: 0x04000528 RID: 1320
	[SerializeField]
	private string prefix = "";

	// Token: 0x04000529 RID: 1321
	[SerializeField]
	private Color color = Color.white;

	// Token: 0x0400052A RID: 1322
	[SerializeField]
	private string colorHex = "";

	// Token: 0x0400052B RID: 1323
	[SerializeField]
	private bool isEnabled = true;
}
