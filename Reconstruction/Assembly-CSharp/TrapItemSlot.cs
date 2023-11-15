using System;

// Token: 0x02000264 RID: 612
public class TrapItemSlot : ItemSlot
{
	// Token: 0x06000F53 RID: 3923 RVA: 0x00028C57 File Offset: 0x00026E57
	public override void OnItemSelect(bool value)
	{
		base.OnItemSelect(value);
		if (this.isLock)
		{
			return;
		}
		if (value)
		{
			Singleton<TipsManager>.Instance.ShowTrapTips((TrapAttribute)this.contenAtt, StaticData.RightTipsPos);
			return;
		}
		Singleton<TipsManager>.Instance.HideTips();
	}
}
