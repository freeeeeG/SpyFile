using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001BB RID: 443
public static class GameObjectUtils
{
	// Token: 0x0600077F RID: 1919 RVA: 0x0002F4B8 File Offset: 0x0002D8B8
	public static void SetObjectVisibility(this GameObject _obj, bool _isVisible)
	{
		if (_obj.GetComponent<Renderer>())
		{
			_obj.GetComponent<Renderer>().enabled = _isVisible;
		}
		for (int i = 0; i < _obj.transform.childCount; i++)
		{
			_obj.transform.GetChild(i).gameObject.SetObjectVisibility(_isVisible);
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x0002F514 File Offset: 0x0002D914
	public static void SetObjectLayer(this GameObject _obj, int _layer)
	{
		_obj.layer = _layer;
		for (int i = 0; i < _obj.transform.childCount; i++)
		{
			_obj.transform.GetChild(i).gameObject.SetObjectLayer(_layer);
		}
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x0002F55B File Offset: 0x0002D95B
	public static T RequestInterface<T>(this GameObject _obj) where T : class
	{
		return _obj.GetComponent<T>();
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0002F563 File Offset: 0x0002D963
	public static bool IsInHierarchyOf(this GameObject _obj, GameObject _parent)
	{
		return _obj == _parent || (_obj.transform.parent != null && _obj.transform.parent.gameObject.IsInHierarchyOf(_parent));
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0002F5A4 File Offset: 0x0002D9A4
	public static T RequestComponentUpwardsRecursive<T>(this GameObject _obj) where T : Component
	{
		if (_obj.GetComponent<T>() != null)
		{
			return _obj.GetComponent<T>();
		}
		if (_obj.transform.parent != null)
		{
			return _obj.transform.parent.gameObject.RequestComponentUpwardsRecursive<T>();
		}
		return (T)((object)null);
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x0002F600 File Offset: 0x0002DA00
	public static T RequireInterface<T>(this GameObject _obj) where T : class
	{
		T t = _obj.RequestInterface<T>();
		if (t != null)
		{
			return t;
		}
		return (T)((object)null);
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x0002F627 File Offset: 0x0002DA27
	public static T[] RequestInterfaces<T>(this GameObject _obj) where T : class
	{
		return _obj.GetComponents<T>();
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x0002F62F File Offset: 0x0002DA2F
	public static T RequestInterfaceRecursive<T>(this GameObject _obj) where T : class
	{
		return _obj.GetComponentInChildren<T>();
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x0002F638 File Offset: 0x0002DA38
	public static T RequireInterfaceRecursive<T>(this GameObject _obj) where T : class
	{
		T componentInChildren = _obj.GetComponentInChildren<T>();
		if (componentInChildren == null)
		{
		}
		return componentInChildren;
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0002F658 File Offset: 0x0002DA58
	public static T RequestInterfaceUpwardsRecursive<T>(this GameObject _obj) where T : class
	{
		T component = _obj.GetComponent<T>();
		if (component != null)
		{
			return component;
		}
		Transform parent = _obj.transform.parent;
		if (parent != null)
		{
			return parent.gameObject.RequestInterfaceUpwardsRecursive<T>();
		}
		return (T)((object)null);
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x0002F6A3 File Offset: 0x0002DAA3
	public static T RequestComponent<T>(this GameObject _obj) where T : Component
	{
		return _obj.GetComponent<T>();
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x0002F6AB File Offset: 0x0002DAAB
	public static T[] RequestComponents<T>(this GameObject _obj) where T : Component
	{
		return _obj.GetComponents<T>();
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x0002F6B4 File Offset: 0x0002DAB4
	public static T RequireComponent<T>(this GameObject _obj) where T : Component
	{
		T component = _obj.GetComponent<T>();
		if (component != null)
		{
			return component;
		}
		return (T)((object)null);
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x0002F6E4 File Offset: 0x0002DAE4
	public static T RequestComponentRecursive<T>(this GameObject _obj) where T : Component
	{
		T component = _obj.GetComponent<T>();
		if (component != null)
		{
			return component;
		}
		for (int i = 0; i < _obj.transform.childCount; i++)
		{
			T t = _obj.transform.GetChild(i).gameObject.RequestComponentRecursive<T>();
			if (t)
			{
				return t;
			}
		}
		return (T)((object)null);
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0002F758 File Offset: 0x0002DB58
	public static T RequireComponentRecursive<T>(this GameObject _obj) where T : Component
	{
		T t = _obj.RequestComponentRecursive<T>();
		if (t != null)
		{
			return t;
		}
		return (T)((object)null);
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x0002F788 File Offset: 0x0002DB88
	public static T RequestInterfaceInImmediateChildren<T>(this GameObject _obj) where T : class
	{
		int childCount = _obj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = _obj.transform.GetChild(i);
			T t = child.gameObject.RequestInterface<T>();
			if (t != null)
			{
				return t;
			}
		}
		return (T)((object)null);
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x0002F7E0 File Offset: 0x0002DBE0
	public static T RequestComponentInImmediateChildren<T>(this GameObject _obj) where T : Component
	{
		int childCount = _obj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = _obj.transform.GetChild(i);
			T component = child.gameObject.GetComponent<T>();
			if (component != null)
			{
				return component;
			}
		}
		return (T)((object)null);
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0002F840 File Offset: 0x0002DC40
	public static T[] RequestComponentsInImmediateChildren<T>(this GameObject _obj) where T : Component
	{
		List<T> list = new List<T>();
		int childCount = _obj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = _obj.transform.GetChild(i);
			T[] components = child.gameObject.GetComponents<T>();
			list.AddRange(components);
		}
		return list.ToArray();
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0002F89C File Offset: 0x0002DC9C
	public static T RequireComponentInImmediateChildren<T>(this GameObject _obj) where T : Component
	{
		T t = _obj.RequestComponentInImmediateChildren<T>();
		if (t != null)
		{
			return t;
		}
		return (T)((object)null);
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x0002F8CC File Offset: 0x0002DCCC
	public static T[] RequestComponentsRecursive<T>(this GameObject _obj) where T : Component
	{
		List<T> list = new List<T>();
		_obj.BuildListOfComponentsRecursive(list);
		return list.ToArray();
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x0002F8EC File Offset: 0x0002DCEC
	private static void BuildListOfComponentsRecursive<T>(this GameObject _obj, List<T> _recursionList) where T : Component
	{
		T[] components = _obj.GetComponents<T>();
		_recursionList.AddRange(components);
		for (int i = 0; i < _obj.transform.childCount; i++)
		{
			_obj.transform.GetChild(i).gameObject.BuildListOfComponentsRecursive(_recursionList);
		}
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x0002F93C File Offset: 0x0002DD3C
	public static T[] RequestInterfacesRecursive<T>(this GameObject _obj) where T : class
	{
		List<T> list = new List<T>();
		_obj.BuildListOfInterfacesRecursive(list);
		return list.ToArray();
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0002F95C File Offset: 0x0002DD5C
	private static void BuildListOfInterfacesRecursive<T>(this GameObject _obj, List<T> _recursionList) where T : class
	{
		T[] collection = _obj.RequestInterfaces<T>();
		_recursionList.AddRange(collection);
		for (int i = 0; i < _obj.transform.childCount; i++)
		{
			_obj.transform.GetChild(i).gameObject.BuildListOfInterfacesRecursive(_recursionList);
		}
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x0002F9AC File Offset: 0x0002DDAC
	public static T[] FindComponentsOfTypeInScene<T>() where T : Component
	{
		List<T> list = new List<T>();
		int sceneCount = SceneManager.sceneCount;
		for (int i = 0; i < sceneCount; i++)
		{
			Scene sceneAt = SceneManager.GetSceneAt(i);
			if (sceneAt.IsValid() && sceneAt.isLoaded)
			{
				GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();
				for (int j = 0; j < rootGameObjects.Length; j++)
				{
					list.AddRange(rootGameObjects[j].GetComponentsInChildren<T>(true));
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x0002FA2F File Offset: 0x0002DE2F
	public static GameObject InstantiateOnCamera(this GameObject _source)
	{
		return _source.InstantiateOnParent(Camera.main.transform, true);
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x0002FA44 File Offset: 0x0002DE44
	public static GameObject InstantiateOnParent(this GameObject _source, Transform _parent, bool _worldPositionStays = true)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(_source);
		Vector3 position = gameObject.transform.position;
		Quaternion rotation = gameObject.transform.rotation;
		gameObject.transform.SetParent(_parent, _worldPositionStays);
		gameObject.transform.localPosition = position;
		gameObject.transform.localRotation = rotation;
		gameObject.name = _source.name;
		return gameObject;
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0002FAA4 File Offset: 0x0002DEA4
	public static GameObject Instantiate(this GameObject _source, Transform _parent, Vector3 _localPosition, Quaternion _localRotation)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(_source, _parent.TransformPoint(_localPosition), _parent.rotation * _localRotation);
		gameObject.transform.SetParent(_parent, true);
		gameObject.name = _source.name;
		return gameObject;
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x0002FAE8 File Offset: 0x0002DEE8
	public static GameObject Instantiate(this GameObject _source, Vector3 _postion, Quaternion _rotation)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(_source, _postion, _rotation);
		gameObject.name = _source.name;
		return gameObject;
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0002FB0C File Offset: 0x0002DF0C
	public static GameObject CreateOnParent(GameObject _parent, string _name)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.SetParent((!(_parent != null)) ? null : _parent.transform, false);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.name = _name;
		return gameObject;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0002FB6C File Offset: 0x0002DF6C
	public static GameObject CreateOnParent<T>(GameObject _parent, string _name)
	{
		GameObject gameObject = new GameObject(_name, new Type[]
		{
			typeof(T)
		});
		gameObject.transform.SetParent((!(_parent != null)) ? null : _parent.transform, false);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		return gameObject;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0002FBD8 File Offset: 0x0002DFD8
	public static Transform FindChildRecursive(this Transform parent, string name)
	{
		if (parent.name.Equals(name))
		{
			return parent;
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform transform = parent.GetChild(i).FindChildRecursive(name);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x0002FC2C File Offset: 0x0002E02C
	public static Transform FindChildStartsWithRecursive(this Transform parent, string name, bool activeInHierarchyRequired = false)
	{
		if (parent.name.StartsWith(name) && (!activeInHierarchyRequired || parent.gameObject.activeInHierarchy))
		{
			return parent;
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform transform = parent.GetChild(i).FindChildStartsWithRecursive(name, activeInHierarchyRequired);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x0002FC97 File Offset: 0x0002E097
	public static Transform FindParentRecursive(this Transform child, string name)
	{
		if (!(child.parent != null))
		{
			return null;
		}
		if (child.parent.name.Equals(name))
		{
			return child.parent;
		}
		return child.parent.FindParentRecursive(name);
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0002FCD5 File Offset: 0x0002E0D5
	public static void Destroy(this GameObject _object)
	{
		_object.SendMessage("OnTrigger", "destroyed", SendMessageOptions.DontRequireReceiver);
		UnityEngine.Object.Destroy(_object);
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0002FCEE File Offset: 0x0002E0EE
	public static void DestroyImmediate(this GameObject _object)
	{
		_object.SendMessage("OnTrigger", "destroyed", SendMessageOptions.DontRequireReceiver);
		UnityEngine.Object.DestroyImmediate(_object);
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x0002FD08 File Offset: 0x0002E108
	public static void SetRendering(this GameObject _object, bool _shouldRender)
	{
		Renderer[] array = _object.RequestComponentsRecursive<Renderer>();
		Light[] array2 = _object.RequestComponentsRecursive<Light>();
		ParticleSystem[] array3 = _object.RequestComponentsRecursive<ParticleSystem>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = _shouldRender;
		}
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].enabled = _shouldRender;
		}
		for (int k = 0; k < array3.Length; k++)
		{
			if (array3[k].isPlaying && !_shouldRender)
			{
				array3[k].Stop();
				array3[k].Clear();
			}
			if (_shouldRender && !array3[k].isPlaying)
			{
				array3[k].Play();
			}
		}
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0002FDC4 File Offset: 0x0002E1C4
	public static void DestroyChildren(this GameObject _object)
	{
		for (int i = 0; i < _object.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(_object.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0002FE04 File Offset: 0x0002E204
	public static GameObject RequestChild(this GameObject _object, string _name)
	{
		Transform transform = _object.transform.Find(_name);
		if (transform != null)
		{
			return transform.gameObject;
		}
		return null;
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0002FE34 File Offset: 0x0002E234
	public static GameObject RequestChildRecursive(this GameObject _object, string _name)
	{
		Transform transform = _object.transform.FindChildRecursive(_name);
		return transform.gameObject;
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x0002FE54 File Offset: 0x0002E254
	public static GameObject RequireChild(this GameObject _object, string _name)
	{
		return _object.RequestChild(_name);
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x0002FE6C File Offset: 0x0002E26C
	public static void SendTrigger(this GameObject _target, string _trigger)
	{
		if (_trigger != string.Empty)
		{
			ITriggerReceiver[] array = _target.RequestInterfaces<ITriggerReceiver>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnTrigger(_trigger);
			}
		}
	}
}
