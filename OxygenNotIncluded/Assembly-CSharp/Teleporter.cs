using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020006A2 RID: 1698
public class Teleporter : KMonoBehaviour
{
	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06002DDC RID: 11740 RVA: 0x000F2AB1 File Offset: 0x000F0CB1
	// (set) Token: 0x06002DDD RID: 11741 RVA: 0x000F2AB9 File Offset: 0x000F0CB9
	[Serialize]
	public int teleporterID { get; private set; }

	// Token: 0x06002DDE RID: 11742 RVA: 0x000F2AC2 File Offset: 0x000F0CC2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002DDF RID: 11743 RVA: 0x000F2ACA File Offset: 0x000F0CCA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Teleporters.Add(this);
		this.SetTeleporterID(0);
		base.Subscribe<Teleporter>(-801688580, Teleporter.OnLogicValueChangedDelegate);
	}

	// Token: 0x06002DE0 RID: 11744 RVA: 0x000F2AF8 File Offset: 0x000F0CF8
	private void OnLogicValueChanged(object data)
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		List<int> list = new List<int>();
		int num = 0;
		int num2 = Mathf.Min(this.ID_LENGTH, component.inputPorts.Count);
		for (int i = 0; i < num2; i++)
		{
			int logicUICell = component.inputPorts[i].GetLogicUICell();
			LogicCircuitNetwork networkForCell = logicCircuitManager.GetNetworkForCell(logicUICell);
			int item = (networkForCell != null) ? networkForCell.OutputValue : 1;
			list.Add(item);
		}
		foreach (int num3 in list)
		{
			num = (num << 1 | num3);
		}
		this.SetTeleporterID(num);
	}

	// Token: 0x06002DE1 RID: 11745 RVA: 0x000F2BC8 File Offset: 0x000F0DC8
	protected override void OnCleanUp()
	{
		Components.Teleporters.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002DE2 RID: 11746 RVA: 0x000F2BDB File Offset: 0x000F0DDB
	public bool HasTeleporterTarget()
	{
		return this.FindTeleportTarget() != null;
	}

	// Token: 0x06002DE3 RID: 11747 RVA: 0x000F2BE9 File Offset: 0x000F0DE9
	public bool IsValidTeleportTarget(Teleporter from_tele)
	{
		return from_tele.teleporterID == this.teleporterID && this.operational.IsOperational;
	}

	// Token: 0x06002DE4 RID: 11748 RVA: 0x000F2C08 File Offset: 0x000F0E08
	public Teleporter FindTeleportTarget()
	{
		List<Teleporter> list = new List<Teleporter>();
		foreach (object obj in Components.Teleporters)
		{
			Teleporter teleporter = (Teleporter)obj;
			if (teleporter.IsValidTeleportTarget(this) && teleporter != this)
			{
				list.Add(teleporter);
			}
		}
		Teleporter result = null;
		if (list.Count > 0)
		{
			result = list.GetRandom<Teleporter>();
		}
		return result;
	}

	// Token: 0x06002DE5 RID: 11749 RVA: 0x000F2C90 File Offset: 0x000F0E90
	public void SetTeleporterID(int ID)
	{
		this.teleporterID = ID;
		foreach (object obj in Components.Teleporters)
		{
			((Teleporter)obj).Trigger(-1266722732, null);
		}
	}

	// Token: 0x06002DE6 RID: 11750 RVA: 0x000F2CF4 File Offset: 0x000F0EF4
	public void SetTeleportTarget(Teleporter target)
	{
		this.teleportTarget.Set(target);
	}

	// Token: 0x06002DE7 RID: 11751 RVA: 0x000F2D04 File Offset: 0x000F0F04
	public void TeleportObjects()
	{
		Teleporter teleporter = this.teleportTarget.Get();
		int widthInCells = base.GetComponent<Building>().Def.WidthInCells;
		int num = base.GetComponent<Building>().Def.HeightInCells - 1;
		Vector3 position = base.transform.GetPosition();
		if (teleporter != null)
		{
			ListPool<ScenePartitionerEntry, Teleporter>.PooledList pooledList = ListPool<ScenePartitionerEntry, Teleporter>.Allocate();
			GameScenePartitioner.Instance.GatherEntries((int)position.x - widthInCells / 2 + 1, (int)position.y - num / 2 + 1, widthInCells, num, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
			int cell = Grid.PosToCell(teleporter);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				GameObject gameObject = (scenePartitionerEntry.obj as Pickupable).gameObject;
				Vector3 vector = gameObject.transform.GetPosition() - position;
				MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
				if (component != null)
				{
					new EmoteChore(component.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", Telepad.PortalBirthAnim, null);
				}
				else
				{
					vector += Vector3.up;
				}
				gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move) + vector);
			}
			pooledList.Recycle();
		}
		TeleportalPad.StatesInstance smi = this.teleportTarget.Get().GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.doTeleport.Trigger(smi);
		this.teleportTarget.Set(null);
	}

	// Token: 0x04001B0B RID: 6923
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001B0D RID: 6925
	[Serialize]
	public Ref<Teleporter> teleportTarget = new Ref<Teleporter>();

	// Token: 0x04001B0E RID: 6926
	public int ID_LENGTH = 4;

	// Token: 0x04001B0F RID: 6927
	private static readonly EventSystem.IntraObjectHandler<Teleporter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Teleporter>(delegate(Teleporter component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
