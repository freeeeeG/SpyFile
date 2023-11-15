using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000927 RID: 2343
public class RequiresFoundation : KGameObjectComponentManager<RequiresFoundation.Data>, IKComponentManager
{
	// Token: 0x060043F9 RID: 17401 RVA: 0x0017D1B4 File Offset: 0x0017B3B4
	public HandleVector<int>.Handle Add(GameObject go)
	{
		BuildingDef def = go.GetComponent<Building>().Def;
		int cell = Grid.PosToCell(go.transform.GetPosition());
		RequiresFoundation.Data data = new RequiresFoundation.Data
		{
			cell = cell,
			width = def.WidthInCells,
			height = def.HeightInCells,
			buildRule = def.BuildLocationRule,
			solid = true,
			go = go
		};
		HandleVector<int>.Handle h = base.Add(go, data);
		if (def.ContinuouslyCheckFoundation)
		{
			data.changeCallback = delegate(object d)
			{
				this.OnSolidChanged(h);
			};
			Rotatable component = data.go.GetComponent<Rotatable>();
			Orientation orientation = (component != null) ? component.GetOrientation() : Orientation.Neutral;
			int num = -(def.WidthInCells - 1) / 2;
			int x = def.WidthInCells / 2;
			CellOffset offset = new CellOffset(num, -1);
			CellOffset offset2 = new CellOffset(x, -1);
			if (def.BuildLocationRule == BuildLocationRule.OnCeiling || def.BuildLocationRule == BuildLocationRule.InCorner)
			{
				offset.y = def.HeightInCells;
				offset2.y = def.HeightInCells;
			}
			else if (def.BuildLocationRule == BuildLocationRule.OnWall)
			{
				offset = new CellOffset(num - 1, 0);
				offset2 = new CellOffset(num - 1, def.HeightInCells);
			}
			else if (def.BuildLocationRule == BuildLocationRule.WallFloor)
			{
				offset = new CellOffset(num - 1, -1);
				offset2 = new CellOffset(x, def.HeightInCells - 1);
			}
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offset, orientation);
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(offset2, orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			Vector2I vector2I = Grid.CellToXY(cell2);
			Vector2I vector2I2 = Grid.CellToXY(cell3);
			float xmin = (float)Mathf.Min(vector2I.x, vector2I2.x);
			float xmax = (float)Mathf.Max(vector2I.x, vector2I2.x);
			float ymin = (float)Mathf.Min(vector2I.y, vector2I2.y);
			float ymax = (float)Mathf.Max(vector2I.y, vector2I2.y);
			Rect rect = Rect.MinMaxRect(xmin, ymin, xmax, ymax);
			data.solidPartitionerEntry = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", go, (int)rect.x, (int)rect.y, (int)rect.width + 1, (int)rect.height + 1, GameScenePartitioner.Instance.solidChangedLayer, data.changeCallback);
			data.buildingPartitionerEntry = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", go, (int)rect.x, (int)rect.y, (int)rect.width + 1, (int)rect.height + 1, GameScenePartitioner.Instance.objectLayers[1], data.changeCallback);
			if (def.BuildLocationRule == BuildLocationRule.BuildingAttachPoint || def.BuildLocationRule == BuildLocationRule.OnFloorOrBuildingAttachPoint)
			{
				AttachableBuilding component2 = data.go.GetComponent<AttachableBuilding>();
				component2.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(component2.onAttachmentNetworkChanged, data.changeCallback);
			}
			base.SetData(h, data);
			this.OnSolidChanged(h);
			data = base.GetData(h);
			this.UpdateSolidState(data.solid, ref data, true);
		}
		return h;
	}

	// Token: 0x060043FA RID: 17402 RVA: 0x0017D4D8 File Offset: 0x0017B6D8
	protected override void OnCleanUp(HandleVector<int>.Handle h)
	{
		RequiresFoundation.Data data = base.GetData(h);
		GameScenePartitioner.Instance.Free(ref data.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref data.buildingPartitionerEntry);
		AttachableBuilding component = data.go.GetComponent<AttachableBuilding>();
		if (!component.IsNullOrDestroyed())
		{
			AttachableBuilding attachableBuilding = component;
			attachableBuilding.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachableBuilding.onAttachmentNetworkChanged, data.changeCallback);
		}
		base.SetData(h, data);
	}

	// Token: 0x060043FB RID: 17403 RVA: 0x0017D548 File Offset: 0x0017B748
	private void OnSolidChanged(HandleVector<int>.Handle h)
	{
		RequiresFoundation.Data data = base.GetData(h);
		SimCellOccupier component = data.go.GetComponent<SimCellOccupier>();
		if (component == null || component.IsReady())
		{
			Rotatable component2 = data.go.GetComponent<Rotatable>();
			Orientation orientation = (component2 != null) ? component2.GetOrientation() : Orientation.Neutral;
			bool flag = BuildingDef.CheckFoundation(data.cell, orientation, data.buildRule, data.width, data.height, default(Tag));
			if (!flag && (data.buildRule == BuildLocationRule.BuildingAttachPoint || data.buildRule == BuildLocationRule.OnFloorOrBuildingAttachPoint))
			{
				List<GameObject> list = new List<GameObject>();
				AttachableBuilding.GetAttachedBelow(data.go.GetComponent<AttachableBuilding>(), ref list);
				if (list.Count > 0)
				{
					Operational component3 = list.Last<GameObject>().GetComponent<Operational>();
					if (component3 != null && component3.GetFlag(RequiresFoundation.solidFoundation))
					{
						flag = true;
					}
				}
			}
			this.UpdateSolidState(flag, ref data, false);
			base.SetData(h, data);
		}
	}

	// Token: 0x060043FC RID: 17404 RVA: 0x0017D640 File Offset: 0x0017B840
	private void UpdateSolidState(bool is_solid, ref RequiresFoundation.Data data, bool forceUpdate = false)
	{
		if (data.solid != is_solid || forceUpdate)
		{
			data.solid = is_solid;
			Operational component = data.go.GetComponent<Operational>();
			if (component != null)
			{
				component.SetFlag(RequiresFoundation.solidFoundation, is_solid);
			}
			AttachableBuilding component2 = data.go.GetComponent<AttachableBuilding>();
			if (component2 != null)
			{
				List<GameObject> buildings = new List<GameObject>();
				AttachableBuilding.GetAttachedAbove(component2, ref buildings);
				AttachableBuilding.NotifyBuildingsNetworkChanged(buildings, null);
			}
			data.go.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.MissingFoundation, !is_solid, this);
		}
	}

	// Token: 0x04002D1F RID: 11551
	public static readonly Operational.Flag solidFoundation = new Operational.Flag("solid_foundation", Operational.Flag.Type.Functional);

	// Token: 0x02001769 RID: 5993
	public struct Data
	{
		// Token: 0x04006EAA RID: 28330
		public int cell;

		// Token: 0x04006EAB RID: 28331
		public int width;

		// Token: 0x04006EAC RID: 28332
		public int height;

		// Token: 0x04006EAD RID: 28333
		public BuildLocationRule buildRule;

		// Token: 0x04006EAE RID: 28334
		public HandleVector<int>.Handle solidPartitionerEntry;

		// Token: 0x04006EAF RID: 28335
		public HandleVector<int>.Handle buildingPartitionerEntry;

		// Token: 0x04006EB0 RID: 28336
		public bool solid;

		// Token: 0x04006EB1 RID: 28337
		public GameObject go;

		// Token: 0x04006EB2 RID: 28338
		public Action<object> changeCallback;
	}
}
