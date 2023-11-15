﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000928 RID: 2344
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Research")]
public class Research : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060043FF RID: 17407 RVA: 0x0017D6EE File Offset: 0x0017B8EE
	public static void DestroyInstance()
	{
		Research.Instance = null;
	}

	// Token: 0x06004400 RID: 17408 RVA: 0x0017D6F8 File Offset: 0x0017B8F8
	public TechInstance GetTechInstance(string techID)
	{
		return this.techs.Find((TechInstance match) => match.tech.Id == techID);
	}

	// Token: 0x06004401 RID: 17409 RVA: 0x0017D729 File Offset: 0x0017B929
	public bool IsBeingResearched(Tech tech)
	{
		return this.activeResearch != null && tech != null && this.activeResearch.tech == tech;
	}

	// Token: 0x06004402 RID: 17410 RVA: 0x0017D746 File Offset: 0x0017B946
	protected override void OnPrefabInit()
	{
		Research.Instance = this;
		this.researchTypes = new ResearchTypes();
	}

	// Token: 0x06004403 RID: 17411 RVA: 0x0017D75C File Offset: 0x0017B95C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.globalPointInventory == null)
		{
			this.globalPointInventory = new ResearchPointInventory();
		}
		this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.OnRolesUpdated));
		this.OnRolesUpdated(null);
		Components.ResearchCenters.OnAdd += new Action<IResearchCenter>(this.CheckResearchBuildings);
		Components.ResearchCenters.OnRemove += new Action<IResearchCenter>(this.CheckResearchBuildings);
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			IResearchCenter component = kprefabID.GetComponent<IResearchCenter>();
			if (component != null)
			{
				this.researchCenterPrefabs.Add(component);
			}
		}
	}

	// Token: 0x06004404 RID: 17412 RVA: 0x0017D828 File Offset: 0x0017BA28
	public ResearchType GetResearchType(string id)
	{
		return this.researchTypes.GetResearchType(id);
	}

	// Token: 0x06004405 RID: 17413 RVA: 0x0017D836 File Offset: 0x0017BA36
	public TechInstance GetActiveResearch()
	{
		return this.activeResearch;
	}

	// Token: 0x06004406 RID: 17414 RVA: 0x0017D83E File Offset: 0x0017BA3E
	public TechInstance GetTargetResearch()
	{
		if (this.queuedTech != null && this.queuedTech.Count > 0)
		{
			return this.queuedTech[this.queuedTech.Count - 1];
		}
		return null;
	}

	// Token: 0x06004407 RID: 17415 RVA: 0x0017D870 File Offset: 0x0017BA70
	public TechInstance Get(Tech tech)
	{
		foreach (TechInstance techInstance in this.techs)
		{
			if (techInstance.tech == tech)
			{
				return techInstance;
			}
		}
		return null;
	}

	// Token: 0x06004408 RID: 17416 RVA: 0x0017D8CC File Offset: 0x0017BACC
	public TechInstance GetOrAdd(Tech tech)
	{
		TechInstance techInstance = this.techs.Find((TechInstance tc) => tc.tech == tech);
		if (techInstance != null)
		{
			return techInstance;
		}
		TechInstance techInstance2 = new TechInstance(tech);
		this.techs.Add(techInstance2);
		return techInstance2;
	}

	// Token: 0x06004409 RID: 17417 RVA: 0x0017D91C File Offset: 0x0017BB1C
	public void GetNextTech()
	{
		if (this.queuedTech.Count > 0)
		{
			this.queuedTech.RemoveAt(0);
		}
		if (this.queuedTech.Count > 0)
		{
			this.SetActiveResearch(this.queuedTech[this.queuedTech.Count - 1].tech, false);
			return;
		}
		this.SetActiveResearch(null, false);
	}

	// Token: 0x0600440A RID: 17418 RVA: 0x0017D980 File Offset: 0x0017BB80
	private void AddTechToQueue(Tech tech)
	{
		TechInstance orAdd = this.GetOrAdd(tech);
		if (!orAdd.IsComplete() && !this.queuedTech.Contains(orAdd))
		{
			this.queuedTech.Add(orAdd);
		}
		orAdd.tech.requiredTech.ForEach(delegate(Tech _tech)
		{
			this.AddTechToQueue(_tech);
		});
	}

	// Token: 0x0600440B RID: 17419 RVA: 0x0017D9D4 File Offset: 0x0017BBD4
	public void CancelResearch(Tech tech, bool clickedEntry = true)
	{
		Research.<>c__DisplayClass26_0 CS$<>8__locals1 = new Research.<>c__DisplayClass26_0();
		CS$<>8__locals1.tech = tech;
		CS$<>8__locals1.ti = this.queuedTech.Find((TechInstance qt) => qt.tech == CS$<>8__locals1.tech);
		if (CS$<>8__locals1.ti == null)
		{
			return;
		}
		if (CS$<>8__locals1.ti == this.queuedTech[this.queuedTech.Count - 1] && clickedEntry)
		{
			this.SetActiveResearch(null, false);
		}
		int i;
		int j;
		for (i = CS$<>8__locals1.ti.tech.unlockedTech.Count - 1; i >= 0; i = j - 1)
		{
			if (this.queuedTech.Find((TechInstance qt) => qt.tech == CS$<>8__locals1.ti.tech.unlockedTech[i]) != null)
			{
				this.CancelResearch(CS$<>8__locals1.ti.tech.unlockedTech[i], false);
			}
			j = i;
		}
		this.queuedTech.Remove(CS$<>8__locals1.ti);
		if (clickedEntry)
		{
			this.NotifyResearchCenters(GameHashes.ActiveResearchChanged, this.queuedTech);
		}
	}

	// Token: 0x0600440C RID: 17420 RVA: 0x0017DAF0 File Offset: 0x0017BCF0
	private void NotifyResearchCenters(GameHashes hash, object data)
	{
		foreach (object obj in Components.ResearchCenters)
		{
			((KMonoBehaviour)obj).Trigger(-1914338957, data);
		}
		base.Trigger((int)hash, data);
	}

	// Token: 0x0600440D RID: 17421 RVA: 0x0017DB54 File Offset: 0x0017BD54
	public void SetActiveResearch(Tech tech, bool clearQueue = false)
	{
		if (clearQueue)
		{
			this.queuedTech.Clear();
		}
		this.activeResearch = null;
		if (tech != null)
		{
			if (this.queuedTech.Count == 0)
			{
				this.AddTechToQueue(tech);
			}
			if (this.queuedTech.Count > 0)
			{
				this.queuedTech.Sort((TechInstance x, TechInstance y) => x.tech.tier.CompareTo(y.tech.tier));
				this.activeResearch = this.queuedTech[0];
			}
		}
		else
		{
			this.queuedTech.Clear();
		}
		this.NotifyResearchCenters(GameHashes.ActiveResearchChanged, this.queuedTech);
		this.CheckBuyResearch();
		this.CheckResearchBuildings(null);
		this.UpdateResearcherRoleNotification();
	}

	// Token: 0x0600440E RID: 17422 RVA: 0x0017DC08 File Offset: 0x0017BE08
	private void UpdateResearcherRoleNotification()
	{
		if (this.NoResearcherRoleNotification != null)
		{
			this.notifier.Remove(this.NoResearcherRoleNotification);
			this.NoResearcherRoleNotification = null;
		}
		if (this.activeResearch != null)
		{
			Skill skill = null;
			if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("advanced") && this.activeResearch.tech.costsByResearchTypeID["advanced"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowAdvancedResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowAdvancedResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("space") && this.activeResearch.tech.costsByResearchTypeID["space"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowInterstellarResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowInterstellarResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("nuclear") && this.activeResearch.tech.costsByResearchTypeID["nuclear"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowNuclearResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowNuclearResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("orbital") && this.activeResearch.tech.costsByResearchTypeID["orbital"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowOrbitalResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowOrbitalResearch)[0];
			}
			if (skill != null)
			{
				this.NoResearcherRoleNotification = new Notification(RESEARCH.MESSAGING.NO_RESEARCHER_SKILL, NotificationType.Bad, new Func<List<Notification>, object, string>(this.NoResearcherRoleTooltip), skill, false, 12f, null, null, null, true, false, false);
				this.notifier.Add(this.NoResearcherRoleNotification, "");
			}
		}
	}

	// Token: 0x0600440F RID: 17423 RVA: 0x0017DE90 File Offset: 0x0017C090
	private string NoResearcherRoleTooltip(List<Notification> list, object data)
	{
		Skill skill = (Skill)data;
		return RESEARCH.MESSAGING.NO_RESEARCHER_SKILL_TOOLTIP.Replace("{ResearchType}", skill.Name);
	}

	// Token: 0x06004410 RID: 17424 RVA: 0x0017DEBC File Offset: 0x0017C0BC
	public void AddResearchPoints(string researchTypeID, float points)
	{
		if (!this.UseGlobalPointInventory && this.activeResearch == null)
		{
			global::Debug.LogWarning("No active research to add research points to. Global research inventory is disabled.");
			return;
		}
		(this.UseGlobalPointInventory ? this.globalPointInventory : this.activeResearch.progressInventory).AddResearchPoints(researchTypeID, points);
		this.CheckBuyResearch();
		this.NotifyResearchCenters(GameHashes.ResearchPointsChanged, null);
	}

	// Token: 0x06004411 RID: 17425 RVA: 0x0017DF18 File Offset: 0x0017C118
	private void CheckBuyResearch()
	{
		if (this.activeResearch != null)
		{
			ResearchPointInventory researchPointInventory = this.UseGlobalPointInventory ? this.globalPointInventory : this.activeResearch.progressInventory;
			if (this.activeResearch.tech.CanAfford(researchPointInventory))
			{
				foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
				{
					researchPointInventory.RemoveResearchPoints(keyValuePair.Key, keyValuePair.Value);
				}
				this.activeResearch.Purchased();
				Game.Instance.Trigger(-107300940, this.activeResearch.tech);
				this.GetNextTech();
			}
		}
	}

	// Token: 0x06004412 RID: 17426 RVA: 0x0017DFE8 File Offset: 0x0017C1E8
	protected override void OnCleanUp()
	{
		if (Game.Instance != null && this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
		}
		Components.ResearchCenters.OnAdd -= new Action<IResearchCenter>(this.CheckResearchBuildings);
		Components.ResearchCenters.OnRemove -= new Action<IResearchCenter>(this.CheckResearchBuildings);
		base.OnCleanUp();
	}

	// Token: 0x06004413 RID: 17427 RVA: 0x0017E050 File Offset: 0x0017C250
	public void CompleteQueue()
	{
		while (this.queuedTech.Count > 0)
		{
			foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
			{
				this.AddResearchPoints(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}

	// Token: 0x06004414 RID: 17428 RVA: 0x0017E0CC File Offset: 0x0017C2CC
	public List<TechInstance> GetResearchQueue()
	{
		return new List<TechInstance>(this.queuedTech);
	}

	// Token: 0x06004415 RID: 17429 RVA: 0x0017E0DC File Offset: 0x0017C2DC
	[OnSerializing]
	internal void OnSerializing()
	{
		this.saveData = default(Research.SaveData);
		if (this.activeResearch != null)
		{
			this.saveData.activeResearchId = this.activeResearch.tech.Id;
		}
		else
		{
			this.saveData.activeResearchId = "";
		}
		if (this.queuedTech != null && this.queuedTech.Count > 0)
		{
			this.saveData.targetResearchId = this.queuedTech[this.queuedTech.Count - 1].tech.Id;
		}
		else
		{
			this.saveData.targetResearchId = "";
		}
		this.saveData.techs = new TechInstance.SaveData[this.techs.Count];
		for (int i = 0; i < this.techs.Count; i++)
		{
			this.saveData.techs[i] = this.techs[i].Save();
		}
	}

	// Token: 0x06004416 RID: 17430 RVA: 0x0017E1D4 File Offset: 0x0017C3D4
	[OnDeserialized]
	internal void OnDeserialized()
	{
		if (this.saveData.techs != null)
		{
			foreach (TechInstance.SaveData saveData in this.saveData.techs)
			{
				Tech tech = Db.Get().Techs.TryGet(saveData.techId);
				if (tech != null)
				{
					this.GetOrAdd(tech).Load(saveData);
				}
			}
		}
		foreach (TechInstance techInstance in this.techs)
		{
			if (this.saveData.targetResearchId == techInstance.tech.Id)
			{
				this.SetActiveResearch(techInstance.tech, false);
				break;
			}
		}
	}

	// Token: 0x06004417 RID: 17431 RVA: 0x0017E2A8 File Offset: 0x0017C4A8
	private void OnRolesUpdated(object data)
	{
		this.UpdateResearcherRoleNotification();
	}

	// Token: 0x06004418 RID: 17432 RVA: 0x0017E2B0 File Offset: 0x0017C4B0
	public string GetMissingResearchBuildingName()
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
		{
			bool flag = true;
			if (keyValuePair.Value > 0f)
			{
				flag = false;
				using (List<IResearchCenter>.Enumerator enumerator2 = Components.ResearchCenters.Items.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetResearchType() == keyValuePair.Key)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				foreach (IResearchCenter researchCenter in this.researchCenterPrefabs)
				{
					if (researchCenter.GetResearchType() == keyValuePair.Key)
					{
						return ((KMonoBehaviour)researchCenter).GetProperName();
					}
				}
				return null;
			}
		}
		return null;
	}

	// Token: 0x06004419 RID: 17433 RVA: 0x0017E3E0 File Offset: 0x0017C5E0
	private void CheckResearchBuildings(object data)
	{
		if (this.activeResearch == null)
		{
			this.notifier.Remove(this.MissingResearchStation);
			return;
		}
		if (string.IsNullOrEmpty(this.GetMissingResearchBuildingName()))
		{
			this.notifier.Remove(this.MissingResearchStation);
			return;
		}
		this.notifier.Add(this.MissingResearchStation, "");
	}

	// Token: 0x04002D20 RID: 11552
	public static Research Instance;

	// Token: 0x04002D21 RID: 11553
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04002D22 RID: 11554
	private List<TechInstance> techs = new List<TechInstance>();

	// Token: 0x04002D23 RID: 11555
	private List<TechInstance> queuedTech = new List<TechInstance>();

	// Token: 0x04002D24 RID: 11556
	private TechInstance activeResearch;

	// Token: 0x04002D25 RID: 11557
	private Notification NoResearcherRoleNotification;

	// Token: 0x04002D26 RID: 11558
	private Notification MissingResearchStation = new Notification(RESEARCH.MESSAGING.MISSING_RESEARCH_STATION, NotificationType.Bad, (List<Notification> list, object data) => RESEARCH.MESSAGING.MISSING_RESEARCH_STATION_TOOLTIP.ToString().Replace("{0}", Research.Instance.GetMissingResearchBuildingName()), null, false, 11f, null, null, null, true, false, false);

	// Token: 0x04002D27 RID: 11559
	private List<IResearchCenter> researchCenterPrefabs = new List<IResearchCenter>();

	// Token: 0x04002D28 RID: 11560
	protected int skillsUpdateHandle = -1;

	// Token: 0x04002D29 RID: 11561
	public ResearchTypes researchTypes;

	// Token: 0x04002D2A RID: 11562
	public bool UseGlobalPointInventory;

	// Token: 0x04002D2B RID: 11563
	[Serialize]
	public ResearchPointInventory globalPointInventory;

	// Token: 0x04002D2C RID: 11564
	[Serialize]
	private Research.SaveData saveData;

	// Token: 0x0200176B RID: 5995
	private struct SaveData
	{
		// Token: 0x04006EB5 RID: 28341
		public string activeResearchId;

		// Token: 0x04006EB6 RID: 28342
		public string targetResearchId;

		// Token: 0x04006EB7 RID: 28343
		public TechInstance.SaveData[] techs;
	}
}
