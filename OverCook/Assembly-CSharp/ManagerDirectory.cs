using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000510 RID: 1296
public class ManagerDirectory : MonoBehaviour
{
	// Token: 0x0600182B RID: 6187 RVA: 0x0007AD11 File Offset: 0x00079111
	private void Awake()
	{
		this.CheckInitialised();
	}

	// Token: 0x0600182C RID: 6188 RVA: 0x0007AD19 File Offset: 0x00079119
	private void OnTransformChildrenChanged()
	{
		this.m_initialised = false;
		this.m_allManagers.Clear();
		this.CheckInitialised();
	}

	// Token: 0x0600182D RID: 6189 RVA: 0x0007AD34 File Offset: 0x00079134
	private void CheckInitialised()
	{
		if (!this.m_initialised)
		{
			Manager[] componentsInChildren = base.gameObject.GetComponentsInChildren<Manager>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				for (Type type = componentsInChildren[i].GetType(); type != null; type = type.BaseType)
				{
					this.m_allManagers.SafeAdd(type, componentsInChildren[i]);
				}
			}
			this.m_initialised = true;
		}
	}

	// Token: 0x0600182E RID: 6190 RVA: 0x0007ADA0 File Offset: 0x000791A0
	public T RequireManager<T>() where T : Manager
	{
		return this.RequestManager<T>();
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x0007ADB8 File Offset: 0x000791B8
	public T RequestManager<T>() where T : Manager
	{
		if (!Application.isPlaying)
		{
			return base.gameObject.RequestComponentInImmediateChildren<T>();
		}
		this.CheckInitialised();
		Manager manager;
		this.m_allManagers.TryGetValue(typeof(T), out manager);
		if (manager != null)
		{
			return manager as T;
		}
		return (T)((object)null);
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x0007AE18 File Offset: 0x00079218
	public T RequestManagerInterface<T>() where T : class
	{
		if (Application.isPlaying)
		{
			this.CheckInitialised();
			foreach (KeyValuePair<Type, Manager> keyValuePair in this.m_allManagers)
			{
				T t = keyValuePair.Value.gameObject.RequestInterface<T>();
				if (t != null)
				{
					return t;
				}
			}
			return (T)((object)null);
		}
		return base.gameObject.RequestInterfaceInImmediateChildren<T>();
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x0007AEB8 File Offset: 0x000792B8
	public T RequireManagerInterface<T>() where T : class
	{
		return this.RequestManagerInterface<T>();
	}

	// Token: 0x04001374 RID: 4980
	private Dictionary<Type, Manager> m_allManagers = new Dictionary<Type, Manager>();

	// Token: 0x04001375 RID: 4981
	private bool m_initialised;
}
