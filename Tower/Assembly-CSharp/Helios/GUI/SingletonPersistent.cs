using System;
using UnityEngine;

namespace Helios.GUI
{
	// Token: 0x020000E5 RID: 229
	public class SingletonPersistent<T> : MonoBehaviour where T : Component
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000EEFC File Offset: 0x0000D0FC
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0000EFFA File Offset: 0x0000D1FA
		public static T Instance
		{
			get
			{
				if (SingletonPersistent<T>.instance == null)
				{
					T[] array = Object.FindObjectsOfType(typeof(T)) as T[];
					if (array.Length != 0)
					{
						if (array.Length == 1)
						{
							SingletonPersistent<T>.instance = array[0];
							SingletonPersistent<T>.instance.gameObject.name = typeof(T).Name;
							return SingletonPersistent<T>.instance;
						}
						Debug.LogError("Class " + typeof(T).Name + " exists multiple times in violation of singleton pattern. Destroying all copies");
						T[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							Object.Destroy(array2[i].gameObject);
						}
					}
					GameObject gameObject = new GameObject(typeof(T).Name, new Type[]
					{
						typeof(T)
					});
					SingletonPersistent<T>.instance = gameObject.GetComponent<T>();
					Object.DontDestroyOnLoad(gameObject);
				}
				return SingletonPersistent<T>.instance;
			}
			set
			{
				SingletonPersistent<T>.instance = value;
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000F002 File Offset: 0x0000D202
		public virtual void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x04000327 RID: 807
		private static T instance;
	}
}
