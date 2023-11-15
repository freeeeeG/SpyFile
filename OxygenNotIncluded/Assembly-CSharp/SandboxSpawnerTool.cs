using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200082A RID: 2090
public class SandboxSpawnerTool : InterfaceTool
{
	// Token: 0x06003CA1 RID: 15521 RVA: 0x0014FF31 File Offset: 0x0014E131
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		colors.Add(new ToolMenu.CellColorData(this.currentCell, this.radiusIndicatorColor));
	}

	// Token: 0x06003CA2 RID: 15522 RVA: 0x0014FF53 File Offset: 0x0014E153
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.currentCell = Grid.PosToCell(cursorPos);
	}

	// Token: 0x06003CA3 RID: 15523 RVA: 0x0014FF68 File Offset: 0x0014E168
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		this.Place(Grid.PosToCell(cursor_pos));
	}

	// Token: 0x06003CA4 RID: 15524 RVA: 0x0014FF78 File Offset: 0x0014E178
	private void Place(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedEntity");
		GameObject prefab = Assets.GetPrefab(stringSetting);
		if (stringSetting == MinionConfig.ID)
		{
			this.SpawnMinion();
		}
		else if (prefab.GetComponent<Building>() != null)
		{
			BuildingDef def = prefab.GetComponent<Building>().Def;
			def.Build(cell, Orientation.Neutral, null, def.DefaultElements(), 298.15f, true, -1f);
		}
		else
		{
			GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(this.currentCell, Grid.SceneLayer.Creatures), Grid.SceneLayer.Creatures, null, 0).SetActive(true);
		}
		GameUtil.KInstantiate(this.fxPrefab, Grid.CellToPosCCC(this.currentCell, Grid.SceneLayer.FXFront), Grid.SceneLayer.FXFront, null, 0).GetComponent<KAnimControllerBase>().Play("placer", KAnim.PlayMode.Once, 1f, 0f);
		KFMOD.PlayUISound(this.soundPath);
	}

	// Token: 0x06003CA5 RID: 15525 RVA: 0x0015005C File Offset: 0x0014E25C
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.entitySelector.row.SetActive(true);
	}

	// Token: 0x06003CA6 RID: 15526 RVA: 0x00150093 File Offset: 0x0014E293
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06003CA7 RID: 15527 RVA: 0x001500AC File Offset: 0x0014E2AC
	private void SpawnMinion()
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID), null, null);
		gameObject.name = Assets.GetPrefab(MinionConfig.ID).name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPosCBC(this.currentCell, Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		new MinionStartingStats(false, null, null, false).Apply(gameObject);
		gameObject.GetMyWorld().SetDupeVisited();
	}

	// Token: 0x06003CA8 RID: 15528 RVA: 0x00150134 File Offset: 0x0014E334
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.SandboxCopyElement))
		{
			int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
			List<ObjectLayer> list = new List<ObjectLayer>();
			list.Add(ObjectLayer.Pickupables);
			list.Add(ObjectLayer.Plants);
			list.Add(ObjectLayer.Minion);
			list.Add(ObjectLayer.Building);
			if (Grid.IsValidCell(cell))
			{
				foreach (ObjectLayer layer in list)
				{
					GameObject gameObject = Grid.Objects[cell, (int)layer];
					if (gameObject)
					{
						SandboxToolParameterMenu.instance.settings.SetStringSetting("SandboxTools.SelectedEntity", gameObject.PrefabID().ToString());
						break;
					}
				}
			}
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x040027A9 RID: 10153
	protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x040027AA RID: 10154
	private int currentCell;

	// Token: 0x040027AB RID: 10155
	private string soundPath = GlobalAssets.GetSound("SandboxTool_Spawner", false);

	// Token: 0x040027AC RID: 10156
	[SerializeField]
	private GameObject fxPrefab;
}
