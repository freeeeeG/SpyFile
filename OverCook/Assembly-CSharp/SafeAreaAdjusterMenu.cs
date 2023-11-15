using System;
using UnityEngine;

// Token: 0x02000ADE RID: 2782
public class SafeAreaAdjusterMenu : FrontendMenuBehaviour
{
	// Token: 0x0600384C RID: 14412 RVA: 0x00108FE7 File Offset: 0x001073E7
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			this.m_SafeAreaAdjuster.Show();
			return true;
		}
		return false;
	}

	// Token: 0x0600384D RID: 14413 RVA: 0x00109008 File Offset: 0x00107408
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		this.m_SafeAreaAdjuster.Hide();
		SaveManager saveManager = GameUtils.RequireManager<SaveManager>();
		saveManager.RegisterOnIdle(delegate
		{
			saveManager.SaveMetaProgress(null);
		});
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x0600384E RID: 14414 RVA: 0x00109050 File Offset: 0x00107450
	protected override void Update()
	{
		base.Update();
		if (this.m_SafeAreaAdjuster.Completed)
		{
			this.Hide(true, false);
		}
	}

	// Token: 0x04002D03 RID: 11523
	[SerializeField]
	private SafeAreaAdjuster m_SafeAreaAdjuster;
}
