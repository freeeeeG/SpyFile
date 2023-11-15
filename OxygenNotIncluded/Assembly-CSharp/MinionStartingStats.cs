using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000866 RID: 2150
public class MinionStartingStats : ITelepadDeliverable
{
	// Token: 0x06003EE9 RID: 16105 RVA: 0x0015DDE8 File Offset: 0x0015BFE8
	public MinionStartingStats(Personality personality, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		this.personality = personality;
		this.GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion, false);
	}

	// Token: 0x06003EEA RID: 16106 RVA: 0x0015DE3C File Offset: 0x0015C03C
	public MinionStartingStats(bool is_starter_minion, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		this.personality = Db.Get().Personalities.GetRandom(true, is_starter_minion);
		this.GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion, is_starter_minion);
	}

	// Token: 0x06003EEB RID: 16107 RVA: 0x0015DEA0 File Offset: 0x0015C0A0
	private void GenerateStats(string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false, bool is_starter_minion = false)
	{
		this.voiceIdx = UnityEngine.Random.Range(0, 4);
		this.Name = this.personality.Name;
		this.NameStringKey = this.personality.nameStringKey;
		this.GenderStringKey = this.personality.genderStringKey;
		this.Traits.Add(Db.Get().traits.Get(MinionConfig.MINION_BASE_TRAIT_ID));
		List<ChoreGroup> disabled_chore_groups = new List<ChoreGroup>();
		this.GenerateAptitudes(guaranteedAptitudeID);
		int pointsDelta = this.GenerateTraits(is_starter_minion, disabled_chore_groups, guaranteedAptitudeID, guaranteedTraitID, isDebugMinion);
		this.GenerateAttributes(pointsDelta, disabled_chore_groups);
		KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(this.personality);
		foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
		{
			if (accessorySlot.accessories.Count != 0)
			{
				Accessory accessory = null;
				if (accessorySlot == Db.Get().AccessorySlots.HeadShape)
				{
					accessory = accessorySlot.Lookup(bodyData.headShape);
					if (accessory == null)
					{
						this.personality.headShape = 0;
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Mouth)
				{
					accessory = accessorySlot.Lookup(bodyData.mouth);
					if (accessory == null)
					{
						this.personality.mouth = 0;
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Eyes)
				{
					accessory = accessorySlot.Lookup(bodyData.eyes);
					if (accessory == null)
					{
						this.personality.eyes = 0;
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Hair)
				{
					accessory = accessorySlot.Lookup(bodyData.hair);
					if (accessory == null)
					{
						this.personality.hair = 0;
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.HatHair)
				{
					accessory = accessorySlot.accessories[0];
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Body)
				{
					accessory = accessorySlot.Lookup(bodyData.body);
					if (accessory == null)
					{
						this.personality.body = 0;
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Arm)
				{
					accessory = accessorySlot.Lookup(bodyData.arms);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.ArmLower)
				{
					accessory = accessorySlot.Lookup(bodyData.armslower);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.ArmLowerSkin)
				{
					accessory = accessorySlot.Lookup(bodyData.armLowerSkin);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.ArmUpperSkin)
				{
					accessory = accessorySlot.Lookup(bodyData.armUpperSkin);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.LegSkin)
				{
					accessory = accessorySlot.Lookup(bodyData.legSkin);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Leg)
				{
					accessory = accessorySlot.Lookup(bodyData.legs);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Belt)
				{
					accessory = accessorySlot.Lookup(bodyData.belt);
					if (accessory == null)
					{
						accessory = accessorySlot.accessories[0];
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Neck)
				{
					accessory = accessorySlot.Lookup(bodyData.neck);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Pelvis)
				{
					accessory = accessorySlot.Lookup(bodyData.pelvis);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Foot)
				{
					accessory = accessorySlot.Lookup(bodyData.foot);
					if (accessory == null)
					{
						accessory = accessorySlot.accessories[0];
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Skirt)
				{
					accessory = accessorySlot.Lookup(bodyData.skirt);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Necklace)
				{
					accessory = accessorySlot.Lookup(bodyData.necklace);
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Cuff)
				{
					accessory = accessorySlot.Lookup(bodyData.cuff);
					if (accessory == null)
					{
						accessory = accessorySlot.accessories[0];
					}
				}
				else if (accessorySlot == Db.Get().AccessorySlots.Hand)
				{
					accessory = accessorySlot.Lookup(bodyData.hand);
					if (accessory == null)
					{
						accessory = accessorySlot.accessories[0];
					}
				}
				this.accessories.Add(accessory);
			}
		}
	}

	// Token: 0x06003EEC RID: 16108 RVA: 0x0015E368 File Offset: 0x0015C568
	private int GenerateTraits(bool is_starter_minion, List<ChoreGroup> disabled_chore_groups, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		int statDelta = 0;
		List<string> selectedTraits = new List<string>();
		KRandom randSeed = new KRandom();
		Trait trait = Db.Get().traits.Get(this.personality.stresstrait);
		this.stressTrait = trait;
		Trait trait2 = Db.Get().traits.Get(this.personality.joyTrait);
		this.joyTrait = trait2;
		this.stickerType = this.personality.stickerType;
		Trait trait3 = Db.Get().traits.TryGet(this.personality.congenitaltrait);
		if (trait3 == null || trait3.Name == "None")
		{
			this.congenitaltrait = null;
		}
		else
		{
			this.congenitaltrait = trait3;
		}
		Func<List<DUPLICANTSTATS.TraitVal>, bool, bool> func = delegate(List<DUPLICANTSTATS.TraitVal> traitPossibilities, bool positiveTrait)
		{
			if (this.Traits.Count > DUPLICANTSTATS.MAX_TRAITS)
			{
				return false;
			}
			Mathf.Abs(Util.GaussianRandom(0f, 1f));
			int num6 = traitPossibilities.Count;
			int num7;
			if (!positiveTrait)
			{
				if (DUPLICANTSTATS.rarityDeckActive.Count < 1)
				{
					DUPLICANTSTATS.rarityDeckActive.AddRange(DUPLICANTSTATS.RARITY_DECK);
				}
				if (DUPLICANTSTATS.rarityDeckActive.Count == DUPLICANTSTATS.RARITY_DECK.Count)
				{
					DUPLICANTSTATS.rarityDeckActive.ShuffleSeeded(randSeed);
				}
				num7 = DUPLICANTSTATS.rarityDeckActive[DUPLICANTSTATS.rarityDeckActive.Count - 1];
				DUPLICANTSTATS.rarityDeckActive.RemoveAt(DUPLICANTSTATS.rarityDeckActive.Count - 1);
			}
			else
			{
				List<int> list = new List<int>();
				if (is_starter_minion)
				{
					list.Add(this.rarityBalance - 1);
					list.Add(this.rarityBalance);
					list.Add(this.rarityBalance);
					list.Add(this.rarityBalance + 1);
				}
				else
				{
					list.Add(this.rarityBalance - 2);
					list.Add(this.rarityBalance - 1);
					list.Add(this.rarityBalance);
					list.Add(this.rarityBalance + 1);
					list.Add(this.rarityBalance + 2);
				}
				list.ShuffleSeeded(randSeed);
				num7 = list[0];
				num7 = Mathf.Max(DUPLICANTSTATS.RARITY_COMMON, num7);
				num7 = Mathf.Min(DUPLICANTSTATS.RARITY_LEGENDARY, num7);
			}
			List<DUPLICANTSTATS.TraitVal> list2 = new List<DUPLICANTSTATS.TraitVal>(traitPossibilities);
			for (int j = list2.Count - 1; j > -1; j--)
			{
				if (list2[j].rarity != num7)
				{
					list2.RemoveAt(j);
					num6--;
				}
			}
			list2.ShuffleSeeded(randSeed);
			foreach (DUPLICANTSTATS.TraitVal traitVal2 in list2)
			{
				if (!DlcManager.IsContentActive(traitVal2.dlcId))
				{
					num6--;
				}
				else if (selectedTraits.Contains(traitVal2.id))
				{
					num6--;
				}
				else
				{
					Trait trait5 = Db.Get().traits.TryGet(traitVal2.id);
					if (trait5 == null)
					{
						global::Debug.LogWarning("Trying to add nonexistent trait: " + traitVal2.id);
						num6--;
					}
					else if (!isDebugMinion || trait5.disabledChoreGroups == null || trait5.disabledChoreGroups.Length == 0)
					{
						if (is_starter_minion && !trait5.ValidStarterTrait)
						{
							num6--;
						}
						else if (traitVal2.doNotGenerateTrait)
						{
							num6--;
						}
						else if (this.AreTraitAndAptitudesExclusive(traitVal2, this.skillAptitudes))
						{
							num6--;
						}
						else if (is_starter_minion && guaranteedAptitudeID != null && this.AreTraitAndArchetypeExclusive(traitVal2, guaranteedAptitudeID))
						{
							num6--;
						}
						else
						{
							if (!this.AreTraitsMutuallyExclusive(traitVal2, selectedTraits))
							{
								selectedTraits.Add(traitVal2.id);
								statDelta += traitVal2.statBonus;
								this.rarityBalance += (positiveTrait ? (-traitVal2.rarity) : traitVal2.rarity);
								this.Traits.Add(trait5);
								if (trait5.disabledChoreGroups != null)
								{
									for (int k = 0; k < trait5.disabledChoreGroups.Length; k++)
									{
										disabled_chore_groups.Add(trait5.disabledChoreGroups[k]);
									}
								}
								return true;
							}
							num6--;
						}
					}
				}
			}
			return false;
		};
		int num;
		int num2;
		if (is_starter_minion)
		{
			num = 1;
			num2 = 1;
		}
		else
		{
			if (DUPLICANTSTATS.podTraitConfigurationsActive.Count < 1)
			{
				DUPLICANTSTATS.podTraitConfigurationsActive.AddRange(DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK);
			}
			if (DUPLICANTSTATS.podTraitConfigurationsActive.Count == DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK.Count)
			{
				DUPLICANTSTATS.podTraitConfigurationsActive.ShuffleSeeded(randSeed);
			}
			num = DUPLICANTSTATS.podTraitConfigurationsActive[DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1].first;
			num2 = DUPLICANTSTATS.podTraitConfigurationsActive[DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1].second;
			DUPLICANTSTATS.podTraitConfigurationsActive.RemoveAt(DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1);
		}
		int num3 = 0;
		int num4 = 0;
		int num5 = (num2 + num) * 4;
		if (!string.IsNullOrEmpty(guaranteedTraitID))
		{
			DUPLICANTSTATS.TraitVal traitVal = DUPLICANTSTATS.GetTraitVal(guaranteedTraitID);
			if (traitVal.id == guaranteedTraitID)
			{
				Trait trait4 = Db.Get().traits.TryGet(traitVal.id);
				bool positiveTrait2 = trait4.PositiveTrait;
				selectedTraits.Add(traitVal.id);
				statDelta += traitVal.statBonus;
				this.rarityBalance += (positiveTrait2 ? (-traitVal.rarity) : traitVal.rarity);
				this.Traits.Add(trait4);
				if (trait4.disabledChoreGroups != null)
				{
					for (int i = 0; i < trait4.disabledChoreGroups.Length; i++)
					{
						disabled_chore_groups.Add(trait4.disabledChoreGroups[i]);
					}
				}
				if (positiveTrait2)
				{
					num3++;
				}
				else
				{
					num4++;
				}
			}
		}
		while (num5 > 0 && (num4 < num2 || num3 < num))
		{
			if (num4 < num2 && func(DUPLICANTSTATS.BADTRAITS, false))
			{
				num4++;
			}
			if (num3 < num && func(DUPLICANTSTATS.GOODTRAITS, true))
			{
				num3++;
			}
			num5--;
		}
		if (num5 > 0)
		{
			this.IsValid = true;
		}
		return statDelta;
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x0015E660 File Offset: 0x0015C860
	private void GenerateAptitudes(string guaranteedAptitudeID = null)
	{
		int num = UnityEngine.Random.Range(1, 4);
		List<SkillGroup> list = new List<SkillGroup>(Db.Get().SkillGroups.resources);
		list.Shuffle<SkillGroup>();
		if (guaranteedAptitudeID != null)
		{
			this.skillAptitudes.Add(Db.Get().SkillGroups.Get(guaranteedAptitudeID), (float)DUPLICANTSTATS.APTITUDE_BONUS);
			list.Remove(Db.Get().SkillGroups.Get(guaranteedAptitudeID));
			num--;
		}
		for (int i = 0; i < num; i++)
		{
			this.skillAptitudes.Add(list[i], (float)DUPLICANTSTATS.APTITUDE_BONUS);
		}
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x0015E6F4 File Offset: 0x0015C8F4
	private void GenerateAttributes(int pointsDelta, List<ChoreGroup> disabled_chore_groups)
	{
		List<string> list = new List<string>(DUPLICANTSTATS.ALL_ATTRIBUTES);
		for (int i = 0; i < list.Count; i++)
		{
			if (!this.StartingLevels.ContainsKey(list[i]))
			{
				this.StartingLevels[list[i]] = 0;
			}
		}
		foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.skillAptitudes)
		{
			if (keyValuePair.Key.relevantAttributes.Count > 0)
			{
				for (int j = 0; j < keyValuePair.Key.relevantAttributes.Count; j++)
				{
					if (!this.StartingLevels.ContainsKey(keyValuePair.Key.relevantAttributes[j].Id))
					{
						global::Debug.LogError("Need to add " + keyValuePair.Key.relevantAttributes[j].Id + " to TUNING.DUPLICANTSTATS.ALL_ATTRIBUTES");
					}
					Dictionary<string, int> startingLevels = this.StartingLevels;
					string id = keyValuePair.Key.relevantAttributes[j].Id;
					startingLevels[id] += DUPLICANTSTATS.APTITUDE_ATTRIBUTE_BONUSES[this.skillAptitudes.Count - 1];
				}
			}
		}
		List<SkillGroup> list2 = new List<SkillGroup>(this.skillAptitudes.Keys);
		if (pointsDelta > 0)
		{
			for (int k = pointsDelta; k > 0; k--)
			{
				list2.Shuffle<SkillGroup>();
				for (int l = 0; l < list2[0].relevantAttributes.Count; l++)
				{
					Dictionary<string, int> startingLevels = this.StartingLevels;
					string id = list2[0].relevantAttributes[l].Id;
					startingLevels[id]++;
				}
			}
		}
		if (disabled_chore_groups.Count > 0)
		{
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<string, int> keyValuePair2 in this.StartingLevels)
			{
				if (keyValuePair2.Value > num)
				{
					num = keyValuePair2.Value;
				}
				if (keyValuePair2.Key == disabled_chore_groups[0].attribute.Id)
				{
					num2 = keyValuePair2.Value;
				}
			}
			if (num == num2)
			{
				foreach (string text in list)
				{
					if (text != disabled_chore_groups[0].attribute.Id)
					{
						int num3 = 0;
						this.StartingLevels.TryGetValue(text, out num3);
						int num4 = 0;
						if (num3 > 0)
						{
							num4 = 1;
						}
						this.StartingLevels[disabled_chore_groups[0].attribute.Id] = num3 - num4;
						this.StartingLevels[text] = num + num4;
						break;
					}
				}
			}
		}
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x0015EA20 File Offset: 0x0015CC20
	public void Apply(GameObject go)
	{
		MinionIdentity component = go.GetComponent<MinionIdentity>();
		component.SetName(this.Name);
		component.nameStringKey = this.NameStringKey;
		component.genderStringKey = this.GenderStringKey;
		component.personalityResourceId = this.personality.IdHash;
		this.ApplyTraits(go);
		this.ApplyRace(go);
		this.ApplyAptitudes(go);
		this.ApplyAccessories(go);
		this.ApplyExperience(go);
		this.ApplyOutfit(this.personality, go);
		this.ApplyJoyResponseOutfit(this.personality, go);
	}

	// Token: 0x06003EF0 RID: 16112 RVA: 0x0015EAA4 File Offset: 0x0015CCA4
	public void ApplyExperience(GameObject go)
	{
		foreach (KeyValuePair<string, int> keyValuePair in this.StartingLevels)
		{
			go.GetComponent<AttributeLevels>().SetLevel(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06003EF1 RID: 16113 RVA: 0x0015EB0C File Offset: 0x0015CD0C
	public void ApplyAccessories(GameObject go)
	{
		Accessorizer component = go.GetComponent<Accessorizer>();
		component.ApplyMinionPersonality(this.personality);
		component.UpdateHairBasedOnHat();
	}

	// Token: 0x06003EF2 RID: 16114 RVA: 0x0015EB28 File Offset: 0x0015CD28
	public void ApplyOutfit(Personality personality, GameObject go)
	{
		WearableAccessorizer component = go.GetComponent<WearableAccessorizer>();
		foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, string> keyValuePair in personality.outfitIds)
		{
			Option<ClothingOutfitTarget> outfit = ClothingOutfitTarget.TryFromTemplateId(keyValuePair.Value);
			if (outfit.HasValue)
			{
				component.AddCustomOutfit(outfit);
			}
		}
		if (personality.outfitIds.ContainsKey(ClothingOutfitUtility.OutfitType.Clothing))
		{
			Option<ClothingOutfitTarget> option = ClothingOutfitTarget.TryFromTemplateId(personality.outfitIds[ClothingOutfitUtility.OutfitType.Clothing]);
			if (option.HasValue)
			{
				component.ApplyClothingItems(option.Value.OutfitType, option.Value.ReadItemValues());
			}
		}
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x0015EBE8 File Offset: 0x0015CDE8
	public void ApplyJoyResponseOutfit(Personality personality, GameObject go)
	{
		JoyResponseOutfitTarget joyResponseOutfitTarget = JoyResponseOutfitTarget.FromPersonality(personality);
		JoyResponseOutfitTarget.FromMinion(go).WriteFacadeId(joyResponseOutfitTarget.ReadFacadeId());
	}

	// Token: 0x06003EF4 RID: 16116 RVA: 0x0015EC11 File Offset: 0x0015CE11
	public void ApplyRace(GameObject go)
	{
		go.GetComponent<MinionIdentity>().voiceIdx = this.voiceIdx;
	}

	// Token: 0x06003EF5 RID: 16117 RVA: 0x0015EC24 File Offset: 0x0015CE24
	public static KCompBuilder.BodyData CreateBodyData(Personality p)
	{
		return new KCompBuilder.BodyData
		{
			eyes = HashCache.Get().Add(string.Format("eyes_{0:000}", p.eyes)),
			hair = HashCache.Get().Add(string.Format("hair_{0:000}", p.hair)),
			headShape = HashCache.Get().Add(string.Format("headshape_{0:000}", p.headShape)),
			mouth = HashCache.Get().Add(string.Format("mouth_{0:000}", p.mouth)),
			neck = HashCache.Get().Add("neck"),
			arms = HashCache.Get().Add(string.Format("arm_sleeve_{0:000}", p.body)),
			armslower = HashCache.Get().Add(string.Format("arm_lower_sleeve_{0:000}", p.body)),
			body = HashCache.Get().Add(string.Format("torso_{0:000}", p.body)),
			hat = HashedString.Invalid,
			faceFX = HashedString.Invalid,
			armLowerSkin = HashCache.Get().Add(string.Format("arm_lower_{0:000}", p.headShape)),
			armUpperSkin = HashCache.Get().Add(string.Format("arm_upper_{0:000}", p.headShape)),
			legSkin = HashCache.Get().Add(string.Format("leg_skin_{0:000}", p.headShape)),
			neck = HashCache.Get().Add((p.neck != 0) ? string.Format("neck_{0:000}", p.neck) : "neck"),
			legs = HashCache.Get().Add((p.leg != 0) ? string.Format("leg_{0:000}", p.leg) : "leg"),
			belt = HashCache.Get().Add((p.belt != 0) ? string.Format("belt_{0:000}", p.belt) : "belt"),
			pelvis = HashCache.Get().Add((p.pelvis != 0) ? string.Format("pelvis_{0:000}", p.pelvis) : "pelvis"),
			foot = HashCache.Get().Add((p.foot != 0) ? string.Format("foot_{0:000}", p.foot) : "foot"),
			hand = HashCache.Get().Add((p.hand != 0) ? string.Format("hand_paint_{0:000}", p.hand) : "hand_paint"),
			cuff = HashCache.Get().Add((p.cuff != 0) ? string.Format("cuff_{0:000}", p.cuff) : "cuff")
		};
	}

	// Token: 0x06003EF6 RID: 16118 RVA: 0x0015EF58 File Offset: 0x0015D158
	public void ApplyAptitudes(GameObject go)
	{
		MinionResume component = go.GetComponent<MinionResume>();
		foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.skillAptitudes)
		{
			component.SetAptitude(keyValuePair.Key.Id, keyValuePair.Value);
		}
	}

	// Token: 0x06003EF7 RID: 16119 RVA: 0x0015EFCC File Offset: 0x0015D1CC
	public void ApplyTraits(GameObject go)
	{
		Traits component = go.GetComponent<Traits>();
		component.Clear();
		foreach (Trait trait in this.Traits)
		{
			component.Add(trait);
		}
		component.Add(this.stressTrait);
		if (this.congenitaltrait != null)
		{
			component.Add(this.congenitaltrait);
		}
		component.Add(this.joyTrait);
		go.GetComponent<MinionIdentity>().SetStickerType(this.stickerType);
		MinionIdentity component2 = go.GetComponent<MinionIdentity>();
		component2.SetName(this.Name);
		component2.nameStringKey = this.NameStringKey;
		go.GetComponent<MinionIdentity>().SetGender(this.GenderStringKey);
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x0015F098 File Offset: 0x0015D298
	public GameObject Deliver(Vector3 location)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID), null, null);
		gameObject.SetActive(true);
		gameObject.transform.SetLocalPosition(location);
		this.Apply(gameObject);
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		new EmoteChore(gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", Telepad.PortalBirthAnim, null);
		return gameObject;
	}

	// Token: 0x06003EF9 RID: 16121 RVA: 0x0015F110 File Offset: 0x0015D310
	private bool AreTraitAndAptitudesExclusive(DUPLICANTSTATS.TraitVal traitVal, Dictionary<SkillGroup, float> aptitudes)
	{
		if (traitVal.mutuallyExclusiveAptitudes == null)
		{
			return false;
		}
		foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.skillAptitudes)
		{
			using (List<HashedString>.Enumerator enumerator2 = traitVal.mutuallyExclusiveAptitudes.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == keyValuePair.Key.IdHash && keyValuePair.Value > 0f)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x0015F1C8 File Offset: 0x0015D3C8
	private bool AreTraitAndArchetypeExclusive(DUPLICANTSTATS.TraitVal traitVal, string guaranteedAptitudeID)
	{
		if (!DUPLICANTSTATS.ARCHETYPE_TRAIT_EXCLUSIONS.ContainsKey(guaranteedAptitudeID))
		{
			global::Debug.LogError("Need to add attribute " + guaranteedAptitudeID + " to ARCHETYPE_TRAIT_EXCLUSIONS");
		}
		using (List<string>.Enumerator enumerator = DUPLICANTSTATS.ARCHETYPE_TRAIT_EXCLUSIONS[guaranteedAptitudeID].GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == traitVal.id)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x0015F250 File Offset: 0x0015D450
	private bool AreTraitsMutuallyExclusive(DUPLICANTSTATS.TraitVal traitVal, List<string> selectedTraits)
	{
		foreach (string text in selectedTraits)
		{
			foreach (DUPLICANTSTATS.TraitVal traitVal2 in DUPLICANTSTATS.GOODTRAITS)
			{
				if (text == traitVal2.id && traitVal2.mutuallyExclusiveTraits != null && traitVal2.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal3 in DUPLICANTSTATS.BADTRAITS)
			{
				if (text == traitVal3.id && traitVal3.mutuallyExclusiveTraits != null && traitVal3.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal4 in DUPLICANTSTATS.CONGENITALTRAITS)
			{
				if (text == traitVal4.id && traitVal4.mutuallyExclusiveTraits != null && traitVal4.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal5 in DUPLICANTSTATS.SPECIALTRAITS)
			{
				if (text == traitVal5.id && traitVal5.mutuallyExclusiveTraits != null && traitVal5.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			if (traitVal.mutuallyExclusiveTraits != null && traitVal.mutuallyExclusiveTraits.Contains(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040028B7 RID: 10423
	public string Name;

	// Token: 0x040028B8 RID: 10424
	public string NameStringKey;

	// Token: 0x040028B9 RID: 10425
	public string GenderStringKey;

	// Token: 0x040028BA RID: 10426
	public List<Trait> Traits = new List<Trait>();

	// Token: 0x040028BB RID: 10427
	public int rarityBalance;

	// Token: 0x040028BC RID: 10428
	public Trait stressTrait;

	// Token: 0x040028BD RID: 10429
	public Trait joyTrait;

	// Token: 0x040028BE RID: 10430
	public Trait congenitaltrait;

	// Token: 0x040028BF RID: 10431
	public string stickerType;

	// Token: 0x040028C0 RID: 10432
	public int voiceIdx;

	// Token: 0x040028C1 RID: 10433
	public Dictionary<string, int> StartingLevels = new Dictionary<string, int>();

	// Token: 0x040028C2 RID: 10434
	public Personality personality;

	// Token: 0x040028C3 RID: 10435
	public List<Accessory> accessories = new List<Accessory>();

	// Token: 0x040028C4 RID: 10436
	public bool IsValid;

	// Token: 0x040028C5 RID: 10437
	public Dictionary<SkillGroup, float> skillAptitudes = new Dictionary<SkillGroup, float>();
}
