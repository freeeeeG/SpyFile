using System;
using UnityEngine;

// Token: 0x020009D6 RID: 2518
public class ChefAvatarData : ScriptableObject
{
	// Token: 0x06003148 RID: 12616 RVA: 0x000E6E1C File Offset: 0x000E521C
	public bool IsAvailableOnThisPlatform()
	{
		return this.m_PC;
	}

	// Token: 0x04002776 RID: 10102
	public GameObject ModelPrefab;

	// Token: 0x04002777 RID: 10103
	public GameObject FrontendModelPrefab;

	// Token: 0x04002778 RID: 10104
	public GameObject UIModelPrefab;

	// Token: 0x04002779 RID: 10105
	public string HeadName;

	// Token: 0x0400277A RID: 10106
	[Space]
	public ChefMeshReplacer.ChefColourisationMode ColourisationMode;

	// Token: 0x0400277B RID: 10107
	[Header("Legacy")]
	public bool ActuallyAllowed = true;

	// Token: 0x0400277C RID: 10108
	[Header("For DLC")]
	public DLCFrontendData ForDlc;

	// Token: 0x0400277D RID: 10109
	[Header("For Platform")]
	public bool m_PC = true;

	// Token: 0x0400277E RID: 10110
	public bool m_XboxOne = true;

	// Token: 0x0400277F RID: 10111
	public bool m_PS4 = true;

	// Token: 0x04002780 RID: 10112
	public bool m_Switch = true;
}
