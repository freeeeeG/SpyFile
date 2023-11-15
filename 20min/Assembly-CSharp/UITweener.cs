using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public abstract class UITweener : MonoBehaviour
{
	// Token: 0x06000403 RID: 1027 RVA: 0x00015640 File Offset: 0x00013840
	private void Awake()
	{
		LeanTween.init(50000);
	}

	// Token: 0x06000404 RID: 1028
	public abstract void Show();

	// Token: 0x06000405 RID: 1029
	public abstract void Hide();

	// Token: 0x06000406 RID: 1030
	public abstract void SetOff();

	// Token: 0x04000217 RID: 535
	public float duration = 0.1f;
}
