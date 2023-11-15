using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200026C RID: 620
public class BonusSlot : MonoBehaviour
{
	// Token: 0x06000F6B RID: 3947 RVA: 0x0002949B File Offset: 0x0002769B
	public void SetBonusInfo(bool value, TurretAttribute attribute = null)
	{
		if (attribute != null)
		{
			this.icon.sprite = attribute.Icon;
			this.nameTxt.text = GameMultiLang.GetTraduction(attribute.Name);
		}
		base.gameObject.SetActive(value);
	}

	// Token: 0x040007D9 RID: 2009
	[SerializeField]
	private Image icon;

	// Token: 0x040007DA RID: 2010
	[SerializeField]
	private Text nameTxt;
}
