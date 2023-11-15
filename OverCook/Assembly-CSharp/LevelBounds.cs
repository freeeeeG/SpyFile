using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004ED RID: 1261
public class LevelBounds : MonoBehaviour
{
	// Token: 0x0600178C RID: 6028 RVA: 0x00078126 File Offset: 0x00076526
	private void Start()
	{
		this.SetupLevelBounds();
		LevelBounds.s_bounds.Add(this);
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x0007813C File Offset: 0x0007653C
	private void SetupLevelBounds()
	{
		RespawnCollider[] array = base.gameObject.RequestComponentsRecursive<RespawnCollider>();
		if (array.Length == 4)
		{
			Collider[] array2 = new Collider[6];
			for (int i = 0; i < 4; i++)
			{
				array2[i] = array[i].gameObject.RequireComponent<Collider>();
			}
			GameObject ceiling = this.GetCeiling();
			array2[4] = ceiling.RequireComponent<Collider>();
			RespawnCollider ground = this.GetGround();
			array2[5] = ground.gameObject.RequireComponent<Collider>();
			Vector3 vector = Vector3.zero;
			for (int j = 0; j < 6; j++)
			{
				vector += array2[j].bounds.center;
			}
			vector /= 6f;
			this.m_bounds.center = vector;
			for (int k = 0; k < 6; k++)
			{
				Vector3 point = this.NearestPointAABB3(array2[k].bounds.min, array2[k].bounds.max, vector);
				this.m_bounds.Encapsulate(point);
			}
		}
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x00078254 File Offset: 0x00076654
	private Vector3 NearestPointAABB3(Vector3 min, Vector3 max, Vector3 p)
	{
		p.x = Mathf.Max(p.x, min.x);
		p.y = Mathf.Max(p.y, min.y);
		p.z = Mathf.Max(p.z, min.z);
		p.x = Mathf.Min(p.x, max.x);
		p.y = Mathf.Min(p.y, max.y);
		p.z = Mathf.Min(p.z, max.z);
		return p;
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x00078300 File Offset: 0x00076700
	private RespawnCollider GetGround()
	{
		Transform transform = GameUtils.GetGameEnvironment().transform.FindChildRecursive("KillPlane");
		return transform.gameObject.RequireComponent<RespawnCollider>();
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x00078330 File Offset: 0x00076730
	private GameObject GetCeiling()
	{
		return GameObject.Find("Ceiling");
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x0007834C File Offset: 0x0007674C
	public static bool ActiveBoundsContain(Vector3 _position)
	{
		if (LevelBounds.s_bounds.Count > 0)
		{
			for (int i = 0; i < LevelBounds.s_bounds.Count; i++)
			{
				LevelBounds levelBounds = LevelBounds.s_bounds[i];
				if (levelBounds.gameObject.activeInHierarchy && levelBounds.enabled && levelBounds.Contains(_position))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x000783BC File Offset: 0x000767BC
	public bool Contains(Vector3 _position)
	{
		return this.m_bounds.Contains(_position);
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x000783CA File Offset: 0x000767CA
	private void OnDestroy()
	{
		LevelBounds.s_bounds.Remove(this);
	}

	// Token: 0x04001136 RID: 4406
	private static List<LevelBounds> s_bounds = new List<LevelBounds>(1);

	// Token: 0x04001137 RID: 4407
	private const string c_floorName = "KillPlane";

	// Token: 0x04001138 RID: 4408
	private const string c_ceilingName = "Ceiling";

	// Token: 0x04001139 RID: 4409
	private const int c_numSides = 4;

	// Token: 0x0400113A RID: 4410
	private const int c_numPlanes = 6;

	// Token: 0x0400113B RID: 4411
	private Bounds m_bounds = default(Bounds);
}
