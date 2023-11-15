using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200009C RID: 156
public class UI_Icon_Upgrade : UI_Icon
{
	// Token: 0x0600056C RID: 1388 RVA: 0x0001F678 File Offset: 0x0001D878
	public virtual void Init(int upID)
	{
		if (upID < 0)
		{
			base.gameObject.SetActive(false);
			this.imageBack.enabled = false;
			this.imageFront.enabled = false;
			return;
		}
		this.upgradeID = upID;
		Upgrade upgrade = DataBase.Inst.Data_Upgrades[upID];
		int rank = (int)upgrade.rank;
		Color color;
		if (rank < 0)
		{
			color = default(Color).SetAlpha(0f);
		}
		else
		{
			color = UI_Setting.Inst.rankColors[rank];
		}
		ResourceLibrary inst = ResourceLibrary.Inst;
		if (!upgrade.ifAvailable)
		{
			this.imageBack.color = default(Color).SetAlpha(0f);
			this.imageFront.color = default(Color).SetAlpha(0f);
			return;
		}
		base.gameObject.SetActive(true);
		this.imageBack.enabled = true;
		this.imageFront.enabled = true;
		this.imageBack.sprite = inst.SpList_Icon_Upgrades_Back.GetSpriteWithId(upID);
		this.imageBack.color = color;
		this.imageFront.sprite = inst.SpList_Icon_Upgrades_Front.GetSpriteWithId(upID);
		this.imageShadow.sprite = this.imageFront.sprite;
		this.imageShadow.gameObject.transform.localPosition = new Vector2(0f, UI_Setting.Inst.upgradeIcon.shadeDeltaY);
		this.imageShadow.color = UI_Setting.Inst.upgradeIcon.shadeColor;
		this.imageShadow.gameObject.GetComponent<RectTransform>().sizeDelta = this.imageFront.gameObject.GetComponent<RectTransform>().sizeDelta;
		if (this.upgradeID == 208 || this.upgradeID == 228)
		{
			this.imageShadow.enabled = false;
			return;
		}
		this.imageShadow.enabled = true;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerEnter(PointerEventData eventData)
	{
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerExit(PointerEventData eventData)
	{
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x0400046C RID: 1132
	[SerializeField]
	protected Image imageBack;

	// Token: 0x0400046D RID: 1133
	[SerializeField]
	protected Image imageFront;

	// Token: 0x0400046E RID: 1134
	[SerializeField]
	protected Image imageShadow;

	// Token: 0x0400046F RID: 1135
	[SerializeField]
	protected int upgradeID;
}
