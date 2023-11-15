using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000936 RID: 2358
public class RestartWarning : MonoBehaviour
{
	// Token: 0x06004474 RID: 17524 RVA: 0x0017FDCA File Offset: 0x0017DFCA
	private void Update()
	{
		if (RestartWarning.ShouldWarn)
		{
			this.text.enabled = true;
			this.image.enabled = true;
		}
	}

	// Token: 0x04002D5B RID: 11611
	public static bool ShouldWarn;

	// Token: 0x04002D5C RID: 11612
	public LocText text;

	// Token: 0x04002D5D RID: 11613
	public Image image;
}
