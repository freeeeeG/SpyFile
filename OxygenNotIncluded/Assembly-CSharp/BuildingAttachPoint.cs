using System;
using UnityEngine;

// Token: 0x020005A5 RID: 1445
[AddComponentMenu("KMonoBehaviour/scripts/BuildingAttachPoint")]
public class BuildingAttachPoint : KMonoBehaviour
{
	// Token: 0x06002352 RID: 9042 RVA: 0x000C16B6 File Offset: 0x000BF8B6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.BuildingAttachPoints.Add(this);
		this.TryAttachEmptyHardpoints();
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000C16CF File Offset: 0x000BF8CF
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x000C16D8 File Offset: 0x000BF8D8
	private void TryAttachEmptyHardpoints()
	{
		for (int i = 0; i < this.points.Length; i++)
		{
			if (!(this.points[i].attachedBuilding != null))
			{
				bool flag = false;
				int num = 0;
				while (num < Components.AttachableBuildings.Count && !flag)
				{
					if (Components.AttachableBuildings[num].attachableToTag == this.points[i].attachableType && Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.points[i].position) == Grid.PosToCell(Components.AttachableBuildings[num]))
					{
						this.points[i].attachedBuilding = Components.AttachableBuildings[num];
						flag = true;
					}
					num++;
				}
			}
		}
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000C17B0 File Offset: 0x000BF9B0
	public bool AcceptsAttachment(Tag type, int cell)
	{
		int cell2 = Grid.PosToCell(base.gameObject);
		for (int i = 0; i < this.points.Length; i++)
		{
			if (Grid.OffsetCell(cell2, this.points[i].position) == cell && this.points[i].attachableType == type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x000C1812 File Offset: 0x000BFA12
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BuildingAttachPoints.Remove(this);
	}

	// Token: 0x04001426 RID: 5158
	public BuildingAttachPoint.HardPoint[] points = new BuildingAttachPoint.HardPoint[0];

	// Token: 0x02001231 RID: 4657
	[Serializable]
	public struct HardPoint
	{
		// Token: 0x06007C28 RID: 31784 RVA: 0x002E033D File Offset: 0x002DE53D
		public HardPoint(CellOffset position, Tag attachableType, AttachableBuilding attachedBuilding)
		{
			this.position = position;
			this.attachableType = attachableType;
			this.attachedBuilding = attachedBuilding;
		}

		// Token: 0x04005EBF RID: 24255
		public CellOffset position;

		// Token: 0x04005EC0 RID: 24256
		public Tag attachableType;

		// Token: 0x04005EC1 RID: 24257
		public AttachableBuilding attachedBuilding;
	}
}
