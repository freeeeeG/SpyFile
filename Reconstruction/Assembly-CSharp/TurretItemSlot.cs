using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000265 RID: 613
public class TurretItemSlot : ItemSlot
{
	// Token: 0x06000F55 RID: 3925 RVA: 0x00028C9C File Offset: 0x00026E9C
	public override void SetContent(ContentAttribute attribute, ToggleGroup group)
	{
		base.SetContent(attribute, group);
		StrategyType strategyType = ((TurretAttribute)attribute).StrategyType;
		if (strategyType == StrategyType.Element)
		{
			this.m_Strategy = new ElementStrategy((TurretAttribute)attribute, 5);
			return;
		}
		if (strategyType != StrategyType.Composite)
		{
			return;
		}
		this.m_Strategy = new RefactorStrategy((TurretAttribute)attribute, 3, null);
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x00028CEC File Offset: 0x00026EEC
	public override void OnItemSelect(bool value)
	{
		base.OnItemSelect(value);
		if (this.isLock)
		{
			return;
		}
		if (value)
		{
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(Camera.main, base.transform.position);
			Singleton<TipsManager>.Instance.ShowTurreTips(this.m_Strategy, (vector.x > (float)(Screen.width / 2)) ? StaticData.LeftTipsPos : StaticData.RightTipsPos, 1);
			return;
		}
		Singleton<TipsManager>.Instance.HideTips();
	}

	// Token: 0x040007B7 RID: 1975
	private StrategyBase m_Strategy;
}
