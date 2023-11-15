using System;
using System.Collections.Generic;

// Token: 0x0200024B RID: 587
public class ShowGameobject : GuideEvent
{
	// Token: 0x06000EFD RID: 3837 RVA: 0x00027B5C File Offset: 0x00025D5C
	public override void Trigger()
	{
		foreach (string name in this.showList)
		{
			Singleton<GuideGirlSystem>.Instance.GetGuideObj(name).SetActive(true);
		}
		foreach (string name2 in this.hideList)
		{
			Singleton<GuideGirlSystem>.Instance.GetGuideObj(name2).SetActive(false);
		}
	}

	// Token: 0x04000765 RID: 1893
	public List<string> showList = new List<string>();

	// Token: 0x04000766 RID: 1894
	public List<string> hideList = new List<string>();
}
