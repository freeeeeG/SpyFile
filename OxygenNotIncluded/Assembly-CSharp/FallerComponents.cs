﻿using System;
using UnityEngine;

// Token: 0x020007BD RID: 1981
public class FallerComponents : KGameObjectComponentManager<FallerComponent>
{
	// Token: 0x060036C5 RID: 14021 RVA: 0x0012795E File Offset: 0x00125B5E
	public HandleVector<int>.Handle Add(GameObject go, Vector2 initial_velocity)
	{
		return base.Add(go, new FallerComponent(go.transform, initial_velocity));
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x00127974 File Offset: 0x00125B74
	public override void Remove(GameObject go)
	{
		HandleVector<int>.Handle handle = base.GetHandle(go);
		this.OnCleanUpImmediate(handle);
		KComponentManager<FallerComponent>.CleanupInfo info = new KComponentManager<FallerComponent>.CleanupInfo(go, handle);
		if (!KComponentCleanUp.InCleanUpPhase)
		{
			base.AddToCleanupList(info);
			return;
		}
		base.InternalRemoveComponent(info);
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x001279B0 File Offset: 0x00125BB0
	protected override void OnPrefabInit(HandleVector<int>.Handle h)
	{
		FallerComponent data = base.GetData(h);
		Vector3 position = data.transform.GetPosition();
		int num = Grid.PosToCell(position);
		data.cellChangedCB = delegate()
		{
			FallerComponents.OnSolidChanged(h);
		};
		float groundOffset = GravityComponent.GetGroundOffset(data.transform.GetComponent<KCollider2D>());
		int num2 = Grid.PosToCell(new Vector3(position.x, position.y - groundOffset - 0.07f, position.z));
		bool flag = Grid.IsValidCell(num2) && Grid.Solid[num2] && data.initialVelocity.sqrMagnitude == 0f;
		if ((Grid.IsValidCell(num) && Grid.Solid[num]) || flag)
		{
			data.solidChangedCB = delegate(object ev_data)
			{
				FallerComponents.OnSolidChanged(h);
			};
			int height = 2;
			Vector2I vector2I = Grid.CellToXY(num);
			vector2I.y--;
			if (vector2I.y < 0)
			{
				vector2I.y = 0;
				height = 1;
			}
			else if (vector2I.y == Grid.HeightInCells - 1)
			{
				height = 1;
			}
			data.partitionerEntry = GameScenePartitioner.Instance.Add("Faller", data.transform.gameObject, vector2I.x, vector2I.y, 1, height, GameScenePartitioner.Instance.solidChangedLayer, data.solidChangedCB);
			GameComps.Fallers.SetData(h, data);
			return;
		}
		GameComps.Fallers.SetData(h, data);
		FallerComponents.AddGravity(data.transform, data.initialVelocity);
	}

	// Token: 0x060036C8 RID: 14024 RVA: 0x00127B50 File Offset: 0x00125D50
	protected override void OnSpawn(HandleVector<int>.Handle h)
	{
		base.OnSpawn(h);
		FallerComponent data = base.GetData(h);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(data.transform, data.cellChangedCB, "FallerComponent.OnSpawn");
	}

	// Token: 0x060036C9 RID: 14025 RVA: 0x00127B88 File Offset: 0x00125D88
	private void OnCleanUpImmediate(HandleVector<int>.Handle h)
	{
		FallerComponent data = base.GetData(h);
		GameScenePartitioner.Instance.Free(ref data.partitionerEntry);
		if (data.cellChangedCB != null)
		{
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(data.transformInstanceId, data.cellChangedCB);
			data.cellChangedCB = null;
		}
		if (GameComps.Gravities.Has(data.transform.gameObject))
		{
			GameComps.Gravities.Remove(data.transform.gameObject);
		}
		base.SetData(h, data);
	}

	// Token: 0x060036CA RID: 14026 RVA: 0x00127C08 File Offset: 0x00125E08
	private static void AddGravity(Transform transform, Vector2 initial_velocity)
	{
		if (!GameComps.Gravities.Has(transform.gameObject))
		{
			GameComps.Gravities.Add(transform.gameObject, initial_velocity, delegate()
			{
				FallerComponents.OnLanded(transform);
			});
			HandleVector<int>.Handle handle = GameComps.Fallers.GetHandle(transform.gameObject);
			FallerComponent data = GameComps.Fallers.GetData(handle);
			if (data.partitionerEntry.IsValid())
			{
				GameScenePartitioner.Instance.Free(ref data.partitionerEntry);
				GameComps.Fallers.SetData(handle, data);
			}
		}
	}

	// Token: 0x060036CB RID: 14027 RVA: 0x00127CAC File Offset: 0x00125EAC
	private static void RemoveGravity(Transform transform)
	{
		if (GameComps.Gravities.Has(transform.gameObject))
		{
			GameComps.Gravities.Remove(transform.gameObject);
			HandleVector<int>.Handle h = GameComps.Fallers.GetHandle(transform.gameObject);
			FallerComponent data = GameComps.Fallers.GetData(h);
			int cell = Grid.CellBelow(Grid.PosToCell(transform.GetPosition()));
			GameScenePartitioner.Instance.Free(ref data.partitionerEntry);
			if (Grid.IsValidCell(cell))
			{
				data.solidChangedCB = delegate(object ev_data)
				{
					FallerComponents.OnSolidChanged(h);
				};
				data.partitionerEntry = GameScenePartitioner.Instance.Add("Faller", transform.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, data.solidChangedCB);
			}
			GameComps.Fallers.SetData(h, data);
		}
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x00127D86 File Offset: 0x00125F86
	private static void OnLanded(Transform transform)
	{
		FallerComponents.RemoveGravity(transform);
	}

	// Token: 0x060036CD RID: 14029 RVA: 0x00127D90 File Offset: 0x00125F90
	private static void OnSolidChanged(HandleVector<int>.Handle handle)
	{
		FallerComponent data = GameComps.Fallers.GetData(handle);
		if (data.transform == null)
		{
			return;
		}
		Vector3 position = data.transform.GetPosition();
		position.y = position.y - data.offset - 0.1f;
		int num = Grid.PosToCell(position);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		bool flag = !Grid.Solid[num];
		if (flag != data.isFalling)
		{
			data.isFalling = flag;
			if (flag)
			{
				FallerComponents.AddGravity(data.transform, Vector2.zero);
				return;
			}
			FallerComponents.RemoveGravity(data.transform);
		}
	}

	// Token: 0x040021B2 RID: 8626
	private const float EPSILON = 0.07f;
}
