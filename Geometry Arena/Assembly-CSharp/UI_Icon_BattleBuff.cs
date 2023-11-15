using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000084 RID: 132
public class UI_Icon_BattleBuff : UI_Icon
{
	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0001C782 File Offset: 0x0001A982
	public float LifeTimePercent
	{
		get
		{
			return Mathf.Clamp(this.timeLefts[this.timeLefts.Count - 1] / this.timeMax, 0f, 1f);
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060004AA RID: 1194 RVA: 0x0001C7B2 File Offset: 0x0001A9B2
	public int BuffLayer
	{
		get
		{
			if (this.timeLefts.Count == 1)
			{
				return this.buff.layerThis;
			}
			return this.timeLefts.Count;
		}
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x0001C7DC File Offset: 0x0001A9DC
	private void Update()
	{
		this.UpdateLayer();
		if (this.mouseOn)
		{
			if (this.updateTimeLeft > 0f)
			{
				this.updateTimeLeft -= Time.unscaledDeltaTime;
				return;
			}
			UI_ToolTip.inst.ShowWithStringAndPlace(UI_ToolTipInfo.GetInfo_BattleBuffInfo(this.buff, this.BuffLayer), 0);
			this.updateTimeLeft = 0.1f;
		}
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0001C840 File Offset: 0x0001AA40
	public void InitWithID(int typeID, BattleBuff buff)
	{
		this.buff = buff;
		this.typeID = typeID;
		int rank = (int)buff.rank;
		BattleBuff.EnumBuffSource source = buff.source;
		if (source != BattleBuff.EnumBuffSource.BATTLEITEM)
		{
			if (source == BattleBuff.EnumBuffSource.UPGRADE)
			{
				this.image_Outline.gameObject.SetActive(false);
				this.image_Icon.gameObject.SetActive(false);
				this.image_Upgrade_Front.gameObject.SetActive(true);
				this.image_Upgrade_Back.gameObject.SetActive(true);
				this.image_Upgrade_Front.sprite = ResourceLibrary.Inst.SpList_Icon_Upgrades_Front.GetSpriteWithId(typeID);
				this.image_Upgrade_Back.sprite = ResourceLibrary.Inst.SpList_Icon_Upgrades_Back.GetSpriteWithId(typeID);
				this.image_Upgrade_Back.color = UI_Setting.Inst.rankColors[rank];
			}
		}
		else
		{
			this.image_Outline.gameObject.SetActive(true);
			this.image_Icon.gameObject.SetActive(true);
			this.image_Upgrade_Front.gameObject.SetActive(false);
			this.image_Upgrade_Back.gameObject.SetActive(false);
			this.image_Icon.sprite = ResourceLibrary.Inst.SpList_Icon_BattleItem.GetSpriteWithId(typeID);
			this.image_Outline.color = UI_Setting.Inst.rankColors[rank];
		}
		this.timeMax = buff.lifeTimeMax;
		this.timeLefts = new List<float>();
		this.timeLefts.Add(this.timeMax);
		this.UpdateProcessBar();
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0001C9BC File Offset: 0x0001ABBC
	public void RefreshTimeMax()
	{
		for (int i = 0; i < this.timeLefts.Count; i++)
		{
			this.timeLefts[i] = this.timeMax;
		}
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x0001C9F1 File Offset: 0x0001ABF1
	public void AddNewLayer()
	{
		this.timeLefts.Add(this.timeMax);
		this.UpdateProcessBar();
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x0001CA0C File Offset: 0x0001AC0C
	public void LifeFadeInUpdate()
	{
		for (int i = 0; i < this.timeLefts.Count; i++)
		{
			List<float> list = this.timeLefts;
			int index = i;
			list[index] -= Time.deltaTime;
			if (this.timeLefts[i] < 0f)
			{
				this.timeLefts[i] = 0f;
			}
		}
		this.UpdateProcessBar();
		while (this.timeLefts.Contains(0f))
		{
			this.timeLefts.Remove(0f);
		}
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0001CA9B File Offset: 0x0001AC9B
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseOn = true;
		UI_ToolTip.inst.ShowWithStringAndPlace(UI_ToolTipInfo.GetInfo_BattleBuffInfo(this.buff, this.BuffLayer), 0);
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0001CAC0 File Offset: 0x0001ACC0
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.mouseOn = false;
		this.updateTimeLeft = 0f;
		UI_ToolTip.inst.Close();
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x0001CAE0 File Offset: 0x0001ACE0
	private void UpdateProcessBar()
	{
		if (TempData.inst.currentSceneType == EnumSceneType.MAINMENU)
		{
			this.obj_ProcessBar.SetActive(false);
			this.text_LayerNum.gameObject.SetActive(false);
			return;
		}
		if (this.timeMax == -1f)
		{
			this.obj_ProcessBar.SetActive(false);
			return;
		}
		this.trans_ProcessBar_Percent.localScale = new Vector3(this.LifeTimePercent, 1f);
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0001CB4C File Offset: 0x0001AD4C
	private void UpdateLayer()
	{
		if (this.BuffLayer == 1)
		{
			this.text_LayerNum.gameObject.SetActive(false);
			return;
		}
		this.text_LayerNum.gameObject.SetActive(true);
		this.text_LayerNum.text = this.BuffLayer.ToString();
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0001CB9E File Offset: 0x0001AD9E
	private void OnDestroy()
	{
		if (this.mouseOn)
		{
			UI_ToolTip.inst.Close();
		}
	}

	// Token: 0x040003F2 RID: 1010
	[Header("BuffAbout")]
	[SerializeField]
	public List<float> timeLefts;

	// Token: 0x040003F3 RID: 1011
	public int typeID = -1;

	// Token: 0x040003F4 RID: 1012
	public float timeMax = -1f;

	// Token: 0x040003F5 RID: 1013
	public BattleBuff buff;

	// Token: 0x040003F6 RID: 1014
	[Header("Images")]
	[SerializeField]
	private Image image_Icon;

	// Token: 0x040003F7 RID: 1015
	[SerializeField]
	private Image image_Outline;

	// Token: 0x040003F8 RID: 1016
	[SerializeField]
	private Image image_Upgrade_Front;

	// Token: 0x040003F9 RID: 1017
	[SerializeField]
	private Image image_Upgrade_Back;

	// Token: 0x040003FA RID: 1018
	[SerializeField]
	private Text text_LayerNum;

	// Token: 0x040003FB RID: 1019
	[SerializeField]
	private float updateTimeLeft;

	// Token: 0x040003FC RID: 1020
	[Header("ProcessBar")]
	[SerializeField]
	private GameObject obj_ProcessBar;

	// Token: 0x040003FD RID: 1021
	[SerializeField]
	private Transform trans_ProcessBar_Percent;

	// Token: 0x040003FE RID: 1022
	private bool mouseOn;
}
