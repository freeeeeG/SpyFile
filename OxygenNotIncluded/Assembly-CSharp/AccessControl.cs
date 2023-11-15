using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000903 RID: 2307
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AccessControl")]
public class AccessControl : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x060042E0 RID: 17120 RVA: 0x00175EFB File Offset: 0x001740FB
	// (set) Token: 0x060042E1 RID: 17121 RVA: 0x00175F03 File Offset: 0x00174103
	public AccessControl.Permission DefaultPermission
	{
		get
		{
			return this._defaultPermission;
		}
		set
		{
			this._defaultPermission = value;
			this.SetStatusItem();
			this.SetGridRestrictions(null, this._defaultPermission);
		}
	}

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x060042E2 RID: 17122 RVA: 0x00175F1F File Offset: 0x0017411F
	public bool Online
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060042E3 RID: 17123 RVA: 0x00175F24 File Offset: 0x00174124
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (AccessControl.accessControlActive == null)
		{
			AccessControl.accessControlActive = new StatusItem("accessControlActive", BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.NAME, BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
		}
		base.Subscribe<AccessControl>(279163026, AccessControl.OnControlStateChangedDelegate);
		base.Subscribe<AccessControl>(-905833192, AccessControl.OnCopySettingsDelegate);
	}

	// Token: 0x060042E4 RID: 17124 RVA: 0x00175F98 File Offset: 0x00174198
	private void CheckForBadData()
	{
		List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> list = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> item in this.savedPermissions)
		{
			if (item.Key.Get() == null)
			{
				list.Add(item);
			}
		}
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> item2 in list)
		{
			this.savedPermissions.Remove(item2);
		}
	}

	// Token: 0x060042E5 RID: 17125 RVA: 0x00176048 File Offset: 0x00174248
	protected override void OnSpawn()
	{
		this.isTeleporter = (base.GetComponent<NavTeleporter>() != null);
		base.OnSpawn();
		if (this.savedPermissions.Count > 0)
		{
			this.CheckForBadData();
		}
		if (this.registered)
		{
			this.RegisterInGrid(true);
			this.RestorePermissions();
		}
		ListPool<global::Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.PooledList pooledList = ListPool<global::Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.Allocate();
		for (int i = this.savedPermissions.Count - 1; i >= 0; i--)
		{
			KPrefabID kprefabID = this.savedPermissions[i].Key.Get();
			if (kprefabID != null)
			{
				MinionIdentity component = kprefabID.GetComponent<MinionIdentity>();
				if (component != null)
				{
					pooledList.Add(new global::Tuple<MinionAssignablesProxy, AccessControl.Permission>(component.assignableProxy.Get(), this.savedPermissions[i].Value));
					this.savedPermissions.RemoveAt(i);
					this.ClearGridRestrictions(kprefabID);
				}
			}
		}
		foreach (global::Tuple<MinionAssignablesProxy, AccessControl.Permission> tuple in pooledList)
		{
			this.SetPermission(tuple.first, tuple.second);
		}
		pooledList.Recycle();
		this.SetStatusItem();
	}

	// Token: 0x060042E6 RID: 17126 RVA: 0x00176184 File Offset: 0x00174384
	protected override void OnCleanUp()
	{
		this.RegisterInGrid(false);
		base.OnCleanUp();
	}

	// Token: 0x060042E7 RID: 17127 RVA: 0x00176193 File Offset: 0x00174393
	private void OnControlStateChanged(object data)
	{
		this.overrideAccess = (Door.ControlState)data;
	}

	// Token: 0x060042E8 RID: 17128 RVA: 0x001761A4 File Offset: 0x001743A4
	private void OnCopySettings(object data)
	{
		AccessControl component = ((GameObject)data).GetComponent<AccessControl>();
		if (component != null)
		{
			this.savedPermissions.Clear();
			foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair in component.savedPermissions)
			{
				if (keyValuePair.Key.Get() != null)
				{
					this.SetPermission(keyValuePair.Key.Get().GetComponent<MinionAssignablesProxy>(), keyValuePair.Value);
				}
			}
			this._defaultPermission = component._defaultPermission;
			this.SetGridRestrictions(null, this.DefaultPermission);
		}
	}

	// Token: 0x060042E9 RID: 17129 RVA: 0x00176260 File Offset: 0x00174460
	public void SetRegistered(bool newRegistered)
	{
		if (newRegistered && !this.registered)
		{
			this.RegisterInGrid(true);
			this.RestorePermissions();
			return;
		}
		if (!newRegistered && this.registered)
		{
			this.RegisterInGrid(false);
		}
	}

	// Token: 0x060042EA RID: 17130 RVA: 0x00176290 File Offset: 0x00174490
	public void SetPermission(MinionAssignablesProxy key, AccessControl.Permission permission)
	{
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.savedPermissions.Count; i++)
		{
			if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
			{
				flag = true;
				KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair = this.savedPermissions[i];
				this.savedPermissions[i] = new KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>(keyValuePair.Key, permission);
				break;
			}
		}
		if (!flag)
		{
			this.savedPermissions.Add(new KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>(new Ref<KPrefabID>(component), permission));
		}
		this.SetStatusItem();
		this.SetGridRestrictions(component, permission);
	}

	// Token: 0x060042EB RID: 17131 RVA: 0x0017633C File Offset: 0x0017453C
	private void RestorePermissions()
	{
		this.SetGridRestrictions(null, this.DefaultPermission);
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair in this.savedPermissions)
		{
			KPrefabID x = keyValuePair.Key.Get();
			if (x == null)
			{
				DebugUtil.Assert(x == null, "Tried to set a duplicant-specific access restriction with a null key! This will result in an invisible default permission!");
			}
			this.SetGridRestrictions(keyValuePair.Key.Get(), keyValuePair.Value);
		}
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x001763D8 File Offset: 0x001745D8
	private void RegisterInGrid(bool register)
	{
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		if (register)
		{
			Rotatable component3 = base.GetComponent<Rotatable>();
			Grid.Restriction.Orientation orientation;
			if (!this.isTeleporter)
			{
				orientation = ((component3 == null || component3.GetOrientation() == Orientation.Neutral) ? Grid.Restriction.Orientation.Vertical : Grid.Restriction.Orientation.Horizontal);
			}
			else
			{
				orientation = Grid.Restriction.Orientation.SingleCell;
			}
			if (component != null)
			{
				this.registeredBuildingCells = component.PlacementCells;
				int[] array = this.registeredBuildingCells;
				for (int i = 0; i < array.Length; i++)
				{
					Grid.RegisterRestriction(array[i], orientation);
				}
			}
			else
			{
				foreach (CellOffset offset in component2.OccupiedCellsOffsets)
				{
					Grid.RegisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), orientation);
				}
			}
			if (this.isTeleporter)
			{
				Grid.RegisterRestriction(base.GetComponent<NavTeleporter>().GetCell(), orientation);
			}
		}
		else
		{
			if (component != null)
			{
				if (component.GetMyWorldId() != 255 && this.registeredBuildingCells != null)
				{
					int[] array = this.registeredBuildingCells;
					for (int i = 0; i < array.Length; i++)
					{
						Grid.UnregisterRestriction(array[i]);
					}
					this.registeredBuildingCells = null;
				}
			}
			else
			{
				foreach (CellOffset offset2 in component2.OccupiedCellsOffsets)
				{
					Grid.UnregisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset2));
				}
			}
			if (this.isTeleporter)
			{
				int cell = base.GetComponent<NavTeleporter>().GetCell();
				if (cell != Grid.InvalidCell)
				{
					Grid.UnregisterRestriction(cell);
				}
			}
		}
		this.registered = register;
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x0017657C File Offset: 0x0017477C
	private void SetGridRestrictions(KPrefabID kpid, AccessControl.Permission permission)
	{
		if (!this.registered || !base.isSpawned)
		{
			return;
		}
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		int minionInstanceID = (kpid != null) ? kpid.InstanceID : -1;
		Grid.Restriction.Directions directions = (Grid.Restriction.Directions)0;
		switch (permission)
		{
		case AccessControl.Permission.Both:
			directions = (Grid.Restriction.Directions)0;
			break;
		case AccessControl.Permission.GoLeft:
			directions = Grid.Restriction.Directions.Right;
			break;
		case AccessControl.Permission.GoRight:
			directions = Grid.Restriction.Directions.Left;
			break;
		case AccessControl.Permission.Neither:
			directions = (Grid.Restriction.Directions.Left | Grid.Restriction.Directions.Right);
			break;
		}
		if (this.isTeleporter)
		{
			if (directions != (Grid.Restriction.Directions)0)
			{
				directions = Grid.Restriction.Directions.Teleport;
			}
			else
			{
				directions = (Grid.Restriction.Directions)0;
			}
		}
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.SetRestriction(array[i], minionInstanceID, directions);
			}
		}
		else
		{
			foreach (CellOffset offset in component2.OccupiedCellsOffsets)
			{
				Grid.SetRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID, directions);
			}
		}
		if (this.isTeleporter)
		{
			Grid.SetRestriction(base.GetComponent<NavTeleporter>().GetCell(), minionInstanceID, directions);
		}
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x00176690 File Offset: 0x00174890
	private void ClearGridRestrictions(KPrefabID kpid)
	{
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		int minionInstanceID = (kpid != null) ? kpid.InstanceID : -1;
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.ClearRestriction(array[i], minionInstanceID);
			}
			return;
		}
		foreach (CellOffset offset in component2.OccupiedCellsOffsets)
		{
			Grid.ClearRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID);
		}
	}

	// Token: 0x060042EF RID: 17135 RVA: 0x00176738 File Offset: 0x00174938
	public AccessControl.Permission GetPermission(Navigator minion)
	{
		Door.ControlState controlState = this.overrideAccess;
		if (controlState == Door.ControlState.Opened)
		{
			return AccessControl.Permission.Both;
		}
		if (controlState == Door.ControlState.Locked)
		{
			return AccessControl.Permission.Neither;
		}
		return this.GetSetPermission(this.GetKeyForNavigator(minion));
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x00176765 File Offset: 0x00174965
	private MinionAssignablesProxy GetKeyForNavigator(Navigator minion)
	{
		return minion.GetComponent<MinionIdentity>().assignableProxy.Get();
	}

	// Token: 0x060042F1 RID: 17137 RVA: 0x00176777 File Offset: 0x00174977
	public AccessControl.Permission GetSetPermission(MinionAssignablesProxy key)
	{
		return this.GetSetPermission(key.GetComponent<KPrefabID>());
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x00176788 File Offset: 0x00174988
	private AccessControl.Permission GetSetPermission(KPrefabID kpid)
	{
		AccessControl.Permission result = this.DefaultPermission;
		if (kpid != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == kpid.InstanceID)
				{
					result = this.savedPermissions[i].Value;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060042F3 RID: 17139 RVA: 0x001767F4 File Offset: 0x001749F4
	public void ClearPermission(MinionAssignablesProxy key)
	{
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
				{
					this.savedPermissions.RemoveAt(i);
					break;
				}
			}
		}
		this.SetStatusItem();
		this.ClearGridRestrictions(component);
	}

	// Token: 0x060042F4 RID: 17140 RVA: 0x00176864 File Offset: 0x00174A64
	public bool IsDefaultPermission(MinionAssignablesProxy key)
	{
		bool flag = false;
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
				{
					flag = true;
					break;
				}
			}
		}
		return !flag;
	}

	// Token: 0x060042F5 RID: 17141 RVA: 0x001768C4 File Offset: 0x00174AC4
	private void SetStatusItem()
	{
		if (this._defaultPermission != AccessControl.Permission.Both || this.savedPermissions.Count > 0)
		{
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, AccessControl.accessControlActive, null);
			return;
		}
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, null, null);
	}

	// Token: 0x060042F6 RID: 17142 RVA: 0x00176928 File Offset: 0x00174B28
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ACCESS_CONTROL, UI.BUILDINGEFFECTS.TOOLTIPS.ACCESS_CONTROL, Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04002B9C RID: 11164
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002B9D RID: 11165
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002B9E RID: 11166
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002B9F RID: 11167
	private bool isTeleporter;

	// Token: 0x04002BA0 RID: 11168
	private int[] registeredBuildingCells;

	// Token: 0x04002BA1 RID: 11169
	[Serialize]
	private List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> savedPermissions = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();

	// Token: 0x04002BA2 RID: 11170
	[Serialize]
	private AccessControl.Permission _defaultPermission;

	// Token: 0x04002BA3 RID: 11171
	[Serialize]
	public bool registered = true;

	// Token: 0x04002BA4 RID: 11172
	[Serialize]
	public bool controlEnabled;

	// Token: 0x04002BA5 RID: 11173
	public Door.ControlState overrideAccess;

	// Token: 0x04002BA6 RID: 11174
	private static StatusItem accessControlActive;

	// Token: 0x04002BA7 RID: 11175
	private static readonly EventSystem.IntraObjectHandler<AccessControl> OnControlStateChangedDelegate = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data)
	{
		component.OnControlStateChanged(data);
	});

	// Token: 0x04002BA8 RID: 11176
	private static readonly EventSystem.IntraObjectHandler<AccessControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001758 RID: 5976
	public enum Permission
	{
		// Token: 0x04006E70 RID: 28272
		Both,
		// Token: 0x04006E71 RID: 28273
		GoLeft,
		// Token: 0x04006E72 RID: 28274
		GoRight,
		// Token: 0x04006E73 RID: 28275
		Neither
	}
}
