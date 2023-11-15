using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200018B RID: 395
public class UI_TowerArrange_Popup : APopupWindow
{
	// Token: 0x06000A7F RID: 2687 RVA: 0x00027267 File Offset: 0x00025467
	private void OnEnable()
	{
		this.button_CloseWindow.onClick.AddListener(new UnityAction(this.OnClickButton_CloseWindow));
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x00027285 File Offset: 0x00025485
	private void OnDisable()
	{
		this.button_CloseWindow.onClick.RemoveListener(new UnityAction(this.OnClickButton_CloseWindow));
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x000272A4 File Offset: 0x000254A4
	private void Update()
	{
		if (this.text_NoOtherTowerCard.gameObject.activeSelf && this.node_ScrollviewContent.childCount > 0)
		{
			this.text_NoOtherTowerCard.gameObject.SetActive(false);
		}
		if (!this.text_NoOtherTowerCard.gameObject.activeSelf && this.node_ScrollviewContent.childCount == 0)
		{
			this.text_NoOtherTowerCard.gameObject.SetActive(true);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			base.CloseWindow();
		}
		bool flag = GameDataManager.instance.GameplayData.GetCollectedTowerList().Count > 0;
		bool flag2 = false;
		using (List<UI_Obj_CardSlot>.Enumerator enumerator = this.list_CardSlot.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasCardInSlot())
				{
					flag2 = true;
					break;
				}
			}
		}
		if (flag)
		{
			if (!flag2 && this.button_CloseWindow.gameObject.activeSelf)
			{
				this.button_CloseWindow.gameObject.SetActive(false);
			}
			if (flag2 && !this.button_CloseWindow.gameObject.activeSelf)
			{
				this.button_CloseWindow.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x000273D4 File Offset: 0x000255D4
	private void OnClickButton_CloseWindow()
	{
		List<TowerIngameData> list = new List<TowerIngameData>();
		for (int i = 0; i < this.list_CardSlot.Count; i++)
		{
			if (this.list_CardSlot[i].HasCardInSlot())
			{
				list.Add(this.list_CardSlot[i].GetCurrentCard().GetTowerIngameData());
			}
		}
		EventMgr.SendEvent<List<TowerIngameData>>(eGameEvents.RequestOverrideTowerLoadout, list);
		GameDataManager.instance.SaveData();
		base.CloseWindow();
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0002744C File Offset: 0x0002564C
	protected override void Start()
	{
		base.Start();
		List<TowerIngameData> loadoutTowerList = GameDataManager.instance.GameplayData.GetLoadoutTowerList();
		List<TowerIngameData> list_CollectedTowers = GameDataManager.instance.GameplayData.GetCollectedTowerList();
		for (int j = 0; j < loadoutTowerList.Count; j++)
		{
			UI_DraggableCard card = this.CreateCard(loadoutTowerList[j]);
			this.list_CardSlot[j].PutCardOnSlot(card);
		}
		bool flag = false;
		int i;
		Predicate<TowerIngameData> <>9__0;
		int i2;
		for (i = 0; i < list_CollectedTowers.Count; i = i2 + 1)
		{
			List<TowerIngameData> list = loadoutTowerList;
			Predicate<TowerIngameData> match;
			if ((match = <>9__0) == null)
			{
				match = (<>9__0 = ((TowerIngameData x) => x.ItemType == list_CollectedTowers[i].ItemType));
			}
			if (!list.Exists(match))
			{
				this.CreateCard(list_CollectedTowers[i]).ReturnToCardPool();
				flag = true;
			}
			i2 = i;
		}
		this.text_NoOtherTowerCard.gameObject.SetActive(!flag);
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00027548 File Offset: 0x00025748
	private UI_DraggableCard CreateCard(TowerIngameData data)
	{
		UI_DraggableCard component = Object.Instantiate<GameObject>(this.prefab_DraggableCard, this.node_DraggingCardParent).GetComponent<UI_DraggableCard>();
		component.SetupContent(data);
		component.SetupReference(this, this.node_ScrollviewContent, this.node_DraggingCardParent);
		UI_DraggableCard ui_DraggableCard = component;
		ui_DraggableCard.OnCardClickCallback = (Action<UI_DraggableCard>)Delegate.Combine(ui_DraggableCard.OnCardClickCallback, new Action<UI_DraggableCard>(this.OnCardClickCallback));
		UI_DraggableCard ui_DraggableCard2 = component;
		ui_DraggableCard2.OnCardStartDragCallback = (Action<UI_DraggableCard>)Delegate.Combine(ui_DraggableCard2.OnCardStartDragCallback, new Action<UI_DraggableCard>(this.OnCardStartDragCallback));
		UI_DraggableCard ui_DraggableCard3 = component;
		ui_DraggableCard3.OnCardEndDragCallback = (Action<UI_DraggableCard>)Delegate.Combine(ui_DraggableCard3.OnCardEndDragCallback, new Action<UI_DraggableCard>(this.OnCardEndDragCallback));
		this.list_DraggableCard.Add(component);
		return component;
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x000275FC File Offset: 0x000257FC
	public UI_Obj_CardSlot GetEmptyCardSlot()
	{
		for (int i = 0; i < this.list_CardSlot.Count; i++)
		{
			if (!this.list_CardSlot[i].HasCardInSlot())
			{
				return this.list_CardSlot[i];
			}
		}
		return null;
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00027640 File Offset: 0x00025840
	public void OnCardStartDragCallback(UI_DraggableCard card)
	{
		foreach (UI_DraggableCard ui_DraggableCard in this.list_DraggableCard)
		{
			ui_DraggableCard.ToggleRaycast(false);
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x00027694 File Offset: 0x00025894
	public void OnCardEndDragCallback(UI_DraggableCard card)
	{
		foreach (UI_DraggableCard ui_DraggableCard in this.list_DraggableCard)
		{
			ui_DraggableCard.ToggleRaycast(true);
		}
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x000276E8 File Offset: 0x000258E8
	public void OnCardClickCallback(UI_DraggableCard card)
	{
		card.transform.SetParent(this.node_DraggingCardParent);
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x000276FB File Offset: 0x000258FB
	public void OnCardDropCallback(UI_DraggableCard card)
	{
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x000276FD File Offset: 0x000258FD
	protected override void ShowWindowProc()
	{
		this.Toggle(true);
		base.StartCoroutine(this.CR_ShowWindowProc());
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x00027713 File Offset: 0x00025913
	private IEnumerator CR_ShowWindowProc()
	{
		yield return new WaitForSeconds(0.2f);
		using (List<UI_DraggableCard>.Enumerator enumerator = this.list_DraggableCard.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UI_DraggableCard ui_DraggableCard = enumerator.Current;
				ui_DraggableCard.StartCardFollowing();
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00027722 File Offset: 0x00025922
	protected override void CloseWindowProc()
	{
		this.Toggle(false);
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x0002772C File Offset: 0x0002592C
	public void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
		if (isOn)
		{
			SoundManager.PlaySound("UI", "CommonPopupWindow", -1f, -1f, -1f);
			return;
		}
		SoundManager.PlaySound("UI", "CommonHideWindow", -1f, -1f, -1f);
	}

	// Token: 0x0400080C RID: 2060
	[SerializeField]
	private GameObject prefab_DraggableCard;

	// Token: 0x0400080D RID: 2061
	[SerializeField]
	private List<UI_Obj_CardSlot> list_CardSlot;

	// Token: 0x0400080E RID: 2062
	[SerializeField]
	private Transform node_ScrollviewContent;

	// Token: 0x0400080F RID: 2063
	[SerializeField]
	private Transform node_DraggingCardParent;

	// Token: 0x04000810 RID: 2064
	[SerializeField]
	private Button button_CloseWindow;

	// Token: 0x04000811 RID: 2065
	[SerializeField]
	private TMP_Text text_NoOtherTowerCard;

	// Token: 0x04000812 RID: 2066
	[SerializeField]
	private List<UI_DraggableCard> list_DraggableCard = new List<UI_DraggableCard>();
}
