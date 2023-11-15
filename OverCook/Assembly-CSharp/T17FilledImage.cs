using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B6B RID: 2923
[AddComponentMenu("T17_UI/Filled Image", 29)]
public class T17FilledImage : T17Image
{
	// Token: 0x06003B73 RID: 15219 RVA: 0x0011AD08 File Offset: 0x00119108
	protected override void Awake()
	{
		base.Awake();
		base.type = Image.Type.Filled;
	}

	// Token: 0x06003B74 RID: 15220 RVA: 0x0011AD17 File Offset: 0x00119117
	public void SetFilledAmount(float fProgress)
	{
		fProgress = Mathf.Clamp(fProgress, 0f, 1f);
		base.fillAmount = fProgress;
	}
}
