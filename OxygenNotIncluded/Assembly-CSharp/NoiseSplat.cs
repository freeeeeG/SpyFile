using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000476 RID: 1142
public class NoiseSplat : IUniformGridObject
{
	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060018EE RID: 6382 RVA: 0x000821FA File Offset: 0x000803FA
	// (set) Token: 0x060018EF RID: 6383 RVA: 0x00082202 File Offset: 0x00080402
	public int dB { get; private set; }

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x060018F0 RID: 6384 RVA: 0x0008220B File Offset: 0x0008040B
	// (set) Token: 0x060018F1 RID: 6385 RVA: 0x00082213 File Offset: 0x00080413
	public float deathTime { get; private set; }

	// Token: 0x060018F2 RID: 6386 RVA: 0x0008221C File Offset: 0x0008041C
	public string GetName()
	{
		return this.provider.GetName();
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x00082229 File Offset: 0x00080429
	public IPolluter GetProvider()
	{
		return this.provider;
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x00082231 File Offset: 0x00080431
	public Vector2 PosMin()
	{
		return new Vector2(this.position.x - (float)this.radius, this.position.y - (float)this.radius);
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x0008225E File Offset: 0x0008045E
	public Vector2 PosMax()
	{
		return new Vector2(this.position.x + (float)this.radius, this.position.y + (float)this.radius);
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x0008228C File Offset: 0x0008048C
	public NoiseSplat(NoisePolluter setProvider, float death_time = 0f)
	{
		this.deathTime = death_time;
		this.dB = 0;
		this.radius = 5;
		if (setProvider.dB != null)
		{
			this.dB = (int)setProvider.dB.GetTotalValue();
		}
		int cell = Grid.PosToCell(setProvider.gameObject);
		if (!NoisePolluter.IsNoiseableCell(cell))
		{
			this.dB = 0;
		}
		if (this.dB == 0)
		{
			return;
		}
		setProvider.Clear();
		OccupyArea occupyArea = setProvider.occupyArea;
		this.baseExtents = occupyArea.GetExtents();
		this.provider = setProvider;
		this.position = setProvider.transform.GetPosition();
		if (setProvider.dBRadius != null)
		{
			this.radius = (int)setProvider.dBRadius.GetTotalValue();
		}
		if (this.radius == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int widthInCells = occupyArea.GetWidthInCells();
		int heightInCells = occupyArea.GetHeightInCells();
		Vector2I vector2I = new Vector2I(num - this.radius, num2 - this.radius);
		Vector2I vector2I2 = vector2I + new Vector2I(this.radius * 2 + widthInCells, this.radius * 2 + heightInCells);
		vector2I = Vector2I.Max(vector2I, Vector2I.zero);
		vector2I2 = Vector2I.Min(vector2I2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
		this.effectExtents = new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatCollectNoisePolluters", setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.noisePolluterLayer, setProvider.onCollectNoisePollutersCallback);
		this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatSolidCheck", setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.solidChangedLayer, setProvider.refreshPartionerCallback);
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x00082474 File Offset: 0x00080674
	public NoiseSplat(IPolluter setProvider, float death_time = 0f)
	{
		this.deathTime = death_time;
		this.provider = setProvider;
		this.provider.Clear();
		this.position = this.provider.GetPosition();
		this.dB = this.provider.GetNoise();
		int cell = Grid.PosToCell(this.position);
		if (!NoisePolluter.IsNoiseableCell(cell))
		{
			this.dB = 0;
		}
		if (this.dB == 0)
		{
			return;
		}
		this.radius = this.provider.GetRadius();
		if (this.radius == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		Vector2I vector2I = new Vector2I(num - this.radius, num2 - this.radius);
		Vector2I vector2I2 = vector2I + new Vector2I(this.radius * 2, this.radius * 2);
		vector2I = Vector2I.Max(vector2I, Vector2I.zero);
		vector2I2 = Vector2I.Min(vector2I2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
		this.effectExtents = new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
		this.baseExtents = new Extents(num, num2, 1, 1);
		this.AddNoise();
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x000825BD File Offset: 0x000807BD
	public void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
		this.RemoveNoise();
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x000825E8 File Offset: 0x000807E8
	private void AddNoise()
	{
		int cell = Grid.PosToCell(this.position);
		int num = this.effectExtents.x + this.effectExtents.width;
		int num2 = this.effectExtents.y + this.effectExtents.height;
		int num3 = this.effectExtents.x;
		int num4 = this.effectExtents.y;
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		num = Math.Min(num, Grid.WidthInCells);
		num2 = Math.Min(num2, Grid.HeightInCells);
		num3 = Math.Max(0, num3);
		num4 = Math.Max(0, num4);
		for (int i = num4; i < num2; i++)
		{
			for (int j = num3; j < num; j++)
			{
				if (Grid.VisibilityTest(x, y, j, i, false))
				{
					int num5 = Grid.XYToCell(j, i);
					float dbforCell = this.GetDBForCell(num5);
					if (dbforCell > 0f)
					{
						float num6 = AudioEventManager.DBToLoudness(dbforCell);
						Grid.Loudness[num5] += num6;
						Pair<int, float> item = new Pair<int, float>(num5, num6);
						this.decibels.Add(item);
					}
				}
			}
		}
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x00082700 File Offset: 0x00080900
	public float GetDBForCell(int cell)
	{
		Vector2 vector = Grid.CellToPos2D(cell);
		float num = Mathf.Floor(Vector2.Distance(this.position, vector));
		if (vector.x >= (float)this.baseExtents.x && vector.x < (float)(this.baseExtents.x + this.baseExtents.width) && vector.y >= (float)this.baseExtents.y && vector.y < (float)(this.baseExtents.y + this.baseExtents.height))
		{
			num = 0f;
		}
		return Mathf.Round((float)this.dB - (float)this.dB * num * 0.05f);
	}

	// Token: 0x060018FB RID: 6395 RVA: 0x000827B8 File Offset: 0x000809B8
	private void RemoveNoise()
	{
		for (int i = 0; i < this.decibels.Count; i++)
		{
			Pair<int, float> pair = this.decibels[i];
			float num = Math.Max(0f, Grid.Loudness[pair.first] - pair.second);
			Grid.Loudness[pair.first] = ((num < 1f) ? 0f : num);
		}
		this.decibels.Clear();
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x00082830 File Offset: 0x00080A30
	public float GetLoudness(int cell)
	{
		float result = 0f;
		for (int i = 0; i < this.decibels.Count; i++)
		{
			Pair<int, float> pair = this.decibels[i];
			if (pair.first == cell)
			{
				result = pair.second;
				break;
			}
		}
		return result;
	}

	// Token: 0x04000DC3 RID: 3523
	public const float noiseFalloff = 0.05f;

	// Token: 0x04000DC6 RID: 3526
	private IPolluter provider;

	// Token: 0x04000DC7 RID: 3527
	private Vector2 position;

	// Token: 0x04000DC8 RID: 3528
	private int radius;

	// Token: 0x04000DC9 RID: 3529
	private Extents effectExtents;

	// Token: 0x04000DCA RID: 3530
	private Extents baseExtents;

	// Token: 0x04000DCB RID: 3531
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04000DCC RID: 3532
	private HandleVector<int>.Handle solidChangedPartitionerEntry;

	// Token: 0x04000DCD RID: 3533
	private List<Pair<int, float>> decibels = new List<Pair<int, float>>();
}
