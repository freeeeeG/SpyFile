using System;
using Characters.Movements;
using Data;
using Level;
using UnityEngine;

// Token: 0x02000035 RID: 53
[CreateAssetMenu]
public class ParticleEffectInfo : ScriptableObject
{
	// Token: 0x060000D4 RID: 212 RVA: 0x00004CCC File Offset: 0x00002ECC
	public void Emit(Vector2 position, Bounds bounds, Push push)
	{
		Vector2 force = Vector2.zero;
		if (push != null && !push.expired)
		{
			force = push.direction * push.totalForce;
		}
		this.Emit(position, bounds, force, true);
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00004D08 File Offset: 0x00002F08
	public void Emit(Vector2 position, Bounds bounds, Vector2 force, bool interpolate = true)
	{
		if (GameData.Settings.particleQuality == 0)
		{
			return;
		}
		foreach (DroppedParts parts2 in this._parts)
		{
			this.SpawnParts(position, bounds, force, interpolate, parts2);
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00004D44 File Offset: 0x00002F44
	public void EmitRandom(Vector2 position, Bounds bounds, Vector2 force, bool interpolate = true)
	{
		if (GameData.Settings.particleQuality == 0)
		{
			return;
		}
		DroppedParts parts = this._parts.Random<DroppedParts>();
		this.SpawnParts(position, bounds, force, interpolate, parts);
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00004D74 File Offset: 0x00002F74
	private void SpawnParts(Vector2 position, Bounds bounds, Vector2 force, bool interpolate, DroppedParts parts)
	{
		if (parts == null)
		{
			Debug.LogError(base.name + " : A parts is missing!");
			return;
		}
		bool flag = parts.collideWithTerrain;
		if (GameData.Settings.particleQuality == 2)
		{
			if (parts.priority == DroppedParts.Priority.Low)
			{
				flag = false;
			}
		}
		else if (GameData.Settings.particleQuality == 1 && parts.priority == DroppedParts.Priority.Low)
		{
			flag = false;
		}
		int num = UnityEngine.Random.Range(parts.count.x, parts.count.y + 1);
		int layer;
		if (flag)
		{
			if (GameData.Settings.particleQuality <= 1)
			{
				layer = 27;
			}
			else
			{
				layer = 11;
			}
		}
		else if (parts.gameObject.layer == 11)
		{
			layer = 0;
		}
		else
		{
			layer = parts.gameObject.layer;
		}
		for (int i = 0; i < num; i++)
		{
			Vector2 v;
			Quaternion rotation;
			if (parts.randomize)
			{
				v = MMMaths.RandomPointWithinBounds(bounds) + parts.transform.position;
				rotation = Quaternion.AngleAxis((float)UnityEngine.Random.Range(0, 360), Vector3.forward);
			}
			else
			{
				v = position + parts.transform.position;
				rotation = parts.transform.rotation;
			}
			PoolObject poolObject = parts.poolObject.Spawn(v, rotation, true);
			poolObject.GetComponent<DroppedParts>().Initialize(force, this.multiplier * 3f, interpolate);
			poolObject.gameObject.layer = layer;
		}
	}

	// Token: 0x040000BC RID: 188
	[SerializeField]
	private float multiplier = 1f;

	// Token: 0x040000BD RID: 189
	[SerializeField]
	private DroppedParts[] _parts;
}
