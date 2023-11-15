using System;
using UnityEngine;

// Token: 0x02000B18 RID: 2840
public interface IT17EventHelper
{
	// Token: 0x0600397A RID: 14714
	void SetEventSystem(T17EventSystem gamersEventSystem = null);

	// Token: 0x0600397B RID: 14715
	T17EventSystem GetDomain();

	// Token: 0x0600397C RID: 14716
	GameObject GetGameobject();
}
