using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000141 RID: 321
public class Obj_UI_MapSceneTowerCard : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600084B RID: 2123 RVA: 0x0001F69F File Offset: 0x0001D89F
	public void SetIsLocked(bool isLocked)
	{
		this.cardFace.gameObject.SetActive(!isLocked);
		this.node_Locked.SetActive(isLocked);
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0001F6C1 File Offset: 0x0001D8C1
	public void SetContent(eItemType itemType, int index)
	{
		this.index = index;
		this.currentData = Singleton<ResourceManager>.Instance.GetItemDataByType(itemType);
		this.cardFace.SetupContent(itemType, eCardType.TOWER_CARD, this.currentData.GetCardIcon(), true);
		this.cardFace.ToggleNameText(true);
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x0001F700 File Offset: 0x0001D900
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.currentData == null)
		{
			return;
		}
		string locNameString = this.currentData.GetLocNameString(true);
		string arg = this.currentData.GetLocStatsString() + "\n" + this.currentData.GetLocFlavorTextString();
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
		EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, locNameString, arg);
		EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._2D, base.transform, Vector3.up * 50f);
		if (this.cardMouseOverTweener != null && this.cardMouseOverTweener.IsActive())
		{
			this.cardMouseOverTweener.Complete();
		}
		this.cardMouseOverTweener = base.transform.DOScale(Vector3.one * 1.2f, 0.15f);
		SoundManager.PlaySound("UI", "CommonButton_MouseOver", 0.9f + 0.03f * (float)this.index, -1f, -1f);
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x0001F808 File Offset: 0x0001DA08
	public void OnPointerExit(PointerEventData eventData)
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
		if (this.cardMouseOverTweener != null && this.cardMouseOverTweener.IsActive())
		{
			this.cardMouseOverTweener.Complete();
		}
		this.cardMouseOverTweener = base.transform.DOScale(Vector3.one * 1f, 0.15f);
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0001F86B File Offset: 0x0001DA6B
	public bool IsHaveCardData()
	{
		return this.currentData != null;
	}

	// Token: 0x040006B2 RID: 1714
	[SerializeField]
	private UI_CardFace cardFace;

	// Token: 0x040006B3 RID: 1715
	[SerializeField]
	private GameObject node_Locked;

	// Token: 0x040006B4 RID: 1716
	private AItemSettingData currentData;

	// Token: 0x040006B5 RID: 1717
	private Tweener cardMouseOverTweener;

	// Token: 0x040006B6 RID: 1718
	private int index = -1;
}
