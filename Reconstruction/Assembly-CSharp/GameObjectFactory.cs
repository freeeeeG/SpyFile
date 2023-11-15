using System;
using UnityEngine;

// Token: 0x02000101 RID: 257
public abstract class GameObjectFactory : ScriptableObject
{
	// Token: 0x06000677 RID: 1655 RVA: 0x000117E3 File Offset: 0x0000F9E3
	protected ReusableObject CreateInstance(ReusableObject prefab)
	{
		return Singleton<ObjectPool>.Instance.Spawn(prefab);
	}
}
