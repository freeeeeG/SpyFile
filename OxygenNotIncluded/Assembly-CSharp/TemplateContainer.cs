using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using TemplateClasses;
using UnityEngine;

// Token: 0x0200074A RID: 1866
[Serializable]
public class TemplateContainer
{
	// Token: 0x170003AA RID: 938
	// (get) Token: 0x0600338E RID: 13198 RVA: 0x00112E88 File Offset: 0x00111088
	// (set) Token: 0x0600338F RID: 13199 RVA: 0x00112E90 File Offset: 0x00111090
	public string name { get; set; }

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x06003390 RID: 13200 RVA: 0x00112E99 File Offset: 0x00111099
	// (set) Token: 0x06003391 RID: 13201 RVA: 0x00112EA1 File Offset: 0x001110A1
	public int priority { get; set; }

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06003392 RID: 13202 RVA: 0x00112EAA File Offset: 0x001110AA
	// (set) Token: 0x06003393 RID: 13203 RVA: 0x00112EB2 File Offset: 0x001110B2
	public TemplateContainer.Info info { get; set; }

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x06003394 RID: 13204 RVA: 0x00112EBB File Offset: 0x001110BB
	// (set) Token: 0x06003395 RID: 13205 RVA: 0x00112EC3 File Offset: 0x001110C3
	public List<Cell> cells { get; set; }

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06003396 RID: 13206 RVA: 0x00112ECC File Offset: 0x001110CC
	// (set) Token: 0x06003397 RID: 13207 RVA: 0x00112ED4 File Offset: 0x001110D4
	public List<Prefab> buildings { get; set; }

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x06003398 RID: 13208 RVA: 0x00112EDD File Offset: 0x001110DD
	// (set) Token: 0x06003399 RID: 13209 RVA: 0x00112EE5 File Offset: 0x001110E5
	public List<Prefab> pickupables { get; set; }

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x0600339A RID: 13210 RVA: 0x00112EEE File Offset: 0x001110EE
	// (set) Token: 0x0600339B RID: 13211 RVA: 0x00112EF6 File Offset: 0x001110F6
	public List<Prefab> elementalOres { get; set; }

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x0600339C RID: 13212 RVA: 0x00112EFF File Offset: 0x001110FF
	// (set) Token: 0x0600339D RID: 13213 RVA: 0x00112F07 File Offset: 0x00111107
	public List<Prefab> otherEntities { get; set; }

	// Token: 0x0600339E RID: 13214 RVA: 0x00112F10 File Offset: 0x00111110
	public void Init(List<Cell> _cells, List<Prefab> _buildings, List<Prefab> _pickupables, List<Prefab> _elementalOres, List<Prefab> _otherEntities)
	{
		if (_cells != null && _cells.Count > 0)
		{
			this.cells = _cells;
		}
		if (_buildings != null && _buildings.Count > 0)
		{
			this.buildings = _buildings;
		}
		if (_pickupables != null && _pickupables.Count > 0)
		{
			this.pickupables = _pickupables;
		}
		if (_elementalOres != null && _elementalOres.Count > 0)
		{
			this.elementalOres = _elementalOres;
		}
		if (_otherEntities != null && _otherEntities.Count > 0)
		{
			this.otherEntities = _otherEntities;
		}
		this.info = new TemplateContainer.Info();
		this.RefreshInfo();
	}

	// Token: 0x0600339F RID: 13215 RVA: 0x00112F93 File Offset: 0x00111193
	public RectInt GetTemplateBounds(int padding = 0)
	{
		return this.GetTemplateBounds(Vector2I.zero, padding);
	}

	// Token: 0x060033A0 RID: 13216 RVA: 0x00112FA1 File Offset: 0x001111A1
	public RectInt GetTemplateBounds(Vector2 position, int padding = 0)
	{
		return this.GetTemplateBounds(new Vector2I((int)position.x, (int)position.y), padding);
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x00112FC0 File Offset: 0x001111C0
	public RectInt GetTemplateBounds(Vector2I position, int padding = 0)
	{
		if ((this.info.min - new Vector2f(0, 0)).sqrMagnitude <= 1E-06f)
		{
			this.RefreshInfo();
		}
		return this.info.GetBounds(position, padding);
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x00113008 File Offset: 0x00111208
	public void RefreshInfo()
	{
		if (this.cells == null)
		{
			return;
		}
		int num = 1;
		int num2 = -1;
		int num3 = 1;
		int num4 = -1;
		foreach (Cell cell in this.cells)
		{
			if (cell.location_x < num)
			{
				num = cell.location_x;
			}
			if (cell.location_x > num2)
			{
				num2 = cell.location_x;
			}
			if (cell.location_y < num3)
			{
				num3 = cell.location_y;
			}
			if (cell.location_y > num4)
			{
				num4 = cell.location_y;
			}
		}
		this.info.size = new Vector2((float)(1 + (num2 - num)), (float)(1 + (num4 - num3)));
		this.info.min = new Vector2((float)num, (float)num3);
		this.info.area = this.cells.Count;
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x00113100 File Offset: 0x00111300
	public void SaveToYaml(string save_name)
	{
		string text = TemplateCache.RewriteTemplatePath(save_name);
		if (!Directory.Exists(Path.GetDirectoryName(text)))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(text));
		}
		YamlIO.Save<TemplateContainer>(this, text + ".yaml", null);
	}

	// Token: 0x020014E4 RID: 5348
	[Serializable]
	public class Info
	{
		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06008626 RID: 34342 RVA: 0x00308151 File Offset: 0x00306351
		// (set) Token: 0x06008627 RID: 34343 RVA: 0x00308159 File Offset: 0x00306359
		public Vector2f size { get; set; }

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06008628 RID: 34344 RVA: 0x00308162 File Offset: 0x00306362
		// (set) Token: 0x06008629 RID: 34345 RVA: 0x0030816A File Offset: 0x0030636A
		public Vector2f min { get; set; }

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x0600862A RID: 34346 RVA: 0x00308173 File Offset: 0x00306373
		// (set) Token: 0x0600862B RID: 34347 RVA: 0x0030817B File Offset: 0x0030637B
		public int area { get; set; }

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x0600862C RID: 34348 RVA: 0x00308184 File Offset: 0x00306384
		// (set) Token: 0x0600862D RID: 34349 RVA: 0x0030818C File Offset: 0x0030638C
		public Tag[] tags { get; set; }

		// Token: 0x0600862E RID: 34350 RVA: 0x00308198 File Offset: 0x00306398
		public RectInt GetBounds(Vector2I position, int padding)
		{
			return new RectInt(position.x + (int)this.min.x - padding, position.y + (int)this.min.y - padding, (int)this.size.x + padding * 2, (int)this.size.y + padding * 2);
		}
	}
}
