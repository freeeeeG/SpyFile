using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200009D RID: 157
public class UI_Icon_UpgradeListMain : UI_Icon_Upgrade
{
	// Token: 0x06000572 RID: 1394 RVA: 0x0001F85C File Offset: 0x0001DA5C
	public override void Init(int upID)
	{
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
			Debug.LogError("Icon_UpgradeListMain_unavailable");
		}
		else
		{
			base.gameObject.SetActive(true);
			this.imageBack.enabled = true;
			this.imageFront.enabled = true;
			this.imageBack.sprite = inst.SpList_Icon_Upgrades_Back.GetSpriteWithId(upID);
			this.imageBack.color = color;
			this.imageFront.sprite = inst.SpList_Icon_Upgrades_Front.GetSpriteWithId(upID);
			this.imageShadow.sprite = this.imageFront.sprite;
			this.imageShadow.gameObject.transform.localPosition = new Vector2(0f, UI_Setting.Inst.upgradeIcon.shadeDeltaY);
			this.imageShadow.color = UI_Setting.Inst.upgradeIcon.shadeColor;
			if (this.upgradeID == 208 || this.upgradeID == 228)
			{
				this.imageShadow.enabled = false;
			}
			else
			{
				this.imageShadow.enabled = true;
			}
		}
		base.UpdateLockIcon();
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0001FA01 File Offset: 0x0001DC01
	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (this.ifUnlocked())
		{
			UI_ToolTip.inst.ShowWithString(UI_ToolTipInfo.GetInfo_UpgradeLibrary(this.upgradeID));
		}
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0001FA20 File Offset: 0x0001DC20
	public override void OnPointerExit(PointerEventData eventData)
	{
		if (this.ifUnlocked())
		{
			UI_ToolTip.inst.Close();
		}
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0001FA34 File Offset: 0x0001DC34
	protected override bool ifUnlocked()
	{
		return GameData.inst.record.upgradeGain[this.upgradeID] > 0;
	}
}
