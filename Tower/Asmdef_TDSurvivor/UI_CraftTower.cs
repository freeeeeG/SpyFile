using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000194 RID: 404
public class UI_CraftTower : AUISituational, IDropHandler, IEventSystemHandler
{
	// Token: 0x06000AD4 RID: 2772 RVA: 0x00028918 File Offset: 0x00026B18
	private void OnEnable()
	{
		EventMgr.Register<bool>(eGameEvents.UI_ToggleCraftTowerUI, new Action<bool>(this.OnToggleCraftTowerUI));
		EventMgr.Register(eGameEvents.UI_RequestUpdateCraftTowerTooltip, new Action(this.OnRequestUpdateCraftTowerTooltip));
		EventMgr.Register<UI_Obj_Card>(eGameEvents.UI_OnCardRemoveFromCraftTowerUI, new Action<UI_Obj_Card>(this.OnCardRemoveFromCraftTowerUI));
		this.button_Craft.onClick.AddListener(new UnityAction(this.OnClick_CraftButton));
		this.button_Close.onClick.AddListener(new UnityAction(this.OnClick_CloseButton));
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x000289B0 File Offset: 0x00026BB0
	private void OnDisable()
	{
		EventMgr.Remove<bool>(eGameEvents.UI_ToggleCraftTowerUI, new Action<bool>(this.OnToggleCraftTowerUI));
		EventMgr.Remove(eGameEvents.UI_RequestUpdateCraftTowerTooltip, new Action(this.OnRequestUpdateCraftTowerTooltip));
		EventMgr.Remove<UI_Obj_Card>(eGameEvents.UI_OnCardRemoveFromCraftTowerUI, new Action<UI_Obj_Card>(this.OnCardRemoveFromCraftTowerUI));
		this.button_Craft.onClick.RemoveListener(new UnityAction(this.OnClick_CraftButton));
		this.button_Close.onClick.RemoveListener(new UnityAction(this.OnClick_CloseButton));
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x00028A46 File Offset: 0x00026C46
	private void OnToggleCraftTowerUI(bool isOn)
	{
		base.Toggle(isOn);
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x00028A4F File Offset: 0x00026C4F
	private void Start()
	{
		this.UpdateStatPageContent();
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x00028A57 File Offset: 0x00026C57
	private void OnRequestUpdateCraftTowerTooltip()
	{
		this.UpdateStatPageContent();
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x00028A5F File Offset: 0x00026C5F
	private void OnCardRemoveFromCraftTowerUI(UI_Obj_Card card)
	{
		if (card == this.dockedCard_Cannon)
		{
			this.dockedCard_Cannon = null;
		}
		if (card == this.dockedCard_Panel)
		{
			this.dockedCard_Panel = null;
		}
		this.UpdateStatPageContent();
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x00028A94 File Offset: 0x00026C94
	private void OnClick_CraftButton()
	{
		if (this.dockedCard_Panel != null && this.dockedCard_Cannon != null)
		{
			EventMgr.SendEvent<CardData, CardData, int>(eGameEvents.RequestCombineCard, this.dockedCard_Cannon.CardData, this.dockedCard_Panel.CardData, -1);
			EventMgr.SendEvent<CardData>(eGameEvents.RequestRemoveCardFromHand, this.dockedCard_Cannon.CardData);
			EventMgr.SendEvent<CardData>(eGameEvents.RequestRemoveCardFromHand, this.dockedCard_Panel.CardData);
			this.dockedCard_Cannon = null;
			this.dockedCard_Panel = null;
		}
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x00028B20 File Offset: 0x00026D20
	private void OnClick_CloseButton()
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleCraftTowerUI, false);
		if (this.dockedCard_Cannon != null)
		{
			this.dockedCard_Cannon.SetDocked(false, null);
		}
		if (this.dockedCard_Panel != null)
		{
			this.dockedCard_Panel.SetDocked(false, null);
		}
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x00028B74 File Offset: 0x00026D74
	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
		{
			return;
		}
		UI_Obj_Card component = eventData.pointerDrag.GetComponent<UI_Obj_Card>();
		if (component != null)
		{
			Vector3 position = component.transform.position;
			switch (component.CardData.CardType)
			{
			case eCardType.PANEL_CARD:
				if (Vector3.Distance(position, this.node_DropArea_Panel.transform.position) > this.dropRange)
				{
					Debug.DrawLine(position, this.node_DropArea_Panel.transform.position, Color.red, 5f);
					DebugManager.Log(eDebugKey.UI, string.Format("製造UI: 拖曳底座卡但不在Drop範圍內 (距離{0:0.00})", Vector3.Distance(component.transform.position, this.node_DropArea_Panel.transform.position)), null);
					return;
				}
				if (this.dockedCard_Panel != null)
				{
					this.dockedCard_Panel.SetDocked(false, null);
				}
				this.dockedCard_Panel = component;
				component.transform.position = this.node_DropArea_Panel.position;
				component.SetDocked(true, this.node_DropArea_Panel);
				break;
			case eCardType.BUFF_CARD:
				throw new NotImplementedException();
			case eCardType.TOWER_CARD:
				if (Vector3.Distance(position, this.node_DropArea_Cannon.transform.position) > this.dropRange)
				{
					Debug.DrawLine(position, this.node_DropArea_Cannon.transform.position, Color.red, 5f);
					DebugManager.Log(eDebugKey.UI, string.Format("製造UI: 拖曳砲台卡但不在Drop範圍內 (距離{0:0.00})", Vector3.Distance(component.transform.position, this.node_DropArea_Cannon.transform.position)), null);
					return;
				}
				if (this.dockedCard_Cannon != null)
				{
					this.dockedCard_Cannon.SetDocked(false, null);
				}
				this.dockedCard_Cannon = component;
				component.transform.position = this.node_DropArea_Cannon.position;
				component.SetDocked(true, this.node_DropArea_Cannon);
				break;
			}
			this.UpdateStatPageContent();
		}
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x00028D64 File Offset: 0x00026F64
	private void UpdateStatPageContent()
	{
		string text = "";
		if (this.dockedCard_Panel != null && this.dockedCard_Cannon != null)
		{
			text += this.dockedCard_Panel.CardData.GetDataSource().GetLocNameString(true);
			text += this.dockedCard_Cannon.CardData.GetDataSource().GetLocNameString(true);
		}
		else
		{
			if (this.dockedCard_Panel != null)
			{
				text = this.dockedCard_Panel.CardData.GetDataSource().GetLocNameString(false);
			}
			if (this.dockedCard_Cannon != null)
			{
				text = this.dockedCard_Cannon.CardData.GetDataSource().GetLocNameString(true);
			}
		}
		this.text_ItemName.text = text;
		string text2 = "";
		if (!(this.dockedCard_Panel != null) || !(this.dockedCard_Cannon != null))
		{
			if (this.dockedCard_Cannon != null)
			{
				text2 += this.dockedCard_Cannon.CardData.GetDataSource().GetLocStatsString();
				text2 += "\n";
			}
			else if (this.dockedCard_Panel != null)
			{
				text2 += this.dockedCard_Panel.CardData.GetDataSource().GetLocStatsString();
			}
		}
		if (this.dockedCard_Cannon != null)
		{
			text2 += "\n";
			text2 += this.dockedCard_Cannon.CardData.GetDataSource().GetLocFlavorTextString();
		}
		this.text_ItemStats.text = text2;
		this.button_Craft.interactable = (this.dockedCard_Panel != null && this.dockedCard_Cannon != null);
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x00028F13 File Offset: 0x00027113
	private GameObject GetSprite(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
		{
			return null;
		}
		eventData.pointerDrag.GetComponent<Image>();
		return null;
	}

	// Token: 0x04000847 RID: 2119
	[SerializeField]
	[Header("砲台卡片的放置節點")]
	private Transform node_DropArea_Cannon;

	// Token: 0x04000848 RID: 2120
	[SerializeField]
	[Header("底座卡片的放置節點")]
	private Transform node_DropArea_Panel;

	// Token: 0x04000849 RID: 2121
	[SerializeField]
	[Header("拖拉卡片過來時的容許範圍")]
	private float dropRange = 10f;

	// Token: 0x0400084A RID: 2122
	[SerializeField]
	[Header("合成的物品名稱文字")]
	private TMP_Text text_ItemName;

	// Token: 0x0400084B RID: 2123
	[SerializeField]
	[Header("合成的物品屬性文字")]
	private TMP_Text text_ItemStats;

	// Token: 0x0400084C RID: 2124
	[SerializeField]
	[Header("按鈕:建造")]
	private Button button_Craft;

	// Token: 0x0400084D RID: 2125
	[SerializeField]
	[Header("按鈕:關閉")]
	private Button button_Close;

	// Token: 0x0400084E RID: 2126
	[SerializeField]
	[Header("--- 資料")]
	private UI_Obj_Card dockedCard_Cannon;

	// Token: 0x0400084F RID: 2127
	[SerializeField]
	private UI_Obj_Card dockedCard_Panel;
}
