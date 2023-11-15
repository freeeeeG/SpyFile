using System;
using Database;
using UnityEngine;

// Token: 0x02000B3C RID: 2876
public interface IKleiPermitDioramaVisTarget
{
	// Token: 0x060058D7 RID: 22743
	GameObject GetGameObject();

	// Token: 0x060058D8 RID: 22744
	void ConfigureSetup();

	// Token: 0x060058D9 RID: 22745
	void ConfigureWith(PermitResource permit);
}
