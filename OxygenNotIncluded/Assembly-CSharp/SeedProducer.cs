using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000737 RID: 1847
[AddComponentMenu("KMonoBehaviour/scripts/SeedProducer")]
public class SeedProducer : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x060032BA RID: 12986 RVA: 0x0010D5E5 File Offset: 0x0010B7E5
	public void Configure(string SeedID, SeedProducer.ProductionType productionType, int newSeedsProduced = 1)
	{
		this.seedInfo.seedId = SeedID;
		this.seedInfo.productionType = productionType;
		this.seedInfo.newSeedsProduced = newSeedsProduced;
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x0010D60B File Offset: 0x0010B80B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SeedProducer>(-216549700, SeedProducer.DropSeedDelegate);
		base.Subscribe<SeedProducer>(1623392196, SeedProducer.DropSeedDelegate);
		base.Subscribe<SeedProducer>(-1072826864, SeedProducer.CropPickedDelegate);
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x0010D648 File Offset: 0x0010B848
	private GameObject ProduceSeed(string seedId, int units = 1, bool canMutate = true)
	{
		if (seedId != null && units > 0)
		{
			Vector3 position = base.gameObject.transform.GetPosition() + new Vector3(0f, 0.5f, 0f);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(seedId)), position, Grid.SceneLayer.Ore, null, 0);
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null)
			{
				MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
				bool flag = false;
				if (canMutate && component2 != null && component2.IsOriginal)
				{
					flag = this.RollForMutation();
				}
				if (flag)
				{
					component2.Mutate();
				}
				else
				{
					component.CopyMutationsTo(component2);
				}
			}
			PrimaryElement component3 = base.gameObject.GetComponent<PrimaryElement>();
			PrimaryElement component4 = gameObject.GetComponent<PrimaryElement>();
			component4.Temperature = component3.Temperature;
			component4.Units = (float)units;
			base.Trigger(472291861, gameObject);
			gameObject.SetActive(true);
			string text = gameObject.GetProperName();
			if (component != null)
			{
				text = component.GetSubSpeciesInfo().GetNameWithMutations(text, component.IsIdentified, false);
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, text, gameObject.transform, 1.5f, false);
			return gameObject;
		}
		return null;
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x0010D77C File Offset: 0x0010B97C
	public void DropSeed(object data = null)
	{
		if (this.droppedSeedAlready)
		{
			return;
		}
		GameObject gameObject = this.ProduceSeed(this.seedInfo.seedId, 1, false);
		Uprootable component = base.GetComponent<Uprootable>();
		if (component != null && component.worker != null)
		{
			gameObject.Trigger(580035959, component.worker);
		}
		base.Trigger(-1736624145, gameObject);
		this.droppedSeedAlready = true;
	}

	// Token: 0x060032BE RID: 12990 RVA: 0x0010D7E8 File Offset: 0x0010B9E8
	public void CropDepleted(object data)
	{
		this.DropSeed(null);
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x0010D7F4 File Offset: 0x0010B9F4
	public void CropPicked(object data)
	{
		if (this.seedInfo.productionType == SeedProducer.ProductionType.Harvest)
		{
			Worker completed_by = base.GetComponent<Harvestable>().completed_by;
			float num = 0.1f;
			if (completed_by != null)
			{
				num += completed_by.GetComponent<AttributeConverters>().Get(Db.Get().AttributeConverters.SeedHarvestChance).Evaluate();
			}
			int num2 = (UnityEngine.Random.Range(0f, 1f) <= num) ? 1 : 0;
			if (num2 > 0)
			{
				this.ProduceSeed(this.seedInfo.seedId, num2, true).Trigger(580035959, completed_by);
			}
		}
	}

	// Token: 0x060032C0 RID: 12992 RVA: 0x0010D888 File Offset: 0x0010BA88
	public bool RollForMutation()
	{
		AttributeInstance attributeInstance = Db.Get().PlantAttributes.MaxRadiationThreshold.Lookup(this);
		int num = Grid.PosToCell(base.gameObject);
		float num2 = Mathf.Clamp(Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f, 0f, attributeInstance.GetTotalValue()) / attributeInstance.GetTotalValue() * 0.8f;
		return UnityEngine.Random.value < num2;
	}

	// Token: 0x060032C1 RID: 12993 RVA: 0x0010D8F8 File Offset: 0x0010BAF8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Assets.GetPrefab(new Tag(this.seedInfo.seedId)) != null;
		switch (this.seedInfo.productionType)
		{
		default:
			return null;
		case SeedProducer.ProductionType.DigOnly:
			return null;
		case SeedProducer.ProductionType.Harvest:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_HARVEST, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_HARVEST, Descriptor.DescriptorType.Lifecycle, true));
			list.Add(new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.BONUS_SEEDS, GameUtil.GetFormattedPercent(10f, GameUtil.TimeSlice.None)), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.BONUS_SEEDS, GameUtil.GetFormattedPercent(10f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			break;
		case SeedProducer.ProductionType.Fruit:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_FRUIT, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_DIG_ONLY, Descriptor.DescriptorType.Lifecycle, true));
			break;
		case SeedProducer.ProductionType.Sterile:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.MUTANT_STERILE, UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_STERILE, Descriptor.DescriptorType.Effect, false));
			break;
		}
		return list;
	}

	// Token: 0x04001E74 RID: 7796
	public SeedProducer.SeedInfo seedInfo;

	// Token: 0x04001E75 RID: 7797
	private bool droppedSeedAlready;

	// Token: 0x04001E76 RID: 7798
	private static readonly EventSystem.IntraObjectHandler<SeedProducer> DropSeedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>(delegate(SeedProducer component, object data)
	{
		component.DropSeed(data);
	});

	// Token: 0x04001E77 RID: 7799
	private static readonly EventSystem.IntraObjectHandler<SeedProducer> CropPickedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>(delegate(SeedProducer component, object data)
	{
		component.CropPicked(data);
	});

	// Token: 0x020014CE RID: 5326
	[Serializable]
	public struct SeedInfo
	{
		// Token: 0x04006673 RID: 26227
		public string seedId;

		// Token: 0x04006674 RID: 26228
		public SeedProducer.ProductionType productionType;

		// Token: 0x04006675 RID: 26229
		public int newSeedsProduced;
	}

	// Token: 0x020014CF RID: 5327
	public enum ProductionType
	{
		// Token: 0x04006677 RID: 26231
		Hidden,
		// Token: 0x04006678 RID: 26232
		DigOnly,
		// Token: 0x04006679 RID: 26233
		Harvest,
		// Token: 0x0400667A RID: 26234
		Fruit,
		// Token: 0x0400667B RID: 26235
		Sterile
	}
}
