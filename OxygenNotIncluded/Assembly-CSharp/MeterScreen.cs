using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000B8E RID: 2958
public class MeterScreen : KScreen, IRender1000ms
{
	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06005BCB RID: 23499 RVA: 0x00219C89 File Offset: 0x00217E89
	// (set) Token: 0x06005BCC RID: 23500 RVA: 0x00219C90 File Offset: 0x00217E90
	public static MeterScreen Instance { get; private set; }

	// Token: 0x06005BCD RID: 23501 RVA: 0x00219C98 File Offset: 0x00217E98
	public static void DestroyInstance()
	{
		MeterScreen.Instance = null;
	}

	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x06005BCE RID: 23502 RVA: 0x00219CA0 File Offset: 0x00217EA0
	public bool StartValuesSet
	{
		get
		{
			return this.startValuesSet;
		}
	}

	// Token: 0x06005BCF RID: 23503 RVA: 0x00219CA8 File Offset: 0x00217EA8
	protected override void OnPrefabInit()
	{
		MeterScreen.Instance = this;
	}

	// Token: 0x06005BD0 RID: 23504 RVA: 0x00219CB0 File Offset: 0x00217EB0
	protected override void OnSpawn()
	{
		this.StressTooltip.OnToolTip = new Func<string>(this.OnStressTooltip);
		this.SickTooltip.OnToolTip = new Func<string>(this.OnSickTooltip);
		this.RationsTooltip.OnToolTip = new Func<string>(this.OnRationsTooltip);
		this.RedAlertTooltip.OnToolTip = new Func<string>(this.OnRedAlertTooltip);
		MultiToggle redAlertButton = this.RedAlertButton;
		redAlertButton.onClick = (System.Action)Delegate.Combine(redAlertButton.onClick, new System.Action(delegate()
		{
			this.OnRedAlertClick();
		}));
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.Refresh();
		});
		Game.Instance.Subscribe(1585324898, delegate(object data)
		{
			this.RefreshRedAlertButtonState();
		});
		Game.Instance.Subscribe(-1393151672, delegate(object data)
		{
			this.RefreshRedAlertButtonState();
		});
	}

	// Token: 0x06005BD1 RID: 23505 RVA: 0x00219D94 File Offset: 0x00217F94
	private void OnRedAlertClick()
	{
		bool flag = !ClusterManager.Instance.activeWorld.AlertManager.IsRedAlertToggledOn();
		ClusterManager.Instance.activeWorld.AlertManager.ToggleRedAlert(flag);
		if (flag)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
	}

	// Token: 0x06005BD2 RID: 23506 RVA: 0x00219DF2 File Offset: 0x00217FF2
	private void RefreshRedAlertButtonState()
	{
		this.RedAlertButton.ChangeState(ClusterManager.Instance.activeWorld.IsRedAlert() ? 1 : 0);
	}

	// Token: 0x06005BD3 RID: 23507 RVA: 0x00219E14 File Offset: 0x00218014
	public void Render1000ms(float dt)
	{
		this.Refresh();
	}

	// Token: 0x06005BD4 RID: 23508 RVA: 0x00219E1C File Offset: 0x0021801C
	public void InitializeValues()
	{
		if (this.startValuesSet)
		{
			return;
		}
		this.startValuesSet = true;
		this.Refresh();
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x00219E34 File Offset: 0x00218034
	private void Refresh()
	{
		this.RefreshWorldMinionIdentities();
		this.RefreshMinions();
		this.RefreshRations();
		this.RefreshStress();
		this.RefreshSick();
		this.RefreshRedAlertButtonState();
	}

	// Token: 0x06005BD6 RID: 23510 RVA: 0x00219E5C File Offset: 0x0021805C
	private void RefreshWorldMinionIdentities()
	{
		this.worldLiveMinionIdentities = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false)
		where !x.IsNullOrDestroyed()
		select x);
	}

	// Token: 0x06005BD7 RID: 23511 RVA: 0x00219EAD File Offset: 0x002180AD
	private List<MinionIdentity> GetWorldMinionIdentities()
	{
		if (this.worldLiveMinionIdentities == null)
		{
			this.RefreshWorldMinionIdentities();
		}
		return this.worldLiveMinionIdentities;
	}

	// Token: 0x06005BD8 RID: 23512 RVA: 0x00219EC4 File Offset: 0x002180C4
	private void RefreshMinions()
	{
		int count = Components.LiveMinionIdentities.Count;
		int count2 = this.GetWorldMinionIdentities().Count;
		if (count2 == this.cachedMinionCount)
		{
			return;
		}
		this.cachedMinionCount = count2;
		string newString;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			ClusterGridEntity component = ClusterManager.Instance.activeWorld.GetComponent<ClusterGridEntity>();
			newString = string.Format(UI.TOOLTIPS.METERSCREEN_POPULATION_CLUSTER, component.Name, count2, count);
			this.currentMinions.text = string.Format("{0}/{1}", count2, count);
		}
		else
		{
			this.currentMinions.text = string.Format("{0}", count);
			newString = string.Format(UI.TOOLTIPS.METERSCREEN_POPULATION, count.ToString("0"));
		}
		this.MinionsTooltip.ClearMultiStringTooltip();
		this.MinionsTooltip.AddMultiStringTooltip(newString, this.ToolTipStyle_Header);
	}

	// Token: 0x06005BD9 RID: 23513 RVA: 0x00219FB0 File Offset: 0x002181B0
	private void RefreshSick()
	{
		int num = this.CountSickDupes();
		this.SickText.text = num.ToString();
	}

	// Token: 0x06005BDA RID: 23514 RVA: 0x00219FD8 File Offset: 0x002181D8
	private void RefreshRations()
	{
		if (this.RationsText != null && RationTracker.Get() != null)
		{
			long num = (long)RationTracker.Get().CountRations(null, ClusterManager.Instance.activeWorld.worldInventory, true);
			if (this.cachedCalories != num)
			{
				this.RationsText.text = GameUtil.GetFormattedCalories((float)num, GameUtil.TimeSlice.None, true);
				this.cachedCalories = num;
			}
		}
		this.rationsSpark.GetComponentInChildren<SparkLayer>().SetColor(((float)this.cachedCalories > (float)this.GetWorldMinionIdentities().Count * 1000000f) ? Constants.NEUTRAL_COLOR : Constants.NEGATIVE_COLOR);
		this.rationsSpark.GetComponentInChildren<LineLayer>().RefreshLine(TrackerTool.Instance.GetWorldTracker<KCalTracker>(ClusterManager.Instance.activeWorldId).ChartableData(600f), "kcal");
	}

	// Token: 0x06005BDB RID: 23515 RVA: 0x0021A0AC File Offset: 0x002182AC
	private IList<MinionIdentity> GetStressedMinions()
	{
		Amount stress_amount = Db.Get().Amounts.Stress;
		return (from x in new List<MinionIdentity>(this.GetWorldMinionIdentities())
		where !x.IsNullOrDestroyed()
		orderby stress_amount.Lookup(x).value descending
		select x).ToList<MinionIdentity>();
	}

	// Token: 0x06005BDC RID: 23516 RVA: 0x0021A11C File Offset: 0x0021831C
	private string OnStressTooltip()
	{
		float maxStressInActiveWorld = GameUtil.GetMaxStressInActiveWorld();
		this.StressTooltip.ClearMultiStringTooltip();
		this.StressTooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_AVGSTRESS, Mathf.Round(maxStressInActiveWorld).ToString() + "%"), this.ToolTipStyle_Header);
		Amount stress = Db.Get().Amounts.Stress;
		IList<MinionIdentity> stressedMinions = this.GetStressedMinions();
		for (int i = 0; i < stressedMinions.Count; i++)
		{
			MinionIdentity minionIdentity = stressedMinions[i];
			AmountInstance amount = stress.Lookup(minionIdentity);
			this.AddToolTipAmountPercentLine(this.StressTooltip, amount, minionIdentity, i == this.stressDisplayInfo.selectedIndex);
		}
		return "";
	}

	// Token: 0x06005BDD RID: 23517 RVA: 0x0021A1D8 File Offset: 0x002183D8
	private string OnSickTooltip()
	{
		int num = this.CountSickDupes();
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		this.SickTooltip.ClearMultiStringTooltip();
		this.SickTooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_SICK_DUPES, num.ToString()), this.ToolTipStyle_Header);
		for (int i = 0; i < worldMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = worldMinionIdentities[i];
			if (!minionIdentity.IsNullOrDestroyed())
			{
				string text = minionIdentity.GetComponent<KSelectable>().GetName();
				Sicknesses sicknesses = minionIdentity.GetComponent<MinionModifiers>().sicknesses;
				if (sicknesses.IsInfected())
				{
					text += " (";
					int num2 = 0;
					foreach (SicknessInstance sicknessInstance in sicknesses)
					{
						text = text + ((num2 > 0) ? ", " : "") + sicknessInstance.modifier.Name;
						num2++;
					}
					text += ")";
				}
				bool selected = i == this.immunityDisplayInfo.selectedIndex;
				this.AddToolTipLine(this.SickTooltip, text, selected);
			}
		}
		return "";
	}

	// Token: 0x06005BDE RID: 23518 RVA: 0x0021A320 File Offset: 0x00218520
	private int CountSickDupes()
	{
		int num = 0;
		foreach (MinionIdentity minionIdentity in this.GetWorldMinionIdentities())
		{
			if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetComponent<MinionModifiers>().sicknesses.IsInfected())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06005BDF RID: 23519 RVA: 0x0021A390 File Offset: 0x00218590
	private void AddToolTipLine(ToolTip tooltip, string str, bool selected)
	{
		if (selected)
		{
			tooltip.AddMultiStringTooltip("<color=#F0B310FF>" + str + "</color>", this.ToolTipStyle_Property);
			return;
		}
		tooltip.AddMultiStringTooltip(str, this.ToolTipStyle_Property);
	}

	// Token: 0x06005BE0 RID: 23520 RVA: 0x0021A3C0 File Offset: 0x002185C0
	private void AddToolTipAmountPercentLine(ToolTip tooltip, AmountInstance amount, MinionIdentity id, bool selected)
	{
		string str = id.GetComponent<KSelectable>().GetName() + ":  " + Mathf.Round(amount.value).ToString() + "%";
		this.AddToolTipLine(tooltip, str, selected);
	}

	// Token: 0x06005BE1 RID: 23521 RVA: 0x0021A408 File Offset: 0x00218608
	private string OnRationsTooltip()
	{
		this.rationsDict.Clear();
		float calories = RationTracker.Get().CountRations(this.rationsDict, ClusterManager.Instance.activeWorld.worldInventory, true);
		this.RationsText.text = GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true);
		this.RationsTooltip.ClearMultiStringTooltip();
		this.RationsTooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_MEALHISTORY, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true)), this.ToolTipStyle_Header);
		this.RationsTooltip.AddMultiStringTooltip("", this.ToolTipStyle_Property);
		foreach (KeyValuePair<string, float> keyValuePair in this.rationsDict.OrderByDescending(delegate(KeyValuePair<string, float> x)
		{
			EdiblesManager.FoodInfo foodInfo2 = EdiblesManager.GetFoodInfo(x.Key);
			return x.Value * ((foodInfo2 != null) ? foodInfo2.CaloriesPerUnit : -1f);
		}).ToDictionary((KeyValuePair<string, float> t) => t.Key, (KeyValuePair<string, float> t) => t.Value))
		{
			EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(keyValuePair.Key);
			this.RationsTooltip.AddMultiStringTooltip((foodInfo != null) ? string.Format("{0}: {1}", foodInfo.Name, GameUtil.GetFormattedCalories(keyValuePair.Value * foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)) : string.Format(UI.TOOLTIPS.METERSCREEN_INVALID_FOOD_TYPE, keyValuePair.Key), this.ToolTipStyle_Property);
		}
		return "";
	}

	// Token: 0x06005BE2 RID: 23522 RVA: 0x0021A5A8 File Offset: 0x002187A8
	private string OnRedAlertTooltip()
	{
		this.RedAlertTooltip.ClearMultiStringTooltip();
		this.RedAlertTooltip.AddMultiStringTooltip(UI.TOOLTIPS.RED_ALERT_TITLE, this.ToolTipStyle_Header);
		this.RedAlertTooltip.AddMultiStringTooltip(UI.TOOLTIPS.RED_ALERT_CONTENT, this.ToolTipStyle_Property);
		return "";
	}

	// Token: 0x06005BE3 RID: 23523 RVA: 0x0021A5FC File Offset: 0x002187FC
	private void RefreshStress()
	{
		float maxStressInActiveWorld = GameUtil.GetMaxStressInActiveWorld();
		this.StressText.text = Mathf.Round(maxStressInActiveWorld).ToString();
		WorldTracker worldTracker = TrackerTool.Instance.GetWorldTracker<StressTracker>(ClusterManager.Instance.activeWorldId);
		this.stressSpark.GetComponentInChildren<SparkLayer>().SetColor((worldTracker.GetCurrentValue() >= STRESS.ACTING_OUT_RESET) ? Constants.NEGATIVE_COLOR : Constants.NEUTRAL_COLOR);
		this.stressSpark.GetComponentInChildren<LineLayer>().RefreshLine(worldTracker.ChartableData(600f), "stressData");
	}

	// Token: 0x06005BE4 RID: 23524 RVA: 0x0021A688 File Offset: 0x00218888
	public void OnClickStress(BaseEventData base_ev_data)
	{
		IList<MinionIdentity> stressedMinions = this.GetStressedMinions();
		this.UpdateDisplayInfo(base_ev_data, ref this.stressDisplayInfo, stressedMinions);
		this.OnStressTooltip();
		this.StressTooltip.forceRefresh = true;
	}

	// Token: 0x06005BE5 RID: 23525 RVA: 0x0021A6BD File Offset: 0x002188BD
	private IList<MinionIdentity> GetSickMinions()
	{
		return this.GetWorldMinionIdentities();
	}

	// Token: 0x06005BE6 RID: 23526 RVA: 0x0021A6C8 File Offset: 0x002188C8
	public void OnClickImmunity(BaseEventData base_ev_data)
	{
		IList<MinionIdentity> sickMinions = this.GetSickMinions();
		this.UpdateDisplayInfo(base_ev_data, ref this.immunityDisplayInfo, sickMinions);
		this.OnSickTooltip();
		this.SickTooltip.forceRefresh = true;
	}

	// Token: 0x06005BE7 RID: 23527 RVA: 0x0021A700 File Offset: 0x00218900
	private void UpdateDisplayInfo(BaseEventData base_ev_data, ref MeterScreen.DisplayInfo display_info, IList<MinionIdentity> minions)
	{
		PointerEventData pointerEventData = base_ev_data as PointerEventData;
		if (pointerEventData == null)
		{
			return;
		}
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		PointerEventData.InputButton button = pointerEventData.button;
		if (button != PointerEventData.InputButton.Left)
		{
			if (button != PointerEventData.InputButton.Right)
			{
				return;
			}
			display_info.selectedIndex = -1;
		}
		else
		{
			if (worldMinionIdentities.Count < display_info.selectedIndex)
			{
				display_info.selectedIndex = -1;
			}
			if (worldMinionIdentities.Count > 0)
			{
				display_info.selectedIndex = (display_info.selectedIndex + 1) % worldMinionIdentities.Count;
				MinionIdentity minionIdentity = minions[display_info.selectedIndex];
				SelectTool.Instance.SelectAndFocus(minionIdentity.transform.GetPosition(), minionIdentity.GetComponent<KSelectable>(), new Vector3(5f, 0f, 0f));
				return;
			}
		}
	}

	// Token: 0x04003DD1 RID: 15825
	[SerializeField]
	private LocText currentMinions;

	// Token: 0x04003DD3 RID: 15827
	public ToolTip MinionsTooltip;

	// Token: 0x04003DD4 RID: 15828
	public LocText StressText;

	// Token: 0x04003DD5 RID: 15829
	public ToolTip StressTooltip;

	// Token: 0x04003DD6 RID: 15830
	public GameObject stressSpark;

	// Token: 0x04003DD7 RID: 15831
	public LocText RationsText;

	// Token: 0x04003DD8 RID: 15832
	public ToolTip RationsTooltip;

	// Token: 0x04003DD9 RID: 15833
	public GameObject rationsSpark;

	// Token: 0x04003DDA RID: 15834
	public LocText SickText;

	// Token: 0x04003DDB RID: 15835
	public ToolTip SickTooltip;

	// Token: 0x04003DDC RID: 15836
	public TextStyleSetting ToolTipStyle_Header;

	// Token: 0x04003DDD RID: 15837
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04003DDE RID: 15838
	private bool startValuesSet;

	// Token: 0x04003DDF RID: 15839
	public MultiToggle RedAlertButton;

	// Token: 0x04003DE0 RID: 15840
	public ToolTip RedAlertTooltip;

	// Token: 0x04003DE1 RID: 15841
	private MeterScreen.DisplayInfo stressDisplayInfo = new MeterScreen.DisplayInfo
	{
		selectedIndex = -1
	};

	// Token: 0x04003DE2 RID: 15842
	private MeterScreen.DisplayInfo immunityDisplayInfo = new MeterScreen.DisplayInfo
	{
		selectedIndex = -1
	};

	// Token: 0x04003DE3 RID: 15843
	private List<MinionIdentity> worldLiveMinionIdentities;

	// Token: 0x04003DE4 RID: 15844
	private int cachedMinionCount = -1;

	// Token: 0x04003DE5 RID: 15845
	private long cachedCalories = -1L;

	// Token: 0x04003DE6 RID: 15846
	private Dictionary<string, float> rationsDict = new Dictionary<string, float>();

	// Token: 0x02001AAE RID: 6830
	private struct DisplayInfo
	{
		// Token: 0x04007A24 RID: 31268
		public int selectedIndex;
	}
}
