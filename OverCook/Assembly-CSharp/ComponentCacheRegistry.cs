using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000119 RID: 281
public class ComponentCacheRegistry : Manager
{
	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000524 RID: 1316 RVA: 0x00029416 File Offset: 0x00027816
	public static bool ScanActive
	{
		get
		{
			return ComponentCacheRegistry.m_scanRoutine != null;
		}
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00029423 File Offset: 0x00027823
	protected void Awake()
	{
		ComponentCacheRegistry.AddCacheScanType(typeof(Transform));
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00029434 File Offset: 0x00027834
	protected void OnDestroy()
	{
		ComponentCacheRegistry.Clear();
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0002943B File Offset: 0x0002783B
	protected void Update()
	{
		if (ComponentCacheRegistry.m_scanRoutine != null && !ComponentCacheRegistry.m_scanRoutine.MoveNext())
		{
			ComponentCacheRegistry.m_scanRoutine = null;
		}
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0002945C File Offset: 0x0002785C
	public static void ScanForInitialObjects(CallbackVoid _finished = null)
	{
		ComponentCacheRegistry.Clear();
		ComponentCacheRegistry.m_scanRoutine = ComponentCacheRegistry.BuildScanRoutine(ComponentCacheRegistry.m_cachedTypes.ToArray(), _finished);
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00029478 File Offset: 0x00027878
	private static IEnumerator BuildScanRoutine(Type[] scanTypes, CallbackVoid _finished)
	{
		HashSet<GameObject> cachedObjects = new HashSet<GameObject>();
		float yieldTime = Time.realtimeSinceStartup + 0.1f;
		GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		for (int i = 0; i < rootObjects.Length; i++)
		{
			string rootObjName = rootObjects[i].gameObject.name;
			foreach (Type type in scanTypes)
			{
				if (rootObjects[i] == null)
				{
					break;
				}
				Component[] objects = rootObjects[i].GetComponentsInChildren(type, true);
				string[] objectNames = objects.ConvertAll((Component x) => x.gameObject.name);
				for (int j = 0; j < objects.Length; j++)
				{
					if (!(objects[j] == null) && !(objects[j].gameObject == null))
					{
						GameObject obj = objects[j].gameObject;
						if (!cachedObjects.Contains(obj))
						{
							ComponentCacheRegistry.UpdateObject(obj);
							cachedObjects.Add(obj);
						}
						if (Time.realtimeSinceStartup > yieldTime)
						{
							yield return null;
							yieldTime = Time.realtimeSinceStartup + 0.1f;
						}
					}
				}
			}
		}
		if (_finished != null)
		{
			_finished();
		}
		yield break;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0002949A File Offset: 0x0002789A
	private static void AddCacheScanType(Type _type)
	{
		if (!ComponentCacheRegistry.m_cachedTypes.Contains(_type))
		{
			ComponentCacheRegistry.m_cachedTypes.Add(_type);
		}
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x000294B8 File Offset: 0x000278B8
	public static void UpdateObject(GameObject _object)
	{
		CachedObject component = _object.GetComponent<CachedObject>();
		if (component == null)
		{
			_object.AddComponent<CachedObject>();
		}
		ComponentCache<IGridLocation>.CacheObject(_object);
		ComponentCache<StaticGridLocation>.CacheObject(_object);
		ComponentCache<ClientInteractable>.CacheObject(_object);
		ComponentCache<IClientHandlePickup>.CacheObject(_object);
		ComponentCache<IClientHandlePlacement>.CacheObject(_object);
		ComponentCache<ClientHandlePickupReferral>.CacheObject(_object);
		ComponentCache<ClientHandlePlacementReferral>.CacheObject(_object);
		ComponentCache<ServerCatchableItem>.CacheObject(_object);
		ComponentCache<IThrowable>.CacheObject(_object);
		ComponentCache<IAttachment>.CacheObject(_object);
		ComponentCache<ServerFlammable>.CacheObject(_object);
		ComponentCache<IHandlePlacement>.CacheObject(_object);
		ComponentCache<ServerHandlePlacementReferral>.CacheObject(_object);
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00029530 File Offset: 0x00027930
	public static void EraseObject(GameObject _object)
	{
		ComponentCache<IGridLocation>.EraseObject(_object);
		ComponentCache<StaticGridLocation>.EraseObject(_object);
		ComponentCache<ClientInteractable>.EraseObject(_object);
		ComponentCache<IClientHandlePickup>.EraseObject(_object);
		ComponentCache<IClientHandlePlacement>.EraseObject(_object);
		ComponentCache<ClientHandlePickupReferral>.EraseObject(_object);
		ComponentCache<ClientHandlePlacementReferral>.EraseObject(_object);
		ComponentCache<ServerCatchableItem>.EraseObject(_object);
		ComponentCache<IThrowable>.EraseObject(_object);
		ComponentCache<IAttachment>.EraseObject(_object);
		ComponentCache<ServerFlammable>.EraseObject(_object);
		ComponentCache<IHandlePlacement>.EraseObject(_object);
		ComponentCache<ServerHandlePlacementReferral>.EraseObject(_object);
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0002958C File Offset: 0x0002798C
	public static void Clear()
	{
		ComponentCache<IGridLocation>.Clear();
		ComponentCache<StaticGridLocation>.Clear();
		ComponentCache<ClientInteractable>.Clear();
		ComponentCache<IClientHandlePickup>.Clear();
		ComponentCache<IClientHandlePlacement>.Clear();
		ComponentCache<ClientHandlePickupReferral>.Clear();
		ComponentCache<ClientHandlePlacementReferral>.Clear();
		ComponentCache<ServerCatchableItem>.Clear();
		ComponentCache<IThrowable>.Clear();
		ComponentCache<IAttachment>.Clear();
		ComponentCache<ServerFlammable>.Clear();
		ComponentCache<IHandlePlacement>.Clear();
		ComponentCache<ServerHandlePlacementReferral>.Clear();
	}

	// Token: 0x04000486 RID: 1158
	private static FastList<Type> m_cachedTypes = new FastList<Type>();

	// Token: 0x04000487 RID: 1159
	private static IEnumerator m_scanRoutine = null;
}
