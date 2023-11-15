using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class SpecialCargoBayClusterReceptacle : SingleEntityReceptacle, IBaggedStateAnimationInstructions
{
	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600111B RID: 4379 RVA: 0x0005C2B9 File Offset: 0x0005A4B9
	public bool IsRocketOnGround
	{
		get
		{
			return base.gameObject.HasTag(GameTags.RocketOnGround);
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x0600111C RID: 4380 RVA: 0x0005C2CB File Offset: 0x0005A4CB
	public bool IsRocketInSpace
	{
		get
		{
			return base.gameObject.HasTag(GameTags.RocketInSpace);
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x0600111D RID: 4381 RVA: 0x0005C2DD File Offset: 0x0005A4DD
	private bool isDoorOpen
	{
		get
		{
			return this.capsule.sm.IsDoorOpen.Get(this.capsule);
		}
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x0005C2FC File Offset: 0x0005A4FC
	protected override void OnSpawn()
	{
		this.capsule = base.gameObject.GetSMI<SpecialCargoBayCluster.Instance>();
		this.SetupLootSymbolObject();
		base.OnSpawn();
		this.SetTrappedCritterAnimations(base.Occupant);
		base.Subscribe(-1697596308, new Action<object>(this.OnCritterStorageChanged));
		base.Subscribe<SpecialCargoBayClusterReceptacle>(-887025858, SpecialCargoBayClusterReceptacle.OnRocketLandedDelegate);
		base.Subscribe<SpecialCargoBayClusterReceptacle>(-1447108533, SpecialCargoBayClusterReceptacle.OnCargoBayRelocatedDelegate);
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x0005C384 File Offset: 0x0005A584
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			SpecialCargoBayClusterReceptacle component = gameObject.GetComponent<SpecialCargoBayClusterReceptacle>();
			if (component != null)
			{
				Tag tag = (component.Occupant != null) ? component.Occupant.PrefabID() : component.requestedEntityTag;
				if (base.Occupant != null && base.Occupant.PrefabID() != tag)
				{
					this.ClearOccupant();
				}
				if (tag != this.requestedEntityTag && this.fetchChore != null)
				{
					base.CancelActiveRequest();
				}
				if (tag != Tag.Invalid)
				{
					this.CreateOrder(tag, component.requestedEntityAdditionalFilterTag);
				}
			}
		}
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x0005C434 File Offset: 0x0005A634
	public void SetupLootSymbolObject()
	{
		Vector3 storePositionForDrops = this.capsule.GetStorePositionForDrops();
		storePositionForDrops.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
		GameObject gameObject = new GameObject();
		gameObject.name = "lootSymbol";
		gameObject.transform.SetParent(base.transform, true);
		gameObject.SetActive(false);
		gameObject.transform.SetPosition(storePositionForDrops);
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kbatchedAnimTracker.symbol = "loot";
		kbatchedAnimTracker.forceAlwaysAlive = true;
		kbatchedAnimTracker.matchParentOffset = true;
		this.lootKBAC = gameObject.AddComponent<KBatchedAnimController>();
		this.lootKBAC.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mushbar_kanim")
		};
		this.lootKBAC.initialAnim = "object";
		this.buildingAnimCtr.SetSymbolVisiblity("loot", false);
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x0005C50C File Offset: 0x0005A70C
	protected override void ClearOccupant()
	{
		this.LastCritterDead = null;
		if (base.occupyingObject != null)
		{
			this.UnsubscribeFromOccupant();
		}
		this.originWorldID = -1;
		base.occupyingObject = null;
		base.UpdateActive();
		this.UpdateStatusItem();
		if (!this.isDoorOpen)
		{
			if (this.IsRocketOnGround)
			{
				this.SetLootSymbolImage(Tag.Invalid);
				this.capsule.OpenDoor();
			}
		}
		else
		{
			this.capsule.DropInventory();
		}
		base.Trigger(-731304873, base.occupyingObject);
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x0005C592 File Offset: 0x0005A792
	private void OnCritterStorageChanged(object obj)
	{
		if (obj != null && this.storage.MassStored() == 0f && base.Occupant != null && base.Occupant == (GameObject)obj)
		{
			this.ClearOccupant();
		}
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x0005C5D0 File Offset: 0x0005A7D0
	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		base.Subscribe(base.Occupant, -1582839653, new Action<object>(this.OnTrappedCritterTagsChanged));
		base.Subscribe(base.Occupant, 395373363, new Action<object>(this.OnCreatureInStorageDied));
		base.Subscribe(base.Occupant, 663420073, new Action<object>(this.OnBabyInStorageGrows));
		this.SetupCritterTracker();
		for (int i = 0; i < SpecialCargoBayClusterReceptacle.tagsForCritter.Length; i++)
		{
			Tag tag = SpecialCargoBayClusterReceptacle.tagsForCritter[i];
			base.Occupant.AddTag(tag);
		}
		base.Occupant.GetComponent<Health>().UpdateHealthBar();
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x0005C680 File Offset: 0x0005A880
	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		base.Unsubscribe(base.Occupant, -1582839653, new Action<object>(this.OnTrappedCritterTagsChanged));
		base.Unsubscribe(base.Occupant, 395373363, new Action<object>(this.OnCreatureInStorageDied));
		base.Unsubscribe(base.Occupant, 663420073, new Action<object>(this.OnBabyInStorageGrows));
		this.RemoveCritterTracker();
		if (base.Occupant != null)
		{
			for (int i = 0; i < SpecialCargoBayClusterReceptacle.tagsForCritter.Length; i++)
			{
				Tag tag = SpecialCargoBayClusterReceptacle.tagsForCritter[i];
				base.occupyingObject.RemoveTag(tag);
			}
			base.occupyingObject.GetComponent<Health>().UpdateHealthBar();
		}
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x0005C738 File Offset: 0x0005A938
	public void SetLootSymbolImage(Tag productTag)
	{
		bool flag = productTag != Tag.Invalid;
		this.lootKBAC.gameObject.SetActive(flag);
		if (flag)
		{
			GameObject prefab = Assets.GetPrefab(productTag.ToString());
			this.lootKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
			this.lootKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x0005C7B3 File Offset: 0x0005A9B3
	private void SetupCritterTracker()
	{
		if (base.Occupant != null)
		{
			KBatchedAnimTracker kbatchedAnimTracker = base.Occupant.AddOrGet<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = "critter";
			kbatchedAnimTracker.forceAlwaysAlive = true;
			kbatchedAnimTracker.matchParentOffset = true;
		}
	}

	// Token: 0x06001127 RID: 4391 RVA: 0x0005C7EC File Offset: 0x0005A9EC
	private void RemoveCritterTracker()
	{
		if (base.Occupant != null)
		{
			KBatchedAnimTracker component = base.Occupant.GetComponent<KBatchedAnimTracker>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	// Token: 0x06001128 RID: 4392 RVA: 0x0005C822 File Offset: 0x0005AA22
	protected override void ConfigureOccupyingObject(GameObject source)
	{
		this.originWorldID = source.GetMyWorldId();
		source.GetComponent<Baggable>().SetWrangled();
		this.SetTrappedCritterAnimations(source);
	}

	// Token: 0x06001129 RID: 4393 RVA: 0x0005C844 File Offset: 0x0005AA44
	private void OnBabyInStorageGrows(object obj)
	{
		int num = this.originWorldID;
		this.UnsubscribeFromOccupant();
		GameObject gameObject = (GameObject)obj;
		this.storage.Store(gameObject, false, false, true, false);
		base.occupyingObject = gameObject;
		this.ConfigureOccupyingObject(gameObject);
		this.originWorldID = num;
		this.PositionOccupyingObject();
		this.SubscribeToOccupant();
		this.UpdateStatusItem();
	}

	// Token: 0x0600112A RID: 4394 RVA: 0x0005C8A0 File Offset: 0x0005AAA0
	private void OnTrappedCritterTagsChanged(object obj)
	{
		if (base.Occupant != null && base.Occupant.HasTag(GameTags.Creatures.Die) && this.LastCritterDead != base.Occupant)
		{
			this.capsule.PlayDeathCloud();
			this.LastCritterDead = base.Occupant;
			this.RemoveCritterTracker();
			base.Occupant.GetComponent<KBatchedAnimController>().SetVisiblity(false);
			Butcherable component = base.Occupant.GetComponent<Butcherable>();
			if (component != null && component.drops != null && component.drops.Length != 0)
			{
				this.SetLootSymbolImage(component.drops[0]);
			}
			else
			{
				this.SetLootSymbolImage(Tag.Invalid);
			}
			if (this.IsRocketInSpace)
			{
				DeathStates.Instance smi = base.Occupant.GetSMI<DeathStates.Instance>();
				smi.GoTo(smi.sm.pst);
			}
		}
	}

	// Token: 0x0600112B RID: 4395 RVA: 0x0005C980 File Offset: 0x0005AB80
	private void OnCreatureInStorageDied(object drops_obj)
	{
		GameObject[] array = drops_obj as GameObject[];
		if (array != null)
		{
			foreach (GameObject go in array)
			{
				this.sideProductStorage.Store(go, false, false, true, false);
			}
		}
	}

	// Token: 0x0600112C RID: 4396 RVA: 0x0005C9BA File Offset: 0x0005ABBA
	private void SetTrappedCritterAnimations(GameObject critter)
	{
		if (critter != null)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			component.FlipX = false;
			component.Play("rocket_biological", KAnim.PlayMode.Loop, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
		}
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x0005C9FA File Offset: 0x0005ABFA
	protected override void PositionOccupyingObject()
	{
		if (base.Occupant != null)
		{
			base.Occupant.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
			this.SetupCritterTracker();
		}
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x0005CA24 File Offset: 0x0005AC24
	protected override void UpdateStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		bool flag = base.Occupant != null;
		if (component != null)
		{
			if (flag)
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored, this);
			}
			else
			{
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored, false);
			}
		}
		base.UpdateStatusItem();
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x0005CA87 File Offset: 0x0005AC87
	private void OnCargoBayRelocated(object data)
	{
		if (base.Occupant != null)
		{
			KBatchedAnimController component = base.Occupant.GetComponent<KBatchedAnimController>();
			component.enabled = false;
			component.enabled = true;
		}
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x0005CAB0 File Offset: 0x0005ACB0
	private void OnRocketLanded(object data)
	{
		if (base.Occupant != null)
		{
			ClusterManager.Instance.MigrateCritter(base.Occupant, base.gameObject.GetMyWorldId(), this.originWorldID);
			this.originWorldID = base.Occupant.GetMyWorldId();
		}
		if (base.Occupant == null && !this.isDoorOpen)
		{
			this.SetLootSymbolImage(Tag.Invalid);
			if (this.sideProductStorage.MassStored() > 0f)
			{
				this.capsule.OpenDoor();
			}
		}
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x0005CB3B File Offset: 0x0005AD3B
	public string GetBaggedAnimationName()
	{
		return "rocket_biological";
	}

	// Token: 0x04000952 RID: 2386
	public const string TRAPPED_CRITTER_ANIM_NAME = "rocket_biological";

	// Token: 0x04000953 RID: 2387
	[MyCmpReq]
	private SymbolOverrideController symbolOverrideComponent;

	// Token: 0x04000954 RID: 2388
	[MyCmpGet]
	private KBatchedAnimController buildingAnimCtr;

	// Token: 0x04000955 RID: 2389
	private KBatchedAnimController lootKBAC;

	// Token: 0x04000956 RID: 2390
	public Storage sideProductStorage;

	// Token: 0x04000957 RID: 2391
	private SpecialCargoBayCluster.Instance capsule;

	// Token: 0x04000958 RID: 2392
	private GameObject LastCritterDead;

	// Token: 0x04000959 RID: 2393
	[Serialize]
	private int originWorldID;

	// Token: 0x0400095A RID: 2394
	private static Tag[] tagsForCritter = new Tag[]
	{
		GameTags.Creatures.TrappedInCargoBay,
		GameTags.Creatures.PausedHunger,
		GameTags.Creatures.PausedReproduction,
		GameTags.Creatures.PreventGrowAnimation,
		GameTags.HideHealthBar,
		GameTags.PreventDeadAnimation
	};

	// Token: 0x0400095B RID: 2395
	private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>(delegate(SpecialCargoBayClusterReceptacle component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x0400095C RID: 2396
	private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnCargoBayRelocatedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>(delegate(SpecialCargoBayClusterReceptacle component, object data)
	{
		component.OnCargoBayRelocated(data);
	});
}
