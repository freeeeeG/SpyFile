using System;
using UnityEngine.UI;

// Token: 0x020001C5 RID: 453
public class TechSlot : ItemSlot
{
	// Token: 0x06000B97 RID: 2967 RVA: 0x0001E362 File Offset: 0x0001C562
	public override void SetContent(ContentAttribute attribute, ToggleGroup group)
	{
		base.SetContent(attribute, group);
		this.m_Tech = TechnologyFactory.GetTech((int)((TechAttribute)attribute).TechName);
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x0001E382 File Offset: 0x0001C582
	public override void OnItemSelect(bool value)
	{
		base.OnItemSelect(value);
		if (this.isLock)
		{
			return;
		}
		if (value)
		{
			Singleton<TipsManager>.Instance.ShowTechInfoTips(this.m_Tech, StaticData.RightTipsPos, true);
			return;
		}
		Singleton<TipsManager>.Instance.HideTips();
	}

	// Token: 0x040005C6 RID: 1478
	private Technology m_Tech;
}
