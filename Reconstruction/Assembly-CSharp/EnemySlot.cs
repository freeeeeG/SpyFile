using System;
using System.Collections.Generic;

// Token: 0x02000262 RID: 610
public class EnemySlot : ItemSlot
{
	// Token: 0x06000F4E RID: 3918 RVA: 0x00028B18 File Offset: 0x00026D18
	public override void OnItemSelect(bool value)
	{
		base.OnItemSelect(value);
		if (this.isLock)
		{
			return;
		}
		if (value)
		{
			Singleton<TipsManager>.Instance.ShowEnemyTips(new List<EnemyAttribute>
			{
				(EnemyAttribute)this.contenAtt
			}, StaticData.RightTipsPos);
			return;
		}
		Singleton<TipsManager>.Instance.HideTips();
	}
}
