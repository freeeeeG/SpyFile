using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000943 RID: 2371
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionResume")]
public class MinionResume : KMonoBehaviour, ISaveLoadable, ISim200ms
{
	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x060044D5 RID: 17621 RVA: 0x00183A74 File Offset: 0x00181C74
	public MinionIdentity GetIdentity
	{
		get
		{
			return this.identity;
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x060044D6 RID: 17622 RVA: 0x00183A7C File Offset: 0x00181C7C
	public float TotalExperienceGained
	{
		get
		{
			return this.totalExperienceGained;
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x060044D7 RID: 17623 RVA: 0x00183A84 File Offset: 0x00181C84
	public int TotalSkillPointsGained
	{
		get
		{
			return MinionResume.CalculateTotalSkillPointsGained(this.TotalExperienceGained);
		}
	}

	// Token: 0x060044D8 RID: 17624 RVA: 0x00183A91 File Offset: 0x00181C91
	public static int CalculateTotalSkillPointsGained(float experience)
	{
		return Mathf.FloorToInt(Mathf.Pow(experience / (float)SKILLS.TARGET_SKILLS_CYCLE / 600f, 1f / SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_EARNED);
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x060044D9 RID: 17625 RVA: 0x00183AC0 File Offset: 0x00181CC0
	public int SkillsMastered
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
			{
				if (keyValuePair.Value)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x060044DA RID: 17626 RVA: 0x00183B1C File Offset: 0x00181D1C
	public int AvailableSkillpoints
	{
		get
		{
			return this.TotalSkillPointsGained - this.SkillsMastered + ((this.GrantedSkillIDs == null) ? 0 : this.GrantedSkillIDs.Count);
		}
	}

	// Token: 0x060044DB RID: 17627 RVA: 0x00183B44 File Offset: 0x00181D44
	[OnDeserialized]
	private void OnDeserializedMethod()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
		{
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
			{
				if (keyValuePair.Value && keyValuePair.Key != "NoRole")
				{
					this.ForceAddSkillPoint();
				}
			}
			foreach (KeyValuePair<HashedString, float> keyValuePair2 in this.AptitudeByRoleGroup)
			{
				this.AptitudeBySkillGroup[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}
		if (this.TotalSkillPointsGained > 1000 || this.TotalSkillPointsGained < 0)
		{
			this.ForceSetSkillPoints(100);
		}
	}

	// Token: 0x060044DC RID: 17628 RVA: 0x00183C40 File Offset: 0x00181E40
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.MinionResumes.Add(this);
	}

	// Token: 0x060044DD RID: 17629 RVA: 0x00183C54 File Offset: 0x00181E54
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.GrantedSkillIDs == null)
		{
			this.GrantedSkillIDs = new List<string>();
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).deprecated)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string skillId in list)
		{
			this.UnmasterSkill(skillId);
		}
		foreach (KeyValuePair<string, bool> keyValuePair2 in this.MasteryBySkillID)
		{
			if (keyValuePair2.Value)
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair2.Key);
				foreach (SkillPerk skillPerk in skill.perks)
				{
					if (skillPerk.OnRemove != null)
					{
						skillPerk.OnRemove(this);
					}
					if (skillPerk.OnApply != null)
					{
						skillPerk.OnApply(this);
					}
				}
				if (!this.ownedHats.ContainsKey(skill.hat))
				{
					this.ownedHats.Add(skill.hat, true);
				}
			}
		}
		this.UpdateExpectations();
		this.UpdateMorale();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		MinionResume.ApplyHat(this.currentHat, component);
		this.ShowNewSkillPointNotification();
	}

	// Token: 0x060044DE RID: 17630 RVA: 0x00183E50 File Offset: 0x00182050
	public void RestoreResume(Dictionary<string, bool> MasteryBySkillID, Dictionary<HashedString, float> AptitudeBySkillGroup, List<string> GrantedSkillIDs, float totalExperienceGained)
	{
		this.MasteryBySkillID = MasteryBySkillID;
		this.GrantedSkillIDs = ((GrantedSkillIDs != null) ? GrantedSkillIDs : new List<string>());
		this.AptitudeBySkillGroup = AptitudeBySkillGroup;
		this.totalExperienceGained = totalExperienceGained;
	}

	// Token: 0x060044DF RID: 17631 RVA: 0x00183E79 File Offset: 0x00182079
	protected override void OnCleanUp()
	{
		Components.MinionResumes.Remove(this);
		if (this.lastSkillNotification != null)
		{
			Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
			this.lastSkillNotification = null;
		}
		base.OnCleanUp();
	}

	// Token: 0x060044E0 RID: 17632 RVA: 0x00183EB0 File Offset: 0x001820B0
	public bool HasMasteredSkill(string skillId)
	{
		return this.MasteryBySkillID.ContainsKey(skillId) && this.MasteryBySkillID[skillId];
	}

	// Token: 0x060044E1 RID: 17633 RVA: 0x00183ED0 File Offset: 0x001820D0
	public void UpdateUrge()
	{
		if (this.targetHat != this.currentHat)
		{
			if (!base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
			{
				base.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
				return;
			}
		}
		else
		{
			base.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x060044E2 RID: 17634 RVA: 0x00183F50 File Offset: 0x00182150
	public string CurrentRole
	{
		get
		{
			return this.currentRole;
		}
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x060044E3 RID: 17635 RVA: 0x00183F58 File Offset: 0x00182158
	public string CurrentHat
	{
		get
		{
			return this.currentHat;
		}
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x060044E4 RID: 17636 RVA: 0x00183F60 File Offset: 0x00182160
	public string TargetHat
	{
		get
		{
			return this.targetHat;
		}
	}

	// Token: 0x060044E5 RID: 17637 RVA: 0x00183F68 File Offset: 0x00182168
	public void SetHats(string current, string target)
	{
		this.currentHat = current;
		this.targetHat = target;
	}

	// Token: 0x060044E6 RID: 17638 RVA: 0x00183F78 File Offset: 0x00182178
	public void SetCurrentRole(string role_id)
	{
		this.currentRole = role_id;
	}

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x060044E7 RID: 17639 RVA: 0x00183F81 File Offset: 0x00182181
	public string TargetRole
	{
		get
		{
			return this.targetRole;
		}
	}

	// Token: 0x060044E8 RID: 17640 RVA: 0x00183F8C File Offset: 0x0018218C
	private void ApplySkillPerks(string skillId)
	{
		foreach (SkillPerk skillPerk in Db.Get().Skills.Get(skillId).perks)
		{
			if (skillPerk.OnApply != null)
			{
				skillPerk.OnApply(this);
			}
		}
	}

	// Token: 0x060044E9 RID: 17641 RVA: 0x00183FFC File Offset: 0x001821FC
	private void RemoveSkillPerks(string skillId)
	{
		foreach (SkillPerk skillPerk in Db.Get().Skills.Get(skillId).perks)
		{
			if (skillPerk.OnRemove != null)
			{
				skillPerk.OnRemove(this);
			}
		}
	}

	// Token: 0x060044EA RID: 17642 RVA: 0x0018406C File Offset: 0x0018226C
	public void Sim200ms(float dt)
	{
		this.DEBUG_SecondsAlive += dt;
		if (!base.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
		{
			this.DEBUG_PassiveExperienceGained += dt * SKILLS.PASSIVE_EXPERIENCE_PORTION;
			this.AddExperience(dt * SKILLS.PASSIVE_EXPERIENCE_PORTION);
		}
	}

	// Token: 0x060044EB RID: 17643 RVA: 0x001840BC File Offset: 0x001822BC
	public bool IsAbleToLearnSkill(string skillId)
	{
		Skill skill = Db.Get().Skills.Get(skillId);
		string choreGroupID = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
		if (!string.IsNullOrEmpty(choreGroupID))
		{
			Traits component = base.GetComponent<Traits>();
			if (component != null && component.IsChoreGroupDisabled(choreGroupID))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060044EC RID: 17644 RVA: 0x00184120 File Offset: 0x00182320
	public bool BelowMoraleExpectation(Skill skill)
	{
		float num = Db.Get().Attributes.QualityOfLife.Lookup(this).GetTotalValue();
		float totalValue = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this).GetTotalValue();
		int moraleExpectation = skill.GetMoraleExpectation();
		if (this.AptitudeBySkillGroup.ContainsKey(skill.skillGroup) && this.AptitudeBySkillGroup[skill.skillGroup] > 0f)
		{
			num += 1f;
		}
		return totalValue + (float)moraleExpectation <= num;
	}

	// Token: 0x060044ED RID: 17645 RVA: 0x001841B0 File Offset: 0x001823B0
	public bool HasMasteredDirectlyRequiredSkillsForSkill(Skill skill)
	{
		for (int i = 0; i < skill.priorSkills.Count; i++)
		{
			if (!this.HasMasteredSkill(skill.priorSkills[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060044EE RID: 17646 RVA: 0x001841EA File Offset: 0x001823EA
	public bool HasSkillPointsRequiredForSkill(Skill skill)
	{
		return this.AvailableSkillpoints >= 1;
	}

	// Token: 0x060044EF RID: 17647 RVA: 0x001841F8 File Offset: 0x001823F8
	public bool HasSkillAptitude(Skill skill)
	{
		return this.AptitudeBySkillGroup.ContainsKey(skill.skillGroup) && this.AptitudeBySkillGroup[skill.skillGroup] > 0f;
	}

	// Token: 0x060044F0 RID: 17648 RVA: 0x00184232 File Offset: 0x00182432
	public bool HasBeenGrantedSkill(Skill skill)
	{
		return this.GrantedSkillIDs != null && this.GrantedSkillIDs.Contains(skill.Id);
	}

	// Token: 0x060044F1 RID: 17649 RVA: 0x00184254 File Offset: 0x00182454
	public bool HasBeenGrantedSkill(string id)
	{
		return this.GrantedSkillIDs != null && this.GrantedSkillIDs.Contains(id);
	}

	// Token: 0x060044F2 RID: 17650 RVA: 0x00184274 File Offset: 0x00182474
	public MinionResume.SkillMasteryConditions[] GetSkillMasteryConditions(string skillId)
	{
		List<MinionResume.SkillMasteryConditions> list = new List<MinionResume.SkillMasteryConditions>();
		Skill skill = Db.Get().Skills.Get(skillId);
		if (this.HasSkillAptitude(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.SkillAptitude);
		}
		if (!this.BelowMoraleExpectation(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.StressWarning);
		}
		if (!this.IsAbleToLearnSkill(skillId))
		{
			list.Add(MinionResume.SkillMasteryConditions.UnableToLearn);
		}
		if (!this.HasSkillPointsRequiredForSkill(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.NeedsSkillPoints);
		}
		if (!this.HasMasteredDirectlyRequiredSkillsForSkill(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.MissingPreviousSkill);
		}
		return list.ToArray();
	}

	// Token: 0x060044F3 RID: 17651 RVA: 0x001842EE File Offset: 0x001824EE
	public bool CanMasterSkill(MinionResume.SkillMasteryConditions[] masteryConditions)
	{
		return !Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.UnableToLearn || element == MinionResume.SkillMasteryConditions.NeedsSkillPoints || element == MinionResume.SkillMasteryConditions.MissingPreviousSkill);
	}

	// Token: 0x060044F4 RID: 17652 RVA: 0x0018431A File Offset: 0x0018251A
	public bool OwnsHat(string hatId)
	{
		return this.ownedHats.ContainsKey(hatId) && this.ownedHats[hatId];
	}

	// Token: 0x060044F5 RID: 17653 RVA: 0x00184338 File Offset: 0x00182538
	public void SkillLearned()
	{
		if (base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
		{
			base.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
		}
		foreach (string key in this.ownedHats.Keys.ToList<string>())
		{
			this.ownedHats[key] = true;
		}
		if (this.targetHat != null && this.currentHat != this.targetHat)
		{
			new PutOnHatChore(this, Db.Get().ChoreTypes.SwitchHat);
		}
	}

	// Token: 0x060044F6 RID: 17654 RVA: 0x0018440C File Offset: 0x0018260C
	public void MasterSkill(string skillId)
	{
		if (!base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
		{
			base.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
		}
		this.MasteryBySkillID[skillId] = true;
		this.ApplySkillPerks(skillId);
		this.UpdateExpectations();
		this.UpdateMorale();
		this.TriggerMasterSkillEvents();
		GameScheduler.Instance.Schedule("Morale Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Morale, true);
		}, null, null);
		if (!this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
		{
			this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
		}
		if (this.AvailableSkillpoints == 0 && this.lastSkillNotification != null)
		{
			Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
			this.lastSkillNotification = null;
		}
	}

	// Token: 0x060044F7 RID: 17655 RVA: 0x00184524 File Offset: 0x00182724
	public void UnmasterSkill(string skillId)
	{
		if (this.MasteryBySkillID.ContainsKey(skillId))
		{
			this.MasteryBySkillID.Remove(skillId);
			this.RemoveSkillPerks(skillId);
			this.UpdateExpectations();
			this.UpdateMorale();
			this.TriggerMasterSkillEvents();
		}
	}

	// Token: 0x060044F8 RID: 17656 RVA: 0x0018455C File Offset: 0x0018275C
	public void GrantSkill(string skillId)
	{
		if (this.GrantedSkillIDs == null)
		{
			this.GrantedSkillIDs = new List<string>();
		}
		if (!this.HasBeenGrantedSkill(skillId))
		{
			this.MasteryBySkillID[skillId] = true;
			this.ApplySkillPerks(skillId);
			this.GrantedSkillIDs.Add(skillId);
			this.UpdateExpectations();
			this.UpdateMorale();
			this.TriggerMasterSkillEvents();
			if (!this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
			{
				this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
			}
		}
	}

	// Token: 0x060044F9 RID: 17657 RVA: 0x001845FA File Offset: 0x001827FA
	private void TriggerMasterSkillEvents()
	{
		base.Trigger(540773776, null);
		Game.Instance.Trigger(-1523247426, this);
	}

	// Token: 0x060044FA RID: 17658 RVA: 0x00184618 File Offset: 0x00182818
	public void ForceSetSkillPoints(int points)
	{
		this.totalExperienceGained = MinionResume.CalculatePreviousExperienceBar(points);
	}

	// Token: 0x060044FB RID: 17659 RVA: 0x00184626 File Offset: 0x00182826
	public void ForceAddSkillPoint()
	{
		this.AddExperience(MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained) - this.totalExperienceGained);
	}

	// Token: 0x060044FC RID: 17660 RVA: 0x00184640 File Offset: 0x00182840
	public static float CalculateNextExperienceBar(int current_skill_points)
	{
		return Mathf.Pow((float)(current_skill_points + 1) / (float)SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_CYCLE * 600f;
	}

	// Token: 0x060044FD RID: 17661 RVA: 0x00184664 File Offset: 0x00182864
	public static float CalculatePreviousExperienceBar(int current_skill_points)
	{
		return Mathf.Pow((float)current_skill_points / (float)SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_CYCLE * 600f;
	}

	// Token: 0x060044FE RID: 17662 RVA: 0x00184688 File Offset: 0x00182888
	private void UpdateExpectations()
	{
		int num = 0;
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && !this.HasBeenGrantedSkill(keyValuePair.Key))
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
				num += skill.tier + 1;
			}
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this);
		if (this.skillsMoraleExpectationModifier != null)
		{
			attributeInstance.Remove(this.skillsMoraleExpectationModifier);
			this.skillsMoraleExpectationModifier = null;
		}
		if (num > 0)
		{
			this.skillsMoraleExpectationModifier = new AttributeModifier(attributeInstance.Id, (float)num, DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_MOD_NAME, false, false, true);
			attributeInstance.Add(this.skillsMoraleExpectationModifier);
		}
	}

	// Token: 0x060044FF RID: 17663 RVA: 0x00184774 File Offset: 0x00182974
	private void UpdateMorale()
	{
		int num = 0;
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && !this.HasBeenGrantedSkill(keyValuePair.Key))
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
				float num2 = 0f;
				if (this.AptitudeBySkillGroup.TryGetValue(new HashedString(skill.skillGroup), out num2))
				{
					num += (int)num2;
				}
			}
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(this);
		if (this.skillsMoraleModifier != null)
		{
			attributeInstance.Remove(this.skillsMoraleModifier);
			this.skillsMoraleModifier = null;
		}
		if (num > 0)
		{
			this.skillsMoraleModifier = new AttributeModifier(attributeInstance.Id, (float)num, DUPLICANTS.NEEDS.QUALITYOFLIFE.APTITUDE_SKILLS_MOD_NAME, false, false, true);
			attributeInstance.Add(this.skillsMoraleModifier);
		}
	}

	// Token: 0x06004500 RID: 17664 RVA: 0x0018487C File Offset: 0x00182A7C
	private void OnSkillPointGained()
	{
		Game.Instance.Trigger(1505456302, this);
		this.ShowNewSkillPointNotification();
		if (PopFXManager.Instance != null)
		{
			string text = MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.identity.GetProperName());
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, text, base.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
		}
		new UpgradeFX.Instance(base.gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, -0.1f)).StartSM();
	}

	// Token: 0x06004501 RID: 17665 RVA: 0x00184928 File Offset: 0x00182B28
	private void ShowNewSkillPointNotification()
	{
		if (this.AvailableSkillpoints == 1)
		{
			this.lastSkillNotification = new ManagementMenuNotification(global::Action.ManageSkills, NotificationValence.Good, this.identity.GetSoleOwner().gameObject.GetInstanceID().ToString(), MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.identity.GetProperName()), NotificationType.Good, new Func<List<Notification>, object, string>(this.GetSkillPointGainedTooltip), this.identity, true, 0f, delegate(object d)
			{
				ManagementMenu.Instance.OpenSkills(this.identity);
			}, null, null, true);
			base.GetComponent<Notifier>().Add(this.lastSkillNotification, "");
		}
	}

	// Token: 0x06004502 RID: 17666 RVA: 0x001849C4 File Offset: 0x00182BC4
	private string GetSkillPointGainedTooltip(List<Notification> notifications, object data)
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", ((MinionIdentity)data).GetProperName());
	}

	// Token: 0x06004503 RID: 17667 RVA: 0x001849E0 File Offset: 0x00182BE0
	public void SetAptitude(HashedString skillGroupID, float amount)
	{
		this.AptitudeBySkillGroup[skillGroupID] = amount;
	}

	// Token: 0x06004504 RID: 17668 RVA: 0x001849F0 File Offset: 0x00182BF0
	public float GetAptitudeExperienceMultiplier(HashedString skillGroupId, float buildingFrequencyMultiplier)
	{
		float num = 0f;
		this.AptitudeBySkillGroup.TryGetValue(skillGroupId, out num);
		return 1f + num * SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER * buildingFrequencyMultiplier;
	}

	// Token: 0x06004505 RID: 17669 RVA: 0x00184A24 File Offset: 0x00182C24
	public void AddExperience(float amount)
	{
		float num = this.totalExperienceGained;
		float num2 = MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained);
		this.totalExperienceGained += amount;
		if (base.isSpawned && this.totalExperienceGained >= num2 && num < num2)
		{
			this.OnSkillPointGained();
		}
	}

	// Token: 0x06004506 RID: 17670 RVA: 0x00184A70 File Offset: 0x00182C70
	public void AddExperienceWithAptitude(string skillGroupId, float amount, float buildingMultiplier)
	{
		float num = amount * this.GetAptitudeExperienceMultiplier(skillGroupId, buildingMultiplier) * SKILLS.ACTIVE_EXPERIENCE_PORTION;
		this.DEBUG_ActiveExperienceGained += num;
		this.AddExperience(num);
	}

	// Token: 0x06004507 RID: 17671 RVA: 0x00184AA8 File Offset: 0x00182CA8
	public bool HasPerk(HashedString perkId)
	{
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perkId))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004508 RID: 17672 RVA: 0x00184B24 File Offset: 0x00182D24
	public bool HasPerk(SkillPerk perk)
	{
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004509 RID: 17673 RVA: 0x00184BA0 File Offset: 0x00182DA0
	public void RemoveHat()
	{
		MinionResume.RemoveHat(base.GetComponent<KBatchedAnimController>());
	}

	// Token: 0x0600450A RID: 17674 RVA: 0x00184BB0 File Offset: 0x00182DB0
	public static void RemoveHat(KBatchedAnimController controller)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		Accessorizer component = controller.GetComponent<Accessorizer>();
		if (component != null)
		{
			Accessory accessory = component.GetAccessory(hat);
			if (accessory != null)
			{
				component.RemoveAccessory(accessory);
			}
		}
		else
		{
			controller.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride(hat.targetSymbolId, 4);
		}
		controller.SetSymbolVisiblity(hat.targetSymbolId, false);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
	}

	// Token: 0x0600450B RID: 17675 RVA: 0x00184C4C File Offset: 0x00182E4C
	public static void AddHat(string hat_id, KBatchedAnimController controller)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		Accessory accessory = hat.Lookup(hat_id);
		if (accessory == null)
		{
			global::Debug.LogWarning("Missing hat: " + hat_id);
		}
		Accessorizer component = controller.GetComponent<Accessorizer>();
		if (component != null)
		{
			Accessory accessory2 = component.GetAccessory(Db.Get().AccessorySlots.Hat);
			if (accessory2 != null)
			{
				component.RemoveAccessory(accessory2);
			}
			if (accessory != null)
			{
				component.AddAccessory(accessory);
			}
		}
		else
		{
			SymbolOverrideController component2 = controller.GetComponent<SymbolOverrideController>();
			component2.TryRemoveSymbolOverride(hat.targetSymbolId, 4);
			component2.AddSymbolOverride(hat.targetSymbolId, accessory.symbol, 4);
		}
		controller.SetSymbolVisiblity(hat.targetSymbolId, true);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
	}

	// Token: 0x0600450C RID: 17676 RVA: 0x00184D34 File Offset: 0x00182F34
	public void ApplyTargetHat()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		MinionResume.ApplyHat(this.targetHat, component);
		this.currentHat = this.targetHat;
		this.targetHat = null;
	}

	// Token: 0x0600450D RID: 17677 RVA: 0x00184D67 File Offset: 0x00182F67
	public static void ApplyHat(string hat_id, KBatchedAnimController controller)
	{
		if (hat_id.IsNullOrWhiteSpace())
		{
			MinionResume.RemoveHat(controller);
			return;
		}
		MinionResume.AddHat(hat_id, controller);
	}

	// Token: 0x0600450E RID: 17678 RVA: 0x00184D7F File Offset: 0x00182F7F
	public string GetSkillsSubtitle()
	{
		return string.Format(DUPLICANTS.NEEDS.QUALITYOFLIFE.TOTAL_SKILL_POINTS, this.TotalSkillPointsGained);
	}

	// Token: 0x0600450F RID: 17679 RVA: 0x00184D9C File Offset: 0x00182F9C
	public static bool AnyMinionHasPerk(string perk, int worldId = -1)
	{
		using (List<MinionResume>.Enumerator enumerator = (from minion in (worldId >= 0) ? Components.MinionResumes.GetWorldItems(worldId, true) : Components.MinionResumes.Items
		where !minion.HasTag(GameTags.Dead)
		select minion).ToList<MinionResume>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasPerk(perk))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004510 RID: 17680 RVA: 0x00184E3C File Offset: 0x0018303C
	public static bool AnyOtherMinionHasPerk(string perk, MinionResume me)
	{
		foreach (MinionResume minionResume in Components.MinionResumes.Items)
		{
			if (!(minionResume == me) && minionResume.HasPerk(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004511 RID: 17681 RVA: 0x00184EAC File Offset: 0x001830AC
	public void ResetSkillLevels(bool returnSkillPoints = true)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string skillId in list)
		{
			this.UnmasterSkill(skillId);
		}
	}

	// Token: 0x04002D99 RID: 11673
	[MyCmpReq]
	private MinionIdentity identity;

	// Token: 0x04002D9A RID: 11674
	[Serialize]
	public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();

	// Token: 0x04002D9B RID: 11675
	[Serialize]
	public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();

	// Token: 0x04002D9C RID: 11676
	[Serialize]
	public List<string> GrantedSkillIDs = new List<string>();

	// Token: 0x04002D9D RID: 11677
	[Serialize]
	public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();

	// Token: 0x04002D9E RID: 11678
	[Serialize]
	public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();

	// Token: 0x04002D9F RID: 11679
	[Serialize]
	private string currentRole = "NoRole";

	// Token: 0x04002DA0 RID: 11680
	[Serialize]
	private string targetRole = "NoRole";

	// Token: 0x04002DA1 RID: 11681
	[Serialize]
	private string currentHat;

	// Token: 0x04002DA2 RID: 11682
	[Serialize]
	private string targetHat;

	// Token: 0x04002DA3 RID: 11683
	private Dictionary<string, bool> ownedHats = new Dictionary<string, bool>();

	// Token: 0x04002DA4 RID: 11684
	[Serialize]
	private float totalExperienceGained;

	// Token: 0x04002DA5 RID: 11685
	private Notification lastSkillNotification;

	// Token: 0x04002DA6 RID: 11686
	private AttributeModifier skillsMoraleExpectationModifier;

	// Token: 0x04002DA7 RID: 11687
	private AttributeModifier skillsMoraleModifier;

	// Token: 0x04002DA8 RID: 11688
	public float DEBUG_PassiveExperienceGained;

	// Token: 0x04002DA9 RID: 11689
	public float DEBUG_ActiveExperienceGained;

	// Token: 0x04002DAA RID: 11690
	public float DEBUG_SecondsAlive;

	// Token: 0x02001792 RID: 6034
	public enum SkillMasteryConditions
	{
		// Token: 0x04006F2D RID: 28461
		SkillAptitude,
		// Token: 0x04006F2E RID: 28462
		StressWarning,
		// Token: 0x04006F2F RID: 28463
		UnableToLearn,
		// Token: 0x04006F30 RID: 28464
		NeedsSkillPoints,
		// Token: 0x04006F31 RID: 28465
		MissingPreviousSkill
	}
}
