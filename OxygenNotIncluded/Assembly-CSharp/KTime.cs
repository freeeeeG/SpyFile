using System;
using UnityEngine;

// Token: 0x0200083B RID: 2107
[AddComponentMenu("KMonoBehaviour/scripts/KTime")]
public class KTime : KMonoBehaviour
{
	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x06003D4F RID: 15695 RVA: 0x0015458B File Offset: 0x0015278B
	// (set) Token: 0x06003D50 RID: 15696 RVA: 0x00154593 File Offset: 0x00152793
	public float UnscaledGameTime { get; set; }

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x06003D51 RID: 15697 RVA: 0x0015459C File Offset: 0x0015279C
	// (set) Token: 0x06003D52 RID: 15698 RVA: 0x001545A3 File Offset: 0x001527A3
	public static KTime Instance { get; private set; }

	// Token: 0x06003D53 RID: 15699 RVA: 0x001545AB File Offset: 0x001527AB
	public static void DestroyInstance()
	{
		KTime.Instance = null;
	}

	// Token: 0x06003D54 RID: 15700 RVA: 0x001545B3 File Offset: 0x001527B3
	protected override void OnPrefabInit()
	{
		KTime.Instance = this;
		this.UnscaledGameTime = Time.unscaledTime;
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x001545C6 File Offset: 0x001527C6
	protected override void OnCleanUp()
	{
		KTime.Instance = null;
	}

	// Token: 0x06003D56 RID: 15702 RVA: 0x001545CE File Offset: 0x001527CE
	public void Update()
	{
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			this.UnscaledGameTime += Time.unscaledDeltaTime;
		}
	}
}
