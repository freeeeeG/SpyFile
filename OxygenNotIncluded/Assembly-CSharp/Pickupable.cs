using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020004EC RID: 1260
[AddComponentMenu("KMonoBehaviour/Workable/Pickupable")]
public class Pickupable : Workable, IHasSortOrder
{
	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06001D43 RID: 7491 RVA: 0x0009B4C1 File Offset: 0x000996C1
	public PrimaryElement PrimaryElement
	{
		get
		{
			return this.primaryElement;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06001D44 RID: 7492 RVA: 0x0009B4C9 File Offset: 0x000996C9
	// (set) Token: 0x06001D45 RID: 7493 RVA: 0x0009B4D1 File Offset: 0x000996D1
	public int sortOrder
	{
		get
		{
			return this._sortOrder;
		}
		set
		{
			this._sortOrder = value;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06001D46 RID: 7494 RVA: 0x0009B4DA File Offset: 0x000996DA
	// (set) Token: 0x06001D47 RID: 7495 RVA: 0x0009B4E2 File Offset: 0x000996E2
	public Storage storage { get; set; }

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06001D48 RID: 7496 RVA: 0x0009B4EB File Offset: 0x000996EB
	public float MinTakeAmount
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06001D49 RID: 7497 RVA: 0x0009B4F2 File Offset: 0x000996F2
	// (set) Token: 0x06001D4A RID: 7498 RVA: 0x0009B4FA File Offset: 0x000996FA
	public bool prevent_absorb_until_stored { get; set; }

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06001D4B RID: 7499 RVA: 0x0009B503 File Offset: 0x00099703
	// (set) Token: 0x06001D4C RID: 7500 RVA: 0x0009B50B File Offset: 0x0009970B
	public bool isKinematic { get; set; }

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06001D4D RID: 7501 RVA: 0x0009B514 File Offset: 0x00099714
	// (set) Token: 0x06001D4E RID: 7502 RVA: 0x0009B51C File Offset: 0x0009971C
	public bool wasAbsorbed { get; private set; }

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06001D4F RID: 7503 RVA: 0x0009B525 File Offset: 0x00099725
	// (set) Token: 0x06001D50 RID: 7504 RVA: 0x0009B52D File Offset: 0x0009972D
	public int cachedCell { get; private set; }

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06001D51 RID: 7505 RVA: 0x0009B536 File Offset: 0x00099736
	// (set) Token: 0x06001D52 RID: 7506 RVA: 0x0009B540 File Offset: 0x00099740
	public bool IsEntombed
	{
		get
		{
			return this.isEntombed;
		}
		set
		{
			if (value != this.isEntombed)
			{
				this.isEntombed = value;
				if (this.isEntombed)
				{
					base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
				}
				else
				{
					base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
				}
				base.Trigger(-1089732772, null);
				this.UpdateEntombedVisualizer();
			}
		}
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x0009B59A File Offset: 0x0009979A
	private bool CouldBePickedUpCommon(GameObject carrier)
	{
		return this.UnreservedAmount >= this.MinTakeAmount && (this.UnreservedAmount > 0f || this.FindReservedAmount(carrier) > 0f);
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x0009B5CC File Offset: 0x000997CC
	public bool CouldBePickedUpByMinion(GameObject carrier)
	{
		return this.CouldBePickedUpCommon(carrier) && (this.storage == null || !this.storage.automatable || !this.storage.automatable.GetAutomationOnly());
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x0009B619 File Offset: 0x00099819
	public bool CouldBePickedUpByTransferArm(GameObject carrier)
	{
		return this.CouldBePickedUpCommon(carrier);
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x0009B624 File Offset: 0x00099824
	public float FindReservedAmount(GameObject reserver)
	{
		for (int i = 0; i < this.reservations.Count; i++)
		{
			if (this.reservations[i].reserver == reserver)
			{
				return this.reservations[i].amount;
			}
		}
		return 0f;
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06001D57 RID: 7511 RVA: 0x0009B677 File Offset: 0x00099877
	public float UnreservedAmount
	{
		get
		{
			return this.TotalAmount - this.ReservedAmount;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06001D58 RID: 7512 RVA: 0x0009B686 File Offset: 0x00099886
	// (set) Token: 0x06001D59 RID: 7513 RVA: 0x0009B68E File Offset: 0x0009988E
	public float ReservedAmount { get; private set; }

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06001D5A RID: 7514 RVA: 0x0009B697 File Offset: 0x00099897
	// (set) Token: 0x06001D5B RID: 7515 RVA: 0x0009B6A4 File Offset: 0x000998A4
	public float TotalAmount
	{
		get
		{
			return this.primaryElement.Units;
		}
		set
		{
			DebugUtil.Assert(this.primaryElement != null);
			this.primaryElement.Units = value;
			if (value < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.primaryElement.KeepZeroMassObject)
			{
				base.gameObject.DeleteObject();
			}
			this.NotifyChanged(Grid.PosToCell(this));
		}
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x0009B6FC File Offset: 0x000998FC
	private void RefreshReservedAmount()
	{
		this.ReservedAmount = 0f;
		for (int i = 0; i < this.reservations.Count; i++)
		{
			this.ReservedAmount += this.reservations[i].amount;
		}
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x0009B748 File Offset: 0x00099948
	[Conditional("UNITY_EDITOR")]
	private void Log(string evt, string param, float value)
	{
	}

	// Token: 0x06001D5E RID: 7518 RVA: 0x0009B74A File Offset: 0x0009994A
	public void ClearReservations()
	{
		this.reservations.Clear();
		this.RefreshReservedAmount();
	}

	// Token: 0x06001D5F RID: 7519 RVA: 0x0009B760 File Offset: 0x00099960
	[ContextMenu("Print Reservations")]
	public void PrintReservations()
	{
		foreach (Pickupable.Reservation reservation in this.reservations)
		{
			global::Debug.Log(reservation.ToString());
		}
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x0009B7C0 File Offset: 0x000999C0
	public int Reserve(string context, GameObject reserver, float amount)
	{
		int num = this.nextTicketNumber;
		this.nextTicketNumber = num + 1;
		int num2 = num;
		Pickupable.Reservation item = new Pickupable.Reservation(reserver, amount, num2);
		this.reservations.Add(item);
		this.RefreshReservedAmount();
		if (this.OnReservationsChanged != null)
		{
			this.OnReservationsChanged();
		}
		return num2;
	}

	// Token: 0x06001D61 RID: 7521 RVA: 0x0009B810 File Offset: 0x00099A10
	public void Unreserve(string context, int ticket)
	{
		int i = 0;
		while (i < this.reservations.Count)
		{
			if (this.reservations[i].ticket == ticket)
			{
				this.reservations.RemoveAt(i);
				this.RefreshReservedAmount();
				if (this.OnReservationsChanged != null)
				{
					this.OnReservationsChanged();
					return;
				}
				break;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x0009B870 File Offset: 0x00099A70
	private Pickupable()
	{
		this.showProgressBar = false;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x0009B8E8 File Offset: 0x00099AE8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.log = new LoggerFSSF("Pickupable");
		this.workerStatusItem = Db.Get().DuplicantStatusItems.PickingUp;
		base.SetWorkTime(1.5f);
		this.targetWorkable = this;
		this.resetProgressOnStop = true;
		base.gameObject.layer = Game.PickupableLayer;
		Vector3 position = base.transform.GetPosition();
		this.UpdateCachedCell(Grid.PosToCell(position));
		base.Subscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate);
		base.Subscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate);
		base.Subscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate);
		base.Subscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate);
		base.Subscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate);
		base.Subscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset);
		this.KPrefabID.AddTag(GameTags.Pickupable, false);
		Components.Pickupables.Add(this);
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x0009B9F1 File Offset: 0x00099BF1
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x0009B9FC File Offset: 0x00099BFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num) && this.deleteOffGrid)
		{
			base.gameObject.DeleteObject();
			return;
		}
		this.UpdateCachedCell(num);
		new ReachabilityMonitor.Instance(this).StartSM();
		new FetchableMonitor.Instance(this).StartSM();
		base.SetWorkTime(1.5f);
		this.faceTargetWhenWorking = true;
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetStatusIndicatorOffset(new Vector3(0f, -0.65f, 0f));
		}
		this.OnTagsChanged(null);
		this.TryToOffsetIfBuried(CellOffset.none);
		DecorProvider component2 = base.GetComponent<DecorProvider>();
		if (component2 != null && string.IsNullOrEmpty(component2.overrideName))
		{
			component2.overrideName = UI.OVERLAYS.DECOR.CLUTTER;
		}
		this.UpdateEntombedVisualizer();
		base.Subscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate);
		this.NotifyChanged(num);
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x0009BAE8 File Offset: 0x00099CE8
	[OnDeserialized]
	public void OnDeserialize()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 28) && base.transform.position.z == 0f)
		{
			KBatchedAnimController component = base.transform.GetComponent<KBatchedAnimController>();
			component.SetSceneLayer(component.sceneLayer);
		}
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x0009BB3C File Offset: 0x00099D3C
	public void RegisterListeners()
	{
		if (this.cleaningUp)
		{
			return;
		}
		if (this.solidPartitionerEntry.IsValid())
		{
			return;
		}
		int num = Grid.PosToCell(this);
		this.objectLayerListItem = new ObjectLayerListItem(base.gameObject, ObjectLayer.Pickupables, num);
		this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterSolidListener", base.gameObject, num, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterPickupable", this, num, GameScenePartitioner.Instance.pickupablesLayer, null);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "Pickupable.OnCellChange");
		Singleton<CellChangeMonitor>.Instance.MarkDirty(base.transform);
		Singleton<CellChangeMonitor>.Instance.ClearLastKnownCell(base.transform);
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x0009BC10 File Offset: 0x00099E10
	public void UnregisterListeners()
	{
		if (this.objectLayerListItem != null)
		{
			this.objectLayerListItem.Clear();
			this.objectLayerListItem = null;
		}
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
	}

	// Token: 0x06001D69 RID: 7529 RVA: 0x0009BC73 File Offset: 0x00099E73
	private void OnSolidChanged(object data)
	{
		this.TryToOffsetIfBuried(CellOffset.none);
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x0009BC80 File Offset: 0x00099E80
	private void SetWorkableOffset(object data)
	{
		CellOffset offset = CellOffset.none;
		Worker worker = data as Worker;
		if (worker != null)
		{
			int num = Grid.PosToCell(worker);
			int base_cell = Grid.PosToCell(this);
			offset = (Grid.IsValidCell(num) ? Grid.GetCellOffsetDirection(base_cell, num) : CellOffset.none);
		}
		this.TryToOffsetIfBuried(offset);
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x0009BCD0 File Offset: 0x00099ED0
	private CellOffset[] GetPreferedOffsets(CellOffset preferedDirectionOffset)
	{
		if (preferedDirectionOffset == CellOffset.left || preferedDirectionOffset == CellOffset.leftup)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.left,
				CellOffset.leftup
			};
		}
		if (preferedDirectionOffset == CellOffset.right || preferedDirectionOffset == CellOffset.rightup)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.right,
				CellOffset.rightup
			};
		}
		if (preferedDirectionOffset == CellOffset.up)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.rightup,
				CellOffset.leftup
			};
		}
		if (preferedDirectionOffset == CellOffset.leftdown)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.leftdown,
				CellOffset.left
			};
		}
		if (preferedDirectionOffset == CellOffset.rightdown)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.rightdown,
				CellOffset.right
			};
		}
		if (preferedDirectionOffset == CellOffset.down)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.leftdown,
				CellOffset.rightdown
			};
		}
		return new CellOffset[0];
	}

	// Token: 0x06001D6C RID: 7532 RVA: 0x0009BE50 File Offset: 0x0009A050
	public void TryToOffsetIfBuried(CellOffset offset)
	{
		if (this.KPrefabID.HasTag(GameTags.Stored) || this.KPrefabID.HasTag(GameTags.Equipped))
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		DeathMonitor.Instance smi = base.gameObject.GetSMI<DeathMonitor.Instance>();
		if ((smi == null || smi.IsDead()) && ((Grid.Solid[num] && Grid.Foundation[num]) || Grid.Properties[num] != 0))
		{
			CellOffset[] array = this.GetPreferedOffsets(offset).Concat(Pickupable.displacementOffsets);
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = Grid.OffsetCell(num, array[i]);
				if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					Vector3 position = Grid.CellToPosCBC(num2, Grid.SceneLayer.Move);
					KCollider2D component = base.GetComponent<KCollider2D>();
					if (component != null)
					{
						position.y += base.transform.GetPosition().y - component.bounds.min.y;
					}
					base.transform.SetPosition(position);
					num = num2;
					this.RemoveFaller();
					this.AddFaller(Vector2.zero);
					break;
				}
			}
		}
		this.HandleSolidCell(num);
	}

	// Token: 0x06001D6D RID: 7533 RVA: 0x0009BFA4 File Offset: 0x0009A1A4
	private bool HandleSolidCell(int cell)
	{
		bool flag = this.IsEntombed;
		bool flag2 = false;
		if (Grid.IsValidCell(cell) && Grid.Solid[cell])
		{
			DeathMonitor.Instance smi = base.gameObject.GetSMI<DeathMonitor.Instance>();
			if (smi == null || smi.IsDead())
			{
				this.Clearable.CancelClearing();
				flag2 = true;
			}
		}
		if (flag2 != flag && !this.KPrefabID.HasTag(GameTags.Stored))
		{
			this.IsEntombed = flag2;
			base.GetComponent<KSelectable>().IsSelectable = !this.IsEntombed;
		}
		this.UpdateEntombedVisualizer();
		return this.IsEntombed;
	}

	// Token: 0x06001D6E RID: 7534 RVA: 0x0009C034 File Offset: 0x0009A234
	private void OnCellChange()
	{
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		if (!Grid.IsValidCell(num))
		{
			Vector2 vector = new Vector2(-0.1f * (float)Grid.WidthInCells, 1.1f * (float)Grid.WidthInCells);
			Vector2 vector2 = new Vector2(-0.1f * (float)Grid.HeightInCells, 1.1f * (float)Grid.HeightInCells);
			if (this.deleteOffGrid && (position.x < vector.x || vector.y < position.x || position.y < vector2.x || vector2.y < position.y))
			{
				this.DeleteObject();
				return;
			}
		}
		else
		{
			this.ReleaseEntombedVisualizerAndAddFaller(true);
			if (this.HandleSolidCell(num))
			{
				return;
			}
			this.objectLayerListItem.Update(num);
			bool flag = false;
			if (this.absorbable && !this.KPrefabID.HasTag(GameTags.Stored))
			{
				int num2 = Grid.CellBelow(num);
				if (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					ObjectLayerListItem nextItem = this.objectLayerListItem.nextItem;
					while (nextItem != null)
					{
						GameObject gameObject = nextItem.gameObject;
						nextItem = nextItem.nextItem;
						Pickupable component = gameObject.GetComponent<Pickupable>();
						if (component != null)
						{
							flag = component.TryAbsorb(this, false, false);
							if (flag)
							{
								break;
							}
						}
					}
				}
			}
			GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, num);
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, num);
			int cachedCell = this.cachedCell;
			this.UpdateCachedCell(num);
			if (!flag)
			{
				this.NotifyChanged(num);
			}
			if (Grid.IsValidCell(cachedCell) && num != cachedCell)
			{
				this.NotifyChanged(cachedCell);
			}
		}
	}

	// Token: 0x06001D6F RID: 7535 RVA: 0x0009C1E0 File Offset: 0x0009A3E0
	private void OnTagsChanged(object data)
	{
		if (!this.KPrefabID.HasTag(GameTags.Stored) && !this.KPrefabID.HasTag(GameTags.Equipped))
		{
			this.RegisterListeners();
			this.AddFaller(Vector2.zero);
			return;
		}
		this.UnregisterListeners();
		this.RemoveFaller();
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x0009C22F File Offset: 0x0009A42F
	private void NotifyChanged(int new_cell)
	{
		GameScenePartitioner.Instance.TriggerEvent(new_cell, GameScenePartitioner.Instance.pickupablesChangedLayer, this);
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x0009C248 File Offset: 0x0009A448
	public bool TryAbsorb(Pickupable other, bool hide_effects, bool allow_cross_storage = false)
	{
		if (other == null)
		{
			return false;
		}
		if (other.wasAbsorbed)
		{
			return false;
		}
		if (this.wasAbsorbed)
		{
			return false;
		}
		if (!other.CanAbsorb(this))
		{
			return false;
		}
		if (this.prevent_absorb_until_stored)
		{
			return false;
		}
		if (!allow_cross_storage && this.storage == null != (other.storage == null))
		{
			return false;
		}
		this.Absorb(other);
		if (!hide_effects && EffectPrefabs.Instance != null && !this.storage)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
			global::Util.KInstantiate(Assets.GetPrefab(EffectConfigs.OreAbsorbId), position, Quaternion.identity, null, null, true, 0).SetActive(true);
		}
		return true;
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x0009C310 File Offset: 0x0009A510
	protected override void OnCleanUp()
	{
		this.cleaningUp = true;
		this.ReleaseEntombedVisualizerAndAddFaller(false);
		this.RemoveFaller();
		if (this.storage)
		{
			this.storage.Remove(base.gameObject, true);
		}
		this.UnregisterListeners();
		Components.Pickupables.Remove(this);
		if (this.reservations.Count > 0)
		{
			this.reservations.Clear();
			if (this.OnReservationsChanged != null)
			{
				this.OnReservationsChanged();
			}
		}
		if (Grid.IsValidCell(this.cachedCell))
		{
			this.NotifyChanged(this.cachedCell);
		}
		base.OnCleanUp();
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x0009C3AC File Offset: 0x0009A5AC
	public Pickupable Take(float amount)
	{
		if (amount <= 0f)
		{
			return null;
		}
		if (this.OnTake == null)
		{
			if (this.storage != null)
			{
				this.storage.Remove(base.gameObject, true);
			}
			return this;
		}
		if (amount >= this.TotalAmount && this.storage != null && !this.primaryElement.KeepZeroMassObject)
		{
			this.storage.Remove(base.gameObject, true);
		}
		float num = Math.Min(this.TotalAmount, amount);
		if (num <= 0f)
		{
			return null;
		}
		return this.OnTake(num);
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x0009C448 File Offset: 0x0009A648
	private void Absorb(Pickupable pickupable)
	{
		global::Debug.Assert(!this.wasAbsorbed);
		global::Debug.Assert(!pickupable.wasAbsorbed);
		base.Trigger(-2064133523, pickupable);
		pickupable.Trigger(-1940207677, base.gameObject);
		pickupable.wasAbsorbed = true;
		KSelectable component = base.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == pickupable.GetComponent<KSelectable>())
		{
			SelectTool.Instance.Select(component, false);
		}
		pickupable.gameObject.DeleteObject();
		this.NotifyChanged(Grid.PosToCell(this));
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x0009C4F8 File Offset: 0x0009A6F8
	private void RefreshStorageTags(object data = null)
	{
		bool flag = data is Storage || (data != null && (bool)data);
		if (flag && data is Storage && ((Storage)data).gameObject == base.gameObject)
		{
			return;
		}
		if (!flag)
		{
			this.KPrefabID.RemoveTag(GameTags.Stored);
			this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
			return;
		}
		this.KPrefabID.AddTag(GameTags.Stored, false);
		if (this.storage == null || !this.storage.allowItemRemoval)
		{
			this.KPrefabID.AddTag(GameTags.StoredPrivate, false);
			return;
		}
		this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x0009C5B4 File Offset: 0x0009A7B4
	public void OnStore(object data)
	{
		this.storage = (data as Storage);
		bool flag = data is Storage || (data != null && (bool)data);
		SaveLoadRoot component = base.GetComponent<SaveLoadRoot>();
		if (this.carryAnimOverride != null && this.lastCarrier != null)
		{
			this.lastCarrier.RemoveAnimOverrides(this.carryAnimOverride);
			this.lastCarrier = null;
		}
		KSelectable component2 = base.GetComponent<KSelectable>();
		if (component2)
		{
			component2.IsSelectable = !flag;
		}
		if (flag)
		{
			int cachedCell = this.cachedCell;
			this.RefreshStorageTags(data);
			if (this.storage != null)
			{
				if (this.carryAnimOverride != null && this.storage.GetComponent<Navigator>() != null)
				{
					this.lastCarrier = this.storage.GetComponent<KBatchedAnimController>();
					if (this.lastCarrier != null && this.lastCarrier.HasTag(GameTags.Minion))
					{
						this.lastCarrier.AddAnimOverrides(this.carryAnimOverride, 0f);
					}
				}
				this.UpdateCachedCell(Grid.PosToCell(this.storage));
			}
			this.NotifyChanged(cachedCell);
			if (component != null)
			{
				component.SetRegistered(false);
				return;
			}
		}
		else
		{
			if (component != null)
			{
				component.SetRegistered(true);
			}
			this.RemovedFromStorage();
		}
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x0009C6FC File Offset: 0x0009A8FC
	private void RemovedFromStorage()
	{
		this.storage = null;
		this.UpdateCachedCell(Grid.PosToCell(this));
		this.RefreshStorageTags(null);
		this.AddFaller(Vector2.zero);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.enabled = true;
		base.gameObject.transform.rotation = Quaternion.identity;
		this.RegisterListeners();
		component.GetBatchInstanceData().ClearOverrideTransformMatrix();
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x0009C760 File Offset: 0x0009A960
	public void UpdateCachedCellFromStoragePosition()
	{
		global::Debug.Assert(this.storage != null, "Only call UpdateCachedCellFromStoragePosition on pickupables in storage!");
		this.UpdateCachedCell(Grid.PosToCell(this.storage));
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x0009C789 File Offset: 0x0009A989
	private void UpdateCachedCell(int cell)
	{
		this.cachedCell = cell;
		this.GetOffsets(this.cachedCell);
	}

	// Token: 0x06001D7A RID: 7546 RVA: 0x0009C7A0 File Offset: 0x0009A9A0
	public override Workable.AnimInfo GetAnim(Worker worker)
	{
		if (this.useGunforPickup && worker.usesMultiTool)
		{
			Workable.AnimInfo anim = base.GetAnim(worker);
			anim.smi = new MultitoolController.Instance(this, worker, "pickup", Assets.GetPrefab(EffectConfigs.OreAbsorbId));
			return anim;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x0009C7F8 File Offset: 0x0009A9F8
	protected override void OnCompleteWork(Worker worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.startWorkInfo;
		float amount = pickupableStartWorkInfo.amount;
		if (!(this != null))
		{
			pickupableStartWorkInfo.setResultCb(null);
			return;
		}
		Pickupable pickupable = this.Take(amount);
		if (pickupable != null)
		{
			component.Store(pickupable.gameObject, false, false, true, false);
			worker.workCompleteData = pickupable;
			pickupableStartWorkInfo.setResultCb(pickupable.gameObject);
			return;
		}
		pickupableStartWorkInfo.setResultCb(null);
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x0009C883 File Offset: 0x0009AA83
	public override bool InstantlyFinish(Worker worker)
	{
		return false;
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x0009C886 File Offset: 0x0009AA86
	public override Vector3 GetTargetPoint()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x0009C893 File Offset: 0x0009AA93
	public bool IsReachable()
	{
		return this.isReachable;
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x0009C89C File Offset: 0x0009AA9C
	private void OnReachableChanged(object data)
	{
		this.isReachable = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.isReachable)
		{
			component.RemoveStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, this);
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x0009C8F3 File Offset: 0x0009AAF3
	private void AddFaller(Vector2 initial_velocity)
	{
		if (base.GetComponent<Health>() != null)
		{
			return;
		}
		if (!GameComps.Fallers.Has(base.gameObject))
		{
			GameComps.Fallers.Add(base.gameObject, initial_velocity);
		}
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x0009C928 File Offset: 0x0009AB28
	private void RemoveFaller()
	{
		if (base.GetComponent<Health>() != null)
		{
			return;
		}
		if (GameComps.Fallers.Has(base.gameObject))
		{
			GameComps.Fallers.Remove(base.gameObject);
		}
	}

	// Token: 0x06001D82 RID: 7554 RVA: 0x0009C95C File Offset: 0x0009AB5C
	private void OnOreSizeChanged(object data)
	{
		Vector3 v = Vector3.zero;
		HandleVector<int>.Handle handle = GameComps.Gravities.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			v = GameComps.Gravities.GetData(handle).velocity;
		}
		this.RemoveFaller();
		if (!this.KPrefabID.HasTag(GameTags.Stored))
		{
			this.AddFaller(v);
		}
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x0009C9C4 File Offset: 0x0009ABC4
	private void OnLanded(object data)
	{
		if (CameraController.Instance == null)
		{
			return;
		}
		Vector3 position = base.transform.GetPosition();
		Vector2I vector2I = Grid.PosToXY(position);
		if (vector2I.x < 0 || Grid.WidthInCells <= vector2I.x || vector2I.y < 0 || Grid.HeightInCells <= vector2I.y)
		{
			this.DeleteObject();
			return;
		}
		Vector2 vector = (Vector2)data;
		if (vector.sqrMagnitude <= 0.2f || SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		Element element = this.primaryElement.Element;
		if (element.substance != null)
		{
			string text = element.substance.GetOreBumpSound();
			if (text == null)
			{
				if (element.HasTag(GameTags.RefinedMetal))
				{
					text = "RefinedMetal";
				}
				else if (element.HasTag(GameTags.Metal))
				{
					text = "RawMetal";
				}
				else
				{
					text = "Rock";
				}
			}
			if (element.tag.ToString() == "Creature" && !base.gameObject.HasTag(GameTags.Seed))
			{
				text = "Bodyfall_rock";
			}
			else
			{
				text = "Ore_bump_" + text;
			}
			string text2 = GlobalAssets.GetSound(text, true);
			text2 = ((text2 != null) ? text2 : GlobalAssets.GetSound("Ore_bump_rock", false));
			if (CameraController.Instance.IsAudibleSound(base.transform.GetPosition(), text2))
			{
				int num = Grid.PosToCell(position);
				bool isLiquid = Grid.Element[num].IsLiquid;
				float value = 0f;
				if (isLiquid)
				{
					value = SoundUtil.GetLiquidDepth(num);
				}
				FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(text2, CameraController.Instance.GetVerticallyScaledPosition(base.transform.GetPosition(), false), 1f);
				instance.setParameterByName("velocity", vector.magnitude, false);
				instance.setParameterByName("liquidDepth", value, false);
				KFMOD.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x0009CBA0 File Offset: 0x0009ADA0
	private void UpdateEntombedVisualizer()
	{
		if (this.IsEntombed)
		{
			if (this.entombedCell == -1)
			{
				int cell = Grid.PosToCell(this);
				if (EntombedItemManager.CanEntomb(this))
				{
					SaveGame.Instance.entombedItemManager.Add(this);
				}
				if (Grid.Objects[cell, 1] == null)
				{
					KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
					if (component != null && Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(cell))
					{
						this.entombedCell = cell;
						component.enabled = false;
						this.RemoveFaller();
						return;
					}
				}
			}
		}
		else
		{
			this.ReleaseEntombedVisualizerAndAddFaller(true);
		}
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x0009CC30 File Offset: 0x0009AE30
	private void ReleaseEntombedVisualizerAndAddFaller(bool add_faller_if_necessary)
	{
		if (this.entombedCell != -1)
		{
			Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.entombedCell);
			this.entombedCell = -1;
			base.GetComponent<KBatchedAnimController>().enabled = true;
			if (add_faller_if_necessary)
			{
				this.AddFaller(Vector2.zero);
			}
		}
	}

	// Token: 0x04001035 RID: 4149
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04001036 RID: 4150
	public const float WorkTime = 1.5f;

	// Token: 0x04001037 RID: 4151
	[SerializeField]
	private int _sortOrder;

	// Token: 0x04001039 RID: 4153
	[MyCmpReq]
	[NonSerialized]
	public KPrefabID KPrefabID;

	// Token: 0x0400103A RID: 4154
	[MyCmpAdd]
	[NonSerialized]
	public Clearable Clearable;

	// Token: 0x0400103B RID: 4155
	[MyCmpAdd]
	[NonSerialized]
	public Prioritizable prioritizable;

	// Token: 0x0400103C RID: 4156
	public bool absorbable;

	// Token: 0x0400103E RID: 4158
	public Func<Pickupable, bool> CanAbsorb = (Pickupable other) => false;

	// Token: 0x0400103F RID: 4159
	public Func<float, Pickupable> OnTake;

	// Token: 0x04001040 RID: 4160
	public System.Action OnReservationsChanged;

	// Token: 0x04001041 RID: 4161
	public ObjectLayerListItem objectLayerListItem;

	// Token: 0x04001042 RID: 4162
	public Workable targetWorkable;

	// Token: 0x04001043 RID: 4163
	public KAnimFile carryAnimOverride;

	// Token: 0x04001044 RID: 4164
	private KBatchedAnimController lastCarrier;

	// Token: 0x04001045 RID: 4165
	public bool useGunforPickup = true;

	// Token: 0x04001047 RID: 4167
	private static CellOffset[] displacementOffsets = new CellOffset[]
	{
		new CellOffset(0, 1),
		new CellOffset(0, -1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(-1, -1)
	};

	// Token: 0x04001048 RID: 4168
	private bool isReachable;

	// Token: 0x04001049 RID: 4169
	private bool isEntombed;

	// Token: 0x0400104A RID: 4170
	private bool cleaningUp;

	// Token: 0x0400104C RID: 4172
	public bool trackOnPickup = true;

	// Token: 0x0400104E RID: 4174
	private int nextTicketNumber;

	// Token: 0x0400104F RID: 4175
	[Serialize]
	public bool deleteOffGrid = true;

	// Token: 0x04001050 RID: 4176
	private List<Pickupable.Reservation> reservations = new List<Pickupable.Reservation>();

	// Token: 0x04001051 RID: 4177
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x04001052 RID: 4178
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001053 RID: 4179
	private LoggerFSSF log;

	// Token: 0x04001055 RID: 4181
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x04001056 RID: 4182
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnLandedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnLanded(data);
	});

	// Token: 0x04001057 RID: 4183
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnOreSizeChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnOreSizeChanged(data);
	});

	// Token: 0x04001058 RID: 4184
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x04001059 RID: 4185
	private static readonly EventSystem.IntraObjectHandler<Pickupable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.RefreshStorageTags(data);
	});

	// Token: 0x0400105A RID: 4186
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnWorkableEntombOffset = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.SetWorkableOffset(data);
	});

	// Token: 0x0400105B RID: 4187
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnTagsChanged(data);
	});

	// Token: 0x0400105C RID: 4188
	private int entombedCell = -1;

	// Token: 0x0200118E RID: 4494
	private struct Reservation
	{
		// Token: 0x060079F0 RID: 31216 RVA: 0x002DA975 File Offset: 0x002D8B75
		public Reservation(GameObject reserver, float amount, int ticket)
		{
			this.reserver = reserver;
			this.amount = amount;
			this.ticket = ticket;
		}

		// Token: 0x060079F1 RID: 31217 RVA: 0x002DA98C File Offset: 0x002D8B8C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.reserver.name,
				", ",
				this.amount.ToString(),
				", ",
				this.ticket.ToString()
			});
		}

		// Token: 0x04005CA6 RID: 23718
		public GameObject reserver;

		// Token: 0x04005CA7 RID: 23719
		public float amount;

		// Token: 0x04005CA8 RID: 23720
		public int ticket;
	}

	// Token: 0x0200118F RID: 4495
	public class PickupableStartWorkInfo : Worker.StartWorkInfo
	{
		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x060079F2 RID: 31218 RVA: 0x002DA9DE File Offset: 0x002D8BDE
		// (set) Token: 0x060079F3 RID: 31219 RVA: 0x002DA9E6 File Offset: 0x002D8BE6
		public float amount { get; private set; }

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x060079F4 RID: 31220 RVA: 0x002DA9EF File Offset: 0x002D8BEF
		// (set) Token: 0x060079F5 RID: 31221 RVA: 0x002DA9F7 File Offset: 0x002D8BF7
		public Pickupable originalPickupable { get; private set; }

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x060079F6 RID: 31222 RVA: 0x002DAA00 File Offset: 0x002D8C00
		// (set) Token: 0x060079F7 RID: 31223 RVA: 0x002DAA08 File Offset: 0x002D8C08
		public Action<GameObject> setResultCb { get; private set; }

		// Token: 0x060079F8 RID: 31224 RVA: 0x002DAA11 File Offset: 0x002D8C11
		public PickupableStartWorkInfo(Pickupable pickupable, float amount, Action<GameObject> set_result_cb) : base(pickupable.targetWorkable)
		{
			this.originalPickupable = pickupable;
			this.amount = amount;
			this.setResultCb = set_result_cb;
		}
	}
}
