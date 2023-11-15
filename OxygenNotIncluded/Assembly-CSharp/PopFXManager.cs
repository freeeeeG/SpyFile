using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02000BC4 RID: 3012
public class PopFXManager : KScreen
{
	// Token: 0x06005E85 RID: 24197 RVA: 0x0022B4F1 File Offset: 0x002296F1
	public static void DestroyInstance()
	{
		PopFXManager.Instance = null;
	}

	// Token: 0x06005E86 RID: 24198 RVA: 0x0022B4F9 File Offset: 0x002296F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PopFXManager.Instance = this;
	}

	// Token: 0x06005E87 RID: 24199 RVA: 0x0022B508 File Offset: 0x00229708
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ready = true;
		if (GenericGameSettings.instance.disablePopFx)
		{
			return;
		}
		for (int i = 0; i < 20; i++)
		{
			PopFX item = this.CreatePopFX();
			this.Pool.Add(item);
		}
	}

	// Token: 0x06005E88 RID: 24200 RVA: 0x0022B54F File Offset: 0x0022974F
	public bool Ready()
	{
		return this.ready;
	}

	// Token: 0x06005E89 RID: 24201 RVA: 0x0022B558 File Offset: 0x00229758
	public PopFX SpawnFX(Sprite icon, string text, Transform target_transform, Vector3 offset, float lifetime = 1.5f, bool track_target = false, bool force_spawn = false)
	{
		if (GenericGameSettings.instance.disablePopFx)
		{
			return null;
		}
		if (Game.IsQuitting())
		{
			return null;
		}
		Vector3 vector = offset;
		if (target_transform != null)
		{
			vector += target_transform.GetPosition();
		}
		if (!force_spawn)
		{
			int cell = Grid.PosToCell(vector);
			if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
			{
				return null;
			}
		}
		PopFX popFX;
		if (this.Pool.Count > 0)
		{
			popFX = this.Pool[0];
			this.Pool[0].gameObject.SetActive(true);
			this.Pool[0].Spawn(icon, text, target_transform, offset, lifetime, track_target);
			this.Pool.RemoveAt(0);
		}
		else
		{
			popFX = this.CreatePopFX();
			popFX.gameObject.SetActive(true);
			popFX.Spawn(icon, text, target_transform, offset, lifetime, track_target);
		}
		return popFX;
	}

	// Token: 0x06005E8A RID: 24202 RVA: 0x0022B631 File Offset: 0x00229831
	public PopFX SpawnFX(Sprite icon, string text, Transform target_transform, float lifetime = 1.5f, bool track_target = false)
	{
		return this.SpawnFX(icon, text, target_transform, Vector3.zero, lifetime, track_target, false);
	}

	// Token: 0x06005E8B RID: 24203 RVA: 0x0022B646 File Offset: 0x00229846
	private PopFX CreatePopFX()
	{
		GameObject gameObject = Util.KInstantiate(this.Prefab_PopFX, base.gameObject, "Pooled_PopFX");
		gameObject.transform.localScale = Vector3.one;
		return gameObject.GetComponent<PopFX>();
	}

	// Token: 0x06005E8C RID: 24204 RVA: 0x0022B673 File Offset: 0x00229873
	public void RecycleFX(PopFX fx)
	{
		this.Pool.Add(fx);
	}

	// Token: 0x04003FC8 RID: 16328
	public static PopFXManager Instance;

	// Token: 0x04003FC9 RID: 16329
	public GameObject Prefab_PopFX;

	// Token: 0x04003FCA RID: 16330
	public List<PopFX> Pool = new List<PopFX>();

	// Token: 0x04003FCB RID: 16331
	public Sprite sprite_Plus;

	// Token: 0x04003FCC RID: 16332
	public Sprite sprite_Negative;

	// Token: 0x04003FCD RID: 16333
	public Sprite sprite_Resource;

	// Token: 0x04003FCE RID: 16334
	public Sprite sprite_Building;

	// Token: 0x04003FCF RID: 16335
	public Sprite sprite_Research;

	// Token: 0x04003FD0 RID: 16336
	private bool ready;
}
