using System;
using UnityEngine;

// Token: 0x020007EC RID: 2028
[AddComponentMenu("KMonoBehaviour/scripts/GridVisibility")]
public class GridVisibility : KMonoBehaviour
{
	// Token: 0x060039AF RID: 14767 RVA: 0x00141D8C File Offset: 0x0013FF8C
	protected override void OnSpawn()
	{
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "GridVisibility.OnSpawn");
		this.OnCellChange();
		base.gameObject.GetMyWorld().SetDiscovered(false);
	}

	// Token: 0x060039B0 RID: 14768 RVA: 0x00141DC8 File Offset: 0x0013FFC8
	private void OnCellChange()
	{
		if (base.gameObject.HasTag(GameTags.Dead))
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (!Grid.Revealed[num])
		{
			int baseX;
			int baseY;
			Grid.PosToXY(base.transform.GetPosition(), out baseX, out baseY);
			GridVisibility.Reveal(baseX, baseY, this.radius, this.innerRadius);
			Grid.Revealed[num] = true;
		}
		FogOfWarMask.ClearMask(num);
	}

	// Token: 0x060039B1 RID: 14769 RVA: 0x00141E40 File Offset: 0x00140040
	public static void Reveal(int baseX, int baseY, int radius, float innerRadius)
	{
		int num = (int)Grid.WorldIdx[baseY * Grid.WidthInCells + baseX];
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				int num2 = baseY + i;
				int num3 = baseX + j;
				if (num2 >= 0 && Grid.HeightInCells - 1 >= num2 && num3 >= 0 && Grid.WidthInCells - 1 >= num3)
				{
					int num4 = num2 * Grid.WidthInCells + num3;
					if (Grid.Visible[num4] < 255 && num == (int)Grid.WorldIdx[num4])
					{
						Vector2 vector = new Vector2((float)j, (float)i);
						float num5 = Mathf.Lerp(1f, 0f, (vector.magnitude - innerRadius) / ((float)radius - innerRadius));
						Grid.Reveal(num4, (byte)(255f * num5), false);
					}
				}
			}
		}
	}

	// Token: 0x060039B2 RID: 14770 RVA: 0x00141F0B File Offset: 0x0014010B
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
	}

	// Token: 0x04002675 RID: 9845
	public int radius = 18;

	// Token: 0x04002676 RID: 9846
	public float innerRadius = 16.5f;
}
