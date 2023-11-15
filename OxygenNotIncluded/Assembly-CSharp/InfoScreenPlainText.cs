using System;
using UnityEngine;

// Token: 0x02000B22 RID: 2850
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenPlainText")]
public class InfoScreenPlainText : KMonoBehaviour
{
	// Token: 0x060057B9 RID: 22457 RVA: 0x002013D1 File Offset: 0x001FF5D1
	public void SetText(string text)
	{
		this.locText.text = text;
	}

	// Token: 0x04003B4E RID: 15182
	[SerializeField]
	private LocText locText;
}
