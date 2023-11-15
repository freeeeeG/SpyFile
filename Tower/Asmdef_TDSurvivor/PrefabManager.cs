using System;
using Lean.Pool;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class PrefabManager : Singleton<PrefabManager>
{
	// Token: 0x0600063F RID: 1599 RVA: 0x00017C3D File Offset: 0x00015E3D
	private void OnDestroy()
	{
		LeanPool.DespawnAll();
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x00017C44 File Offset: 0x00015E44
	private GameObject GetPrefab(string name)
	{
		if (this.dic_Prefabs == null)
		{
			return null;
		}
		if (this.dic_Prefabs.Count == 0)
		{
			return null;
		}
		if (this.dic_Prefabs.Contains(name))
		{
			return this.dic_Prefabs[name];
		}
		return null;
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00017C7C File Offset: 0x00015E7C
	public GameObject InstantiatePrefab(string name, Vector3 position, Quaternion rotation, Transform parent = null)
	{
		GameObject prefab = this.GetPrefab(name);
		return this.InstantiatePrefab(prefab, position, rotation, parent);
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x00017C9C File Offset: 0x00015E9C
	public GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
	{
		GameObject gameObject = LeanPool.Spawn(prefab, position, rotation, parent);
		gameObject.transform.SetParent(parent, true);
		return gameObject;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00017CB6 File Offset: 0x00015EB6
	public void DespawnPrefab(GameObject obj, float delay = 0f)
	{
		LeanPool.Despawn(obj, delay);
	}

	// Token: 0x0400054E RID: 1358
	[SerializeField]
	private PrefabManager.StringPrefabDictionary dic_Prefabs;

	// Token: 0x02000255 RID: 597
	[Serializable]
	public class StringPrefabDictionary : SerializableDictionary<string, GameObject>
	{
	}
}
