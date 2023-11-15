using System;
using UnityEngine;

// Token: 0x02000860 RID: 2144
[AddComponentMenu("KMonoBehaviour/scripts/Meter")]
public class Meter : KMonoBehaviour
{
	// Token: 0x02001639 RID: 5689
	public enum Offset
	{
		// Token: 0x04006AFE RID: 27390
		Infront,
		// Token: 0x04006AFF RID: 27391
		Behind,
		// Token: 0x04006B00 RID: 27392
		UserSpecified,
		// Token: 0x04006B01 RID: 27393
		NoChange
	}
}
