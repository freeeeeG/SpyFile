using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000145 RID: 325
public class UI_BasicButton : Button
{
	// Token: 0x06000885 RID: 2181 RVA: 0x00020B37 File Offset: 0x0001ED37
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (base.interactable)
		{
			SoundManager.PlaySound("UI", "CommonButton_MouseOver", -1f, -1f, -1f);
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00020B67 File Offset: 0x0001ED67
	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if (base.interactable)
		{
			SoundManager.PlaySound("UI", "CommonButton_Click", -1f, -1f, -1f);
		}
	}
}
