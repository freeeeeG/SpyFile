using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012E RID: 302
public class UI_Obj_Card : AUICard
{
	// Token: 0x060007C6 RID: 1990 RVA: 0x0001D7FC File Offset: 0x0001B9FC
	protected override void SetupContentProc(CardData cardData)
	{
		if (cardData == null)
		{
			Debug.LogError("CardData是空的!");
		}
		this.cardData = cardData;
		if (this.cardFace == null)
		{
			Debug.LogError("沒有指定cardFace物件連結!", base.gameObject);
		}
		Debug.Log(string.Format("cardData.CardType = {0}", cardData.CardType));
		this.cardFace.SetupContent(cardData.ItemType, cardData.CardType, cardData.GetCardIcon(), true);
		this.cardFace.ToggleNameText(cardData.CardType == eCardType.TOWER_CARD || cardData.CardType == eCardType.BUFF_CARD);
		this.cardFace.transform.localRotation = Quaternion.Euler(0f, 0f, -8f);
		this.cardFace.transform.localScale = Vector3.one * 0.9f;
		base.SwitchCardState(eCardState.HIDE);
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x0001D8E0 File Offset: 0x0001BAE0
	protected override void DraggingOntoFieldProc()
	{
		if (this.cardData.CardType == eCardType.TOWER_CARD)
		{
			if (Vector3.Distance(base.transform.localPosition, this.dragStartLocalPosition) > 50f)
			{
				Debug.Log("拖移超過範圍, 觸發建造功能");
				base.SwitchCardState(eCardState.BUILDING);
				EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.EDIT_MODE);
				TowerSettingData data = this.cardData.GetDataSource().GetScriptableObjectData() as TowerSettingData;
				GameObject gameObject = Singleton<ResourceManager>.Instance.CreateTower(data).gameObject;
				EventMgr.SendEvent<GameObject, Action>(eGameEvents.RequestStartPlacement, gameObject, new Action(this.OnPlacementSuccessCallback));
				return;
			}
		}
		else if (this.cardData.CardType == eCardType.PANEL_CARD)
		{
			if (Vector3.Distance(base.transform.localPosition, this.dragStartLocalPosition) > 50f)
			{
				Debug.Log("拖移超過範圍, 觸發建造功能");
				base.SwitchCardState(eCardState.BUILDING);
				EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.EDIT_MODE);
				eItemType itemType = this.cardData.GetDataSource().GetItemType();
				PanelSettingData panelSettingData = Singleton<ResourceManager>.Instance.GetItemDataByType(itemType) as PanelSettingData;
				GameObject arg = Singleton<PrefabManager>.Instance.InstantiatePrefab(panelSettingData.GetPrefab(), Vector3.zero, Quaternion.identity, null);
				EventMgr.SendEvent<GameObject, Action>(eGameEvents.RequestStartPlacement, arg, new Action(this.OnPlacementSuccessCallback));
				return;
			}
		}
		else if (this.cardData.CardType == eCardType.BUFF_CARD && Vector3.Distance(base.transform.localPosition, this.dragStartLocalPosition) > 50f)
		{
			Debug.Log("拖移超過範圍, 觸發功能");
			base.SwitchCardState(eCardState.BUILDING);
			EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.BUFF_MODE);
			ABaseBuffSettingData arg2 = this.cardData.data.GetScriptableObjectData() as ABaseBuffSettingData;
			EventMgr.SendEvent<ABaseBuffSettingData, Action>(eGameEvents.RequestStartBuffSelection, arg2, new Action(this.OnPlacementSuccessCallback));
			EventMgr.SendEvent<bool>(eGameEvents.UI_TogglePlacementPointerArrow, true);
			EventMgr.SendEvent<Transform, Transform>(eGameEvents.UI_SetPlacementPointerArrowTarget, base.transform, base.transform);
		}
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x0001DAD3 File Offset: 0x0001BCD3
	private void OnPlacementSuccessCallback()
	{
		base.RemoveCard();
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0001DADB File Offset: 0x0001BCDB
	protected override void EndDragProc()
	{
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x0001DADD File Offset: 0x0001BCDD
	public void OverrideIconSprite(Sprite sprite)
	{
		this.image_Icon.sprite = sprite;
		this.image_Icon.AdjustSizeToSprite();
	}

	// Token: 0x04000652 RID: 1618
	[SerializeField]
	private UI_CardFace cardFace;

	// Token: 0x04000653 RID: 1619
	[SerializeField]
	private Image image_Icon;
}
