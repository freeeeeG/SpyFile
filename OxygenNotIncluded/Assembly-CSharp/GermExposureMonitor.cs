﻿using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200087E RID: 2174
public class GermExposureMonitor : GameStateMachine<GermExposureMonitor, GermExposureMonitor.Instance>
{
	// Token: 0x06003F60 RID: 16224 RVA: 0x0016212C File Offset: 0x0016032C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.Update(delegate(GermExposureMonitor.Instance smi, float dt)
		{
			smi.OnInhaleExposureTick(dt);
		}, UpdateRate.SIM_1000ms, true).EventHandler(GameHashes.EatCompleteEater, delegate(GermExposureMonitor.Instance smi, object obj)
		{
			smi.OnEatComplete(obj);
		}).EventHandler(GameHashes.SicknessAdded, delegate(GermExposureMonitor.Instance smi, object data)
		{
			smi.OnSicknessAdded(data);
		}).EventHandler(GameHashes.SicknessCured, delegate(GermExposureMonitor.Instance smi, object data)
		{
			smi.OnSicknessCured(data);
		}).EventHandler(GameHashes.SleepFinished, delegate(GermExposureMonitor.Instance smi)
		{
			smi.OnSleepFinished();
		});
	}

	// Token: 0x06003F61 RID: 16225 RVA: 0x00162219 File Offset: 0x00160419
	public static float GetContractionChance(float rating)
	{
		return 0.5f - 0.5f * (float)Math.Tanh(0.25 * (double)rating);
	}

	// Token: 0x0200167E RID: 5758
	public enum ExposureState
	{
		// Token: 0x04006BE6 RID: 27622
		None,
		// Token: 0x04006BE7 RID: 27623
		Contact,
		// Token: 0x04006BE8 RID: 27624
		Exposed,
		// Token: 0x04006BE9 RID: 27625
		Contracted,
		// Token: 0x04006BEA RID: 27626
		Sick
	}

	// Token: 0x0200167F RID: 5759
	public class ExposureStatusData
	{
		// Token: 0x04006BEB RID: 27627
		public ExposureType exposure_type;

		// Token: 0x04006BEC RID: 27628
		public GermExposureMonitor.Instance owner;
	}

	// Token: 0x02001680 RID: 5760
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<GermExposureMonitor, GermExposureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B0C RID: 35596 RVA: 0x00315230 File Offset: 0x00313430
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sicknesses = master.GetComponent<MinionModifiers>().sicknesses;
			this.primaryElement = master.GetComponent<PrimaryElement>();
			this.traits = master.GetComponent<Traits>();
			this.lastDiseaseSources = new Dictionary<HashedString, GermExposureMonitor.Instance.DiseaseSourceInfo>();
			this.lastExposureTime = new Dictionary<HashedString, float>();
			this.inhaleExposureTick = new Dictionary<HashedString, GermExposureMonitor.Instance.InhaleTickInfo>();
			GameClock.Instance.Subscribe(-722330267, new Action<object>(this.OnNightTime));
			OxygenBreather component = base.GetComponent<OxygenBreather>();
			component.onSimConsume = (Action<Sim.MassConsumedCallback>)Delegate.Combine(component.onSimConsume, new Action<Sim.MassConsumedCallback>(this.OnAirConsumed));
		}

		// Token: 0x06008B0D RID: 35597 RVA: 0x003152FD File Offset: 0x003134FD
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshStatusItems();
		}

		// Token: 0x06008B0E RID: 35598 RVA: 0x0031530C File Offset: 0x0031350C
		public override void StopSM(string reason)
		{
			GameClock.Instance.Unsubscribe(-722330267, new Action<object>(this.OnNightTime));
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				Guid guid;
				this.statusItemHandles.TryGetValue(exposureType.germ_id, out guid);
				guid = base.GetComponent<KSelectable>().RemoveStatusItem(guid, false);
			}
			base.StopSM(reason);
		}

		// Token: 0x06008B0F RID: 35599 RVA: 0x00315378 File Offset: 0x00313578
		public void OnEatComplete(object obj)
		{
			Edible edible = (Edible)obj;
			HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(edible.gameObject);
			if (handle != HandleVector<int>.InvalidHandle)
			{
				DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(handle);
				if (header.diseaseIdx != 255)
				{
					Disease disease = Db.Get().Diseases[(int)header.diseaseIdx];
					float num = edible.unitsConsumed / (edible.unitsConsumed + edible.Units);
					int num2 = Mathf.CeilToInt((float)header.diseaseCount * num);
					GameComps.DiseaseContainers.ModifyDiseaseCount(handle, -num2);
					KPrefabID component = edible.GetComponent<KPrefabID>();
					this.InjectDisease(disease, num2, component.PrefabID(), Sickness.InfectionVector.Digestion);
				}
			}
		}

		// Token: 0x06008B10 RID: 35600 RVA: 0x00315428 File Offset: 0x00313628
		public void OnAirConsumed(Sim.MassConsumedCallback mass_cb_info)
		{
			if (mass_cb_info.diseaseIdx != 255)
			{
				Disease disease = Db.Get().Diseases[(int)mass_cb_info.diseaseIdx];
				this.InjectDisease(disease, mass_cb_info.diseaseCount, ElementLoader.elements[(int)mass_cb_info.elemIdx].tag, Sickness.InfectionVector.Inhalation);
			}
		}

		// Token: 0x06008B11 RID: 35601 RVA: 0x0031547C File Offset: 0x0031367C
		public void OnInhaleExposureTick(float dt)
		{
			foreach (KeyValuePair<HashedString, GermExposureMonitor.Instance.InhaleTickInfo> keyValuePair in this.inhaleExposureTick)
			{
				if (keyValuePair.Value.inhaled)
				{
					keyValuePair.Value.inhaled = false;
					keyValuePair.Value.ticks++;
				}
				else
				{
					keyValuePair.Value.ticks = Mathf.Max(0, keyValuePair.Value.ticks - 1);
				}
			}
		}

		// Token: 0x06008B12 RID: 35602 RVA: 0x0031551C File Offset: 0x0031371C
		public void TryInjectDisease(byte disease_idx, int count, Tag source, Sickness.InfectionVector vector)
		{
			if (disease_idx != 255)
			{
				Disease disease = Db.Get().Diseases[(int)disease_idx];
				this.InjectDisease(disease, count, source, vector);
			}
		}

		// Token: 0x06008B13 RID: 35603 RVA: 0x0031554D File Offset: 0x0031374D
		public float GetGermResistance()
		{
			return Db.Get().Attributes.GermResistance.Lookup(base.gameObject).GetTotalValue();
		}

		// Token: 0x06008B14 RID: 35604 RVA: 0x00315570 File Offset: 0x00313770
		public float GetResistanceToExposureType(ExposureType exposureType, float overrideExposureTier = -1f)
		{
			float num = overrideExposureTier;
			if (num == -1f)
			{
				num = this.GetExposureTier(exposureType.germ_id);
			}
			num = Mathf.Clamp(num, 1f, 3f);
			float num2 = GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[(int)num - 1];
			float totalValue = Db.Get().Attributes.GermResistance.Lookup(base.gameObject).GetTotalValue();
			return (float)exposureType.base_resistance + totalValue + num2;
		}

		// Token: 0x06008B15 RID: 35605 RVA: 0x003155DC File Offset: 0x003137DC
		public int AssessDigestedGerms(ExposureType exposure_type, int count)
		{
			int exposure_threshold = exposure_type.exposure_threshold;
			int val = count / exposure_threshold;
			return MathUtil.Clamp(1, 3, val);
		}

		// Token: 0x06008B16 RID: 35606 RVA: 0x003155FC File Offset: 0x003137FC
		public bool AssessInhaledGerms(ExposureType exposure_type)
		{
			GermExposureMonitor.Instance.InhaleTickInfo inhaleTickInfo;
			this.inhaleExposureTick.TryGetValue(exposure_type.germ_id, out inhaleTickInfo);
			if (inhaleTickInfo == null)
			{
				inhaleTickInfo = new GermExposureMonitor.Instance.InhaleTickInfo();
				this.inhaleExposureTick[exposure_type.germ_id] = inhaleTickInfo;
			}
			if (!inhaleTickInfo.inhaled)
			{
				float exposureTier = this.GetExposureTier(exposure_type.germ_id);
				inhaleTickInfo.inhaled = true;
				return inhaleTickInfo.ticks >= GERM_EXPOSURE.INHALE_TICK_THRESHOLD[(int)exposureTier];
			}
			return false;
		}

		// Token: 0x06008B17 RID: 35607 RVA: 0x00315674 File Offset: 0x00313874
		public void InjectDisease(Disease disease, int count, Tag source, Sickness.InfectionVector vector)
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (disease.id == exposureType.germ_id && count > exposureType.exposure_threshold && this.HasMinExposurePeriodElapsed(exposureType.germ_id) && this.IsExposureValidForTraits(exposureType))
				{
					Sickness sickness = (exposureType.sickness_id != null) ? Db.Get().Sicknesses.Get(exposureType.sickness_id) : null;
					if (sickness == null || sickness.infectionVectors.Contains(vector))
					{
						GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
						float exposureTier = this.GetExposureTier(exposureType.germ_id);
						if (exposureState == GermExposureMonitor.ExposureState.None || exposureState == GermExposureMonitor.ExposureState.Contact)
						{
							float contractionChance = GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f));
							this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Contact);
							if (contractionChance > 0f)
							{
								this.lastDiseaseSources[disease.id] = new GermExposureMonitor.Instance.DiseaseSourceInfo(source, vector, contractionChance, base.transform.GetPosition());
								if (exposureType.infect_immediately)
								{
									this.InfectImmediately(exposureType);
								}
								else
								{
									bool flag = true;
									bool flag2 = vector == Sickness.InfectionVector.Inhalation;
									bool flag3 = vector == Sickness.InfectionVector.Digestion;
									int num = 1;
									if (flag2)
									{
										flag = this.AssessInhaledGerms(exposureType);
									}
									if (flag3)
									{
										num = this.AssessDigestedGerms(exposureType, count);
									}
									if (flag)
									{
										if (flag2)
										{
											this.inhaleExposureTick[exposureType.germ_id].ticks = 0;
										}
										this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Exposed);
										this.SetExposureTier(exposureType.germ_id, (float)num);
										float amount = Mathf.Clamp01(contractionChance);
										GermExposureTracker.Instance.AddExposure(exposureType, amount);
									}
								}
							}
						}
						else if (exposureState == GermExposureMonitor.ExposureState.Exposed && exposureTier < 3f)
						{
							float contractionChance2 = GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f));
							if (contractionChance2 > 0f)
							{
								this.lastDiseaseSources[disease.id] = new GermExposureMonitor.Instance.DiseaseSourceInfo(source, vector, contractionChance2, base.transform.GetPosition());
								if (!exposureType.infect_immediately)
								{
									bool flag4 = true;
									bool flag5 = vector == Sickness.InfectionVector.Inhalation;
									bool flag6 = vector == Sickness.InfectionVector.Digestion;
									int num2 = 1;
									if (flag5)
									{
										flag4 = this.AssessInhaledGerms(exposureType);
									}
									if (flag6)
									{
										num2 = this.AssessDigestedGerms(exposureType, count);
									}
									if (flag4)
									{
										if (flag5)
										{
											this.inhaleExposureTick[exposureType.germ_id].ticks = 0;
										}
										this.SetExposureTier(exposureType.germ_id, this.GetExposureTier(exposureType.germ_id) + (float)num2);
										float amount2 = Mathf.Clamp01(GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f)) - contractionChance2);
										GermExposureTracker.Instance.AddExposure(exposureType, amount2);
									}
								}
							}
						}
					}
				}
			}
			this.RefreshStatusItems();
		}

		// Token: 0x06008B18 RID: 35608 RVA: 0x00315938 File Offset: 0x00313B38
		public GermExposureMonitor.ExposureState GetExposureState(string germ_id)
		{
			GermExposureMonitor.ExposureState result;
			this.exposureStates.TryGetValue(germ_id, out result);
			return result;
		}

		// Token: 0x06008B19 RID: 35609 RVA: 0x00315958 File Offset: 0x00313B58
		public float GetExposureTier(string germ_id)
		{
			float value = 1f;
			this.exposureTiers.TryGetValue(germ_id, out value);
			return Mathf.Clamp(value, 1f, 3f);
		}

		// Token: 0x06008B1A RID: 35610 RVA: 0x0031598A File Offset: 0x00313B8A
		public void SetExposureState(string germ_id, GermExposureMonitor.ExposureState exposure_state)
		{
			this.exposureStates[germ_id] = exposure_state;
			this.RefreshStatusItems();
		}

		// Token: 0x06008B1B RID: 35611 RVA: 0x0031599F File Offset: 0x00313B9F
		public void SetExposureTier(string germ_id, float tier)
		{
			tier = Mathf.Clamp(tier, 0f, 3f);
			this.exposureTiers[germ_id] = tier;
			this.RefreshStatusItems();
		}

		// Token: 0x06008B1C RID: 35612 RVA: 0x003159C6 File Offset: 0x00313BC6
		public void ContractGerms(string germ_id)
		{
			DebugUtil.DevAssert(this.GetExposureState(germ_id) == GermExposureMonitor.ExposureState.Exposed, "Duplicant is contracting a sickness but was never exposed to it!", null);
			this.SetExposureState(germ_id, GermExposureMonitor.ExposureState.Contracted);
		}

		// Token: 0x06008B1D RID: 35613 RVA: 0x003159E8 File Offset: 0x00313BE8
		public void OnSicknessAdded(object sickness_instance_data)
		{
			SicknessInstance sicknessInstance = (SicknessInstance)sickness_instance_data;
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (exposureType.sickness_id == sicknessInstance.Sickness.Id)
				{
					this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Sick);
				}
			}
		}

		// Token: 0x06008B1E RID: 35614 RVA: 0x00315A3C File Offset: 0x00313C3C
		public void OnSicknessCured(object sickness_instance_data)
		{
			SicknessInstance sicknessInstance = (SicknessInstance)sickness_instance_data;
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (exposureType.sickness_id == sicknessInstance.Sickness.Id)
				{
					this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.None);
				}
			}
		}

		// Token: 0x06008B1F RID: 35615 RVA: 0x00315A90 File Offset: 0x00313C90
		private bool IsExposureValidForTraits(ExposureType exposure_type)
		{
			if (exposure_type.required_traits != null && exposure_type.required_traits.Count > 0)
			{
				foreach (string trait_id in exposure_type.required_traits)
				{
					if (!this.traits.HasTrait(trait_id))
					{
						return false;
					}
				}
			}
			if (exposure_type.excluded_traits != null && exposure_type.excluded_traits.Count > 0)
			{
				foreach (string trait_id2 in exposure_type.excluded_traits)
				{
					if (this.traits.HasTrait(trait_id2))
					{
						return false;
					}
				}
			}
			if (exposure_type.excluded_effects != null && exposure_type.excluded_effects.Count > 0)
			{
				Effects component = base.master.GetComponent<Effects>();
				foreach (string effect_id in exposure_type.excluded_effects)
				{
					if (component.HasEffect(effect_id))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008B20 RID: 35616 RVA: 0x00315BDC File Offset: 0x00313DDC
		private bool HasMinExposurePeriodElapsed(string germ_id)
		{
			float num;
			this.lastExposureTime.TryGetValue(germ_id, out num);
			return num == 0f || GameClock.Instance.GetTime() - num > 540f;
		}

		// Token: 0x06008B21 RID: 35617 RVA: 0x00315C1C File Offset: 0x00313E1C
		private void RefreshStatusItems()
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				Guid guid;
				this.contactStatusItemHandles.TryGetValue(exposureType.germ_id, out guid);
				Guid guid2;
				this.statusItemHandles.TryGetValue(exposureType.germ_id, out guid2);
				GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
				if (guid2 == Guid.Empty && (exposureState == GermExposureMonitor.ExposureState.Exposed || exposureState == GermExposureMonitor.ExposureState.Contracted) && !string.IsNullOrEmpty(exposureType.sickness_id))
				{
					guid2 = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExposedToGerms, new GermExposureMonitor.ExposureStatusData
					{
						exposure_type = exposureType,
						owner = this
					});
				}
				else if (guid2 != Guid.Empty && exposureState != GermExposureMonitor.ExposureState.Exposed && exposureState != GermExposureMonitor.ExposureState.Contracted)
				{
					guid2 = base.GetComponent<KSelectable>().RemoveStatusItem(guid2, false);
				}
				this.statusItemHandles[exposureType.germ_id] = guid2;
				if (guid == Guid.Empty && exposureState == GermExposureMonitor.ExposureState.Contact)
				{
					if (!string.IsNullOrEmpty(exposureType.sickness_id))
					{
						guid = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ContactWithGerms, new GermExposureMonitor.ExposureStatusData
						{
							exposure_type = exposureType,
							owner = this
						});
					}
				}
				else if (guid != Guid.Empty && exposureState != GermExposureMonitor.ExposureState.Contact)
				{
					guid = base.GetComponent<KSelectable>().RemoveStatusItem(guid, false);
				}
				this.contactStatusItemHandles[exposureType.germ_id] = guid;
			}
		}

		// Token: 0x06008B22 RID: 35618 RVA: 0x00315D8F File Offset: 0x00313F8F
		private void OnNightTime(object data)
		{
			this.UpdateReports();
		}

		// Token: 0x06008B23 RID: 35619 RVA: 0x00315D98 File Offset: 0x00313F98
		private void UpdateReports()
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseStatus, (float)this.primaryElement.DiseaseCount, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.GERMS, "{0}", base.master.name), base.master.gameObject.GetProperName());
		}

		// Token: 0x06008B24 RID: 35620 RVA: 0x00315DEC File Offset: 0x00313FEC
		public void InfectImmediately(ExposureType exposure_type)
		{
			if (exposure_type.infection_effect != null)
			{
				base.master.GetComponent<Effects>().Add(exposure_type.infection_effect, true);
			}
			if (exposure_type.sickness_id != null)
			{
				string lastDiseaseSource = this.GetLastDiseaseSource(exposure_type.germ_id);
				SicknessExposureInfo exposure_info = new SicknessExposureInfo(exposure_type.sickness_id, lastDiseaseSource);
				this.sicknesses.Infect(exposure_info);
			}
		}

		// Token: 0x06008B25 RID: 35621 RVA: 0x00315E48 File Offset: 0x00314048
		public void OnSleepFinished()
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (!exposureType.infect_immediately && exposureType.sickness_id != null)
				{
					GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
					if (exposureState == GermExposureMonitor.ExposureState.Exposed)
					{
						this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.None);
					}
					if (exposureState == GermExposureMonitor.ExposureState.Contracted)
					{
						this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Sick);
						string lastDiseaseSource = this.GetLastDiseaseSource(exposureType.germ_id);
						SicknessExposureInfo exposure_info = new SicknessExposureInfo(exposureType.sickness_id, lastDiseaseSource);
						this.sicknesses.Infect(exposure_info);
					}
					this.SetExposureTier(exposureType.germ_id, 0f);
				}
			}
		}

		// Token: 0x06008B26 RID: 35622 RVA: 0x00315EE8 File Offset: 0x003140E8
		public string GetLastDiseaseSource(string id)
		{
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			string result;
			if (this.lastDiseaseSources.TryGetValue(id, out diseaseSourceInfo))
			{
				switch (diseaseSourceInfo.vector)
				{
				case Sickness.InfectionVector.Contact:
					result = DUPLICANTS.DISEASES.INFECTIONSOURCES.SKIN;
					break;
				case Sickness.InfectionVector.Digestion:
					result = string.Format(DUPLICANTS.DISEASES.INFECTIONSOURCES.FOOD, diseaseSourceInfo.sourceObject.ProperName());
					break;
				case Sickness.InfectionVector.Inhalation:
					result = string.Format(DUPLICANTS.DISEASES.INFECTIONSOURCES.AIR, diseaseSourceInfo.sourceObject.ProperName());
					break;
				default:
					result = DUPLICANTS.DISEASES.INFECTIONSOURCES.UNKNOWN;
					break;
				}
			}
			else
			{
				result = DUPLICANTS.DISEASES.INFECTIONSOURCES.UNKNOWN;
			}
			return result;
		}

		// Token: 0x06008B27 RID: 35623 RVA: 0x00315F84 File Offset: 0x00314184
		public Vector3 GetLastExposurePosition(string germ_id)
		{
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			if (this.lastDiseaseSources.TryGetValue(germ_id, out diseaseSourceInfo))
			{
				return diseaseSourceInfo.position;
			}
			return base.transform.GetPosition();
		}

		// Token: 0x06008B28 RID: 35624 RVA: 0x00315FB8 File Offset: 0x003141B8
		public float GetExposureWeight(string id)
		{
			float exposureTier = this.GetExposureTier(id);
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			if (this.lastDiseaseSources.TryGetValue(id, out diseaseSourceInfo))
			{
				return diseaseSourceInfo.factor * exposureTier;
			}
			return 0f;
		}

		// Token: 0x04006BED RID: 27629
		[Serialize]
		public Dictionary<HashedString, GermExposureMonitor.Instance.DiseaseSourceInfo> lastDiseaseSources;

		// Token: 0x04006BEE RID: 27630
		[Serialize]
		public Dictionary<HashedString, float> lastExposureTime;

		// Token: 0x04006BEF RID: 27631
		private Dictionary<HashedString, GermExposureMonitor.Instance.InhaleTickInfo> inhaleExposureTick;

		// Token: 0x04006BF0 RID: 27632
		private Sicknesses sicknesses;

		// Token: 0x04006BF1 RID: 27633
		private PrimaryElement primaryElement;

		// Token: 0x04006BF2 RID: 27634
		private Traits traits;

		// Token: 0x04006BF3 RID: 27635
		[Serialize]
		private Dictionary<string, GermExposureMonitor.ExposureState> exposureStates = new Dictionary<string, GermExposureMonitor.ExposureState>();

		// Token: 0x04006BF4 RID: 27636
		[Serialize]
		private Dictionary<string, float> exposureTiers = new Dictionary<string, float>();

		// Token: 0x04006BF5 RID: 27637
		private Dictionary<string, Guid> statusItemHandles = new Dictionary<string, Guid>();

		// Token: 0x04006BF6 RID: 27638
		private Dictionary<string, Guid> contactStatusItemHandles = new Dictionary<string, Guid>();

		// Token: 0x020021A6 RID: 8614
		[Serializable]
		public class DiseaseSourceInfo
		{
			// Token: 0x0600AB2D RID: 43821 RVA: 0x003750DD File Offset: 0x003732DD
			public DiseaseSourceInfo(Tag sourceObject, Sickness.InfectionVector vector, float factor, Vector3 position)
			{
				this.sourceObject = sourceObject;
				this.vector = vector;
				this.factor = factor;
				this.position = position;
			}

			// Token: 0x040096C3 RID: 38595
			public Tag sourceObject;

			// Token: 0x040096C4 RID: 38596
			public Sickness.InfectionVector vector;

			// Token: 0x040096C5 RID: 38597
			public float factor;

			// Token: 0x040096C6 RID: 38598
			public Vector3 position;
		}

		// Token: 0x020021A7 RID: 8615
		public class InhaleTickInfo
		{
			// Token: 0x040096C7 RID: 38599
			public bool inhaled;

			// Token: 0x040096C8 RID: 38600
			public int ticks;
		}
	}
}
