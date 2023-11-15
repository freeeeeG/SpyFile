using System;
using UnityEngine;

// Token: 0x02000BFC RID: 3068
public class AutoPlumberSideScreen : SideScreenContent
{
	// Token: 0x06006117 RID: 24855 RVA: 0x0023E11C File Offset: 0x0023C31C
	protected override void OnSpawn()
	{
		this.activateButton.onClick += delegate()
		{
			DevAutoPlumber.AutoPlumbBuilding(this.building);
		};
		this.powerButton.onClick += delegate()
		{
			DevAutoPlumber.DoElectricalPlumbing(this.building);
		};
		this.pipesButton.onClick += delegate()
		{
			DevAutoPlumber.DoLiquidAndGasPlumbing(this.building);
		};
		this.solidsButton.onClick += delegate()
		{
			DevAutoPlumber.SetupSolidOreDelivery(this.building);
		};
		this.minionButton.onClick += delegate()
		{
			this.SpawnMinion();
		};
		this.applyTestFacade.onClick += delegate()
		{
			this.CycleAvailableFacades();
		};
	}

	// Token: 0x06006118 RID: 24856 RVA: 0x0023E1B4 File Offset: 0x0023C3B4
	private void SpawnMinion()
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID), null, null);
		gameObject.name = Assets.GetPrefab(MinionConfig.ID).name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPos(Grid.PosToCell(this.building), CellAlignment.Bottom, Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		new MinionStartingStats(false, null, null, true).Apply(gameObject);
	}

	// Token: 0x06006119 RID: 24857 RVA: 0x0023E234 File Offset: 0x0023C434
	public override int GetSideScreenSortOrder()
	{
		return -150;
	}

	// Token: 0x0600611A RID: 24858 RVA: 0x0023E23B File Offset: 0x0023C43B
	public override bool IsValidForTarget(GameObject target)
	{
		return DebugHandler.InstantBuildMode && target.GetComponent<Building>() != null;
	}

	// Token: 0x0600611B RID: 24859 RVA: 0x0023E252 File Offset: 0x0023C452
	public override void SetTarget(GameObject target)
	{
		this.building = target.GetComponent<Building>();
		this.Refresh();
	}

	// Token: 0x0600611C RID: 24860 RVA: 0x0023E266 File Offset: 0x0023C466
	public override void ClearTarget()
	{
	}

	// Token: 0x0600611D RID: 24861 RVA: 0x0023E268 File Offset: 0x0023C468
	private void Refresh()
	{
		bool active = this.building != null && this.building.Def.AvailableFacades.Count > 0;
		this.applyTestFacade.gameObject.SetActive(active);
	}

	// Token: 0x0600611E RID: 24862 RVA: 0x0023E2B0 File Offset: 0x0023C4B0
	private void CycleAvailableFacades()
	{
		BuildingFacade component = this.building.GetComponent<BuildingFacade>();
		if (component != null)
		{
			string nextFacade = component.GetNextFacade();
			component.ApplyBuildingFacade(Db.GetBuildingFacades().TryGet(nextFacade));
		}
	}

	// Token: 0x04004228 RID: 16936
	public KButton activateButton;

	// Token: 0x04004229 RID: 16937
	public KButton powerButton;

	// Token: 0x0400422A RID: 16938
	public KButton pipesButton;

	// Token: 0x0400422B RID: 16939
	public KButton solidsButton;

	// Token: 0x0400422C RID: 16940
	public KButton minionButton;

	// Token: 0x0400422D RID: 16941
	public KButton applyTestFacade;

	// Token: 0x0400422E RID: 16942
	private Building building;
}
