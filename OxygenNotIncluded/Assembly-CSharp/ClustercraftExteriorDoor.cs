using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000997 RID: 2455
public class ClustercraftExteriorDoor : KMonoBehaviour
{
	// Token: 0x060048D2 RID: 18642 RVA: 0x0019A624 File Offset: 0x00198824
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.targetWorldId < 0)
		{
			GameObject gameObject = base.GetComponent<RocketModuleCluster>().CraftInterface.gameObject;
			WorldContainer worldContainer = ClusterManager.Instance.CreateRocketInteriorWorld(gameObject, this.interiorTemplateName, delegate
			{
				this.PairWithInteriorDoor();
			});
			if (worldContainer != null)
			{
				this.targetWorldId = worldContainer.id;
			}
		}
		else
		{
			this.PairWithInteriorDoor();
		}
		base.Subscribe<ClustercraftExteriorDoor>(-1277991738, ClustercraftExteriorDoor.OnLaunchDelegate);
		base.Subscribe<ClustercraftExteriorDoor>(-887025858, ClustercraftExteriorDoor.OnLandDelegate);
	}

	// Token: 0x060048D3 RID: 18643 RVA: 0x0019A6AE File Offset: 0x001988AE
	protected override void OnCleanUp()
	{
		ClusterManager.Instance.DestoryRocketInteriorWorld(this.targetWorldId, this);
		base.OnCleanUp();
	}

	// Token: 0x060048D4 RID: 18644 RVA: 0x0019A6C8 File Offset: 0x001988C8
	private void PairWithInteriorDoor()
	{
		foreach (object obj in Components.ClusterCraftInteriorDoors)
		{
			ClustercraftInteriorDoor clustercraftInteriorDoor = (ClustercraftInteriorDoor)obj;
			if (clustercraftInteriorDoor.GetMyWorldId() == this.targetWorldId)
			{
				this.SetTarget(clustercraftInteriorDoor);
				break;
			}
		}
		if (this.targetDoor == null)
		{
			global::Debug.LogWarning("No ClusterCraftInteriorDoor found on world");
		}
		WorldContainer targetWorld = this.GetTargetWorld();
		int myWorldId = this.GetMyWorldId();
		if (targetWorld != null && myWorldId != -1)
		{
			targetWorld.SetParentIdx(myWorldId);
		}
		if (base.gameObject.GetComponent<KSelectable>().IsSelected)
		{
			RocketModuleSideScreen.instance.UpdateButtonStates();
		}
		base.Trigger(-1118736034, null);
		targetWorld.gameObject.Trigger(-1118736034, null);
	}

	// Token: 0x060048D5 RID: 18645 RVA: 0x0019A7A8 File Offset: 0x001989A8
	public void SetTarget(ClustercraftInteriorDoor target)
	{
		this.targetDoor = target;
		target.GetComponent<AssignmentGroupController>().SetGroupID(base.GetComponent<AssignmentGroupController>().AssignmentGroupID);
		base.GetComponent<NavTeleporter>().TwoWayTarget(target.GetComponent<NavTeleporter>());
	}

	// Token: 0x060048D6 RID: 18646 RVA: 0x0019A7D8 File Offset: 0x001989D8
	public bool HasTargetWorld()
	{
		return this.targetDoor != null;
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x0019A7E6 File Offset: 0x001989E6
	public WorldContainer GetTargetWorld()
	{
		global::Debug.Assert(this.targetDoor != null, "Clustercraft Exterior Door has no targetDoor");
		return this.targetDoor.GetMyWorld();
	}

	// Token: 0x060048D8 RID: 18648 RVA: 0x0019A80C File Offset: 0x00198A0C
	public void FerryMinion(GameObject minion)
	{
		Vector3 b = Vector3.left * 3f;
		minion.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(this.targetDoor.transform.position + b), CellAlignment.Bottom, Grid.SceneLayer.Move));
		ClusterManager.Instance.MigrateMinion(minion.GetComponent<MinionIdentity>(), this.targetDoor.GetMyWorldId());
	}

	// Token: 0x060048D9 RID: 18649 RVA: 0x0019A874 File Offset: 0x00198A74
	private void OnLaunch(object data)
	{
		NavTeleporter component = base.GetComponent<NavTeleporter>();
		component.EnableTwoWayTarget(false);
		component.Deregister();
		WorldContainer targetWorld = this.GetTargetWorld();
		if (targetWorld != null)
		{
			targetWorld.SetParentIdx(targetWorld.id);
		}
	}

	// Token: 0x060048DA RID: 18650 RVA: 0x0019A8B0 File Offset: 0x00198AB0
	private void OnLand(object data)
	{
		base.GetComponent<NavTeleporter>().EnableTwoWayTarget(true);
		WorldContainer targetWorld = this.GetTargetWorld();
		if (targetWorld != null)
		{
			int myWorldId = this.GetMyWorldId();
			targetWorld.SetParentIdx(myWorldId);
		}
	}

	// Token: 0x060048DB RID: 18651 RVA: 0x0019A8E7 File Offset: 0x00198AE7
	public int TargetCell()
	{
		return this.targetDoor.GetComponent<NavTeleporter>().GetCell();
	}

	// Token: 0x060048DC RID: 18652 RVA: 0x0019A8F9 File Offset: 0x00198AF9
	public ClustercraftInteriorDoor GetInteriorDoor()
	{
		return this.targetDoor;
	}

	// Token: 0x04003015 RID: 12309
	public string interiorTemplateName;

	// Token: 0x04003016 RID: 12310
	private ClustercraftInteriorDoor targetDoor;

	// Token: 0x04003017 RID: 12311
	[Serialize]
	private int targetWorldId = -1;

	// Token: 0x04003018 RID: 12312
	private static readonly EventSystem.IntraObjectHandler<ClustercraftExteriorDoor> OnLaunchDelegate = new EventSystem.IntraObjectHandler<ClustercraftExteriorDoor>(delegate(ClustercraftExteriorDoor component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x04003019 RID: 12313
	private static readonly EventSystem.IntraObjectHandler<ClustercraftExteriorDoor> OnLandDelegate = new EventSystem.IntraObjectHandler<ClustercraftExteriorDoor>(delegate(ClustercraftExteriorDoor component, object data)
	{
		component.OnLand(data);
	});
}
