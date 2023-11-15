using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AE RID: 174
public class UI_Panel_Battle_UpgradeShop : MonoBehaviour
{
	// Token: 0x06000603 RID: 1539 RVA: 0x000223E7 File Offset: 0x000205E7
	private void Awake()
	{
		UI_Panel_Battle_UpgradeShop.inst = this;
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x000223EF File Offset: 0x000205EF
	private void Update()
	{
		this.UpdateInput();
		if (MyInput.IfGetCloseButtonDown())
		{
			this.Close();
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x00022404 File Offset: 0x00020604
	public void Init()
	{
		base.gameObject.SetActive(true);
		this.UpdateLanguages();
		this.panels[0].gameObject.SetActive(true);
		this.panels[1].gameObject.SetActive(true);
		int num;
		if (!TempData.inst.diffiOptFlag[19])
		{
			this.panels[2].gameObject.SetActive(true);
			num = 3;
		}
		else
		{
			this.panels[2].gameObject.SetActive(false);
			num = 2;
		}
		if (Battle.inst.specialEffect[31] >= 1)
		{
			this.panels[3].gameObject.SetActive(true);
			num++;
		}
		else
		{
			this.panels[3].gameObject.SetActive(false);
		}
		if (num >= 4)
		{
			this.rects[0].localPosition = new Vector2(138f, 0f);
		}
		else
		{
			this.rects[0].localPosition = new Vector2(0f, 0f);
		}
		for (int i = 0; i < this.panels.Length; i++)
		{
			this.panels[i].Init(i);
		}
		RectTransform[] array = this.rects;
		for (int j = 0; j < array.Length; j++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(array[j]);
		}
		this.mysteryBag.gameObject.SetActive(this.mysteryBag.ifActive);
		this.Record_UnlockUpgrade();
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x00021C56 File Offset: 0x0001FE56
	private void OnEnable()
	{
		if (TempData.inst.modeType == EnumModeType.WANDER && BattleManager.inst.wander_On)
		{
			TimeManager.inst.PauseSet();
		}
		TutorialMaster.inst.Activate();
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00021C85 File Offset: 0x0001FE85
	private void OnDisable()
	{
		if (TempData.inst.modeType == EnumModeType.WANDER && BattleManager.inst.wander_On)
		{
			TimeManager.inst.PauseRecover();
		}
		TutorialMaster.inst.Activate();
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0002256C File Offset: 0x0002076C
	public void Buy(int no)
	{
		UpgradeShop.Goods goods = Battle.inst.upgradeShop.upgradeGoods[no];
		int upgradeID = goods.upgradeID;
		if (upgradeID == -1)
		{
			Debug.Log("已售罄！");
			UI_FloatTextControl.inst.Special_SoldOut();
			return;
		}
		int price = goods.price;
		long fragment = Battle.inst.Fragment;
		if ((long)price <= fragment)
		{
			Debug.Log(string.Format("购买成功，花费frag={0},获得upID={1}", price, upgradeID));
			Battle.inst.Upgrade_Gain(upgradeID);
			Battle.inst.Fragment_Spend(price);
			goods.upgradeID = -1;
			goods.price = 0;
			if (Battle.inst.specialEffect[48] >= 1)
			{
				goods.InitWithRank(EnumRank.NORMAL);
			}
			if (Battle.inst.specialEffect[70] >= 1 && DataBase.Inst.Data_Upgrades[upgradeID].rank == EnumRank.LEGENDARY)
			{
				Battle.inst.upgradeShop_RefreshFreeChance = 1;
			}
			UI_ToolTip.inst.Close();
			this.Init();
			return;
		}
		Debug.Log("碎片不足！");
		UI_FloatTextControl.inst.Special_LackOfFragment();
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x00022670 File Offset: 0x00020870
	private void UpdateInput()
	{
		if (this.mysteryBag.gameObject.activeSelf)
		{
			if (this.mysteryBag.slots[0].gameObject.activeSelf)
			{
				if (MyInput.GetKeyDownWithSound(KeyCode.Alpha1))
				{
					this.mysteryBag.Get(0);
				}
				if (MyInput.GetKeyDownWithSound(KeyCode.Alpha2))
				{
					this.mysteryBag.Refuse(0);
				}
			}
			if (this.mysteryBag.slots[1].gameObject.activeSelf)
			{
				if (MyInput.GetKeyDownWithSound(KeyCode.Alpha3))
				{
					this.mysteryBag.Get(1);
				}
				if (MyInput.GetKeyDownWithSound(KeyCode.Alpha4))
				{
					this.mysteryBag.Refuse(1);
					return;
				}
			}
		}
		else
		{
			if (this.panels[0].gameObject.activeSelf && MyInput.GetKeyDownWithSound(KeyCode.Alpha1))
			{
				this.Buy(0);
			}
			if (this.panels[1].gameObject.activeSelf && MyInput.GetKeyDownWithSound(KeyCode.Alpha2))
			{
				this.Buy(1);
			}
			if (this.panels[2].gameObject.activeSelf && MyInput.GetKeyDownWithSound(KeyCode.Alpha3))
			{
				this.Buy(2);
			}
			if (this.panels[3].gameObject.activeSelf && MyInput.GetKeyDownWithSound(KeyCode.Alpha4))
			{
				this.Buy(3);
			}
			if (MyInput.GetKeyDownWithSound(KeyCode.R))
			{
				this.Button_Refresh();
			}
			if (MyInput.GetKeyDownWithSound(KeyCode.C))
			{
				this.Button_Lock();
			}
		}
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x000227D0 File Offset: 0x000209D0
	public void UpdateLanguages()
	{
		this.text_FragmentCount.text = Battle.inst.Fragment.ToString();
		this.text_FragmentCount.GetComponent<Animator>().SetTrigger("Exp");
		LanguageText.ShopMenu shopMenu = LanguageText.Inst.shopMenu;
		this.title_Shop.text = shopMenu.panel_Shop;
		this.button_Return.text = shopMenu.button_Return.AppendKeycode("Q");
		this.lang_Refresh.text = shopMenu.refresh.AppendKeycode("R").Colored(UI_Setting.Inst.skill.Color_SmallTitle);
		this.lang_LockInfo.text = (Battle.inst.upgradeShop_IfLocked ? shopMenu.lock_Locked.ReplaceLineBreak().AppendKeycode("C").Colored(Color.red) : shopMenu.lock_Unlocked.ReplaceLineBreak().AppendKeycode("C").Colored(Color.green));
		if (Battle.inst.upgradeShop_RefreshFreeChance <= 0)
		{
			this.num_RefreshCost.text = Battle.inst.upgradeShop_RefreshPrice.ToString();
		}
		else
		{
			this.num_RefreshCost.text = shopMenu.refresh_FreeOnce;
		}
		for (int i = 0; i < this.button_Buy.Length; i++)
		{
			this.button_Buy[i].text = shopMenu.button_Buy.AppendKeycode((i + 1).ToString());
		}
		Text[] array = this.info_Price;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].text = shopMenu.info_Price;
		}
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00022965 File Offset: 0x00020B65
	public void Button_Lock()
	{
		Battle.inst.Store_SetLock(!Battle.inst.upgradeShop_IfLocked);
		this.Init();
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00022984 File Offset: 0x00020B84
	public void Button_Refresh()
	{
		int num = Battle.inst.upgradeShop_RefreshPrice;
		if (Battle.inst.Fragment >= (long)num || Battle.inst.upgradeShop_RefreshFreeChance >= 1)
		{
			if (Battle.inst.upgradeShop_RefreshFreeChance <= 0)
			{
				Battle.inst.Fragment_Spend(num);
			}
			else
			{
				Battle.inst.upgradeShop_RefreshFreeChance = 0;
			}
			num++;
			Battle.inst.upgradeShop.UpdateShop();
			Battle.inst.Store_TryUnlock();
			Battle.inst.upgradeShop_RefreshPrice = num;
			UI_ToolTip.inst.Close();
			this.Init();
			SaveFile.SaveByJson(true);
			return;
		}
		Debug.Log("碎片不足");
		UI_FloatTextControl.inst.Special_LackOfFragment();
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00022A30 File Offset: 0x00020C30
	private void Record_UnlockUpgrade()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.panels[i].gameObject.activeSelf)
			{
				GameData.inst.record.Upgrade_GainOnce(this.panels[i].upgradeID);
			}
		}
	}

	// Token: 0x040004FE RID: 1278
	[SerializeField]
	public static UI_Panel_Battle_UpgradeShop inst;

	// Token: 0x040004FF RID: 1279
	[SerializeField]
	private UI_Panel_Battle_UpgradeShop_Single[] panels;

	// Token: 0x04000500 RID: 1280
	[SerializeField]
	public UI_Panel_Battle_UpgradeShop_MysteryBag mysteryBag;

	// Token: 0x04000501 RID: 1281
	[SerializeField]
	private RectTransform[] rects;

	// Token: 0x04000502 RID: 1282
	[Header("Languages")]
	[SerializeField]
	private Text title_Shop;

	// Token: 0x04000503 RID: 1283
	[SerializeField]
	private Text button_Return;

	// Token: 0x04000504 RID: 1284
	[SerializeField]
	private Text[] button_Buy;

	// Token: 0x04000505 RID: 1285
	[SerializeField]
	private Text[] info_Price;

	// Token: 0x04000506 RID: 1286
	[Header("Refresh")]
	[SerializeField]
	private Text lang_Refresh;

	// Token: 0x04000507 RID: 1287
	[SerializeField]
	private Text num_RefreshCost;

	// Token: 0x04000508 RID: 1288
	[Header("Lock")]
	[SerializeField]
	private Text lang_LockInfo;

	// Token: 0x04000509 RID: 1289
	[SerializeField]
	private Text text_FragmentCount;
}
