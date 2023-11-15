using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011B RID: 283
public static class ComponentCache<T> where T : class
{
	// Token: 0x06000531 RID: 1329 RVA: 0x000298A5 File Offset: 0x00027CA5
	public static void Clear()
	{
		ComponentCache<T>.m_Component.Clear();
		ComponentCache<T>.m_Components.Clear();
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x000298BB File Offset: 0x00027CBB
	public static void CacheObject(GameObject _object)
	{
		if (!ComponentCache<T>.m_Component.ContainsKey(_object))
		{
			ComponentCache<T>.AddObject(_object);
		}
		ComponentCache<T>.UpdateCacheForObject(_object);
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x000298D9 File Offset: 0x00027CD9
	public static void EraseObject(GameObject _object)
	{
		if (!ComponentCache<T>.m_Component.ContainsKey(_object))
		{
			return;
		}
		ComponentCache<T>.RemoveObject(_object);
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x000298F4 File Offset: 0x00027CF4
	public static T GetComponent(GameObject _object)
	{
		T result = (T)((object)null);
		if (!ComponentCache<T>.m_Component.TryGetValue(_object, out result))
		{
			ComponentCache<T>.CacheObject(_object);
			if (!ComponentCache<T>.m_Component.TryGetValue(_object, out result))
			{
				result = _object.GetComponent<T>();
			}
		}
		return result;
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x0002993C File Offset: 0x00027D3C
	public static T[] GetComponents(GameObject _object)
	{
		T[] result = null;
		if (!ComponentCache<T>.m_Components.TryGetValue(_object, out result))
		{
			ComponentCache<T>.CacheObject(_object);
			if (!ComponentCache<T>.m_Components.TryGetValue(_object, out result))
			{
				result = _object.GetComponents<T>();
			}
		}
		return result;
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0002997D File Offset: 0x00027D7D
	private static void AddObject(GameObject _object)
	{
		ComponentCache<T>.m_Component.Add(_object, (T)((object)null));
		ComponentCache<T>.m_Components.Add(_object, new T[0]);
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x000299A1 File Offset: 0x00027DA1
	private static void RemoveObject(GameObject _object)
	{
		ComponentCache<T>.m_Component.Remove(_object);
		ComponentCache<T>.m_Components.Remove(_object);
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x000299BB File Offset: 0x00027DBB
	private static void UpdateCacheForObject(GameObject _object)
	{
		ComponentCache<T>.m_Component[_object] = _object.GetComponent<T>();
		ComponentCache<T>.m_Components[_object] = _object.GetComponents<T>();
	}

	// Token: 0x04000488 RID: 1160
	public static Dictionary<GameObject, T> m_Component = new Dictionary<GameObject, T>();

	// Token: 0x04000489 RID: 1161
	public static Dictionary<GameObject, T[]> m_Components = new Dictionary<GameObject, T[]>();
}
