using System;

// Token: 0x02000031 RID: 49
[Serializable]
public class RuneData
{
	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600023E RID: 574 RVA: 0x0000D820 File Offset: 0x0000BA20
	public string Language_Name
	{
		get
		{
			return this.nameArray[(int)Setting.Inst.language];
		}
	}

	// Token: 0x040001F4 RID: 500
	public string dataName = "UNINITED";

	// Token: 0x040001F5 RID: 501
	public int id;

	// Token: 0x040001F6 RID: 502
	public string[] nameArray = new string[3];

	// Token: 0x040001F7 RID: 503
	public EnumRunePropertyType bigType = EnumRunePropertyType.UNINITED;

	// Token: 0x040001F8 RID: 504
	public int smallType = -1;

	// Token: 0x040001F9 RID: 505
	public bool factorPlayer_IfPositive = true;
}
