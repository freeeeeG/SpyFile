using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020008DE RID: 2270
[AddComponentMenu("KMonoBehaviour/scripts/BuddingTrunk")]
public class BuddingTrunk : KMonoBehaviour, ISim4000ms
{
	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x06004194 RID: 16788 RVA: 0x0016F134 File Offset: 0x0016D334
	public bool ExtraSeedAvailable
	{
		get
		{
			return this.hasExtraSeedAvailable;
		}
	}

	// Token: 0x06004195 RID: 16789 RVA: 0x0016F13C File Offset: 0x0016D33C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
		this.growingBranchesStatusItem = new StatusItem("GROWINGBRANCHES", "MISC", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		base.Subscribe<BuddingTrunk>(1119167081, BuddingTrunk.OnNewGameSpawnDelegate);
	}

	// Token: 0x06004196 RID: 16790 RVA: 0x0016F190 File Offset: 0x0016D390
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<BuddingTrunk>(-216549700, BuddingTrunk.OnUprootedDelegate);
		base.Subscribe<BuddingTrunk>(-750750377, BuddingTrunk.OnDrownedDelegate);
		base.Subscribe<BuddingTrunk>(-266953818, BuddingTrunk.OnHarvestDesignationChangedDelegate);
	}

	// Token: 0x06004197 RID: 16791 RVA: 0x0016F1CB File Offset: 0x0016D3CB
	protected override void OnCleanUp()
	{
		if (this.newGameSpawnRoutine != null)
		{
			base.StopCoroutine(this.newGameSpawnRoutine);
		}
		base.OnCleanUp();
	}

	// Token: 0x06004198 RID: 16792 RVA: 0x0016F1E8 File Offset: 0x0016D3E8
	private void OnNewGameSpawn(object data)
	{
		float percent = 1f;
		if ((double)UnityEngine.Random.value < 0.1)
		{
			percent = UnityEngine.Random.Range(0.75f, 0.99f);
		}
		else
		{
			this.newGameSpawnRoutine = base.StartCoroutine(this.NewGameSproutBudRoutine());
		}
		this.growing.OverrideMaturityLevel(percent);
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x0016F23C File Offset: 0x0016D43C
	private IEnumerator NewGameSproutBudRoutine()
	{
		int num;
		for (int i = 0; i < this.buds.Length; i = num + 1)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
			float growth_percentage = UnityEngine.Random.Range(0f, 1f);
			this.TrySpawnRandomBud(null, growth_percentage);
			num = i;
		}
		this.newGameSpawnRoutine = null;
		yield return SequenceUtil.WaitForNextFrame;
		yield break;
	}

	// Token: 0x0600419A RID: 16794 RVA: 0x0016F24C File Offset: 0x0016D44C
	public void Sim4000ms(float dt)
	{
		if (this.growing.IsGrown() && !this.wilting.IsWilting())
		{
			this.TrySpawnRandomBud(null, 0f);
			base.GetComponent<KSelectable>().AddStatusItem(this.growingBranchesStatusItem, null);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(this.growingBranchesStatusItem, false);
	}

	// Token: 0x0600419B RID: 16795 RVA: 0x0016F2A6 File Offset: 0x0016D4A6
	private void OnUprooted(object data = null)
	{
		this.YieldWood();
	}

	// Token: 0x0600419C RID: 16796 RVA: 0x0016F2B0 File Offset: 0x0016D4B0
	private void YieldWood()
	{
		foreach (Ref<HarvestDesignatable> @ref in this.buds)
		{
			HarvestDesignatable harvestDesignatable = (@ref != null) ? @ref.Get() : null;
			if (harvestDesignatable != null)
			{
				harvestDesignatable.Trigger(-216549700, null);
			}
		}
	}

	// Token: 0x0600419D RID: 16797 RVA: 0x0016F2F8 File Offset: 0x0016D4F8
	public float GetMaxBranchMaturity()
	{
		float result = 0f;
		this.GetMostMatureBranch(out result);
		return result;
	}

	// Token: 0x0600419E RID: 16798 RVA: 0x0016F318 File Offset: 0x0016D518
	public void ConsumeMass(float mass_to_consume)
	{
		float num;
		HarvestDesignatable mostMatureBranch = this.GetMostMatureBranch(out num);
		if (mostMatureBranch)
		{
			Growing component = mostMatureBranch.GetComponent<Growing>();
			if (component)
			{
				component.ConsumeMass(mass_to_consume);
			}
		}
	}

	// Token: 0x0600419F RID: 16799 RVA: 0x0016F34C File Offset: 0x0016D54C
	private HarvestDesignatable GetMostMatureBranch(out float max_maturity)
	{
		max_maturity = 0f;
		HarvestDesignatable result = null;
		foreach (Ref<HarvestDesignatable> @ref in this.buds)
		{
			HarvestDesignatable harvestDesignatable = (@ref != null) ? @ref.Get() : null;
			if (harvestDesignatable != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(harvestDesignatable);
				if (amountInstance != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					if (num > max_maturity)
					{
						max_maturity = num;
						result = harvestDesignatable;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060041A0 RID: 16800 RVA: 0x0016F3D0 File Offset: 0x0016D5D0
	public void TrySpawnRandomBud(object data = null, float growth_percentage = 0f)
	{
		if (this.uprooted.IsUprooted)
		{
			return;
		}
		BuddingTrunk.spawn_choices.Clear();
		int num = 0;
		for (int i = 0; i < this.buds.Length; i++)
		{
			int cell = Grid.PosToCell(this.GetBudPosition(i));
			if ((this.buds[i] == null || this.buds[i].Get() == null) && this.CanGrowInto(cell))
			{
				BuddingTrunk.spawn_choices.Add(i);
			}
			else if (this.buds[i] != null && this.buds[i].Get() != null)
			{
				num++;
			}
		}
		if (num >= this.maxBuds)
		{
			return;
		}
		BuddingTrunk.spawn_choices.Shuffle<int>();
		if (BuddingTrunk.spawn_choices.Count > 0)
		{
			int num2 = BuddingTrunk.spawn_choices[0];
			Vector3 budPosition = this.GetBudPosition(num2);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(this.budPrefabID), budPosition);
			gameObject.SetActive(true);
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null)
			{
				MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
				if (component2 != null)
				{
					component.CopyMutationsTo(component2);
					PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = component2.GetSubSpeciesInfo();
					PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(subSpeciesInfo, component2);
					PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(subSpeciesInfo.ID);
				}
			}
			gameObject.GetComponent<Growing>().OverrideMaturityLevel(growth_percentage);
			gameObject.GetComponent<TreeBud>().SetTrunkPosition(this, num2);
			HarvestDesignatable component3 = gameObject.GetComponent<HarvestDesignatable>();
			this.buds[num2] = new Ref<HarvestDesignatable>(component3);
			this.UpdateBudHarvestState(component3);
			this.TryRollNewSeed();
		}
	}

	// Token: 0x060041A1 RID: 16801 RVA: 0x0016F560 File Offset: 0x0016D760
	public void TryRollNewSeed()
	{
		if (!this.hasExtraSeedAvailable && UnityEngine.Random.Range(0, 100) < 5)
		{
			this.hasExtraSeedAvailable = true;
		}
	}

	// Token: 0x060041A2 RID: 16802 RVA: 0x0016F57C File Offset: 0x0016D77C
	public TreeBud GetBranchAtPosition(int idx)
	{
		if (this.buds[idx] == null)
		{
			return null;
		}
		HarvestDesignatable harvestDesignatable = this.buds[idx].Get();
		if (!(harvestDesignatable != null))
		{
			return null;
		}
		return harvestDesignatable.GetComponent<TreeBud>();
	}

	// Token: 0x060041A3 RID: 16803 RVA: 0x0016F5B4 File Offset: 0x0016D7B4
	public void ExtractExtraSeed()
	{
		if (!this.hasExtraSeedAvailable)
		{
			return;
		}
		this.hasExtraSeedAvailable = false;
		Vector3 position = base.transform.position;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		Util.KInstantiate(Assets.GetPrefab("ForestTreeSeed"), position).SetActive(true);
	}

	// Token: 0x060041A4 RID: 16804 RVA: 0x0016F608 File Offset: 0x0016D808
	private void UpdateBudHarvestState(HarvestDesignatable bud)
	{
		HarvestDesignatable component = base.GetComponent<HarvestDesignatable>();
		bud.SetHarvestWhenReady(component.HarvestWhenReady);
	}

	// Token: 0x060041A5 RID: 16805 RVA: 0x0016F628 File Offset: 0x0016D828
	public void OnBranchRemoved(int idx, TreeBud treeBud)
	{
		if (idx < 0 || idx >= this.buds.Length)
		{
			global::Debug.Assert(false, "invalid branch index " + idx.ToString());
		}
		HarvestDesignatable component = treeBud.GetComponent<HarvestDesignatable>();
		HarvestDesignatable harvestDesignatable = (this.buds[idx] != null) ? this.buds[idx].Get() : null;
		if (component != harvestDesignatable)
		{
			global::Debug.LogWarningFormat(base.gameObject, "OnBranchRemoved branch {0} does not match known branch {1}", new object[]
			{
				component,
				harvestDesignatable
			});
		}
		this.buds[idx] = null;
	}

	// Token: 0x060041A6 RID: 16806 RVA: 0x0016F6B0 File Offset: 0x0016D8B0
	private void UpdateAllBudsHarvestStatus(object data = null)
	{
		foreach (Ref<HarvestDesignatable> @ref in this.buds)
		{
			if (@ref != null)
			{
				HarvestDesignatable harvestDesignatable = @ref.Get();
				if (harvestDesignatable == null)
				{
					global::Debug.LogWarning("harvest_designatable was null");
				}
				else
				{
					this.UpdateBudHarvestState(harvestDesignatable);
				}
			}
		}
	}

	// Token: 0x060041A7 RID: 16807 RVA: 0x0016F6FC File Offset: 0x0016D8FC
	public bool CanGrowInto(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		if (Grid.Solid[cell])
		{
			return false;
		}
		int cell2 = Grid.CellAbove(cell);
		return Grid.IsValidCell(cell2) && !Grid.IsSubstantialLiquid(cell2, 0.35f) && !(Grid.Objects[cell, 1] != null) && !(Grid.Objects[cell, 5] != null) && !Grid.Foundation[cell];
	}

	// Token: 0x060041A8 RID: 16808 RVA: 0x0016F780 File Offset: 0x0016D980
	private Vector3 GetBudPosition(int idx)
	{
		Vector3 result = base.transform.position;
		switch (idx)
		{
		case 0:
			result = base.transform.position + Vector3.left;
			break;
		case 1:
			result = base.transform.position + Vector3.left + Vector3.up;
			break;
		case 2:
			result = base.transform.position + Vector3.left + Vector3.up + Vector3.up;
			break;
		case 3:
			result = base.transform.position + Vector3.up + Vector3.up;
			break;
		case 4:
			result = base.transform.position + Vector3.right + Vector3.up + Vector3.up;
			break;
		case 5:
			result = base.transform.position + Vector3.right + Vector3.up;
			break;
		case 6:
			result = base.transform.position + Vector3.right;
			break;
		}
		return result;
	}

	// Token: 0x04002AB5 RID: 10933
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04002AB6 RID: 10934
	[MyCmpReq]
	private WiltCondition wilting;

	// Token: 0x04002AB7 RID: 10935
	[MyCmpReq]
	private UprootedMonitor uprooted;

	// Token: 0x04002AB8 RID: 10936
	public string budPrefabID;

	// Token: 0x04002AB9 RID: 10937
	public int maxBuds = 5;

	// Token: 0x04002ABA RID: 10938
	[Serialize]
	private Ref<HarvestDesignatable>[] buds = new Ref<HarvestDesignatable>[7];

	// Token: 0x04002ABB RID: 10939
	private StatusItem growingBranchesStatusItem;

	// Token: 0x04002ABC RID: 10940
	[Serialize]
	private bool hasExtraSeedAvailable;

	// Token: 0x04002ABD RID: 10941
	private static readonly EventSystem.IntraObjectHandler<BuddingTrunk> OnNewGameSpawnDelegate = new EventSystem.IntraObjectHandler<BuddingTrunk>(delegate(BuddingTrunk component, object data)
	{
		component.OnNewGameSpawn(data);
	});

	// Token: 0x04002ABE RID: 10942
	private Coroutine newGameSpawnRoutine;

	// Token: 0x04002ABF RID: 10943
	private static readonly EventSystem.IntraObjectHandler<BuddingTrunk> OnUprootedDelegate = new EventSystem.IntraObjectHandler<BuddingTrunk>(delegate(BuddingTrunk component, object data)
	{
		component.OnUprooted(data);
	});

	// Token: 0x04002AC0 RID: 10944
	private static readonly EventSystem.IntraObjectHandler<BuddingTrunk> OnDrownedDelegate = new EventSystem.IntraObjectHandler<BuddingTrunk>(delegate(BuddingTrunk component, object data)
	{
		component.OnUprooted(data);
	});

	// Token: 0x04002AC1 RID: 10945
	private static readonly EventSystem.IntraObjectHandler<BuddingTrunk> OnHarvestDesignationChangedDelegate = new EventSystem.IntraObjectHandler<BuddingTrunk>(delegate(BuddingTrunk component, object data)
	{
		component.UpdateAllBudsHarvestStatus(data);
	});

	// Token: 0x04002AC2 RID: 10946
	private static List<int> spawn_choices = new List<int>();
}
