using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000347 RID: 839
	public abstract class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x00042956 File Offset: 0x00040D56
		public static T Instance
		{
			get
			{
				return SingletonMonoBehavior<T>.GetInstance();
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00042960 File Offset: 0x00040D60
		private static void CreateInstance()
		{
			GameObject gameObject = new GameObject();
			gameObject.name = typeof(T).ToString();
			Debug.Log("Creating instance of singleton: " + gameObject.name);
			SingletonMonoBehavior<T>.instance = gameObject.AddComponent<T>();
			SingletonMonoBehavior<T>.hasInstance = true;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x000429B0 File Offset: 0x00040DB0
		private static T GetInstance()
		{
			object obj = SingletonMonoBehavior<T>.lockObject;
			T result;
			lock (obj)
			{
				if (SingletonMonoBehavior<T>.hasInstance)
				{
					result = SingletonMonoBehavior<T>.instance;
				}
				else
				{
					Type typeFromHandle = typeof(T);
					T[] array = UnityEngine.Object.FindObjectsOfType<T>();
					if (array.Length > 0)
					{
						SingletonMonoBehavior<T>.instance = array[0];
						SingletonMonoBehavior<T>.hasInstance = true;
						if (array.Length > 1)
						{
							Debug.LogWarning("Multiple instances of singleton " + typeFromHandle + " found; destroying all but the first.");
							for (int i = 1; i < array.Length; i++)
							{
								UnityEngine.Object.DestroyImmediate(array[i].gameObject);
							}
						}
						result = SingletonMonoBehavior<T>.instance;
					}
					else
					{
						SingletonPrefabAttribute singletonPrefabAttribute = Attribute.GetCustomAttribute(typeFromHandle, typeof(SingletonPrefabAttribute)) as SingletonPrefabAttribute;
						if (singletonPrefabAttribute == null)
						{
							SingletonMonoBehavior<T>.CreateInstance();
						}
						else
						{
							string name = singletonPrefabAttribute.Name;
							GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(name));
							if (gameObject == null)
							{
								Debug.LogError(string.Concat(new object[]
								{
									"Could not find prefab ",
									name,
									" for singleton of type ",
									typeFromHandle,
									"."
								}));
								SingletonMonoBehavior<T>.CreateInstance();
							}
							else
							{
								gameObject.name = name;
								SingletonMonoBehavior<T>.instance = gameObject.GetComponent<T>();
								if (SingletonMonoBehavior<T>.instance == null)
								{
									Debug.LogWarning(string.Concat(new object[]
									{
										"There wasn't a component of type \"",
										typeFromHandle,
										"\" inside prefab \"",
										name,
										"\"; creating one now."
									}));
									SingletonMonoBehavior<T>.instance = gameObject.AddComponent<T>();
									SingletonMonoBehavior<T>.hasInstance = true;
								}
							}
						}
						result = SingletonMonoBehavior<T>.instance;
					}
				}
			}
			return result;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00042B88 File Offset: 0x00040F88
		private static void EnforceSingleton()
		{
			object obj = SingletonMonoBehavior<T>.lockObject;
			lock (obj)
			{
				if (SingletonMonoBehavior<T>.hasInstance)
				{
					T[] array = UnityEngine.Object.FindObjectsOfType<T>();
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].GetInstanceID() != SingletonMonoBehavior<T>.instance.GetInstanceID())
						{
							UnityEngine.Object.DestroyImmediate(array[i].gameObject);
						}
					}
				}
			}
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00042C24 File Offset: 0x00041024
		protected bool SetupSingleton()
		{
			SingletonMonoBehavior<T>.EnforceSingleton();
			int instanceID = base.GetInstanceID();
			T t = SingletonMonoBehavior<T>.Instance;
			return instanceID == t.GetInstanceID();
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00042C51 File Offset: 0x00041051
		private void OnDestroy()
		{
			SingletonMonoBehavior<T>.hasInstance = false;
		}

		// Token: 0x04000C19 RID: 3097
		private static T instance;

		// Token: 0x04000C1A RID: 3098
		private static bool hasInstance;

		// Token: 0x04000C1B RID: 3099
		private static object lockObject = new object();
	}
}
