using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000131 RID: 305
public class UI_Obj_WallCard : AUICard
{
	// Token: 0x060007EF RID: 2031 RVA: 0x0001E4FE File Offset: 0x0001C6FE
	protected override void SetupContentProc(CardData cardData)
	{
		this.image_Icon.sprite = cardData.GetCardIcon();
		this.image_Icon.SetNativeSize();
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x0001E51C File Offset: 0x0001C71C
	protected override void DraggingOntoFieldProc()
	{
		if (Vector3.Distance(base.transform.localPosition, this.dragStartLocalPosition) > 50f)
		{
			Debug.Log("拖移超過範圍, 觸發建造功能");
			base.SwitchCardState(eCardState.BUILDING);
			EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.EDIT_MODE);
			eItemType itemType = this.cardData.GetDataSource().GetItemType();
			WallSettingData wallSettingData = Singleton<ResourceManager>.Instance.GetItemDataByType(itemType) as WallSettingData;
			GameObject arg = Singleton<PrefabManager>.Instance.InstantiatePrefab(wallSettingData.GetPrefab(), Vector3.zero, Quaternion.identity, null);
			EventMgr.SendEvent<GameObject, Action>(eGameEvents.RequestStartPlacement, arg, new Action(this.OnPlacementSuccessCallback));
		}
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x0001E5BC File Offset: 0x0001C7BC
	private void OnPlacementSuccessCallback()
	{
		base.RemoveCard();
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x0001E5C4 File Offset: 0x0001C7C4
	protected override void EndDragProc()
	{
	}

	// Token: 0x04000671 RID: 1649
	[SerializeField]
	private Image image_Icon;
}
