using System;
using UnityEngine;

// Token: 0x02000AE7 RID: 2791
public class DebugElementMenu : KButtonMenu
{
	// Token: 0x060055F7 RID: 22007 RVA: 0x001F4ABB File Offset: 0x001F2CBB
	protected override void OnPrefabInit()
	{
		DebugElementMenu.Instance = this;
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060055F8 RID: 22008 RVA: 0x001F4AD0 File Offset: 0x001F2CD0
	protected override void OnForcedCleanUp()
	{
		DebugElementMenu.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060055F9 RID: 22009 RVA: 0x001F4ADE File Offset: 0x001F2CDE
	public void Turnoff()
	{
		this.root.gameObject.SetActive(false);
	}

	// Token: 0x040039B2 RID: 14770
	public static DebugElementMenu Instance;

	// Token: 0x040039B3 RID: 14771
	public GameObject root;
}
