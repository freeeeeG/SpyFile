using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020009BB RID: 2491
[SerializationConfig(MemberSerialization.OptIn)]
public class Spacecraft
{
	// Token: 0x06004A3B RID: 19003 RVA: 0x001A2515 File Offset: 0x001A0715
	public Spacecraft(LaunchConditionManager launchConditions)
	{
		this.launchConditions = launchConditions;
	}

	// Token: 0x06004A3C RID: 19004 RVA: 0x001A2546 File Offset: 0x001A0746
	public Spacecraft()
	{
	}

	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x06004A3D RID: 19005 RVA: 0x001A2570 File Offset: 0x001A0770
	// (set) Token: 0x06004A3E RID: 19006 RVA: 0x001A257D File Offset: 0x001A077D
	public LaunchConditionManager launchConditions
	{
		get
		{
			return this.refLaunchConditions.Get();
		}
		set
		{
			this.refLaunchConditions.Set(value);
		}
	}

	// Token: 0x06004A3F RID: 19007 RVA: 0x001A258B File Offset: 0x001A078B
	public void SetRocketName(string newName)
	{
		this.rocketName = newName;
		this.UpdateNameOnRocketModules();
	}

	// Token: 0x06004A40 RID: 19008 RVA: 0x001A259A File Offset: 0x001A079A
	public string GetRocketName()
	{
		return this.rocketName;
	}

	// Token: 0x06004A41 RID: 19009 RVA: 0x001A25A4 File Offset: 0x001A07A4
	public void UpdateNameOnRocketModules()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				component.SetParentRocketName(this.rocketName);
			}
		}
	}

	// Token: 0x06004A42 RID: 19010 RVA: 0x001A2614 File Offset: 0x001A0814
	public bool HasInvalidID()
	{
		return this.id == -1;
	}

	// Token: 0x06004A43 RID: 19011 RVA: 0x001A261F File Offset: 0x001A081F
	public void SetID(int id)
	{
		this.id = id;
	}

	// Token: 0x06004A44 RID: 19012 RVA: 0x001A2628 File Offset: 0x001A0828
	public void SetState(Spacecraft.MissionState state)
	{
		this.state = state;
	}

	// Token: 0x06004A45 RID: 19013 RVA: 0x001A2631 File Offset: 0x001A0831
	public void BeginMission(SpaceDestination destination)
	{
		this.missionElapsed = 0f;
		this.missionDuration = (float)destination.OneBasedDistance * ROCKETRY.MISSION_DURATION_SCALE / this.GetPilotNavigationEfficiency();
		this.SetState(Spacecraft.MissionState.Launching);
	}

	// Token: 0x06004A46 RID: 19014 RVA: 0x001A2660 File Offset: 0x001A0860
	private float GetPilotNavigationEfficiency()
	{
		List<MinionStorage.Info> storedMinionInfo = this.launchConditions.GetComponent<MinionStorage>().GetStoredMinionInfo();
		if (storedMinionInfo.Count < 1)
		{
			return 1f;
		}
		StoredMinionIdentity component = storedMinionInfo[0].serializedMinion.Get().GetComponent<StoredMinionIdentity>();
		string b = Db.Get().Attributes.SpaceNavigation.Id;
		float num = 1f;
		foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
		{
			foreach (SkillPerk skillPerk in Db.Get().Skills.Get(keyValuePair.Key).perks)
			{
				SkillAttributePerk skillAttributePerk = skillPerk as SkillAttributePerk;
				if (skillAttributePerk != null && skillAttributePerk.modifier.AttributeId == b)
				{
					num += skillAttributePerk.modifier.Value;
				}
			}
		}
		return num;
	}

	// Token: 0x06004A47 RID: 19015 RVA: 0x001A2780 File Offset: 0x001A0980
	public void ForceComplete()
	{
		this.missionElapsed = this.missionDuration;
	}

	// Token: 0x06004A48 RID: 19016 RVA: 0x001A2790 File Offset: 0x001A0990
	public void ProgressMission(float deltaTime)
	{
		if (this.state == Spacecraft.MissionState.Underway)
		{
			this.missionElapsed += deltaTime;
			if (this.controlStationBuffTimeRemaining > 0f)
			{
				this.missionElapsed += deltaTime * 0.20000005f;
				this.controlStationBuffTimeRemaining -= deltaTime;
			}
			else
			{
				this.controlStationBuffTimeRemaining = 0f;
			}
			if (this.missionElapsed > this.missionDuration)
			{
				this.CompleteMission();
			}
		}
	}

	// Token: 0x06004A49 RID: 19017 RVA: 0x001A2804 File Offset: 0x001A0A04
	public float GetTimeLeft()
	{
		return this.missionDuration - this.missionElapsed;
	}

	// Token: 0x06004A4A RID: 19018 RVA: 0x001A2813 File Offset: 0x001A0A13
	public float GetDuration()
	{
		return this.missionDuration;
	}

	// Token: 0x06004A4B RID: 19019 RVA: 0x001A281B File Offset: 0x001A0A1B
	public void CompleteMission()
	{
		SpacecraftManager.instance.PushReadyToLandNotification(this);
		this.SetState(Spacecraft.MissionState.WaitingToLand);
		this.Land();
	}

	// Token: 0x06004A4C RID: 19020 RVA: 0x001A2838 File Offset: 0x001A0A38
	private void Land()
	{
		this.launchConditions.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(this.id));
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
		{
			if (gameObject != this.launchConditions.gameObject)
			{
				gameObject.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(this.id));
			}
		}
	}

	// Token: 0x06004A4D RID: 19021 RVA: 0x001A28DC File Offset: 0x001A0ADC
	public void TemporallyTear()
	{
		SpacecraftManager.instance.hasVisitedWormHole = true;
		LaunchConditionManager launchConditions = this.launchConditions;
		for (int i = launchConditions.rocketModules.Count - 1; i >= 0; i--)
		{
			Storage component = launchConditions.rocketModules[i].GetComponent<Storage>();
			if (component != null)
			{
				component.ConsumeAllIgnoringDisease();
			}
			MinionStorage component2 = launchConditions.rocketModules[i].GetComponent<MinionStorage>();
			if (component2 != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component2.GetStoredMinionInfo();
				for (int j = storedMinionInfo.Count - 1; j >= 0; j--)
				{
					component2.DeleteStoredMinion(storedMinionInfo[j].id);
				}
			}
			Util.KDestroyGameObject(launchConditions.rocketModules[i].gameObject);
		}
	}

	// Token: 0x06004A4E RID: 19022 RVA: 0x001A299F File Offset: 0x001A0B9F
	public void GenerateName()
	{
		this.SetRocketName(GameUtil.GenerateRandomRocketName());
	}

	// Token: 0x040030DB RID: 12507
	[Serialize]
	public int id = -1;

	// Token: 0x040030DC RID: 12508
	[Serialize]
	public string rocketName = UI.STARMAP.DEFAULT_NAME;

	// Token: 0x040030DD RID: 12509
	[Serialize]
	public float controlStationBuffTimeRemaining;

	// Token: 0x040030DE RID: 12510
	[Serialize]
	public Ref<LaunchConditionManager> refLaunchConditions = new Ref<LaunchConditionManager>();

	// Token: 0x040030DF RID: 12511
	[Serialize]
	public Spacecraft.MissionState state;

	// Token: 0x040030E0 RID: 12512
	[Serialize]
	private float missionElapsed;

	// Token: 0x040030E1 RID: 12513
	[Serialize]
	private float missionDuration;

	// Token: 0x02001845 RID: 6213
	public enum MissionState
	{
		// Token: 0x04007183 RID: 29059
		Grounded,
		// Token: 0x04007184 RID: 29060
		Launching,
		// Token: 0x04007185 RID: 29061
		Underway,
		// Token: 0x04007186 RID: 29062
		WaitingToLand,
		// Token: 0x04007187 RID: 29063
		Landing,
		// Token: 0x04007188 RID: 29064
		Destroyed
	}
}
