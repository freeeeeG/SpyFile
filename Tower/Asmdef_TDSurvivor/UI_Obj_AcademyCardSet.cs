using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000176 RID: 374
public class UI_Obj_AcademyCardSet : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060009E3 RID: 2531 RVA: 0x000253E5 File Offset: 0x000235E5
	public void Setup(int index, Action<int> OnClickCallback)
	{
		this.index = index;
		this.OnClickCallback = OnClickCallback;
		this.image_Frame_Selected.enabled = false;
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x00025404 File Offset: 0x00023604
	public void SetTowerCardToNode(UI_Obj_ShopCard card, int index)
	{
		card.transform.SetParent(this.list_TowerCardNodes[index].transform, false);
		card.OnCardClicked = (Action<UI_Obj_ShopCard>)Delegate.Combine(card.OnCardClicked, new Action<UI_Obj_ShopCard>(this.OnClickCard));
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x00025450 File Offset: 0x00023650
	public void SetTetrisCardToNode(UI_Obj_ShopCard card, int index)
	{
		card.transform.SetParent(this.list_TetrisCardNodes[index].transform, false);
		card.OnCardClicked = (Action<UI_Obj_ShopCard>)Delegate.Combine(card.OnCardClicked, new Action<UI_Obj_ShopCard>(this.OnClickCard));
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x0002549C File Offset: 0x0002369C
	private void OnClickCard(UI_Obj_ShopCard card)
	{
		Debug.Log("OnClickCard: " + card.gameObject.name, card.gameObject);
		this.OnClickButton();
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x000254C4 File Offset: 0x000236C4
	private void OnEnable()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x000254E2 File Offset: 0x000236E2
	private void OnDisable()
	{
		this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00025500 File Offset: 0x00023700
	private void OnClickButton()
	{
		this.OnClickCallback(this.index);
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00025513 File Offset: 0x00023713
	public void PlaySelectedAnim()
	{
		this.animator.SetTrigger("Selected");
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00025525 File Offset: 0x00023725
	public void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
		if (isOn)
		{
			SoundManager.PlaySound("MapScene", "MapNodePage_Academy_ShowCardSet", -1f, -1f, -1f);
		}
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x0002555A File Offset: 0x0002375A
	public void OnPointerEnter(PointerEventData eventData)
	{
		SoundManager.PlaySound("MapScene", "MapNodePage_Academy_MouseOverCardSet", -1f, -1f, -1f);
		this.image_Frame_Selected.enabled = true;
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00025587 File Offset: 0x00023787
	public void OnPointerExit(PointerEventData eventData)
	{
		this.image_Frame_Selected.enabled = false;
	}

	// Token: 0x040007B5 RID: 1973
	[SerializeField]
	private Animator animator;

	// Token: 0x040007B6 RID: 1974
	[SerializeField]
	private Image image_Frame_Selected;

	// Token: 0x040007B7 RID: 1975
	[SerializeField]
	[FormerlySerializedAs("list_CardNodes")]
	private List<GameObject> list_TowerCardNodes;

	// Token: 0x040007B8 RID: 1976
	[SerializeField]
	private List<GameObject> list_TetrisCardNodes;

	// Token: 0x040007B9 RID: 1977
	[SerializeField]
	private Button button;

	// Token: 0x040007BA RID: 1978
	private int index;

	// Token: 0x040007BB RID: 1979
	private Action<int> OnClickCallback;
}
