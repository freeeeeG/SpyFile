using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200050E RID: 1294
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Storage")]
public class Storage : Workable, ISaveLoadableDetails, IGameObjectEffectDescriptor, IStorage
{
	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06001E99 RID: 7833 RVA: 0x000A32B3 File Offset: 0x000A14B3
	public bool ShouldOnlyTransferFromLowerPriority
	{
		get
		{
			return this.onlyTransferFromLowerPriority || this.allowItemRemoval;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06001E9A RID: 7834 RVA: 0x000A32C5 File Offset: 0x000A14C5
	// (set) Token: 0x06001E9B RID: 7835 RVA: 0x000A32CD File Offset: 0x000A14CD
	public bool allowUIItemRemoval { get; set; }

	// Token: 0x1700014C RID: 332
	public GameObject this[int idx]
	{
		get
		{
			return this.items[idx];
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06001E9D RID: 7837 RVA: 0x000A32E4 File Offset: 0x000A14E4
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x000A32F1 File Offset: 0x000A14F1
	public bool ShouldShowInUI()
	{
		return this.showInUI;
	}

	// Token: 0x06001E9F RID: 7839 RVA: 0x000A32F9 File Offset: 0x000A14F9
	public List<GameObject> GetItems()
	{
		return this.items;
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x000A3301 File Offset: 0x000A1501
	public void SetDefaultStoredItemModifiers(List<Storage.StoredItemModifier> modifiers)
	{
		this.defaultStoredItemModifers = modifiers;
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06001EA1 RID: 7841 RVA: 0x000A330A File Offset: 0x000A150A
	public PrioritySetting masterPriority
	{
		get
		{
			if (this.prioritizable)
			{
				return this.prioritizable.GetMasterPriority();
			}
			return Chore.DefaultPrioritySetting;
		}
	}

	// Token: 0x06001EA2 RID: 7842 RVA: 0x000A332C File Offset: 0x000A152C
	public override Workable.AnimInfo GetAnim(Worker worker)
	{
		if (this.useGunForDelivery && worker.usesMultiTool)
		{
			Workable.AnimInfo anim = base.GetAnim(worker);
			anim.smi = new MultitoolController.Instance(this, worker, "store", Assets.GetPrefab(EffectConfigs.OreAbsorbId));
			return anim;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06001EA3 RID: 7843 RVA: 0x000A3384 File Offset: 0x000A1584
	public override Vector3 GetTargetPoint()
	{
		Vector3 vector = base.GetTargetPoint();
		if (this.useGunForDelivery && this.gunTargetOffset != Vector2.zero)
		{
			if (this.rotatable != null)
			{
				vector += this.rotatable.GetRotatedOffset(this.gunTargetOffset);
			}
			else
			{
				vector += new Vector3(this.gunTargetOffset.x, this.gunTargetOffset.y, 0f);
			}
		}
		return vector;
	}

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06001EA4 RID: 7844 RVA: 0x000A3408 File Offset: 0x000A1608
	// (remove) Token: 0x06001EA5 RID: 7845 RVA: 0x000A3440 File Offset: 0x000A1640
	public event System.Action OnStorageIncreased;

	// Token: 0x06001EA6 RID: 7846 RVA: 0x000A3478 File Offset: 0x000A1678
	protected override void OnPrefabInit()
	{
		if (this.useWideOffsets)
		{
			base.SetOffsetTable(OffsetGroups.InvertedWideTable);
		}
		else
		{
			base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		}
		this.showProgressBar = false;
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		GameUtil.SubscribeToTags<Storage>(this, Storage.OnDeadTagAddedDelegate, true);
		base.Subscribe<Storage>(1502190696, Storage.OnQueueDestroyObjectDelegate);
		base.Subscribe<Storage>(-905833192, Storage.OnCopySettingsDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Storing;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.SetupStorageStatusItems();
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x000A3520 File Offset: 0x000A1720
	private void SetupStorageStatusItems()
	{
		if (Storage.capacityStatusItem == null)
		{
			Storage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Storage.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				Storage storage = (Storage)data;
				float num = storage.MassStored();
				float num2 = storage.capacityKg;
				if (num > num2 - storage.storageFullMargin && num < num2)
				{
					num = num2;
				}
				else
				{
					num = Mathf.Floor(num);
				}
				string newValue = Util.FormatWholeNumber(num);
				IUserControlledCapacity component = storage.GetComponent<IUserControlledCapacity>();
				if (component != null)
				{
					num2 = Mathf.Min(component.UserMaxCapacity, num2);
				}
				string newValue2 = Util.FormatWholeNumber(num2);
				str = str.Replace("{Stored}", newValue);
				str = str.Replace("{Capacity}", newValue2);
				if (component != null)
				{
					str = str.Replace("{Units}", component.CapacityUnits);
				}
				else
				{
					str = str.Replace("{Units}", GameUtil.GetCurrentMassUnit(false));
				}
				return str;
			};
		}
		if (this.showCapacityStatusItem)
		{
			if (this.showCapacityAsMainStatus)
			{
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Storage.capacityStatusItem, this);
				return;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, Storage.capacityStatusItem, this);
		}
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x000A35D8 File Offset: 0x000A17D8
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (!this.allowSettingOnlyFetchMarkedItems)
		{
			this.onlyFetchMarkedItems = false;
		}
		this.UpdateFetchCategory();
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x000A35F0 File Offset: 0x000A17F0
	protected override void OnSpawn()
	{
		base.SetWorkTime(this.storageWorkTime);
		foreach (GameObject go in this.items)
		{
			this.ApplyStoredItemModifiers(go, true, true);
			if (this.sendOnStoreOnSpawn)
			{
				go.Trigger(856640610, this);
			}
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.SetSymbolVisiblity("sweep", this.onlyFetchMarkedItems);
		}
		Prioritizable component2 = base.GetComponent<Prioritizable>();
		if (component2 != null)
		{
			Prioritizable prioritizable = component2;
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
		}
		this.UpdateFetchCategory();
		if (this.showUnreachableStatus)
		{
			base.Subscribe<Storage>(-1432940121, Storage.OnReachableChangedDelegate);
			new ReachabilityMonitor.Instance(this).StartSM();
		}
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x000A36E8 File Offset: 0x000A18E8
	public GameObject Store(GameObject go, bool hide_popups = false, bool block_events = false, bool do_disease_transfer = true, bool is_deserializing = false)
	{
		if (go == null)
		{
			return null;
		}
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		GameObject result = go;
		if (!hide_popups && PopFXManager.Instance != null)
		{
			LocString loc_string;
			Transform transform;
			if (this.fxPrefix == Storage.FXPrefix.Delivered)
			{
				loc_string = UI.DELIVERED;
				transform = base.transform;
			}
			else
			{
				loc_string = UI.PICKEDUP;
				transform = go.transform;
			}
			string text;
			if (!Assets.IsTagCountable(go.PrefabID()))
			{
				text = string.Format(loc_string, GameUtil.GetFormattedMass(component.Units, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), go.GetProperName());
			}
			else
			{
				text = string.Format(loc_string, (int)component.Units, go.GetProperName());
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, text, transform, this.storageFXOffset, 1.5f, false, false);
		}
		go.transform.parent = base.transform;
		Vector3 position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Move);
		position.z = go.transform.GetPosition().z;
		go.transform.SetPosition(position);
		if (!block_events && do_disease_transfer)
		{
			this.TransferDiseaseWithObject(go);
		}
		if (!is_deserializing)
		{
			Pickupable component2 = go.GetComponent<Pickupable>();
			if (component2 != null)
			{
				if (component2 != null && component2.prevent_absorb_until_stored)
				{
					component2.prevent_absorb_until_stored = false;
				}
				foreach (GameObject gameObject in this.items)
				{
					if (gameObject != null)
					{
						Pickupable component3 = gameObject.GetComponent<Pickupable>();
						if (component3 != null && component3.TryAbsorb(component2, hide_popups, true))
						{
							if (!block_events)
							{
								base.Trigger(-1697596308, go);
								base.Trigger(-778359855, this);
								if (this.OnStorageIncreased != null)
								{
									this.OnStorageIncreased();
								}
							}
							this.ApplyStoredItemModifiers(go, true, false);
							result = gameObject;
							go = null;
							break;
						}
					}
				}
			}
		}
		if (go != null)
		{
			this.items.Add(go);
			if (!is_deserializing)
			{
				this.ApplyStoredItemModifiers(go, true, false);
			}
			if (!block_events)
			{
				go.Trigger(856640610, this);
				base.Trigger(-1697596308, go);
				base.Trigger(-778359855, this);
				if (this.OnStorageIncreased != null)
				{
					this.OnStorageIncreased();
				}
			}
		}
		return result;
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x000A3950 File Offset: 0x000A1B50
	public PrimaryElement AddElement(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		Element element2 = ElementLoader.FindElementByHash(element);
		if (element2.IsGas)
		{
			return this.AddGasChunk(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		if (element2.IsLiquid)
		{
			return this.AddLiquid(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		if (element2.IsSolid)
		{
			return this.AddOre(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		return null;
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x000A39B4 File Offset: 0x000A1BB4
	public PrimaryElement AddOre(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.Mass += mass;
			primaryElement.Temperature = finalTemperature;
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddOre");
			base.Trigger(-1697596308, primaryElement.gameObject);
		}
		else
		{
			Element element2 = ElementLoader.FindElementByHash(element);
			GameObject gameObject = element2.substance.SpawnResource(base.transform.GetPosition(), mass, temperature, disease_idx, disease_count, true, false, true);
			gameObject.GetComponent<Pickupable>().prevent_absorb_until_stored = true;
			element2.substance.ActivateSubstanceGameObject(gameObject, disease_idx, disease_count);
			this.Store(gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x000A3A80 File Offset: 0x000A1C80
	public PrimaryElement AddLiquid(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.Mass += mass;
			primaryElement.Temperature = finalTemperature;
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddLiquid");
			base.Trigger(-1697596308, primaryElement.gameObject);
		}
		else
		{
			SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, base.transform.GetPosition());
			primaryElement = substanceChunk.GetComponent<PrimaryElement>();
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			this.Store(substanceChunk.gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x000A3B3C File Offset: 0x000A1D3C
	public PrimaryElement AddGasChunk(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float mass2 = primaryElement.Mass;
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, mass2, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.SetMassTemperature(mass2 + mass, finalTemperature);
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddGasChunk");
			base.Trigger(-1697596308, primaryElement.gameObject);
		}
		else
		{
			SubstanceChunk substanceChunk = GasSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, base.transform.GetPosition());
			primaryElement = substanceChunk.GetComponent<PrimaryElement>();
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			this.Store(substanceChunk.gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x000A3BED File Offset: 0x000A1DED
	public void Transfer(Storage target, bool block_events = false, bool hide_popups = false)
	{
		while (this.items.Count > 0)
		{
			this.Transfer(this.items[0], target, block_events, hide_popups);
		}
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x000A3C18 File Offset: 0x000A1E18
	public float Transfer(Storage dest_storage, Tag tag, float amount, bool block_events = false, bool hide_popups = false)
	{
		GameObject gameObject = this.FindFirst(tag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (amount < component.Units)
			{
				Pickupable component2 = gameObject.GetComponent<Pickupable>();
				Pickupable pickupable = component2.Take(amount);
				dest_storage.Store(pickupable.gameObject, hide_popups, block_events, true, false);
				if (!block_events)
				{
					base.Trigger(-1697596308, component2.gameObject);
				}
			}
			else
			{
				this.Transfer(gameObject, dest_storage, block_events, hide_popups);
				amount = component.Units;
			}
			return amount;
		}
		return 0f;
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x000A3C9C File Offset: 0x000A1E9C
	public bool Transfer(GameObject go, Storage target, bool block_events = false, bool hide_popups = false)
	{
		this.items.RemoveAll((GameObject it) => it == null);
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.items[i] == go)
			{
				this.items.RemoveAt(i);
				this.ApplyStoredItemModifiers(go, false, false);
				target.Store(go, hide_popups, block_events, true, false);
				if (!block_events)
				{
					base.Trigger(-1697596308, go);
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x000A3D34 File Offset: 0x000A1F34
	public bool DropSome(Tag tag, float amount, bool ventGas = false, bool dumpLiquid = false, Vector3 offset = default(Vector3), bool doDiseaseTransfer = true, bool showInWorldNotification = false)
	{
		bool result = false;
		float num = amount;
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.Find(tag, pooledList);
		foreach (GameObject gameObject in pooledList)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			if (component)
			{
				Pickupable pickupable = component.Take(num);
				if (pickupable != null)
				{
					bool flag = false;
					if (ventGas || dumpLiquid)
					{
						Dumpable component2 = pickupable.GetComponent<Dumpable>();
						if (component2 != null)
						{
							if (ventGas && pickupable.GetComponent<PrimaryElement>().Element.IsGas)
							{
								component2.Dump(base.transform.GetPosition() + offset);
								flag = true;
								num -= pickupable.GetComponent<PrimaryElement>().Mass;
								base.Trigger(-1697596308, pickupable.gameObject);
								result = true;
								if (showInWorldNotification)
								{
									PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
								}
							}
							if (dumpLiquid && pickupable.GetComponent<PrimaryElement>().Element.IsLiquid)
							{
								component2.Dump(base.transform.GetPosition() + offset);
								flag = true;
								num -= pickupable.GetComponent<PrimaryElement>().Mass;
								base.Trigger(-1697596308, pickupable.gameObject);
								result = true;
								if (showInWorldNotification)
								{
									PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
								}
							}
						}
					}
					if (!flag)
					{
						Vector3 position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore) + offset;
						pickupable.transform.SetPosition(position);
						KBatchedAnimController component3 = pickupable.GetComponent<KBatchedAnimController>();
						if (component3)
						{
							component3.SetSceneLayer(Grid.SceneLayer.Ore);
						}
						num -= pickupable.GetComponent<PrimaryElement>().Mass;
						this.MakeWorldActive(pickupable.gameObject);
						base.Trigger(-1697596308, pickupable.gameObject);
						result = true;
						if (showInWorldNotification)
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
						}
					}
				}
			}
			if (num <= 0f)
			{
				break;
			}
		}
		pooledList.Recycle();
		return result;
	}

	// Token: 0x06001EB3 RID: 7859 RVA: 0x000A4040 File Offset: 0x000A2240
	public void DropAll(Vector3 position, bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default(Vector3), bool do_disease_transfer = true, List<GameObject> collect_dropped_items = null)
	{
		while (this.items.Count > 0)
		{
			GameObject gameObject = this.items[0];
			if (do_disease_transfer)
			{
				this.TransferDiseaseWithObject(gameObject);
			}
			this.items.RemoveAt(0);
			if (gameObject != null)
			{
				bool flag = false;
				if (vent_gas || dump_liquid)
				{
					Dumpable component = gameObject.GetComponent<Dumpable>();
					if (component != null)
					{
						if (vent_gas && gameObject.GetComponent<PrimaryElement>().Element.IsGas)
						{
							component.Dump(position + offset);
							flag = true;
						}
						if (dump_liquid && gameObject.GetComponent<PrimaryElement>().Element.IsLiquid)
						{
							component.Dump(position + offset);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					gameObject.transform.SetPosition(position + offset);
					KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
					if (component2)
					{
						component2.SetSceneLayer(Grid.SceneLayer.Ore);
					}
					this.MakeWorldActive(gameObject);
					if (collect_dropped_items != null)
					{
						collect_dropped_items.Add(gameObject);
					}
				}
			}
		}
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x000A4135 File Offset: 0x000A2335
	public void DropAll(bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default(Vector3), bool do_disease_transfer = true, List<GameObject> collect_dropped_items = null)
	{
		this.DropAll(Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore), vent_gas, dump_liquid, offset, do_disease_transfer, collect_dropped_items);
	}

	// Token: 0x06001EB5 RID: 7861 RVA: 0x000A4154 File Offset: 0x000A2354
	public void Drop(Tag t, List<GameObject> obj_list)
	{
		this.Find(t, obj_list);
		foreach (GameObject go in obj_list)
		{
			this.Drop(go, true);
		}
	}

	// Token: 0x06001EB6 RID: 7862 RVA: 0x000A41B0 File Offset: 0x000A23B0
	public void Drop(Tag t)
	{
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.Find(t, pooledList);
		foreach (GameObject go in pooledList)
		{
			this.Drop(go, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x06001EB7 RID: 7863 RVA: 0x000A4218 File Offset: 0x000A2418
	public void DropUnlessMatching(FetchChore chore)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				KPrefabID component = this.items[i].GetComponent<KPrefabID>();
				if (!(((chore.criteria == FetchChore.MatchCriteria.MatchID && chore.tags.Contains(component.PrefabTag)) || (chore.criteria == FetchChore.MatchCriteria.MatchTags && component.HasTag(chore.tagsFirst))) & (!chore.requiredTag.IsValid || component.HasTag(chore.requiredTag)) & !component.HasAnyTags(chore.forbiddenTags)))
				{
					GameObject gameObject = this.items[i];
					this.items.RemoveAt(i);
					i--;
					this.TransferDiseaseWithObject(gameObject);
					this.MakeWorldActive(gameObject);
				}
			}
		}
	}

	// Token: 0x06001EB8 RID: 7864 RVA: 0x000A42FC File Offset: 0x000A24FC
	public void DropUnlessHasTag(Tag tag)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null) && !this.items[i].GetComponent<KPrefabID>().HasTag(tag))
			{
				GameObject gameObject = this.items[i];
				this.items.RemoveAt(i);
				i--;
				this.TransferDiseaseWithObject(gameObject);
				this.MakeWorldActive(gameObject);
				Dumpable component = gameObject.GetComponent<Dumpable>();
				if (component != null)
				{
					component.Dump(base.transform.GetPosition());
				}
			}
		}
	}

	// Token: 0x06001EB9 RID: 7865 RVA: 0x000A439C File Offset: 0x000A259C
	public GameObject Drop(GameObject go, bool do_disease_transfer = true)
	{
		if (go == null)
		{
			return null;
		}
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(go != this.items[i]))
			{
				this.items[i] = this.items[count - 1];
				this.items.RemoveAt(count - 1);
				if (do_disease_transfer)
				{
					this.TransferDiseaseWithObject(go);
				}
				this.MakeWorldActive(go);
				break;
			}
		}
		return go;
	}

	// Token: 0x06001EBA RID: 7866 RVA: 0x000A441C File Offset: 0x000A261C
	public void RenotifyAll()
	{
		this.items.RemoveAll((GameObject it) => it == null);
		foreach (GameObject go in this.items)
		{
			go.Trigger(856640610, this);
		}
	}

	// Token: 0x06001EBB RID: 7867 RVA: 0x000A44A0 File Offset: 0x000A26A0
	private void TransferDiseaseWithObject(GameObject obj)
	{
		if (obj == null || !this.doDiseaseTransfer || this.primaryElement == null)
		{
			return;
		}
		PrimaryElement component = obj.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
		invalid.idx = component.DiseaseIdx;
		invalid.count = (int)((float)component.DiseaseCount * 0.05f);
		SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
		invalid2.idx = this.primaryElement.DiseaseIdx;
		invalid2.count = (int)((float)this.primaryElement.DiseaseCount * 0.05f);
		component.ModifyDiseaseCount(-invalid.count, "Storage.TransferDiseaseWithObject");
		this.primaryElement.ModifyDiseaseCount(-invalid2.count, "Storage.TransferDiseaseWithObject");
		if (invalid.count > 0)
		{
			this.primaryElement.AddDisease(invalid.idx, invalid.count, "Storage.TransferDiseaseWithObject");
		}
		if (invalid2.count > 0)
		{
			component.AddDisease(invalid2.idx, invalid2.count, "Storage.TransferDiseaseWithObject");
		}
	}

	// Token: 0x06001EBC RID: 7868 RVA: 0x000A45A8 File Offset: 0x000A27A8
	private void MakeWorldActive(GameObject go)
	{
		go.transform.parent = null;
		if (this.dropOffset != Vector2.zero)
		{
			go.transform.Translate(this.dropOffset);
		}
		go.Trigger(856640610, null);
		base.Trigger(-1697596308, go);
		this.ApplyStoredItemModifiers(go, false, false);
		if (go != null)
		{
			PrimaryElement component = go.GetComponent<PrimaryElement>();
			if (component != null && component.KeepZeroMassObject)
			{
				component.KeepZeroMassObject = false;
				if (component.Mass <= 0f)
				{
					Util.KDestroyGameObject(go);
				}
			}
		}
	}

	// Token: 0x06001EBD RID: 7869 RVA: 0x000A4648 File Offset: 0x000A2848
	public List<GameObject> Find(Tag tag, List<GameObject> result)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				result.Add(gameObject);
			}
		}
		return result;
	}

	// Token: 0x06001EBE RID: 7870 RVA: 0x000A4694 File Offset: 0x000A2894
	public GameObject FindFirst(Tag tag)
	{
		GameObject result = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				result = gameObject;
				break;
			}
		}
		return result;
	}

	// Token: 0x06001EBF RID: 7871 RVA: 0x000A46E0 File Offset: 0x000A28E0
	public PrimaryElement FindFirstWithMass(Tag tag, float mass = 0f)
	{
		PrimaryElement result = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.Mass > 0f && component.Mass >= mass)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x000A4748 File Offset: 0x000A2948
	public HashSet<Tag> GetAllIDsInStorage()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject go = this.items[i];
			hashSet.Add(go.PrefabID());
		}
		return hashSet;
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x000A478C File Offset: 0x000A298C
	public GameObject Find(int ID)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (ID == gameObject.PrefabID().GetHashCode())
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x000A47D6 File Offset: 0x000A29D6
	public void ConsumeAllIgnoringDisease()
	{
		this.ConsumeAllIgnoringDisease(Tag.Invalid);
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x000A47E4 File Offset: 0x000A29E4
	public void ConsumeAllIgnoringDisease(Tag tag)
	{
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (!(tag != Tag.Invalid) || this.items[i].HasTag(tag))
			{
				this.ConsumeIgnoringDisease(this.items[i]);
			}
		}
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x000A483C File Offset: 0x000A2A3C
	public void ConsumeAndGetDisease(Tag tag, float amount, out float amount_consumed, out SimUtil.DiseaseInfo disease_info, out float aggregate_temperature)
	{
		DebugUtil.Assert(tag.IsValid);
		amount_consumed = 0f;
		disease_info = SimUtil.DiseaseInfo.Invalid;
		aggregate_temperature = 0f;
		bool flag = false;
		int num = 0;
		while (num < this.items.Count && amount > 0f)
		{
			GameObject gameObject = this.items[num];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.Units > 0f)
				{
					flag = true;
					float num2 = Math.Min(component.Units, amount);
					global::Debug.Assert(num2 > 0f, "Delta amount was zero, which should be impossible.");
					aggregate_temperature = SimUtil.CalculateFinalTemperature(amount_consumed, aggregate_temperature, num2, component.Temperature);
					SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(component, num2 / component.Units);
					disease_info = SimUtil.CalculateFinalDiseaseInfo(disease_info, percentOfDisease);
					component.Units -= num2;
					component.ModifyDiseaseCount(-percentOfDisease.count, "Storage.ConsumeAndGetDisease");
					amount -= num2;
					amount_consumed += num2;
				}
				if (component.Units <= 0f && !component.KeepZeroMassObject)
				{
					if (this.deleted_objects == null)
					{
						this.deleted_objects = new List<GameObject>();
					}
					this.deleted_objects.Add(gameObject);
				}
				base.Trigger(-1697596308, gameObject);
			}
			num++;
		}
		if (!flag)
		{
			aggregate_temperature = base.GetComponent<PrimaryElement>().Temperature;
		}
		if (this.deleted_objects != null)
		{
			for (int i = 0; i < this.deleted_objects.Count; i++)
			{
				this.items.Remove(this.deleted_objects[i]);
				Util.KDestroyGameObject(this.deleted_objects[i]);
			}
			this.deleted_objects.Clear();
		}
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x000A4A0C File Offset: 0x000A2C0C
	public void ConsumeAndGetDisease(Recipe.Ingredient ingredient, out SimUtil.DiseaseInfo disease_info, out float temperature)
	{
		float num;
		this.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out num, out disease_info, out temperature);
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x000A4A30 File Offset: 0x000A2C30
	public void ConsumeIgnoringDisease(Tag tag, float amount)
	{
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		this.ConsumeAndGetDisease(tag, amount, out num, out diseaseInfo, out num2);
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x000A4A4C File Offset: 0x000A2C4C
	public void ConsumeIgnoringDisease(GameObject item_go)
	{
		if (this.items.Contains(item_go))
		{
			PrimaryElement component = item_go.GetComponent<PrimaryElement>();
			if (component != null && component.KeepZeroMassObject)
			{
				component.Units = 0f;
				component.ModifyDiseaseCount(-component.DiseaseCount, "consume item");
				base.Trigger(-1697596308, item_go);
				return;
			}
			this.items.Remove(item_go);
			base.Trigger(-1697596308, item_go);
			item_go.DeleteObject();
		}
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x000A4AC8 File Offset: 0x000A2CC8
	public GameObject Drop(int ID)
	{
		return this.Drop(this.Find(ID), true);
	}

	// Token: 0x06001EC9 RID: 7881 RVA: 0x000A4AD8 File Offset: 0x000A2CD8
	private void OnDeath(object data)
	{
		this.DropAll(true, true, default(Vector3), true, null);
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x000A4AF8 File Offset: 0x000A2CF8
	public bool IsFull()
	{
		return this.RemainingCapacity() <= 0f;
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x000A4B0A File Offset: 0x000A2D0A
	public bool IsEmpty()
	{
		return this.items.Count == 0;
	}

	// Token: 0x06001ECC RID: 7884 RVA: 0x000A4B1A File Offset: 0x000A2D1A
	public float Capacity()
	{
		return this.capacityKg;
	}

	// Token: 0x06001ECD RID: 7885 RVA: 0x000A4B22 File Offset: 0x000A2D22
	public bool IsEndOfLife()
	{
		return this.endOfLife;
	}

	// Token: 0x06001ECE RID: 7886 RVA: 0x000A4B2C File Offset: 0x000A2D2C
	public float ExactMassStored()
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				PrimaryElement component = this.items[i].GetComponent<PrimaryElement>();
				if (component != null)
				{
					num += component.Units * component.MassPerUnit;
				}
			}
		}
		return num;
	}

	// Token: 0x06001ECF RID: 7887 RVA: 0x000A4B95 File Offset: 0x000A2D95
	public float MassStored()
	{
		return (float)Mathf.RoundToInt(this.ExactMassStored() * 1000f) / 1000f;
	}

	// Token: 0x06001ED0 RID: 7888 RVA: 0x000A4BB0 File Offset: 0x000A2DB0
	public float UnitsStored()
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				PrimaryElement component = this.items[i].GetComponent<PrimaryElement>();
				if (component != null)
				{
					num += component.Units;
				}
			}
		}
		return (float)Mathf.RoundToInt(num * 1000f) / 1000f;
	}

	// Token: 0x06001ED1 RID: 7889 RVA: 0x000A4C24 File Offset: 0x000A2E24
	public bool Has(Tag tag)
	{
		bool result = false;
		foreach (GameObject gameObject in this.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.HasTag(tag) && component.Mass > 0f)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06001ED2 RID: 7890 RVA: 0x000A4CA0 File Offset: 0x000A2EA0
	public PrimaryElement AddToPrimaryElement(SimHashes element, float additional_mass, float temperature)
	{
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, additional_mass);
			primaryElement.Mass += additional_mass;
			primaryElement.Temperature = finalTemperature;
		}
		return primaryElement;
	}

	// Token: 0x06001ED3 RID: 7891 RVA: 0x000A4CE8 File Offset: 0x000A2EE8
	public PrimaryElement FindPrimaryElement(SimHashes element)
	{
		PrimaryElement result = null;
		foreach (GameObject gameObject in this.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.ElementID == element)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06001ED4 RID: 7892 RVA: 0x000A4D54 File Offset: 0x000A2F54
	public float RemainingCapacity()
	{
		return this.capacityKg - this.MassStored();
	}

	// Token: 0x06001ED5 RID: 7893 RVA: 0x000A4D63 File Offset: 0x000A2F63
	public bool GetOnlyFetchMarkedItems()
	{
		return this.onlyFetchMarkedItems;
	}

	// Token: 0x06001ED6 RID: 7894 RVA: 0x000A4D6B File Offset: 0x000A2F6B
	public void SetOnlyFetchMarkedItems(bool is_set)
	{
		if (is_set != this.onlyFetchMarkedItems)
		{
			this.onlyFetchMarkedItems = is_set;
			this.UpdateFetchCategory();
			base.Trigger(644822890, null);
			base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("sweep", is_set);
		}
	}

	// Token: 0x06001ED7 RID: 7895 RVA: 0x000A4DA5 File Offset: 0x000A2FA5
	private void UpdateFetchCategory()
	{
		if (this.fetchCategory == Storage.FetchCategory.Building)
		{
			return;
		}
		this.fetchCategory = (this.onlyFetchMarkedItems ? Storage.FetchCategory.StorageSweepOnly : Storage.FetchCategory.GeneralStorage);
	}

	// Token: 0x06001ED8 RID: 7896 RVA: 0x000A4DC2 File Offset: 0x000A2FC2
	protected override void OnCleanUp()
	{
		if (this.items.Count != 0)
		{
			global::Debug.LogWarning("Storage for [" + base.gameObject.name + "] is being destroyed but it still contains items!", base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x000A4DFC File Offset: 0x000A2FFC
	private void OnQueueDestroyObject(object data)
	{
		this.endOfLife = true;
		this.DropAll(true, false, default(Vector3), true, null);
		this.OnCleanUp();
	}

	// Token: 0x06001EDA RID: 7898 RVA: 0x000A4E29 File Offset: 0x000A3029
	public void Remove(GameObject go, bool do_disease_transfer = true)
	{
		this.items.Remove(go);
		if (do_disease_transfer)
		{
			this.TransferDiseaseWithObject(go);
		}
		base.Trigger(-1697596308, go);
		this.ApplyStoredItemModifiers(go, false, false);
	}

	// Token: 0x06001EDB RID: 7899 RVA: 0x000A4E58 File Offset: 0x000A3058
	public bool ForceStore(Tag tag, float amount)
	{
		global::Debug.Assert(amount < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT);
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				gameObject.GetComponent<PrimaryElement>().Mass += amount;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001EDC RID: 7900 RVA: 0x000A4EC0 File Offset: 0x000A30C0
	public float GetAmountAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x06001EDD RID: 7901 RVA: 0x000A4F18 File Offset: 0x000A3118
	public float GetAmountAvailable(Tag tag, Tag[] forbiddenTags = null)
	{
		if (forbiddenTags == null)
		{
			return this.GetAmountAvailable(tag);
		}
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag) && !gameObject.HasAnyTags(forbiddenTags))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x06001EDE RID: 7902 RVA: 0x000A4F84 File Offset: 0x000A3184
	public float GetUnitsAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x000A4FDC File Offset: 0x000A31DC
	public float GetMassAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Mass;
			}
		}
		return num;
	}

	// Token: 0x06001EE0 RID: 7904 RVA: 0x000A5034 File Offset: 0x000A3234
	public float GetMassAvailable(SimHashes element)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.ElementID == element)
				{
					num += component.Mass;
				}
			}
		}
		return num;
	}

	// Token: 0x06001EE1 RID: 7905 RVA: 0x000A5090 File Offset: 0x000A3290
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.showDescriptor)
		{
			descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.STORAGECAPACITY, GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STORAGECAPACITY, GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
		}
		return descriptors;
	}

	// Token: 0x06001EE2 RID: 7906 RVA: 0x000A5100 File Offset: 0x000A3300
	public static void MakeItemTemperatureInsulated(GameObject go, bool is_stored, bool is_initializing)
	{
		SimTemperatureTransfer component = go.GetComponent<SimTemperatureTransfer>();
		if (component == null)
		{
			return;
		}
		component.enabled = !is_stored;
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x000A5128 File Offset: 0x000A3328
	public static void MakeItemInvisible(GameObject go, bool is_stored, bool is_initializing)
	{
		if (is_initializing)
		{
			return;
		}
		bool flag = !is_stored;
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component != null && component.enabled != flag)
		{
			component.enabled = flag;
		}
		KSelectable component2 = go.GetComponent<KSelectable>();
		if (component2 != null && component2.enabled != flag)
		{
			component2.enabled = flag;
		}
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x000A517E File Offset: 0x000A337E
	public static void MakeItemSealed(GameObject go, bool is_stored, bool is_initializing)
	{
		if (go != null)
		{
			if (is_stored)
			{
				go.GetComponent<KPrefabID>().AddTag(GameTags.Sealed, false);
				return;
			}
			go.GetComponent<KPrefabID>().RemoveTag(GameTags.Sealed);
		}
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x000A51AE File Offset: 0x000A33AE
	public static void MakeItemPreserved(GameObject go, bool is_stored, bool is_initializing)
	{
		if (go != null)
		{
			if (is_stored)
			{
				go.GetComponent<KPrefabID>().AddTag(GameTags.Preserved, false);
				return;
			}
			go.GetComponent<KPrefabID>().RemoveTag(GameTags.Preserved);
		}
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x000A51E0 File Offset: 0x000A33E0
	private void ApplyStoredItemModifiers(GameObject go, bool is_stored, bool is_initializing)
	{
		List<Storage.StoredItemModifier> list = this.defaultStoredItemModifers;
		for (int i = 0; i < list.Count; i++)
		{
			Storage.StoredItemModifier storedItemModifier = list[i];
			for (int j = 0; j < Storage.StoredItemModifierHandlers.Count; j++)
			{
				Storage.StoredItemModifierInfo storedItemModifierInfo = Storage.StoredItemModifierHandlers[j];
				if (storedItemModifierInfo.modifier == storedItemModifier)
				{
					storedItemModifierInfo.toggleState(go, is_stored, is_initializing);
					break;
				}
			}
		}
	}

	// Token: 0x06001EE7 RID: 7911 RVA: 0x000A524C File Offset: 0x000A344C
	protected virtual void OnCopySettings(object data)
	{
		Storage component = ((GameObject)data).GetComponent<Storage>();
		if (component != null)
		{
			this.SetOnlyFetchMarkedItems(component.onlyFetchMarkedItems);
		}
	}

	// Token: 0x06001EE8 RID: 7912 RVA: 0x000A527C File Offset: 0x000A347C
	private void OnPriorityChanged(PrioritySetting priority)
	{
		foreach (GameObject go in this.items)
		{
			go.Trigger(-1626373771, this);
		}
	}

	// Token: 0x06001EE9 RID: 7913 RVA: 0x000A52D4 File Offset: 0x000A34D4
	private void OnReachableChanged(object data)
	{
		bool flag = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (flag)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, this);
	}

	// Token: 0x06001EEA RID: 7914 RVA: 0x000A5320 File Offset: 0x000A3520
	public void SetContentsDeleteOffGrid(bool delete_off_grid)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			Pickupable component = this.items[i].GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = delete_off_grid;
			}
			Storage component2 = this.items[i].GetComponent<Storage>();
			if (component2 != null)
			{
				component2.SetContentsDeleteOffGrid(delete_off_grid);
			}
		}
	}

	// Token: 0x06001EEB RID: 7915 RVA: 0x000A5388 File Offset: 0x000A3588
	private bool ShouldSaveItem(GameObject go)
	{
		bool result = false;
		if (go != null && go.GetComponent<SaveLoadRoot>() != null && go.GetComponent<PrimaryElement>().Mass > 0f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x000A53C4 File Offset: 0x000A35C4
	public void Serialize(BinaryWriter writer)
	{
		int num = 0;
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ShouldSaveItem(this.items[i]))
			{
				num++;
			}
		}
		writer.Write(num);
		if (num == 0)
		{
			return;
		}
		if (this.items != null && this.items.Count > 0)
		{
			for (int j = 0; j < this.items.Count; j++)
			{
				GameObject gameObject = this.items[j];
				if (this.ShouldSaveItem(gameObject))
				{
					SaveLoadRoot component = gameObject.GetComponent<SaveLoadRoot>();
					if (component != null)
					{
						string name = gameObject.GetComponent<KPrefabID>().GetSaveLoadTag().Name;
						writer.WriteKleiString(name);
						component.Save(writer);
					}
					else
					{
						global::Debug.Log("Tried to save obj in storage but obj has no SaveLoadRoot", gameObject);
					}
				}
			}
		}
	}

	// Token: 0x06001EED RID: 7917 RVA: 0x000A54A0 File Offset: 0x000A36A0
	public void Deserialize(IReader reader)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		this.ClearItems();
		int num4 = reader.ReadInt32();
		this.items = new List<GameObject>(num4);
		for (int i = 0; i < num4; i++)
		{
			float realtimeSinceStartup2 = Time.realtimeSinceStartup;
			Tag tag = TagManager.Create(reader.ReadKleiString());
			SaveLoadRoot saveLoadRoot = SaveLoadRoot.Load(tag, reader);
			num += Time.realtimeSinceStartup - realtimeSinceStartup2;
			if (saveLoadRoot != null)
			{
				KBatchedAnimController component = saveLoadRoot.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					component.enabled = false;
				}
				saveLoadRoot.SetRegistered(false);
				float realtimeSinceStartup3 = Time.realtimeSinceStartup;
				GameObject gameObject = this.Store(saveLoadRoot.gameObject, true, true, false, true);
				num2 += Time.realtimeSinceStartup - realtimeSinceStartup3;
				if (gameObject != null)
				{
					Pickupable component2 = gameObject.GetComponent<Pickupable>();
					if (component2 != null)
					{
						float realtimeSinceStartup4 = Time.realtimeSinceStartup;
						component2.OnStore(this);
						num3 += Time.realtimeSinceStartup - realtimeSinceStartup4;
					}
					Storable component3 = gameObject.GetComponent<Storable>();
					if (component3 != null)
					{
						float realtimeSinceStartup5 = Time.realtimeSinceStartup;
						component3.OnStore(this);
						num3 += Time.realtimeSinceStartup - realtimeSinceStartup5;
					}
					if (this.dropOnLoad)
					{
						this.Drop(saveLoadRoot.gameObject, true);
					}
				}
			}
			else
			{
				global::Debug.LogWarning("Tried to deserialize " + tag.ToString() + " into storage but failed", base.gameObject);
			}
		}
	}

	// Token: 0x06001EEE RID: 7918 RVA: 0x000A561C File Offset: 0x000A381C
	private void ClearItems()
	{
		foreach (GameObject go in this.items)
		{
			go.DeleteObject();
		}
		this.items.Clear();
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x000A5678 File Offset: 0x000A3878
	public void UpdateStoredItemCachedCells()
	{
		foreach (GameObject gameObject in this.items)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			if (component != null)
			{
				component.UpdateCachedCellFromStoragePosition();
			}
		}
	}

	// Token: 0x0400112D RID: 4397
	public bool allowItemRemoval;

	// Token: 0x0400112E RID: 4398
	public bool ignoreSourcePriority;

	// Token: 0x0400112F RID: 4399
	public bool onlyTransferFromLowerPriority;

	// Token: 0x04001130 RID: 4400
	public float capacityKg = 20000f;

	// Token: 0x04001131 RID: 4401
	public bool showDescriptor;

	// Token: 0x04001133 RID: 4403
	public bool doDiseaseTransfer = true;

	// Token: 0x04001134 RID: 4404
	public List<Tag> storageFilters;

	// Token: 0x04001135 RID: 4405
	public bool useGunForDelivery = true;

	// Token: 0x04001136 RID: 4406
	public bool sendOnStoreOnSpawn;

	// Token: 0x04001137 RID: 4407
	public bool showInUI = true;

	// Token: 0x04001138 RID: 4408
	public bool storeDropsFromButcherables;

	// Token: 0x04001139 RID: 4409
	public bool allowClearable;

	// Token: 0x0400113A RID: 4410
	public bool showCapacityStatusItem;

	// Token: 0x0400113B RID: 4411
	public bool showCapacityAsMainStatus;

	// Token: 0x0400113C RID: 4412
	public bool showUnreachableStatus;

	// Token: 0x0400113D RID: 4413
	public bool showSideScreenTitleBar;

	// Token: 0x0400113E RID: 4414
	public bool useWideOffsets;

	// Token: 0x0400113F RID: 4415
	public Vector2 dropOffset = Vector2.zero;

	// Token: 0x04001140 RID: 4416
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001141 RID: 4417
	public Vector2 gunTargetOffset;

	// Token: 0x04001142 RID: 4418
	public Storage.FetchCategory fetchCategory;

	// Token: 0x04001143 RID: 4419
	public int storageNetworkID = -1;

	// Token: 0x04001144 RID: 4420
	public float storageFullMargin;

	// Token: 0x04001145 RID: 4421
	public Vector3 storageFXOffset = Vector3.zero;

	// Token: 0x04001146 RID: 4422
	private static readonly EventSystem.IntraObjectHandler<Storage> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x04001147 RID: 4423
	public Storage.FXPrefix fxPrefix;

	// Token: 0x04001148 RID: 4424
	public List<GameObject> items = new List<GameObject>();

	// Token: 0x04001149 RID: 4425
	[MyCmpGet]
	public Prioritizable prioritizable;

	// Token: 0x0400114A RID: 4426
	[MyCmpGet]
	public Automatable automatable;

	// Token: 0x0400114B RID: 4427
	[MyCmpGet]
	protected PrimaryElement primaryElement;

	// Token: 0x0400114C RID: 4428
	public bool dropOnLoad;

	// Token: 0x0400114D RID: 4429
	protected float maxKGPerItem = float.MaxValue;

	// Token: 0x0400114E RID: 4430
	private bool endOfLife;

	// Token: 0x0400114F RID: 4431
	public bool allowSettingOnlyFetchMarkedItems = true;

	// Token: 0x04001150 RID: 4432
	[Serialize]
	private bool onlyFetchMarkedItems;

	// Token: 0x04001151 RID: 4433
	public float storageWorkTime = 1.5f;

	// Token: 0x04001152 RID: 4434
	private static readonly List<Storage.StoredItemModifierInfo> StoredItemModifierHandlers = new List<Storage.StoredItemModifierInfo>
	{
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Hide, new Action<GameObject, bool, bool>(Storage.MakeItemInvisible)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Insulate, new Action<GameObject, bool, bool>(Storage.MakeItemTemperatureInsulated)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Seal, new Action<GameObject, bool, bool>(Storage.MakeItemSealed)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Preserve, new Action<GameObject, bool, bool>(Storage.MakeItemPreserved))
	};

	// Token: 0x04001153 RID: 4435
	[SerializeField]
	private List<Storage.StoredItemModifier> defaultStoredItemModifers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide
	};

	// Token: 0x04001154 RID: 4436
	public static readonly List<Storage.StoredItemModifier> StandardSealedStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};

	// Token: 0x04001155 RID: 4437
	public static readonly List<Storage.StoredItemModifier> StandardFabricatorStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};

	// Token: 0x04001156 RID: 4438
	public static readonly List<Storage.StoredItemModifier> StandardInsulatedStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal,
		Storage.StoredItemModifier.Insulate
	};

	// Token: 0x04001158 RID: 4440
	private static StatusItem capacityStatusItem;

	// Token: 0x04001159 RID: 4441
	private static readonly EventSystem.IntraObjectHandler<Storage> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<Storage>(GameTags.Dead, delegate(Storage component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x0400115A RID: 4442
	private static readonly EventSystem.IntraObjectHandler<Storage> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnQueueDestroyObject(data);
	});

	// Token: 0x0400115B RID: 4443
	private static readonly EventSystem.IntraObjectHandler<Storage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400115C RID: 4444
	private List<GameObject> deleted_objects;

	// Token: 0x020011B5 RID: 4533
	public enum StoredItemModifier
	{
		// Token: 0x04005D54 RID: 23892
		Insulate,
		// Token: 0x04005D55 RID: 23893
		Hide,
		// Token: 0x04005D56 RID: 23894
		Seal,
		// Token: 0x04005D57 RID: 23895
		Preserve
	}

	// Token: 0x020011B6 RID: 4534
	public enum FetchCategory
	{
		// Token: 0x04005D59 RID: 23897
		Building,
		// Token: 0x04005D5A RID: 23898
		GeneralStorage,
		// Token: 0x04005D5B RID: 23899
		StorageSweepOnly
	}

	// Token: 0x020011B7 RID: 4535
	public enum FXPrefix
	{
		// Token: 0x04005D5D RID: 23901
		Delivered,
		// Token: 0x04005D5E RID: 23902
		PickedUp
	}

	// Token: 0x020011B8 RID: 4536
	private struct StoredItemModifierInfo
	{
		// Token: 0x06007A91 RID: 31377 RVA: 0x002DC9D0 File Offset: 0x002DABD0
		public StoredItemModifierInfo(Storage.StoredItemModifier modifier, Action<GameObject, bool, bool> toggle_state)
		{
			this.modifier = modifier;
			this.toggleState = toggle_state;
		}

		// Token: 0x04005D5F RID: 23903
		public Storage.StoredItemModifier modifier;

		// Token: 0x04005D60 RID: 23904
		public Action<GameObject, bool, bool> toggleState;
	}
}
