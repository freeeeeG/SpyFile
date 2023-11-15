﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000797 RID: 1943
[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemVisualizer")]
public class EntombedItemVisualizer : KMonoBehaviour
{
	// Token: 0x06003610 RID: 13840 RVA: 0x001243CC File Offset: 0x001225CC
	public void Clear()
	{
		this.cellEntombedCounts.Clear();
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x001243D9 File Offset: 0x001225D9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.entombedItemPool = new GameObjectPool(new Func<GameObject>(this.InstantiateEntombedObject), 32);
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x001243FC File Offset: 0x001225FC
	public bool AddItem(int cell)
	{
		bool result = false;
		if (Grid.Objects[cell, 9] == null)
		{
			result = true;
			EntombedItemVisualizer.Data data;
			this.cellEntombedCounts.TryGetValue(cell, out data);
			if (data.refCount == 0)
			{
				GameObject instance = this.entombedItemPool.GetInstance();
				instance.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront));
				instance.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.value * 360f);
				KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
				int num = UnityEngine.Random.Range(0, EntombedItemVisualizer.EntombedVisualizerAnims.Length);
				string text = EntombedItemVisualizer.EntombedVisualizerAnims[num];
				component.initialAnim = text;
				instance.SetActive(true);
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				data.controller = component;
			}
			data.refCount++;
			this.cellEntombedCounts[cell] = data;
		}
		return result;
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x001244EC File Offset: 0x001226EC
	public void RemoveItem(int cell)
	{
		EntombedItemVisualizer.Data data;
		if (this.cellEntombedCounts.TryGetValue(cell, out data))
		{
			data.refCount--;
			if (data.refCount == 0)
			{
				this.ReleaseVisualizer(cell, data);
				return;
			}
			this.cellEntombedCounts[cell] = data;
		}
	}

	// Token: 0x06003614 RID: 13844 RVA: 0x00124534 File Offset: 0x00122734
	public void ForceClear(int cell)
	{
		EntombedItemVisualizer.Data data;
		if (this.cellEntombedCounts.TryGetValue(cell, out data))
		{
			this.ReleaseVisualizer(cell, data);
		}
	}

	// Token: 0x06003615 RID: 13845 RVA: 0x0012455C File Offset: 0x0012275C
	private void ReleaseVisualizer(int cell, EntombedItemVisualizer.Data data)
	{
		if (data.controller != null)
		{
			data.controller.gameObject.SetActive(false);
			this.entombedItemPool.ReleaseInstance(data.controller.gameObject);
		}
		this.cellEntombedCounts.Remove(cell);
	}

	// Token: 0x06003616 RID: 13846 RVA: 0x001245AB File Offset: 0x001227AB
	public bool IsEntombedItem(int cell)
	{
		return this.cellEntombedCounts.ContainsKey(cell) && this.cellEntombedCounts[cell].refCount > 0;
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x001245D1 File Offset: 0x001227D1
	private GameObject InstantiateEntombedObject()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.entombedItemPrefab, Grid.SceneLayer.FXFront, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x040020FA RID: 8442
	[SerializeField]
	private GameObject entombedItemPrefab;

	// Token: 0x040020FB RID: 8443
	private static readonly string[] EntombedVisualizerAnims = new string[]
	{
		"idle1",
		"idle2",
		"idle3",
		"idle4"
	};

	// Token: 0x040020FC RID: 8444
	private GameObjectPool entombedItemPool;

	// Token: 0x040020FD RID: 8445
	private Dictionary<int, EntombedItemVisualizer.Data> cellEntombedCounts = new Dictionary<int, EntombedItemVisualizer.Data>();

	// Token: 0x02001519 RID: 5401
	private struct Data
	{
		// Token: 0x04006750 RID: 26448
		public int refCount;

		// Token: 0x04006751 RID: 26449
		public KBatchedAnimController controller;
	}
}
