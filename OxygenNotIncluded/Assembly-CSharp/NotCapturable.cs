using System;
using UnityEngine;

// Token: 0x0200072C RID: 1836
[AddComponentMenu("KMonoBehaviour/scripts/NotCapturable")]
public class NotCapturable : KMonoBehaviour
{
	// Token: 0x06003275 RID: 12917 RVA: 0x0010BF0E File Offset: 0x0010A10E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (base.GetComponent<Capturable>() != null)
		{
			DebugUtil.LogErrorArgs(this, new object[]
			{
				"Entity has both Capturable and NotCapturable!"
			});
		}
		Components.NotCapturables.Add(this);
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x0010BF43 File Offset: 0x0010A143
	protected override void OnCleanUp()
	{
		Components.NotCapturables.Remove(this);
		base.OnCleanUp();
	}
}
