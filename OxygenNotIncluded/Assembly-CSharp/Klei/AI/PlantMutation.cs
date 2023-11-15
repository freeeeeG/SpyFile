using System;
using System.Collections.Generic;
using System.Text;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000E0F RID: 3599
	public class PlantMutation : Modifier
	{
		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06006E68 RID: 28264 RVA: 0x002B727E File Offset: 0x002B547E
		public List<string> AdditionalSoundEvents
		{
			get
			{
				return this.additionalSoundEvents;
			}
		}

		// Token: 0x06006E69 RID: 28265 RVA: 0x002B7288 File Offset: 0x002B5488
		public PlantMutation(string id, string name, string desc) : base(id, name, desc)
		{
		}

		// Token: 0x06006E6A RID: 28266 RVA: 0x002B730C File Offset: 0x002B550C
		public void ApplyTo(MutantPlant target)
		{
			this.ApplyFunctionalTo(target);
			if (!target.HasTag(GameTags.Seed) && !target.HasTag(GameTags.CropSeed) && !target.HasTag(GameTags.Compostable))
			{
				this.ApplyVisualTo(target);
			}
		}

		// Token: 0x06006E6B RID: 28267 RVA: 0x002B7344 File Offset: 0x002B5544
		private void ApplyFunctionalTo(MutantPlant target)
		{
			SeedProducer component = target.GetComponent<SeedProducer>();
			if (component != null && component.seedInfo.productionType == SeedProducer.ProductionType.Harvest)
			{
				component.Configure(component.seedInfo.seedId, SeedProducer.ProductionType.Sterile, 0);
			}
			if (this.bonusCropID.IsValid)
			{
				target.Subscribe(-1072826864, new Action<object>(this.OnHarvestBonusCrop));
			}
			if (!this.forcePrefersDarkness)
			{
				if (this.SelfModifiers.Find((AttributeModifier m) => m.AttributeId == Db.Get().PlantAttributes.MinLightLux.Id) == null)
				{
					goto IL_F0;
				}
			}
			IlluminationVulnerable illuminationVulnerable = target.GetComponent<IlluminationVulnerable>();
			if (illuminationVulnerable == null)
			{
				illuminationVulnerable = target.gameObject.AddComponent<IlluminationVulnerable>();
			}
			if (this.forcePrefersDarkness)
			{
				if (illuminationVulnerable != null)
				{
					illuminationVulnerable.SetPrefersDarkness(true);
				}
			}
			else
			{
				if (illuminationVulnerable != null)
				{
					illuminationVulnerable.SetPrefersDarkness(false);
				}
				target.GetComponent<Modifiers>().attributes.Add(Db.Get().PlantAttributes.MinLightLux);
			}
			IL_F0:
			byte b = this.droppedDiseaseID;
			if (this.harvestDiseaseID != 255)
			{
				target.Subscribe(35625290, new Action<object>(this.OnCropSpawnedAddDisease));
			}
			bool isValid = this.ensureIrrigationInfo.tag.IsValid;
			Attributes attributes = target.GetAttributes();
			this.AddTo(attributes);
		}

		// Token: 0x06006E6C RID: 28268 RVA: 0x002B7494 File Offset: 0x002B5694
		private void ApplyVisualTo(MutantPlant target)
		{
			KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
			if (this.symbolOverrideInfo != null && this.symbolOverrideInfo.Count > 0)
			{
				SymbolOverrideController component2 = target.GetComponent<SymbolOverrideController>();
				if (component2 != null)
				{
					foreach (PlantMutation.SymbolOverrideInfo symbolOverrideInfo in this.symbolOverrideInfo)
					{
						KAnim.Build.Symbol symbol = Assets.GetAnim(symbolOverrideInfo.sourceAnim).GetData().build.GetSymbol(symbolOverrideInfo.sourceSymbol);
						component2.AddSymbolOverride(symbolOverrideInfo.targetSymbolName, symbol, 0);
					}
				}
			}
			if (this.bGFXAnim != null)
			{
				PlantMutation.CreateFXObject(target, this.bGFXAnim, "_BGFX", 0.1f);
			}
			if (this.fGFXAnim != null)
			{
				PlantMutation.CreateFXObject(target, this.fGFXAnim, "_FGFX", -0.1f);
			}
			if (this.plantTint != Color.white)
			{
				component.TintColour = this.plantTint;
			}
			if (this.symbolTints.Count > 0)
			{
				for (int i = 0; i < this.symbolTints.Count; i++)
				{
					component.SetSymbolTint(this.symbolTintTargets[i], this.symbolTints[i]);
				}
			}
			if (this.symbolScales.Count > 0)
			{
				for (int j = 0; j < this.symbolScales.Count; j++)
				{
					component.SetSymbolScale(this.symbolScaleTargets[j], this.symbolScales[j]);
				}
			}
			if (this.additionalSoundEvents.Count > 0)
			{
				for (int k = 0; k < this.additionalSoundEvents.Count; k++)
				{
				}
			}
		}

		// Token: 0x06006E6D RID: 28269 RVA: 0x002B7678 File Offset: 0x002B5878
		private static void CreateFXObject(MutantPlant target, string anim, string nameSuffix, float offset)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab(SimpleFXConfig.ID));
			gameObject.name = target.name + nameSuffix;
			gameObject.transform.parent = target.transform;
			gameObject.AddComponent<LoopingSounds>();
			gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(gameObject.name);
			Extents extents = target.GetComponent<OccupyArea>().GetExtents();
			Vector3 position = target.transform.GetPosition();
			position.x = (float)extents.x + (float)extents.width / 2f;
			position.y = (float)extents.y + (float)extents.height / 2f;
			position.z += offset;
			gameObject.transform.SetPosition(position);
			KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
			component.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim(anim)
			};
			component.initialAnim = "idle";
			component.initialMode = KAnim.PlayMode.Loop;
			component.randomiseLoopedOffset = true;
			component.fgLayer = Grid.SceneLayer.NoLayer;
			if (target.HasTag(GameTags.Hanging))
			{
				component.Rotation = 180f;
			}
			gameObject.SetActive(true);
		}

		// Token: 0x06006E6E RID: 28270 RVA: 0x002B77A7 File Offset: 0x002B59A7
		private void OnHarvestBonusCrop(object data)
		{
			((Crop)data).SpawnSomeFruit(this.bonusCropID, this.bonusCropAmount);
		}

		// Token: 0x06006E6F RID: 28271 RVA: 0x002B77C0 File Offset: 0x002B59C0
		private void OnCropSpawnedAddDisease(object data)
		{
			((GameObject)data).GetComponent<PrimaryElement>().AddDisease(this.harvestDiseaseID, this.harvestDiseaseAmount, this.Name);
		}

		// Token: 0x06006E70 RID: 28272 RVA: 0x002B77E4 File Offset: 0x002B59E4
		public string GetTooltip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.desc);
			foreach (AttributeModifier attributeModifier in this.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
				if (attribute == null)
				{
					attribute = Db.Get().PlantAttributes.Get(attributeModifier.AttributeId);
				}
				if (attribute.ShowInUI != Attribute.Display.Never)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(string.Format(DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, attribute.Name, attributeModifier.GetFormattedString()));
				}
			}
			if (this.bonusCropID != null)
			{
				string newValue;
				if (GameTags.DisplayAsCalories.Contains(this.bonusCropID))
				{
					EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.bonusCropID.Name);
					DebugUtil.Assert(foodInfo != null, "Eeh? Trying to spawn a bonus crop that is caloric but isn't a food??", this.bonusCropID.ToString());
					newValue = GameUtil.GetFormattedCalories(this.bonusCropAmount * foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true);
				}
				else if (GameTags.DisplayAsUnits.Contains(this.bonusCropID))
				{
					newValue = GameUtil.GetFormattedUnits(this.bonusCropAmount, GameUtil.TimeSlice.None, false, "");
				}
				else
				{
					newValue = GameUtil.GetFormattedMass(this.bonusCropAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
				}
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(CREATURES.PLANT_MUTATIONS.BONUS_CROP_FMT.Replace("{Crop}", this.bonusCropID.ProperName()).Replace("{Amount}", newValue));
			}
			if (this.droppedDiseaseID != 255)
			{
				if (this.droppedDiseaseOnGrowAmount > 0)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.droppedDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.droppedDiseaseOnGrowAmount, GameUtil.TimeSlice.None)));
				}
				if (this.droppedDiseaseContinuousAmount > 0)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.droppedDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.droppedDiseaseContinuousAmount, GameUtil.TimeSlice.PerSecond)));
				}
			}
			if (this.harvestDiseaseID != 255)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)));
			}
			if (this.forcePrefersDarkness)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS);
			}
			if (this.forceSelfHarvestOnGrown)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.AUTO_SELF_HARVEST);
			}
			if (this.ensureIrrigationInfo.tag.IsValid)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(string.Format(UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, this.ensureIrrigationInfo.tag.ProperName(), GameUtil.GetFormattedMass(-this.ensureIrrigationInfo.massConsumptionRate, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), true));
			}
			if (!this.originalMutation)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.GAMEOBJECTEFFECTS.MUTANT_STERILE);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006E71 RID: 28273 RVA: 0x002B7B94 File Offset: 0x002B5D94
		public void GetDescriptors(ref List<Descriptor> descriptors, GameObject go)
		{
			if (this.harvestDiseaseID != 255)
			{
				descriptors.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			}
			if (this.forceSelfHarvestOnGrown)
			{
				descriptors.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.AUTO_SELF_HARVEST, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.AUTO_SELF_HARVEST, Descriptor.DescriptorType.Effect, false));
			}
		}

		// Token: 0x06006E72 RID: 28274 RVA: 0x002B7C48 File Offset: 0x002B5E48
		public PlantMutation Original()
		{
			this.originalMutation = true;
			return this;
		}

		// Token: 0x06006E73 RID: 28275 RVA: 0x002B7C52 File Offset: 0x002B5E52
		public PlantMutation RequiredPrefabID(string requiredID)
		{
			this.requiredPrefabIDs.Add(requiredID);
			return this;
		}

		// Token: 0x06006E74 RID: 28276 RVA: 0x002B7C61 File Offset: 0x002B5E61
		public PlantMutation RestrictPrefabID(string restrictedID)
		{
			this.restrictedPrefabIDs.Add(restrictedID);
			return this;
		}

		// Token: 0x06006E75 RID: 28277 RVA: 0x002B7C70 File Offset: 0x002B5E70
		public PlantMutation AttributeModifier(Attribute attribute, float amount, bool multiplier = false)
		{
			DebugUtil.Assert(!this.forcePrefersDarkness || attribute != Db.Get().PlantAttributes.MinLightLux, "A plant mutation has both darkness and light defined!", this.Id);
			base.Add(new AttributeModifier(attribute.Id, amount, this.Name, multiplier, false, true));
			return this;
		}

		// Token: 0x06006E76 RID: 28278 RVA: 0x002B7CC9 File Offset: 0x002B5EC9
		public PlantMutation BonusCrop(Tag cropPrefabID, float bonucCropAmount)
		{
			this.bonusCropID = cropPrefabID;
			this.bonusCropAmount = bonucCropAmount;
			return this;
		}

		// Token: 0x06006E77 RID: 28279 RVA: 0x002B7CDA File Offset: 0x002B5EDA
		public PlantMutation DiseaseDropper(byte diseaseID, int onGrowAmount, int continuousAmount)
		{
			this.droppedDiseaseID = diseaseID;
			this.droppedDiseaseOnGrowAmount = onGrowAmount;
			this.droppedDiseaseContinuousAmount = continuousAmount;
			return this;
		}

		// Token: 0x06006E78 RID: 28280 RVA: 0x002B7CF2 File Offset: 0x002B5EF2
		public PlantMutation AddDiseaseToHarvest(byte diseaseID, int amount)
		{
			this.harvestDiseaseID = diseaseID;
			this.harvestDiseaseAmount = amount;
			return this;
		}

		// Token: 0x06006E79 RID: 28281 RVA: 0x002B7D04 File Offset: 0x002B5F04
		public PlantMutation ForcePrefersDarkness()
		{
			DebugUtil.Assert(this.SelfModifiers.Find((AttributeModifier m) => m.AttributeId == Db.Get().PlantAttributes.MinLightLux.Id) == null, "A plant mutation has both darkness and light defined!", this.Id);
			this.forcePrefersDarkness = true;
			return this;
		}

		// Token: 0x06006E7A RID: 28282 RVA: 0x002B7D56 File Offset: 0x002B5F56
		public PlantMutation ForceSelfHarvestOnGrown()
		{
			this.forceSelfHarvestOnGrown = true;
			this.AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute, -0.999999f, true);
			return this;
		}

		// Token: 0x06006E7B RID: 28283 RVA: 0x002B7D81 File Offset: 0x002B5F81
		public PlantMutation EnsureIrrigated(PlantElementAbsorber.ConsumeInfo consumeInfo)
		{
			this.ensureIrrigationInfo = consumeInfo;
			return this;
		}

		// Token: 0x06006E7C RID: 28284 RVA: 0x002B7D8C File Offset: 0x002B5F8C
		public PlantMutation VisualTint(float r, float g, float b)
		{
			global::Debug.Assert(Mathf.Sign(r) == Mathf.Sign(g) && Mathf.Sign(r) == Mathf.Sign(b), "Vales for tints must be all positive or all negative for the shader to work correctly!");
			if (r < 0f)
			{
				this.plantTint = Color.white + new Color(r, g, b, 0f);
			}
			else
			{
				this.plantTint = new Color(r, g, b, 0f);
			}
			return this;
		}

		// Token: 0x06006E7D RID: 28285 RVA: 0x002B7E00 File Offset: 0x002B6000
		public PlantMutation VisualSymbolTint(string targetSymbolName, float r, float g, float b)
		{
			global::Debug.Assert(Mathf.Sign(r) == Mathf.Sign(g) && Mathf.Sign(r) == Mathf.Sign(b), "Vales for tints must be all positive or all negative for the shader to work correctly!");
			this.symbolTintTargets.Add(targetSymbolName);
			this.symbolTints.Add(Color.white + new Color(r, g, b, 0f));
			return this;
		}

		// Token: 0x06006E7E RID: 28286 RVA: 0x002B7E67 File Offset: 0x002B6067
		public PlantMutation VisualSymbolOverride(string targetSymbolName, string sourceAnim, string sourceSymbol)
		{
			if (this.symbolOverrideInfo == null)
			{
				this.symbolOverrideInfo = new List<PlantMutation.SymbolOverrideInfo>();
			}
			this.symbolOverrideInfo.Add(new PlantMutation.SymbolOverrideInfo
			{
				targetSymbolName = targetSymbolName,
				sourceAnim = sourceAnim,
				sourceSymbol = sourceSymbol
			});
			return this;
		}

		// Token: 0x06006E7F RID: 28287 RVA: 0x002B7EA2 File Offset: 0x002B60A2
		public PlantMutation VisualSymbolScale(string targetSymbolName, float scale)
		{
			this.symbolScaleTargets.Add(targetSymbolName);
			this.symbolScales.Add(scale);
			return this;
		}

		// Token: 0x06006E80 RID: 28288 RVA: 0x002B7EBD File Offset: 0x002B60BD
		public PlantMutation VisualBGFX(string animName)
		{
			this.bGFXAnim = animName;
			return this;
		}

		// Token: 0x06006E81 RID: 28289 RVA: 0x002B7EC7 File Offset: 0x002B60C7
		public PlantMutation VisualFGFX(string animName)
		{
			this.fGFXAnim = animName;
			return this;
		}

		// Token: 0x06006E82 RID: 28290 RVA: 0x002B7ED1 File Offset: 0x002B60D1
		public PlantMutation AddSoundEvent(string soundEventName)
		{
			this.additionalSoundEvents.Add(soundEventName);
			return this;
		}

		// Token: 0x040052A5 RID: 21157
		public string desc;

		// Token: 0x040052A6 RID: 21158
		public string animationSoundEvent;

		// Token: 0x040052A7 RID: 21159
		public bool originalMutation;

		// Token: 0x040052A8 RID: 21160
		public List<string> requiredPrefabIDs = new List<string>();

		// Token: 0x040052A9 RID: 21161
		public List<string> restrictedPrefabIDs = new List<string>();

		// Token: 0x040052AA RID: 21162
		private Tag bonusCropID;

		// Token: 0x040052AB RID: 21163
		private float bonusCropAmount;

		// Token: 0x040052AC RID: 21164
		private byte droppedDiseaseID = byte.MaxValue;

		// Token: 0x040052AD RID: 21165
		private int droppedDiseaseOnGrowAmount;

		// Token: 0x040052AE RID: 21166
		private int droppedDiseaseContinuousAmount;

		// Token: 0x040052AF RID: 21167
		private byte harvestDiseaseID = byte.MaxValue;

		// Token: 0x040052B0 RID: 21168
		private int harvestDiseaseAmount;

		// Token: 0x040052B1 RID: 21169
		private bool forcePrefersDarkness;

		// Token: 0x040052B2 RID: 21170
		private bool forceSelfHarvestOnGrown;

		// Token: 0x040052B3 RID: 21171
		private PlantElementAbsorber.ConsumeInfo ensureIrrigationInfo;

		// Token: 0x040052B4 RID: 21172
		private Color plantTint = Color.white;

		// Token: 0x040052B5 RID: 21173
		private List<string> symbolTintTargets = new List<string>();

		// Token: 0x040052B6 RID: 21174
		private List<Color> symbolTints = new List<Color>();

		// Token: 0x040052B7 RID: 21175
		private List<PlantMutation.SymbolOverrideInfo> symbolOverrideInfo;

		// Token: 0x040052B8 RID: 21176
		private List<string> symbolScaleTargets = new List<string>();

		// Token: 0x040052B9 RID: 21177
		private List<float> symbolScales = new List<float>();

		// Token: 0x040052BA RID: 21178
		private string bGFXAnim;

		// Token: 0x040052BB RID: 21179
		private string fGFXAnim;

		// Token: 0x040052BC RID: 21180
		private List<string> additionalSoundEvents = new List<string>();

		// Token: 0x02001F97 RID: 8087
		private class SymbolOverrideInfo
		{
			// Token: 0x04008EAE RID: 36526
			public string targetSymbolName;

			// Token: 0x04008EAF RID: 36527
			public string sourceAnim;

			// Token: 0x04008EB0 RID: 36528
			public string sourceSymbol;
		}
	}
}
