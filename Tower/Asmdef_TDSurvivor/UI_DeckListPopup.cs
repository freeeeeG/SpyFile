using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200015E RID: 350
public class UI_DeckListPopup : APopupWindow
{
	// Token: 0x06000928 RID: 2344 RVA: 0x00022CCD File Offset: 0x00020ECD
	private void OnEnable()
	{
		this.button_Leave.onClick.AddListener(new UnityAction(this.OnClickButton_Leave));
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00022CEB File Offset: 0x00020EEB
	private void OnDisable()
	{
		this.button_Leave.onClick.RemoveListener(new UnityAction(this.OnClickButton_Leave));
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00022D09 File Offset: 0x00020F09
	private void OnClickButton_Leave()
	{
		base.CloseWindow();
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00022D11 File Offset: 0x00020F11
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			base.CloseWindow();
		}
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00022D22 File Offset: 0x00020F22
	protected override void CloseWindowProc()
	{
		this.Toggle(false);
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00022D2C File Offset: 0x00020F2C
	protected override void ShowWindowProc()
	{
		List<CardData> list_ItemStorage = GameDataManager.instance.GameplayData.list_ItemStorage;
		this.list_CardData = list_ItemStorage.ToList<CardData>();
		this.list_CardData.Sort((CardData x, CardData y) => x.ItemType.CompareTo(y.ItemType));
		this.list_Cards = new List<UI_Obj_ShopCard>();
		foreach (CardData cardData in this.list_CardData)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.prefab_Card, this.node_GridLayout);
			UI_Obj_ShopCard component = gameObject.GetComponent<UI_Obj_ShopCard>();
			component.SetupContent(cardData.ItemType, UI_Obj_ShopCard.eCardSelectType.SELECTABLE, 0);
			this.list_Cards.Add(component);
			gameObject.transform.localScale = Vector3.one * 0.66f;
		}
		this.text_NoCard.gameObject.SetActive(this.list_Cards.Count == 0);
		this.Toggle(true);
		base.StartCoroutine(this.CR_ShowWindowProc());
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00022E48 File Offset: 0x00021048
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

	// Token: 0x0600092F RID: 2351 RVA: 0x00022EA8 File Offset: 0x000210A8
	private IEnumerator CR_ShowWindowProc()
	{
		for (int i = 0; i < this.list_Cards.Count; i++)
		{
			this.list_Cards[i].ToggleCard(true);
		}
		yield return new WaitForSeconds(0.5f);
		yield break;
	}

	// Token: 0x04000747 RID: 1863
	[SerializeField]
	private Transform node_GridLayout;

	// Token: 0x04000748 RID: 1864
	[SerializeField]
	private GameObject prefab_Card;

	// Token: 0x04000749 RID: 1865
	[SerializeField]
	private Button button_Leave;

	// Token: 0x0400074A RID: 1866
	[SerializeField]
	private TMP_Text text_NoCard;

	// Token: 0x0400074B RID: 1867
	private List<CardData> list_CardData;

	// Token: 0x0400074C RID: 1868
	private List<UI_Obj_ShopCard> list_Cards;
}
