using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000537 RID: 1335
[AddComponentMenu("KMonoBehaviour/scripts/Tutorial")]
public class Tutorial : KMonoBehaviour, IRender1000ms
{
	// Token: 0x06001FCD RID: 8141 RVA: 0x000A9EAC File Offset: 0x000A80AC
	public static void ResetHiddenTutorialMessages()
	{
		if (Tutorial.Instance != null)
		{
			Tutorial.Instance.tutorialMessagesRemaining.Clear();
		}
		foreach (object obj in Enum.GetValues(typeof(Tutorial.TutorialMessages)))
		{
			Tutorial.TutorialMessages tutorialMessages = (Tutorial.TutorialMessages)obj;
			KPlayerPrefs.SetInt("HideTutorial_" + tutorialMessages.ToString(), 0);
			if (Tutorial.Instance != null)
			{
				Tutorial.Instance.tutorialMessagesRemaining.Add(tutorialMessages);
				Tutorial.Instance.hiddenTutorialMessages[tutorialMessages] = false;
			}
		}
		KPlayerPrefs.SetInt("HideTutorial_CheckState", 0);
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x000A9F7C File Offset: 0x000A817C
	private void LoadHiddenTutorialMessages()
	{
		foreach (object obj in Enum.GetValues(typeof(Tutorial.TutorialMessages)))
		{
			Tutorial.TutorialMessages key = (Tutorial.TutorialMessages)obj;
			bool value = KPlayerPrefs.GetInt("HideTutorial_" + key.ToString(), 0) != 0;
			this.hiddenTutorialMessages[key] = value;
		}
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x000AA008 File Offset: 0x000A8208
	public void HideTutorialMessage(Tutorial.TutorialMessages message)
	{
		this.hiddenTutorialMessages[message] = true;
		KPlayerPrefs.SetInt("HideTutorial_" + message.ToString(), 1);
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x000AA034 File Offset: 0x000A8234
	// (set) Token: 0x06001FD1 RID: 8145 RVA: 0x000AA03B File Offset: 0x000A823B
	public static Tutorial Instance { get; private set; }

	// Token: 0x06001FD2 RID: 8146 RVA: 0x000AA043 File Offset: 0x000A8243
	public static void DestroyInstance()
	{
		Tutorial.Instance = null;
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x000AA04C File Offset: 0x000A824C
	private void UpdateNotifierPosition()
	{
		if (this.notifierPosition == Vector3.zero)
		{
			GameObject activeTelepad = GameUtil.GetActiveTelepad();
			if (activeTelepad != null)
			{
				this.notifierPosition = activeTelepad.transform.GetPosition();
			}
		}
		this.notifier.transform.SetPosition(this.notifierPosition);
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x000AA0A2 File Offset: 0x000A82A2
	protected override void OnPrefabInit()
	{
		Tutorial.Instance = this;
		this.LoadHiddenTutorialMessages();
	}

	// Token: 0x06001FD5 RID: 8149 RVA: 0x000AA0B0 File Offset: 0x000A82B0
	protected override void OnSpawn()
	{
		if (this.tutorialMessagesRemaining.Count == 0)
		{
			for (int i = 0; i <= 20; i++)
			{
				this.tutorialMessagesRemaining.Add((Tutorial.TutorialMessages)i);
			}
		}
		List<Tutorial.Item> list = new List<Tutorial.Item>();
		List<Tutorial.Item> list2 = list;
		Tutorial.Item item = new Tutorial.Item();
		item.notification = new Notification(MISC.NOTIFICATIONS.NEEDTOILET.NAME, NotificationType.Tutorial, (List<Notification> n, object d) => MISC.NOTIFICATIONS.NEEDTOILET.TOOLTIP.text, null, true, 5f, delegate(object d)
		{
			PlanScreen.Instance.OpenCategoryByName("Plumbing");
		}, null, null, true, false, false);
		item.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.ToiletExists);
		list2.Add(item);
		this.itemTree.Add(list);
		List<Tutorial.Item> list3 = new List<Tutorial.Item>();
		List<Tutorial.Item> list4 = list3;
		Tutorial.Item item2 = new Tutorial.Item();
		item2.notification = new Notification(MISC.NOTIFICATIONS.NEEDFOOD.NAME, NotificationType.Tutorial, (List<Notification> n, object d) => MISC.NOTIFICATIONS.NEEDFOOD.TOOLTIP.text, null, true, 20f, delegate(object d)
		{
			PlanScreen.Instance.OpenCategoryByName("Food");
		}, null, null, true, false, false);
		item2.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.FoodSourceExistsOnStartingWorld);
		list4.Add(item2);
		List<Tutorial.Item> list5 = list3;
		Tutorial.Item item3 = new Tutorial.Item();
		item3.notification = new Notification(MISC.NOTIFICATIONS.THERMALCOMFORT.NAME, NotificationType.Tutorial, (List<Notification> n, object d) => MISC.NOTIFICATIONS.THERMALCOMFORT.TOOLTIP.text, null, true, 0f, null, null, null, true, false, false);
		list5.Add(item3);
		this.itemTree.Add(list3);
		List<Tutorial.Item> list6 = new List<Tutorial.Item>();
		List<Tutorial.Item> list7 = list6;
		Tutorial.Item item4 = new Tutorial.Item();
		item4.notification = new Notification(MISC.NOTIFICATIONS.HYGENE_NEEDED.NAME, NotificationType.Tutorial, (List<Notification> n, object d) => MISC.NOTIFICATIONS.HYGENE_NEEDED.TOOLTIP, null, true, 20f, delegate(object d)
		{
			PlanScreen.Instance.OpenCategoryByName("Medicine");
		}, null, null, true, false, false);
		item4.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.HygeneExists);
		list7.Add(item4);
		this.itemTree.Add(list6);
		List<Tutorial.Item> list8 = this.warningItems;
		Tutorial.Item item5 = new Tutorial.Item();
		item5.notification = new Notification(MISC.NOTIFICATIONS.NO_OXYGEN_GENERATOR.NAME, NotificationType.Tutorial, (List<Notification> n, object d) => MISC.NOTIFICATIONS.NO_OXYGEN_GENERATOR.TOOLTIP, null, false, 0f, delegate(object d)
		{
			PlanScreen.Instance.OpenCategoryByName("Oxygen");
		}, null, null, true, false, false);
		item5.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.OxygenGeneratorBuilt);
		item5.minTimeToNotify = 80f;
		item5.lastNotifyTime = 0f;
		list8.Add(item5);
		this.warningItems.Add(new Tutorial.Item
		{
			notification = new Notification(MISC.NOTIFICATIONS.INSUFFICIENTOXYGENLASTCYCLE.NAME, NotificationType.Tutorial, new Func<List<Notification>, object, string>(this.OnOxygenTooltip), null, false, 0f, delegate(object d)
			{
				this.ZoomToNextOxygenGenerator();
			}, null, null, true, false, false),
			hideCondition = new Tutorial.HideConditionDelegate(this.OxygenGeneratorNotBuilt),
			requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.SufficientOxygenLastCycleAndThisCycle),
			minTimeToNotify = 80f,
			lastNotifyTime = 0f
		});
		this.warningItems.Add(new Tutorial.Item
		{
			notification = new Notification(MISC.NOTIFICATIONS.UNREFRIGERATEDFOOD.NAME, NotificationType.Tutorial, new Func<List<Notification>, object, string>(this.UnrefrigeratedFoodTooltip), null, false, 0f, delegate(object d)
			{
				this.ZoomToNextUnrefrigeratedFood();
			}, null, null, true, false, false),
			requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.FoodIsRefrigerated),
			minTimeToNotify = 6f,
			lastNotifyTime = 0f
		});
		List<Tutorial.Item> list9 = this.warningItems;
		Tutorial.Item item6 = new Tutorial.Item();
		item6.notification = new Notification(MISC.NOTIFICATIONS.NO_MEDICAL_COTS.NAME, NotificationType.Bad, (List<Notification> n, object o) => MISC.NOTIFICATIONS.NO_MEDICAL_COTS.TOOLTIP, null, false, 0f, delegate(object d)
		{
			PlanScreen.Instance.OpenCategoryByName("Medicine");
		}, null, null, true, false, false);
		item6.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.CanTreatSickDuplicant);
		item6.minTimeToNotify = 10f;
		item6.lastNotifyTime = 0f;
		list9.Add(item6);
		List<Tutorial.Item> list10 = this.warningItems;
		Tutorial.Item item7 = new Tutorial.Item();
		item7.notification = new Notification(string.Format(UI.ENDOFDAYREPORT.TRAVELTIMEWARNING.WARNING_TITLE, Array.Empty<object>()), NotificationType.BadMinor, (List<Notification> n, object d) => string.Format(UI.ENDOFDAYREPORT.TRAVELTIMEWARNING.WARNING_MESSAGE, GameUtil.GetFormattedPercent(40f, GameUtil.TimeSlice.None)), null, true, 0f, delegate(object d)
		{
			ManagementMenu.Instance.OpenReports(GameClock.Instance.GetCycle());
		}, null, null, true, false, false);
		item7.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.LongTravelTimes);
		item7.minTimeToNotify = 1f;
		item7.lastNotifyTime = 0f;
		list10.Add(item7);
		DiscoveredResources.Instance.OnDiscover += this.OnDiscover;
	}

	// Token: 0x06001FD6 RID: 8150 RVA: 0x000AA5BB File Offset: 0x000A87BB
	protected override void OnCleanUp()
	{
		DiscoveredResources.Instance.OnDiscover -= this.OnDiscover;
	}

	// Token: 0x06001FD7 RID: 8151 RVA: 0x000AA5D4 File Offset: 0x000A87D4
	private void OnDiscover(Tag category_tag, Tag tag)
	{
		Element element = ElementLoader.FindElementByHash(SimHashes.UraniumOre);
		if (element != null && tag == element.tag)
		{
			this.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
		}
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x000AA608 File Offset: 0x000A8808
	public Message TutorialMessage(Tutorial.TutorialMessages tm, bool queueMessage = true)
	{
		bool flag = false;
		Message message = null;
		switch (tm)
		{
		case Tutorial.TutorialMessages.TM_Basics:
			if (DistributionPlatform.Initialized && KInputManager.currentControllerIsGamepad)
			{
				message = new TutorialMessage(Tutorial.TutorialMessages.TM_Basics, MISC.NOTIFICATIONS.BASICCONTROLS.NAME, MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODYALT, MISC.NOTIFICATIONS.BASICCONTROLS.TOOLTIP, null, null, null, "", null);
			}
			else
			{
				message = new TutorialMessage(Tutorial.TutorialMessages.TM_Basics, MISC.NOTIFICATIONS.BASICCONTROLS.NAME, MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODY, MISC.NOTIFICATIONS.BASICCONTROLS.TOOLTIP, null, null, null, "", null);
			}
			break;
		case Tutorial.TutorialMessages.TM_Welcome:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Welcome, MISC.NOTIFICATIONS.WELCOMEMESSAGE.NAME, MISC.NOTIFICATIONS.WELCOMEMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.WELCOMEMESSAGE.TOOLTIP, null, null, null, "", null);
			break;
		case Tutorial.TutorialMessages.TM_StressManagement:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_StressManagement, MISC.NOTIFICATIONS.STRESSMANAGEMENTMESSAGE.NAME, MISC.NOTIFICATIONS.STRESSMANAGEMENTMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.STRESSMANAGEMENTMESSAGE.TOOLTIP, null, null, null, "hud_stress", null);
			break;
		case Tutorial.TutorialMessages.TM_Scheduling:
			flag = true;
			break;
		case Tutorial.TutorialMessages.TM_Mopping:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Mopping, MISC.NOTIFICATIONS.MOPPINGMESSAGE.NAME, MISC.NOTIFICATIONS.MOPPINGMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.MOPPINGMESSAGE.TOOLTIP, null, null, null, "icon_action_mop", null);
			break;
		case Tutorial.TutorialMessages.TM_Locomotion:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.NAME, MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.TOOLTIP, "tutorials\\Locomotion", "Tute_Locomotion", VIDEOS.LOCOMOTION, "action_navigable_regions", null);
			break;
		case Tutorial.TutorialMessages.TM_Priorities:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Priorities, MISC.NOTIFICATIONS.PRIORITIESMESSAGE.NAME, MISC.NOTIFICATIONS.PRIORITIESMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.PRIORITIESMESSAGE.TOOLTIP, null, null, null, "icon_action_prioritize", null);
			break;
		case Tutorial.TutorialMessages.TM_FetchingWater:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, MISC.NOTIFICATIONS.FETCHINGWATERMESSAGE.NAME, MISC.NOTIFICATIONS.FETCHINGWATERMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.FETCHINGWATERMESSAGE.TOOLTIP, null, null, null, "element_liquid", null);
			break;
		case Tutorial.TutorialMessages.TM_ThermalComfort:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, MISC.NOTIFICATIONS.THERMALCOMFORT.NAME, MISC.NOTIFICATIONS.THERMALCOMFORT.MESSAGEBODY, MISC.NOTIFICATIONS.THERMALCOMFORT.TOOLTIP, null, null, null, "temperature", null);
			break;
		case Tutorial.TutorialMessages.TM_OverheatingBuildings:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_OverheatingBuildings, MISC.NOTIFICATIONS.TUTORIAL_OVERHEATING.NAME, MISC.NOTIFICATIONS.TUTORIAL_OVERHEATING.MESSAGEBODY, MISC.NOTIFICATIONS.TUTORIAL_OVERHEATING.TOOLTIP, null, null, null, "temperature", null);
			break;
		case Tutorial.TutorialMessages.TM_LotsOfGerms:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, MISC.NOTIFICATIONS.LOTS_OF_GERMS.NAME, MISC.NOTIFICATIONS.LOTS_OF_GERMS.MESSAGEBODY, MISC.NOTIFICATIONS.LOTS_OF_GERMS.TOOLTIP, null, null, null, "overlay_disease", null);
			break;
		case Tutorial.TutorialMessages.TM_DiseaseCooking:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_DiseaseCooking, MISC.NOTIFICATIONS.DISEASE_COOKING.NAME, MISC.NOTIFICATIONS.DISEASE_COOKING.MESSAGEBODY, MISC.NOTIFICATIONS.DISEASE_COOKING.TOOLTIP, null, null, null, "icon_category_food", null);
			break;
		case Tutorial.TutorialMessages.TM_Suits:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Suits, MISC.NOTIFICATIONS.SUITS.NAME, MISC.NOTIFICATIONS.SUITS.MESSAGEBODY, MISC.NOTIFICATIONS.SUITS.TOOLTIP, null, null, null, "overlay_suit", null);
			break;
		case Tutorial.TutorialMessages.TM_Morale:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Morale, MISC.NOTIFICATIONS.MORALE.NAME, MISC.NOTIFICATIONS.MORALE.MESSAGEBODY, MISC.NOTIFICATIONS.MORALE.TOOLTIP, "tutorials\\Morale", "Tute_Morale", VIDEOS.MORALE, "icon_category_morale", null);
			break;
		case Tutorial.TutorialMessages.TM_Schedule:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, MISC.NOTIFICATIONS.SCHEDULEMESSAGE.NAME, MISC.NOTIFICATIONS.SCHEDULEMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.SCHEDULEMESSAGE.TOOLTIP, null, null, null, "OverviewUI_schedule2_icon", null);
			break;
		case Tutorial.TutorialMessages.TM_Digging:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Digging, MISC.NOTIFICATIONS.DIGGING.NAME, MISC.NOTIFICATIONS.DIGGING.MESSAGEBODY, MISC.NOTIFICATIONS.DIGGING.TOOLTIP, "tutorials\\Digging", "Tute_Digging", VIDEOS.DIGGING, "icon_action_dig", null);
			break;
		case Tutorial.TutorialMessages.TM_Power:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Power, MISC.NOTIFICATIONS.POWER.NAME, MISC.NOTIFICATIONS.POWER.MESSAGEBODY, MISC.NOTIFICATIONS.POWER.TOOLTIP, "tutorials\\Power", "Tute_Power", VIDEOS.POWER, "overlay_power", null);
			break;
		case Tutorial.TutorialMessages.TM_Insulation:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, MISC.NOTIFICATIONS.INSULATION.NAME, MISC.NOTIFICATIONS.INSULATION.MESSAGEBODY, MISC.NOTIFICATIONS.INSULATION.TOOLTIP, "tutorials\\Insulation", "Tute_Insulation", VIDEOS.INSULATION, "icon_thermal_conductivity", null);
			break;
		case Tutorial.TutorialMessages.TM_Plumbing:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, MISC.NOTIFICATIONS.PLUMBING.NAME, MISC.NOTIFICATIONS.PLUMBING.MESSAGEBODY, MISC.NOTIFICATIONS.PLUMBING.TOOLTIP, "tutorials\\Piping", "Tute_Plumbing", VIDEOS.PLUMBING, "icon_category_plumbing", null);
			break;
		case Tutorial.TutorialMessages.TM_Radiation:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, MISC.NOTIFICATIONS.RADIATION.NAME, MISC.NOTIFICATIONS.RADIATION.MESSAGEBODY, MISC.NOTIFICATIONS.RADIATION.TOOLTIP, null, null, null, "icon_category_radiation", DlcManager.AVAILABLE_EXPANSION1_ONLY);
			break;
		}
		DebugUtil.AssertArgs(message != null || flag, new object[]
		{
			"No tutorial message:",
			tm
		});
		if (queueMessage)
		{
			DebugUtil.AssertArgs(!flag, new object[]
			{
				"Attempted to queue deprecated Tutorial Message",
				tm
			});
			if (!this.tutorialMessagesRemaining.Contains(tm))
			{
				return null;
			}
			if (this.hiddenTutorialMessages.ContainsKey(tm) && this.hiddenTutorialMessages[tm])
			{
				return null;
			}
			this.tutorialMessagesRemaining.Remove(tm);
			Messenger.Instance.QueueMessage(message);
		}
		return message;
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x000AAB7C File Offset: 0x000A8D7C
	private string OnOxygenTooltip(List<Notification> notifications, object data)
	{
		ReportManager.ReportEntry entry = ReportManager.Instance.YesterdaysReport.GetEntry(ReportManager.ReportType.OxygenCreated);
		return MISC.NOTIFICATIONS.INSUFFICIENTOXYGENLASTCYCLE.TOOLTIP.Replace("{EmittingRate}", GameUtil.GetFormattedMass(entry.Positive, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{ConsumptionRate}", GameUtil.GetFormattedMass(Mathf.Abs(entry.Negative), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06001FDA RID: 8154 RVA: 0x000AABE4 File Offset: 0x000A8DE4
	private string UnrefrigeratedFoodTooltip(List<Notification> notifications, object data)
	{
		string text = MISC.NOTIFICATIONS.UNREFRIGERATEDFOOD.TOOLTIP;
		ListPool<Pickupable, Tutorial>.PooledList pooledList = ListPool<Pickupable, Tutorial>.Allocate();
		this.GetUnrefrigeratedFood(pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			text = text + "\n" + pooledList[i].GetProperName();
		}
		pooledList.Recycle();
		return text;
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x000AAC3C File Offset: 0x000A8E3C
	private string OnLowFoodTooltip(List<Notification> notifications, object data)
	{
		global::Debug.Assert(((WorldContainer)data).id == ClusterManager.Instance.activeWorldId);
		float calories = RationTracker.Get().CountRations(null, ((WorldContainer)data).worldInventory, true);
		float f = (float)Components.LiveMinionIdentities.GetWorldItems(((WorldContainer)data).id, false).Count * -1000000f;
		return string.Format(MISC.NOTIFICATIONS.FOODLOW.TOOLTIP, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories(Mathf.Abs(f), GameUtil.TimeSlice.None, true));
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x000AACC4 File Offset: 0x000A8EC4
	public void DebugNotification()
	{
		NotificationType type;
		string text;
		if (this.debugMessageCount % 3 == 0)
		{
			type = NotificationType.Tutorial;
			text = "Warning message e.g. \"not enough oxygen\" uses Warning Color";
		}
		else if (this.debugMessageCount % 3 == 1)
		{
			type = NotificationType.BadMinor;
			text = "Normal message e.g. Idle. Uses Normal Color BG";
		}
		else
		{
			type = NotificationType.Bad;
			text = "Urgent important message. Uses Bad Color BG";
		}
		string format = "{0} ({1})";
		object arg = text;
		int num = this.debugMessageCount;
		this.debugMessageCount = num + 1;
		Notification notification = new Notification(string.Format(format, arg, num.ToString()), type, (List<Notification> n, object d) => MISC.NOTIFICATIONS.NEEDTOILET.TOOLTIP.text, null, true, 0f, null, null, null, true, false, false);
		this.notifier.Add(notification, "");
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x000AAD70 File Offset: 0x000A8F70
	public void DebugNotificationMessage()
	{
		string str = "This is a message notification. ";
		int num = this.debugMessageCount;
		this.debugMessageCount = num + 1;
		Message message = new GenericMessage(str + num.ToString(), MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.TOOLTIP, null);
		Messenger.Instance.QueueMessage(message);
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x000AADC4 File Offset: 0x000A8FC4
	public void Render1000ms(float dt)
	{
		if (App.isLoading)
		{
			return;
		}
		if (Components.LiveMinionIdentities.Count == 0)
		{
			return;
		}
		if (this.itemTree.Count > 0)
		{
			List<Tutorial.Item> list = this.itemTree[0];
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Tutorial.Item item = list[i];
				if (item != null)
				{
					if (item.requirementSatisfied == null || item.requirementSatisfied())
					{
						item.notification.Clear();
						list.RemoveAt(i);
					}
					else if (item.hideCondition != null && item.hideCondition())
					{
						item.notification.Clear();
						list.RemoveAt(i);
					}
					else
					{
						this.UpdateNotifierPosition();
						this.notifier.Add(item.notification, "");
					}
				}
			}
			if (list.Count == 0)
			{
				this.itemTree.RemoveAt(0);
			}
		}
		foreach (Tutorial.Item item2 in this.warningItems)
		{
			if (item2.requirementSatisfied())
			{
				item2.notification.Clear();
				item2.lastNotifyTime = Time.time;
			}
			else if (item2.hideCondition != null && item2.hideCondition())
			{
				item2.notification.Clear();
				item2.lastNotifyTime = Time.time;
			}
			else if (item2.lastNotifyTime == 0f || Time.time - item2.lastNotifyTime > item2.minTimeToNotify)
			{
				this.notifier.Add(item2.notification, "");
				item2.lastNotifyTime = Time.time;
			}
		}
		if (GameClock.Instance.GetCycle() > 0 && !this.tutorialMessagesRemaining.Contains(Tutorial.TutorialMessages.TM_Priorities) && !this.queuedPrioritiesMessage)
		{
			this.queuedPrioritiesMessage = true;
			GameScheduler.Instance.Schedule("PrioritiesTutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Priorities, true);
			}, null, null);
		}
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x000AAFF0 File Offset: 0x000A91F0
	private bool OxygenGeneratorBuilt()
	{
		return this.oxygenGenerators.Count > 0;
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x000AB000 File Offset: 0x000A9200
	private bool OxygenGeneratorNotBuilt()
	{
		return this.oxygenGenerators.Count == 0;
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000AB010 File Offset: 0x000A9210
	private bool SufficientOxygenLastCycleAndThisCycle()
	{
		if (ReportManager.Instance.YesterdaysReport == null)
		{
			return true;
		}
		ReportManager.ReportEntry entry = ReportManager.Instance.YesterdaysReport.GetEntry(ReportManager.ReportType.OxygenCreated);
		return ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.OxygenCreated).Net > 0.0001f || entry.Net > 0.0001f || (GameClock.Instance.GetCycle() < 1 && !GameClock.Instance.IsNighttime());
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000AB085 File Offset: 0x000A9285
	private bool FoodIsRefrigerated()
	{
		return this.GetUnrefrigeratedFood(null) <= 0;
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x000AB094 File Offset: 0x000A9294
	private int GetUnrefrigeratedFood(List<Pickupable> foods)
	{
		int num = 0;
		if (ClusterManager.Instance.activeWorld.worldInventory != null)
		{
			ICollection<Pickupable> pickupables = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(GameTags.Edible, false);
			if (pickupables == null)
			{
				return 0;
			}
			foreach (Pickupable pickupable in pickupables)
			{
				if (pickupable.storage != null && (pickupable.storage.GetComponent<RationBox>() != null || pickupable.storage.GetComponent<Refrigerator>() != null))
				{
					Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
					if (smi != null && Rottable.RefrigerationLevel(smi) == Rottable.RotRefrigerationLevel.Normal && Rottable.AtmosphereQuality(smi) != Rottable.RotAtmosphereQuality.Sterilizing && smi != null && smi.RotConstitutionPercentage < 0.8f)
					{
						num++;
						if (foods != null)
						{
							foods.Add(pickupable);
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000AB188 File Offset: 0x000A9388
	private bool EnergySourceExists()
	{
		return Game.Instance.circuitManager.HasGenerators();
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x000AB199 File Offset: 0x000A9399
	private bool BedExists()
	{
		return Components.Sleepables.Count > 0;
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x000AB1A8 File Offset: 0x000A93A8
	private bool EnoughFood()
	{
		int count = Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false).Count;
		float num = RationTracker.Get().CountRations(null, ClusterManager.Instance.activeWorld.worldInventory, true);
		float num2 = (float)count * 1000000f;
		return num / num2 >= 1f;
	}

	// Token: 0x06001FE7 RID: 8167 RVA: 0x000AB200 File Offset: 0x000A9400
	private bool CanTreatSickDuplicant()
	{
		bool flag = Components.Clinics.Count >= 1;
		bool flag2 = false;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			using (IEnumerator<SicknessInstance> enumerator = Components.LiveMinionIdentities[i].GetSicknesses().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Sickness.severity >= Sickness.Severity.Major)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag2)
			{
				break;
			}
		}
		return !flag2 || flag;
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x000AB294 File Offset: 0x000A9494
	private bool LongTravelTimes()
	{
		if (ReportManager.Instance.reports.Count < 3)
		{
			return true;
		}
		float num = 0f;
		float num2 = 0f;
		for (int i = ReportManager.Instance.reports.Count - 1; i >= ReportManager.Instance.reports.Count - 3; i--)
		{
			ReportManager.ReportEntry entry = ReportManager.Instance.reports[i].GetEntry(ReportManager.ReportType.TravelTime);
			num += entry.Net;
			num2 += 600f * (float)entry.contextEntries.Count;
		}
		return num / num2 <= 0.4f;
	}

	// Token: 0x06001FE9 RID: 8169 RVA: 0x000AB330 File Offset: 0x000A9530
	private bool FoodSourceExistsOnStartingWorld()
	{
		using (List<ComplexFabricator>.Enumerator enumerator = Components.ComplexFabricators.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetType() == typeof(MicrobeMusher))
				{
					return true;
				}
			}
		}
		return Components.PlantablePlots.GetItems(ClusterManager.Instance.GetStartWorld().id).Count > 0;
	}

	// Token: 0x06001FEA RID: 8170 RVA: 0x000AB3BC File Offset: 0x000A95BC
	private bool HygeneExists()
	{
		return Components.HandSanitizers.Count > 0;
	}

	// Token: 0x06001FEB RID: 8171 RVA: 0x000AB3CB File Offset: 0x000A95CB
	private bool ToiletExists()
	{
		return Components.Toilets.Count > 0;
	}

	// Token: 0x06001FEC RID: 8172 RVA: 0x000AB3DC File Offset: 0x000A95DC
	private void ZoomToNextOxygenGenerator()
	{
		if (this.oxygenGenerators.Count == 0)
		{
			return;
		}
		this.focusedOxygenGenerator %= this.oxygenGenerators.Count;
		GameObject gameObject = this.oxygenGenerators[this.focusedOxygenGenerator];
		if (gameObject != null)
		{
			Vector3 position = gameObject.transform.GetPosition();
			CameraController.Instance.SetTargetPos(position, 8f, true);
		}
		else
		{
			DebugUtil.DevLogErrorFormat("ZoomToNextOxygenGenerator generator was null: {0}", new object[]
			{
				gameObject
			});
		}
		this.focusedOxygenGenerator++;
	}

	// Token: 0x06001FED RID: 8173 RVA: 0x000AB46C File Offset: 0x000A966C
	private void ZoomToNextUnrefrigeratedFood()
	{
		ListPool<Pickupable, Tutorial>.PooledList pooledList = ListPool<Pickupable, Tutorial>.Allocate();
		int unrefrigeratedFood = this.GetUnrefrigeratedFood(pooledList);
		if (pooledList.Count == 0)
		{
			return;
		}
		this.focusedUnrefrigFood++;
		if (this.focusedUnrefrigFood >= unrefrigeratedFood)
		{
			this.focusedUnrefrigFood = 0;
		}
		Pickupable pickupable = pooledList[this.focusedUnrefrigFood];
		if (pickupable != null)
		{
			CameraController.Instance.SetTargetPos(pickupable.transform.GetPosition(), 8f, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x040011C5 RID: 4549
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x040011C6 RID: 4550
	[Serialize]
	private SerializedList<Tutorial.TutorialMessages> tutorialMessagesRemaining = new SerializedList<Tutorial.TutorialMessages>();

	// Token: 0x040011C7 RID: 4551
	private const string HIDDEN_TUTORIAL_PREF_KEY_PREFIX = "HideTutorial_";

	// Token: 0x040011C8 RID: 4552
	public const string HIDDEN_TUTORIAL_PREF_BUTTON_KEY = "HideTutorial_CheckState";

	// Token: 0x040011C9 RID: 4553
	private Dictionary<Tutorial.TutorialMessages, bool> hiddenTutorialMessages = new Dictionary<Tutorial.TutorialMessages, bool>();

	// Token: 0x040011CA RID: 4554
	private int debugMessageCount;

	// Token: 0x040011CB RID: 4555
	private bool queuedPrioritiesMessage;

	// Token: 0x040011CC RID: 4556
	private const float LOW_RATION_AMOUNT = 1f;

	// Token: 0x040011CE RID: 4558
	private List<List<Tutorial.Item>> itemTree = new List<List<Tutorial.Item>>();

	// Token: 0x040011CF RID: 4559
	private List<Tutorial.Item> warningItems = new List<Tutorial.Item>();

	// Token: 0x040011D0 RID: 4560
	private Vector3 notifierPosition;

	// Token: 0x040011D1 RID: 4561
	public List<GameObject> oxygenGenerators = new List<GameObject>();

	// Token: 0x040011D2 RID: 4562
	private int focusedOxygenGenerator;

	// Token: 0x040011D3 RID: 4563
	private int focusedUnrefrigFood = -1;

	// Token: 0x020011CC RID: 4556
	public enum TutorialMessages
	{
		// Token: 0x04005D89 RID: 23945
		TM_Basics,
		// Token: 0x04005D8A RID: 23946
		TM_Welcome,
		// Token: 0x04005D8B RID: 23947
		TM_StressManagement,
		// Token: 0x04005D8C RID: 23948
		TM_Scheduling,
		// Token: 0x04005D8D RID: 23949
		TM_Mopping,
		// Token: 0x04005D8E RID: 23950
		TM_Locomotion,
		// Token: 0x04005D8F RID: 23951
		TM_Priorities,
		// Token: 0x04005D90 RID: 23952
		TM_FetchingWater,
		// Token: 0x04005D91 RID: 23953
		TM_ThermalComfort,
		// Token: 0x04005D92 RID: 23954
		TM_OverheatingBuildings,
		// Token: 0x04005D93 RID: 23955
		TM_LotsOfGerms,
		// Token: 0x04005D94 RID: 23956
		TM_DiseaseCooking,
		// Token: 0x04005D95 RID: 23957
		TM_Suits,
		// Token: 0x04005D96 RID: 23958
		TM_Morale,
		// Token: 0x04005D97 RID: 23959
		TM_Schedule,
		// Token: 0x04005D98 RID: 23960
		TM_Digging,
		// Token: 0x04005D99 RID: 23961
		TM_Power,
		// Token: 0x04005D9A RID: 23962
		TM_Insulation,
		// Token: 0x04005D9B RID: 23963
		TM_Plumbing,
		// Token: 0x04005D9C RID: 23964
		TM_Radiation,
		// Token: 0x04005D9D RID: 23965
		TM_COUNT
	}

	// Token: 0x020011CD RID: 4557
	// (Invoke) Token: 0x06007AD6 RID: 31446
	private delegate bool HideConditionDelegate();

	// Token: 0x020011CE RID: 4558
	// (Invoke) Token: 0x06007ADA RID: 31450
	private delegate bool RequirementSatisfiedDelegate();

	// Token: 0x020011CF RID: 4559
	private class Item
	{
		// Token: 0x04005D9E RID: 23966
		public Notification notification;

		// Token: 0x04005D9F RID: 23967
		public Tutorial.HideConditionDelegate hideCondition;

		// Token: 0x04005DA0 RID: 23968
		public Tutorial.RequirementSatisfiedDelegate requirementSatisfied;

		// Token: 0x04005DA1 RID: 23969
		public float minTimeToNotify;

		// Token: 0x04005DA2 RID: 23970
		public float lastNotifyTime;
	}
}
