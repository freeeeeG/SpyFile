using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200012F RID: 303
public class UI_Obj_DiscoverCard : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060007CC RID: 1996 RVA: 0x0001DAFE File Offset: 0x0001BCFE
	public DiscoverRewardData Data
	{
		get
		{
			return this.curData;
		}
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x0001DB06 File Offset: 0x0001BD06
	private void OnEnable()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x0001DB24 File Offset: 0x0001BD24
	private void OnDisable()
	{
		this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
		this.curData = null;
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0001DB4C File Offset: 0x0001BD4C
	private void OnClickButton()
	{
		if (this.isClicked)
		{
			return;
		}
		this.particle_Clicked.Play();
		this.animator.SetBool("isSelected", true);
		SoundManager.PlaySound("UI", "DiscoverCard_Select", -1f, -1f, -1f);
		Action<UI_Obj_DiscoverCard> onCardClicked = this.OnCardClicked;
		if (onCardClicked != null)
		{
			onCardClicked(this);
		}
		this.isClicked = true;
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0001DBB8 File Offset: 0x0001BDB8
	public void SetupContent(DiscoverRewardData data)
	{
		this.curData = data;
		this.isClicked = false;
		new CardData().CardType = data.DiscoverRewardType.ToCardType();
		eItemType itemTypeFromData = this.GetItemTypeFromData(data);
		eDiscoverRewardType discoverRewardType = data.DiscoverRewardType;
		Sprite iconSprite = null;
		switch (discoverRewardType)
		{
		default:
			Debug.LogError("DiscoverRewardData沒有設定內容");
			break;
		case eDiscoverRewardType.HP:
			this.text_ItemName.text = LocalizationManager.Instance.GetString("UI", "HP", Array.Empty<object>());
			this.text_Count.text = string.Format("+{0}", data.value);
			this.text_Cost.text = "";
			iconSprite = this.sprite_HP;
			break;
		case eDiscoverRewardType.COIN:
			this.text_ItemName.text = LocalizationManager.Instance.GetString("UI", "COIN", Array.Empty<object>());
			this.text_Count.text = string.Format("+{0}", data.value);
			this.text_Cost.text = "";
			iconSprite = this.sprite_Coin;
			break;
		case eDiscoverRewardType.TOWER_CARD:
		{
			AItemSettingData itemDataByType = Singleton<ResourceManager>.Instance.GetItemDataByType(itemTypeFromData);
			this.text_ItemName.text = LocalizationManager.Instance.GetString("TowerType", itemTypeFromData.ToString(), Array.Empty<object>());
			this.text_Count.text = "";
			this.text_Cost.text = itemDataByType.GetBuildCost(1f).ToString();
			iconSprite = itemDataByType.GetCardIcon();
			break;
		}
		case eDiscoverRewardType.PANEL_CARD:
		{
			this.text_ItemName.text = LocalizationManager.Instance.GetString("UI", "PANEL", Array.Empty<object>());
			this.text_Count.text = ((data.value > 1) ? string.Format("x{0}", data.value) : "");
			this.text_Cost.text = "";
			AItemSettingData itemDataByType = Singleton<ResourceManager>.Instance.GetItemDataByType(itemTypeFromData);
			iconSprite = itemDataByType.GetCardIcon();
			break;
		}
		case eDiscoverRewardType.RANDOM_PANEL_CARD:
			this.text_ItemName.text = LocalizationManager.Instance.GetString("UI", "RANDOM_PANEL", Array.Empty<object>());
			this.text_Count.text = ((data.value > 1) ? string.Format("x{0}", data.value) : "");
			this.text_Cost.text = "";
			iconSprite = this.sprite_QuestionMark;
			break;
		case eDiscoverRewardType.BUFF_CARD:
		{
			this.text_ItemName.text = LocalizationManager.Instance.GetString("BuffCardName", itemTypeFromData.ToString(), Array.Empty<object>());
			this.text_Count.text = ((data.value > 1) ? string.Format("x{0}", data.value) : "");
			this.text_Cost.text = "";
			AItemSettingData itemDataByType = Singleton<ResourceManager>.Instance.GetItemDataByType(itemTypeFromData);
			iconSprite = itemDataByType.GetCardIcon();
			break;
		}
		case eDiscoverRewardType.RANDOM_BUFF_CARD:
			this.text_ItemName.text = LocalizationManager.Instance.GetString("UI", "RANDOM_BUFF", Array.Empty<object>());
			this.text_Count.text = ((data.value > 1) ? string.Format("x{0}", data.value) : "");
			this.text_Cost.text = "";
			iconSprite = this.sprite_QuestionMark;
			break;
		}
		this.cardFace.SetupContent(itemTypeFromData, data.DiscoverRewardType.ToCardType(), iconSprite, false);
		this.animator.SetBool("isSelected", false);
		this.animator.CrossFade("Off", 0f);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x0001DF7C File Offset: 0x0001C17C
	private eItemType GetItemTypeFromData(DiscoverRewardData data)
	{
		if (data.List_RewardContentType != null && data.List_RewardContentType.Count != 0)
		{
			return data.List_RewardContentType[0];
		}
		return eItemType.NONE;
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0001DFA1 File Offset: 0x0001C1A1
	private void SetIconSprite(Sprite sprite)
	{
		this.image_Icon.sprite = sprite;
		this.image_Icon.AdjustSizeToSprite();
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0001DFBA File Offset: 0x0001C1BA
	public void ToggleCard(bool isOn)
	{
		this.Toggle(isOn);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0001DFC3 File Offset: 0x0001C1C3
	private void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0001DFD8 File Offset: 0x0001C1D8
	public void OnPointerEnter(PointerEventData eventData)
	{
		eItemType itemTypeFromData = this.GetItemTypeFromData(this.curData);
		AItemSettingData itemDataByType = Singleton<ResourceManager>.Instance.GetItemDataByType(itemTypeFromData);
		string locNameString = itemDataByType.GetLocNameString(false);
		string arg = itemDataByType.GetLocStatsString() + "\n" + itemDataByType.GetLocFlavorTextString();
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
		EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, locNameString, arg);
		EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._2D, base.transform, Vector3.up * 50f);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x0001E066 File Offset: 0x0001C266
	public void OnPointerExit(PointerEventData eventData)
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x0001E079 File Offset: 0x0001C279
	public void FlyToTarget(Vector3 targetPos, float duration)
	{
		base.StartCoroutine(this.Co_FlyToTarget(targetPos, duration));
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0001E08A File Offset: 0x0001C28A
	private IEnumerator Co_FlyToTarget(Vector3 targetPos, float duration)
	{
		Vector3 startPos = base.transform.position;
		Vector3 startScale = base.transform.localScale;
		float timer = 0f;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			float num = timer / duration;
			base.transform.position = Vector3.Lerp(startPos, targetPos, Easing.GetEasingFunction(Easing.Type.EaseOutCubic, num));
			base.transform.localPosition += Vector3.right * Mathf.Sin(num * 3.1415927f) * 100f;
			base.transform.localPosition += Vector3.up * Mathf.Sin(num * 3.1415927f) * 300f;
			base.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, Easing.GetEasingFunction(Easing.Type.EaseInCubic, num));
			yield return null;
		}
		base.transform.position = targetPos;
		yield break;
	}

	// Token: 0x04000654 RID: 1620
	[SerializeField]
	private Animator animator;

	// Token: 0x04000655 RID: 1621
	[SerializeField]
	private UI_CardFace cardFace;

	// Token: 0x04000656 RID: 1622
	[SerializeField]
	private Button button;

	// Token: 0x04000657 RID: 1623
	[SerializeField]
	private GameObject node_InventoryFull;

	// Token: 0x04000658 RID: 1624
	[SerializeField]
	private TMP_Text text_ItemName;

	// Token: 0x04000659 RID: 1625
	[SerializeField]
	private TMP_Text text_Count;

	// Token: 0x0400065A RID: 1626
	[SerializeField]
	private TMP_Text text_Cost;

	// Token: 0x0400065B RID: 1627
	[SerializeField]
	private Image image_Icon;

	// Token: 0x0400065C RID: 1628
	[SerializeField]
	private Sprite sprite_QuestionMark;

	// Token: 0x0400065D RID: 1629
	[SerializeField]
	private Sprite sprite_HP;

	// Token: 0x0400065E RID: 1630
	[SerializeField]
	private Sprite sprite_Coin;

	// Token: 0x0400065F RID: 1631
	[SerializeField]
	private ParticleSystem particle_Clicked;

	// Token: 0x04000660 RID: 1632
	[SerializeField]
	private DiscoverRewardData curData;

	// Token: 0x04000661 RID: 1633
	private bool isClicked;

	// Token: 0x04000662 RID: 1634
	public Action<UI_Obj_DiscoverCard> OnCardClicked;
}
