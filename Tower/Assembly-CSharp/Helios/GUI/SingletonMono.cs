using System;
using UnityEngine;

namespace Helios.GUI
{
	// Token: 0x020000E3 RID: 227
	public class SingletonMono<T> : MonoBehaviour where T : Component
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000EE04 File Offset: 0x0000D004
		public static T Instance
		{
			get
			{
				if (SingletonMono<T>._instance == null)
				{
					T[] array = Object.FindObjectsOfType(typeof(T)) as T[];
					if (array.Length != 0)
					{
						SingletonMono<T>._instance = array[0];
					}
					if (array.Length > 1)
					{
						Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
					}
					if (SingletonMono<T>._instance == null)
					{
						Debug.LogError("Null instance " + typeof(T).Name + " in the scene");
					}
				}
				return SingletonMono<T>._instance;
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000EEAE File Offset: 0x0000D0AE
		protected virtual void OnDestroy()
		{
			SingletonMono<T>._instance = default(T);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000EEBB File Offset: 0x0000D0BB
		protected virtual void OnApplicationQuit()
		{
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000EEBD File Offset: 0x0000D0BD
		public static bool IsNull()
		{
			return SingletonMono<T>._instance == null;
		}

		// Token: 0x04000325 RID: 805
		private static T _instance;
	}
}
