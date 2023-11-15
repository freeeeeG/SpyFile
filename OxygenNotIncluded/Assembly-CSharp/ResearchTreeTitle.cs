using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000006 RID: 6
public class ResearchTreeTitle : MonoBehaviour
{
	// Token: 0x06000016 RID: 22 RVA: 0x00002510 File Offset: 0x00000710
	public void SetLabel(string txt)
	{
		this.treeLabel.text = txt;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000251E File Offset: 0x0000071E
	public void SetColor(int id)
	{
		this.BG.enabled = (id % 2 != 0);
	}

	// Token: 0x04000009 RID: 9
	[Header("References")]
	[SerializeField]
	private LocText treeLabel;

	// Token: 0x0400000A RID: 10
	[SerializeField]
	private Image BG;
}
