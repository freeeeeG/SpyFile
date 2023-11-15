using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200091C RID: 2332
[Serializable]
public class RadiationGridEmitter
{
	// Token: 0x06004396 RID: 17302 RVA: 0x0017AA64 File Offset: 0x00178C64
	public RadiationGridEmitter(int originCell, int intensity)
	{
		this.originCell = originCell;
		this.intensity = intensity;
	}

	// Token: 0x06004397 RID: 17303 RVA: 0x0017AAB4 File Offset: 0x00178CB4
	public void Emit()
	{
		this.scanCells.Clear();
		Vector2 a = Grid.CellToPosCCC(this.originCell, Grid.SceneLayer.Building);
		for (float num = (float)this.direction - (float)this.angle / 2f; num < (float)this.direction + (float)this.angle / 2f; num += (float)(this.angle / this.projectionCount))
		{
			float num2 = UnityEngine.Random.Range((float)(-(float)this.angle / this.projectionCount) / 2f, (float)(this.angle / this.projectionCount) / 2f);
			Vector2 vector = new Vector2(Mathf.Cos((num + num2) * 3.1415927f / 180f), Mathf.Sin((num + num2) * 3.1415927f / 180f));
			int num3 = 3;
			float num4 = (float)(this.intensity / 4);
			Vector2 a2 = vector;
			float num5 = 0f;
			while ((double)num4 > 0.01 && num5 < (float)RadiationGridEmitter.MAX_EMIT_DISTANCE)
			{
				num5 += 1f / (float)num3;
				int num6 = Grid.PosToCell(a + a2 * num5);
				if (!Grid.IsValidCell(num6))
				{
					break;
				}
				if (!this.scanCells.Contains(num6))
				{
					SimMessages.ModifyRadiationOnCell(num6, (float)Mathf.RoundToInt(num4), -1);
					this.scanCells.Add(num6);
				}
				num4 *= Mathf.Max(0f, 1f - Mathf.Pow(Grid.Mass[num6], 1.25f) * Grid.Element[num6].molarMass / 1000000f);
				num4 *= UnityEngine.Random.Range(0.96f, 0.98f);
			}
		}
	}

	// Token: 0x06004398 RID: 17304 RVA: 0x0017AC67 File Offset: 0x00178E67
	private int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x04002CC2 RID: 11458
	private static int MAX_EMIT_DISTANCE = 128;

	// Token: 0x04002CC3 RID: 11459
	public int originCell = -1;

	// Token: 0x04002CC4 RID: 11460
	public int intensity = 1;

	// Token: 0x04002CC5 RID: 11461
	public int projectionCount = 20;

	// Token: 0x04002CC6 RID: 11462
	public int direction;

	// Token: 0x04002CC7 RID: 11463
	public int angle = 360;

	// Token: 0x04002CC8 RID: 11464
	public bool enabled;

	// Token: 0x04002CC9 RID: 11465
	private HashSet<int> scanCells = new HashSet<int>();
}
