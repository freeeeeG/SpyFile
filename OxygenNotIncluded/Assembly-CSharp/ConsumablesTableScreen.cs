﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B68 RID: 2920
public class ConsumablesTableScreen : TableScreen
{
	// Token: 0x06005A3E RID: 23102 RVA: 0x00210A20 File Offset: 0x0020EC20
	protected override void OnActivate()
	{
		this.title = UI.CONSUMABLESSCREEN.TITLE;
		base.OnActivate();
		base.AddPortraitColumn("Portrait", new Action<IAssignableIdentity, GameObject>(base.on_load_portrait), null, true);
		base.AddButtonLabelColumn("Names", new Action<IAssignableIdentity, GameObject>(base.on_load_name_label), new Func<IAssignableIdentity, GameObject, string>(base.get_value_name_label), delegate(GameObject widget_go)
		{
			base.GetWidgetRow(widget_go).SelectMinion();
		}, delegate(GameObject widget_go)
		{
			base.GetWidgetRow(widget_go).SelectAndFocusMinion();
		}, new Comparison<IAssignableIdentity>(base.compare_rows_alphabetical), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_name), new Action<IAssignableIdentity, GameObject, ToolTip>(base.on_tooltip_sort_alphabetically), false);
		base.AddLabelColumn("QOLExpectations", new Action<IAssignableIdentity, GameObject>(this.on_load_qualityoflife_expectations), new Func<IAssignableIdentity, GameObject, string>(this.get_value_qualityoflife_label), new Comparison<IAssignableIdentity>(this.compare_rows_qualityoflife_expectations), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_qualityoflife_expectations), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_qualityoflife_expectations), 96, true);
		List<IConsumableUIItem> list = new List<IConsumableUIItem>();
		for (int i = 0; i < EdiblesManager.GetAllFoodTypes().Count; i++)
		{
			list.Add(EdiblesManager.GetAllFoodTypes()[i]);
		}
		List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(GameTags.Medicine);
		for (int j = 0; j < prefabsWithTag.Count; j++)
		{
			MedicinalPillWorkable component = prefabsWithTag[j].GetComponent<MedicinalPillWorkable>();
			if (component)
			{
				list.Add(component);
			}
			else
			{
				DebugUtil.DevLogErrorFormat("Prefab tagged Medicine does not have MedicinalPill component: {0}", new object[]
				{
					prefabsWithTag[j]
				});
			}
		}
		list.Sort(delegate(IConsumableUIItem a, IConsumableUIItem b)
		{
			int num2 = a.MajorOrder.CompareTo(b.MajorOrder);
			if (num2 == 0)
			{
				num2 = a.MinorOrder.CompareTo(b.MinorOrder);
			}
			return num2;
		});
		ConsumerManager.instance.OnDiscover += this.OnConsumableDiscovered;
		List<ConsumableInfoTableColumn> list2 = new List<ConsumableInfoTableColumn>();
		List<DividerColumn> list3 = new List<DividerColumn>();
		List<ConsumableInfoTableColumn> list4 = new List<ConsumableInfoTableColumn>();
		base.StartScrollableContent("consumableScroller");
		int num = 0;
		for (int k = 0; k < list.Count; k++)
		{
			if (list[k].Display)
			{
				if (list[k].MajorOrder != num && k != 0)
				{
					string id = "QualityDivider_" + list[k].MajorOrder.ToString();
					ConsumableInfoTableColumn[] quality_group_columns = list4.ToArray();
					DividerColumn dividerColumn = new DividerColumn(delegate()
					{
						ConsumableInfoTableColumn[] quality_group_columns;
						if (quality_group_columns == null || quality_group_columns.Length == 0)
						{
							return true;
						}
						quality_group_columns = quality_group_columns;
						for (int l = 0; l < quality_group_columns.Length; l++)
						{
							if (quality_group_columns[l].isRevealed)
							{
								return true;
							}
						}
						return false;
					}, "consumableScroller");
					list3.Add(dividerColumn);
					base.RegisterColumn(id, dividerColumn);
					list4.Clear();
				}
				ConsumableInfoTableColumn item = this.AddConsumableInfoColumn(list[k].ConsumableId, list[k], new Action<IAssignableIdentity, GameObject>(this.on_load_consumable_info), new Func<IAssignableIdentity, GameObject, TableScreen.ResultValues>(this.get_value_consumable_info), new Action<GameObject>(this.on_click_consumable_info), new Action<GameObject, TableScreen.ResultValues>(this.set_value_consumable_info), new Comparison<IAssignableIdentity>(this.compare_consumable_info), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_consumable_info), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_consumable_info));
				list2.Add(item);
				num = list[k].MajorOrder;
				list4.Add(item);
			}
		}
		string id2 = "SuperCheckConsumable";
		CheckboxTableColumn[] columns_affected = list2.ToArray();
		base.AddSuperCheckboxColumn(id2, columns_affected, new Action<IAssignableIdentity, GameObject>(base.on_load_value_checkbox_column_super), new Func<IAssignableIdentity, GameObject, TableScreen.ResultValues>(this.get_value_checkbox_column_super), new Action<GameObject>(base.on_press_checkbox_column_super), new Action<GameObject, TableScreen.ResultValues>(base.set_value_checkbox_column_super), null, new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_consumable_info_super));
	}

	// Token: 0x06005A3F RID: 23103 RVA: 0x00210D80 File Offset: 0x0020EF80
	private void refresh_scrollers()
	{
		int num = 0;
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			if (DebugHandler.InstantBuildMode || ConsumerManager.instance.isDiscovered(foodInfo.ConsumableId.ToTag()))
			{
				num++;
			}
		}
		foreach (TableRow tableRow in this.rows)
		{
			GameObject scroller = tableRow.GetScroller("consumableScroller");
			if (scroller != null)
			{
				ScrollRect component = scroller.transform.parent.GetComponent<ScrollRect>();
				if (component.horizontalScrollbar != null)
				{
					component.horizontalScrollbar.gameObject.SetActive(num >= 12);
					tableRow.GetScrollerBorder("consumableScroller").gameObject.SetActive(num >= 12);
				}
				component.horizontal = (num >= 12);
				component.enabled = (num >= 12);
			}
		}
	}

	// Token: 0x06005A40 RID: 23104 RVA: 0x00210EBC File Offset: 0x0020F0BC
	private void on_load_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN.QUALITYOFLIFE_EXPECTATIONS.ToString());
	}

	// Token: 0x06005A41 RID: 23105 RVA: 0x00210F1C File Offset: 0x0020F11C
	private string get_value_qualityoflife_label(IAssignableIdentity minion, GameObject widget_go)
	{
		string result = "";
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			result = Db.Get().Attributes.QualityOfLife.Lookup(minion as MinionIdentity).GetFormattedValue();
		}
		else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			result = UI.TABLESCREENS.NA;
		}
		return result;
	}

	// Token: 0x06005A42 RID: 23106 RVA: 0x00210F78 File Offset: 0x0020F178
	private int compare_rows_qualityoflife_expectations(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float totalValue = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(minionIdentity).GetTotalValue();
		float totalValue2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(minionIdentity2).GetTotalValue();
		return totalValue.CompareTo(totalValue2);
	}

	// Token: 0x06005A43 RID: 23107 RVA: 0x00210FFC File Offset: 0x0020F1FC
	protected void on_tooltip_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(Db.Get().Attributes.QualityOfLife.Lookup(minionIdentity).GetAttributeValueTooltip(), null);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06005A44 RID: 23108 RVA: 0x00211070 File Offset: 0x0020F270
	protected void on_tooltip_sort_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_EXPECTATIONS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06005A45 RID: 23109 RVA: 0x002110B8 File Offset: 0x0020F2B8
	private TableScreen.ResultValues get_value_food_info_super(MinionIdentity minion, GameObject widget_go)
	{
		SuperCheckboxTableColumn superCheckboxTableColumn = base.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		bool flag4 = false;
		foreach (CheckboxTableColumn checkboxTableColumn in superCheckboxTableColumn.columns_affected)
		{
			switch (checkboxTableColumn.get_value_action(widgetRow.GetIdentity(), widgetRow.GetWidget(checkboxTableColumn)))
			{
			case TableScreen.ResultValues.False:
				flag2 = false;
				if (!flag)
				{
					flag4 = true;
				}
				break;
			case TableScreen.ResultValues.Partial:
				flag3 = true;
				flag4 = true;
				break;
			case TableScreen.ResultValues.True:
				flag = false;
				if (!flag2)
				{
					flag4 = true;
				}
				break;
			}
			if (flag4)
			{
				break;
			}
		}
		if (flag3)
		{
			return TableScreen.ResultValues.Partial;
		}
		if (flag2)
		{
			return TableScreen.ResultValues.True;
		}
		if (flag)
		{
			return TableScreen.ResultValues.False;
		}
		return TableScreen.ResultValues.Partial;
	}

	// Token: 0x06005A46 RID: 23110 RVA: 0x00211164 File Offset: 0x0020F364
	private void set_value_consumable_info(GameObject widget_go, TableScreen.ResultValues new_value)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow == null)
		{
			global::Debug.LogWarning("Row is null");
			return;
		}
		ConsumableInfoTableColumn consumableInfoTableColumn = base.GetWidgetColumn(widget_go) as ConsumableInfoTableColumn;
		IAssignableIdentity identity = widgetRow.GetIdentity();
		IConsumableUIItem consumable_info = consumableInfoTableColumn.consumable_info;
		switch (widgetRow.rowType)
		{
		case TableRow.RowType.Header:
			this.set_value_consumable_info(this.default_row.GetComponent<TableRow>().GetWidget(consumableInfoTableColumn), new_value);
			base.StartCoroutine(base.CascadeSetColumnCheckBoxes(this.all_sortable_rows, consumableInfoTableColumn, new_value, widget_go));
			return;
		case TableRow.RowType.Default:
			if (new_value == TableScreen.ResultValues.True)
			{
				ConsumerManager.instance.DefaultForbiddenTagsList.Remove(consumable_info.ConsumableId.ToTag());
			}
			else
			{
				ConsumerManager.instance.DefaultForbiddenTagsList.Add(consumable_info.ConsumableId.ToTag());
			}
			consumableInfoTableColumn.on_load_action(identity, widget_go);
			using (Dictionary<TableRow, GameObject>.Enumerator enumerator = consumableInfoTableColumn.widgets_by_row.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<TableRow, GameObject> keyValuePair = enumerator.Current;
					if (keyValuePair.Key.rowType == TableRow.RowType.Header)
					{
						consumableInfoTableColumn.on_load_action(null, keyValuePair.Value);
						break;
					}
				}
				return;
			}
			break;
		case TableRow.RowType.Minion:
			break;
		case TableRow.RowType.StoredMinon:
			return;
		default:
			return;
		}
		MinionIdentity minionIdentity = identity as MinionIdentity;
		if (minionIdentity != null)
		{
			ConsumableConsumer component = minionIdentity.GetComponent<ConsumableConsumer>();
			if (component == null)
			{
				global::Debug.LogError("Could not find minion identity / row associated with the widget");
				return;
			}
			if (new_value > TableScreen.ResultValues.Partial)
			{
				if (new_value - TableScreen.ResultValues.True <= 1)
				{
					component.SetPermitted(consumable_info.ConsumableId, true);
				}
			}
			else
			{
				component.SetPermitted(consumable_info.ConsumableId, false);
			}
			consumableInfoTableColumn.on_load_action(widgetRow.GetIdentity(), widget_go);
			foreach (KeyValuePair<TableRow, GameObject> keyValuePair2 in consumableInfoTableColumn.widgets_by_row)
			{
				if (keyValuePair2.Key.rowType == TableRow.RowType.Header)
				{
					consumableInfoTableColumn.on_load_action(null, keyValuePair2.Value);
					break;
				}
			}
		}
	}

	// Token: 0x06005A47 RID: 23111 RVA: 0x0021137C File Offset: 0x0020F57C
	private void on_click_consumable_info(GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		IAssignableIdentity identity = widgetRow.GetIdentity();
		ConsumableInfoTableColumn consumableInfoTableColumn = base.GetWidgetColumn(widget_go) as ConsumableInfoTableColumn;
		switch (widgetRow.rowType)
		{
		case TableRow.RowType.Header:
			switch (this.get_value_consumable_info(null, widget_go))
			{
			case TableScreen.ResultValues.False:
			case TableScreen.ResultValues.Partial:
			case TableScreen.ResultValues.ConditionalGroup:
				consumableInfoTableColumn.on_set_action(widget_go, TableScreen.ResultValues.True);
				break;
			case TableScreen.ResultValues.True:
				consumableInfoTableColumn.on_set_action(widget_go, TableScreen.ResultValues.False);
				break;
			}
			consumableInfoTableColumn.on_load_action(null, widget_go);
			return;
		case TableRow.RowType.Default:
		{
			IConsumableUIItem consumable_info = consumableInfoTableColumn.consumable_info;
			bool flag = !ConsumerManager.instance.DefaultForbiddenTagsList.Contains(consumable_info.ConsumableId.ToTag());
			consumableInfoTableColumn.on_set_action(widget_go, flag ? TableScreen.ResultValues.False : TableScreen.ResultValues.True);
			return;
		}
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				IConsumableUIItem consumable_info = consumableInfoTableColumn.consumable_info;
				ConsumableConsumer component = minionIdentity.GetComponent<ConsumableConsumer>();
				if (component == null)
				{
					global::Debug.LogError("Could not find minion identity / row associated with the widget");
					return;
				}
				bool flag2 = component.IsPermitted(consumable_info.ConsumableId);
				consumableInfoTableColumn.on_set_action(widget_go, flag2 ? TableScreen.ResultValues.False : TableScreen.ResultValues.True);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
		{
			StoredMinionIdentity storedMinionIdentity = identity as StoredMinionIdentity;
			if (storedMinionIdentity != null)
			{
				IConsumableUIItem consumable_info = consumableInfoTableColumn.consumable_info;
				bool flag3 = storedMinionIdentity.IsPermittedToConsume(consumable_info.ConsumableId);
				consumableInfoTableColumn.on_set_action(widget_go, flag3 ? TableScreen.ResultValues.False : TableScreen.ResultValues.True);
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06005A48 RID: 23112 RVA: 0x002114E8 File Offset: 0x0020F6E8
	private void on_tooltip_consumable_info(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		ConsumableInfoTableColumn consumableInfoTableColumn = base.GetWidgetColumn(widget_go) as ConsumableInfoTableColumn;
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		EdiblesManager.FoodInfo foodInfo = consumableInfoTableColumn.consumable_info as EdiblesManager.FoodInfo;
		int num = 0;
		if (foodInfo != null)
		{
			int num2 = foodInfo.Quality;
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				AttributeInstance attributeInstance = minionIdentity.GetAttributes().Get(Db.Get().Attributes.FoodExpectation);
				num2 += Mathf.RoundToInt(attributeInstance.GetTotalValue());
			}
			string effectForFoodQuality = Edible.GetEffectForFoodQuality(num2);
			foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(effectForFoodQuality).SelfModifiers)
			{
				if (attributeModifier.AttributeId == Db.Get().Attributes.QualityOfLife.Id)
				{
					num += Mathf.RoundToInt(attributeModifier.Value);
				}
			}
		}
		switch (widgetRow.rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(consumableInfoTableColumn.consumable_info.ConsumableName, null);
			if (foodInfo != null)
			{
				tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.FOOD_AVAILABLE, GameUtil.GetFormattedCalories(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(consumableInfoTableColumn.consumable_info.ConsumableId.ToTag(), false) * foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), null);
				tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.FOOD_MORALE, GameUtil.AddPositiveSign(num.ToString(), num > 0)), null);
				tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(foodInfo.Quality), GameUtil.AddPositiveSign(foodInfo.Quality.ToString(), foodInfo.Quality > 0)), null);
				tooltip.AddMultiStringTooltip("\n" + foodInfo.Description, null);
				return;
			}
			tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.FOOD_AVAILABLE, GameUtil.GetFormattedUnits(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(consumableInfoTableColumn.consumable_info.ConsumableId.ToTag(), false), GameUtil.TimeSlice.None, true, "")), null);
			return;
		case TableRow.RowType.Default:
			if (consumableInfoTableColumn.get_value_action(minion, widget_go) == TableScreen.ResultValues.True)
			{
				tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.NEW_MINIONS_FOOD_PERMISSION_ON, consumableInfoTableColumn.consumable_info.ConsumableName), null);
				return;
			}
			tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.NEW_MINIONS_FOOD_PERMISSION_OFF, consumableInfoTableColumn.consumable_info.ConsumableName), null);
			return;
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			if (minion != null)
			{
				if (consumableInfoTableColumn.get_value_action(minion, widget_go) == TableScreen.ResultValues.True)
				{
					tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.FOOD_PERMISSION_ON, minion.GetProperName(), consumableInfoTableColumn.consumable_info.ConsumableName), null);
				}
				else
				{
					tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.FOOD_PERMISSION_OFF, minion.GetProperName(), consumableInfoTableColumn.consumable_info.ConsumableName), null);
				}
				if (foodInfo != null && minion as MinionIdentity != null)
				{
					tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.FOOD_QUALITY_VS_EXPECTATION, GameUtil.AddPositiveSign(num.ToString(), num > 0), minion.GetProperName()), null);
					return;
				}
				if (minion as StoredMinionIdentity != null)
				{
					tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.CANNOT_ADJUST_PERMISSIONS, (minion as StoredMinionIdentity).GetStorageReason()), null);
				}
			}
			return;
		default:
			return;
		}
	}

	// Token: 0x06005A49 RID: 23113 RVA: 0x00211860 File Offset: 0x0020FA60
	private void on_tooltip_sort_consumable_info(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
	}

	// Token: 0x06005A4A RID: 23114 RVA: 0x00211864 File Offset: 0x0020FA64
	private void on_tooltip_consumable_info_super(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.CONSUMABLESSCREEN.TOOLTIP_TOGGLE_ALL.text, null);
			return;
		case TableRow.RowType.Default:
			tooltip.AddMultiStringTooltip(UI.CONSUMABLESSCREEN.NEW_MINIONS_TOOLTIP_TOGGLE_ROW, null);
			return;
		case TableRow.RowType.Minion:
			if (minion as MinionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(string.Format(UI.CONSUMABLESSCREEN.TOOLTIP_TOGGLE_ROW.text, (minion as MinionIdentity).gameObject.GetProperName()), null);
			}
			break;
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06005A4B RID: 23115 RVA: 0x002118F4 File Offset: 0x0020FAF4
	private void on_load_consumable_info(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		TableColumn widgetColumn = base.GetWidgetColumn(widget_go);
		IConsumableUIItem consumable_info = (widgetColumn as ConsumableInfoTableColumn).consumable_info;
		EdiblesManager.FoodInfo foodInfo = consumable_info as EdiblesManager.FoodInfo;
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		if (!widgetColumn.isRevealed)
		{
			widget_go.SetActive(false);
			return;
		}
		if (!widget_go.activeSelf)
		{
			widget_go.SetActive(true);
		}
		switch (widgetRow.rowType)
		{
		case TableRow.RowType.Header:
		{
			GameObject prefab = Assets.GetPrefab(consumable_info.ConsumableId.ToTag());
			if (prefab == null)
			{
				return;
			}
			KBatchedAnimController component2 = prefab.GetComponent<KBatchedAnimController>();
			Image image = widget_go.GetComponent<HierarchyReferences>().GetReference("PortraitImage") as Image;
			if (component2.AnimFiles.Length != 0)
			{
				Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component2.AnimFiles[0], "ui", false, "");
				image.sprite = uispriteFromMultiObjectAnim;
			}
			image.color = Color.white;
			image.material = ((ClusterManager.Instance.activeWorld.worldInventory.GetAmount(consumable_info.ConsumableId.ToTag(), false) > 0f) ? Assets.UIPrefabs.TableScreenWidgets.DefaultUIMaterial : Assets.UIPrefabs.TableScreenWidgets.DesaturatedUIMaterial);
			break;
		}
		case TableRow.RowType.Default:
			switch (this.get_value_consumable_info(minion, widget_go))
			{
			case TableScreen.ResultValues.False:
				component.ChangeState(0);
				break;
			case TableScreen.ResultValues.True:
				component.ChangeState(1);
				break;
			case TableScreen.ResultValues.ConditionalGroup:
				component.ChangeState(2);
				break;
			}
			break;
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			switch (this.get_value_consumable_info(minion, widget_go))
			{
			case TableScreen.ResultValues.False:
				component.ChangeState(0);
				break;
			case TableScreen.ResultValues.True:
				component.ChangeState(1);
				break;
			case TableScreen.ResultValues.ConditionalGroup:
				component.ChangeState(2);
				break;
			}
			if (foodInfo != null && minion as MinionIdentity != null)
			{
				Graphic graphic = widget_go.GetComponent<HierarchyReferences>().GetReference("BGImage") as Image;
				Color color = new Color(0.72156864f, 0.44313726f, 0.5803922f, Mathf.Max((float)foodInfo.Quality - Db.Get().Attributes.FoodExpectation.Lookup(minion as MinionIdentity).GetTotalValue() + 1f, 0f) * 0.25f);
				graphic.color = color;
			}
			break;
		}
		this.refresh_scrollers();
	}

	// Token: 0x06005A4C RID: 23116 RVA: 0x00211B42 File Offset: 0x0020FD42
	private int compare_consumable_info(IAssignableIdentity a, IAssignableIdentity b)
	{
		return 0;
	}

	// Token: 0x06005A4D RID: 23117 RVA: 0x00211B48 File Offset: 0x0020FD48
	private TableScreen.ResultValues get_value_consumable_info(IAssignableIdentity minion, GameObject widget_go)
	{
		ConsumableInfoTableColumn consumableInfoTableColumn = base.GetWidgetColumn(widget_go) as ConsumableInfoTableColumn;
		IConsumableUIItem consumable_info = consumableInfoTableColumn.consumable_info;
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		TableScreen.ResultValues result = TableScreen.ResultValues.Partial;
		switch (widgetRow.rowType)
		{
		case TableRow.RowType.Header:
		{
			bool flag = true;
			bool flag2 = true;
			bool flag3 = false;
			bool flag4 = false;
			foreach (KeyValuePair<TableRow, GameObject> keyValuePair in consumableInfoTableColumn.widgets_by_row)
			{
				GameObject value = keyValuePair.Value;
				if (!(value == widget_go) && !(value == null))
				{
					switch (consumableInfoTableColumn.get_value_action(keyValuePair.Key.GetIdentity(), value))
					{
					case TableScreen.ResultValues.False:
						flag2 = false;
						if (!flag)
						{
							flag4 = true;
						}
						break;
					case TableScreen.ResultValues.Partial:
						flag3 = true;
						flag4 = true;
						break;
					case TableScreen.ResultValues.True:
						flag = false;
						if (!flag2)
						{
							flag4 = true;
						}
						break;
					}
					if (flag4)
					{
						break;
					}
				}
			}
			if (flag3)
			{
				result = TableScreen.ResultValues.Partial;
			}
			else if (flag2)
			{
				result = TableScreen.ResultValues.True;
			}
			else if (flag)
			{
				result = TableScreen.ResultValues.False;
			}
			else
			{
				result = TableScreen.ResultValues.Partial;
			}
			break;
		}
		case TableRow.RowType.Default:
			result = (ConsumerManager.instance.DefaultForbiddenTagsList.Contains(consumable_info.ConsumableId.ToTag()) ? TableScreen.ResultValues.False : TableScreen.ResultValues.True);
			break;
		case TableRow.RowType.Minion:
			if (minion as MinionIdentity != null)
			{
				result = (((MinionIdentity)minion).GetComponent<ConsumableConsumer>().IsPermitted(consumable_info.ConsumableId) ? TableScreen.ResultValues.True : TableScreen.ResultValues.False);
			}
			else
			{
				result = TableScreen.ResultValues.True;
			}
			break;
		case TableRow.RowType.StoredMinon:
			if (minion as StoredMinionIdentity != null)
			{
				result = (((StoredMinionIdentity)minion).IsPermittedToConsume(consumable_info.ConsumableId) ? TableScreen.ResultValues.True : TableScreen.ResultValues.False);
			}
			else
			{
				result = TableScreen.ResultValues.True;
			}
			break;
		}
		return result;
	}

	// Token: 0x06005A4E RID: 23118 RVA: 0x00211CF8 File Offset: 0x0020FEF8
	protected void on_tooltip_name(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
		case TableRow.RowType.StoredMinon:
			break;
		case TableRow.RowType.Minion:
			if (minion != null)
			{
				tooltip.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.GOTO_DUPLICANT_BUTTON, minion.GetProperName()), null);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06005A4F RID: 23119 RVA: 0x00211D50 File Offset: 0x0020FF50
	protected ConsumableInfoTableColumn AddConsumableInfoColumn(string id, IConsumableUIItem consumable_info, Action<IAssignableIdentity, GameObject> load_value_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip)
	{
		ConsumableInfoTableColumn consumableInfoTableColumn = new ConsumableInfoTableColumn(consumable_info, load_value_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip, on_sort_tooltip, (GameObject widget_go) => "");
		consumableInfoTableColumn.scrollerID = "consumableScroller";
		if (base.RegisterColumn(id, consumableInfoTableColumn))
		{
			return consumableInfoTableColumn;
		}
		return null;
	}

	// Token: 0x06005A50 RID: 23120 RVA: 0x00211DA8 File Offset: 0x0020FFA8
	private void OnConsumableDiscovered(Tag tag)
	{
		base.MarkRowsDirty();
	}

	// Token: 0x06005A51 RID: 23121 RVA: 0x00211DB0 File Offset: 0x0020FFB0
	private void StoredMinionTooltip(IAssignableIdentity minion, ToolTip tooltip)
	{
		StoredMinionIdentity storedMinionIdentity = minion as StoredMinionIdentity;
		if (storedMinionIdentity != null)
		{
			tooltip.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, storedMinionIdentity.GetStorageReason(), storedMinionIdentity.GetProperName()), null);
		}
	}

	// Token: 0x04003D27 RID: 15655
	private const int CONSUMABLE_COLUMNS_BEFORE_SCROLL = 12;
}
