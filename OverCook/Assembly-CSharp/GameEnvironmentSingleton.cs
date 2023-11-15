using System;
using UnityEngine;

// Token: 0x02000504 RID: 1284
public class GameEnvironmentSingleton : MonoBehaviour
{
	// Token: 0x060017EE RID: 6126 RVA: 0x0007A11F File Offset: 0x0007851F
	public static GameObject GetActiveGameObject()
	{
		return GameEnvironmentSingleton.s_gameEnvironment;
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x0007A126 File Offset: 0x00078526
	private void Awake()
	{
		GameEnvironmentSingleton.s_gameEnvironment = base.gameObject;
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x0007A133 File Offset: 0x00078533
	private void OnDestroy()
	{
		GameEnvironmentSingleton.s_gameEnvironment = null;
	}

	// Token: 0x04001358 RID: 4952
	private static GameObject s_gameEnvironment;
}
