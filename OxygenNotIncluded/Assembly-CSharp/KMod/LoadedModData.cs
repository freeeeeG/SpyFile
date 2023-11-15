using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace KMod
{
	// Token: 0x02000D71 RID: 3441
	public class LoadedModData
	{
		// Token: 0x04004EA4 RID: 20132
		public Harmony harmony;

		// Token: 0x04004EA5 RID: 20133
		public Dictionary<Assembly, UserMod2> userMod2Instances;

		// Token: 0x04004EA6 RID: 20134
		public ICollection<Assembly> dlls;

		// Token: 0x04004EA7 RID: 20135
		public ICollection<MethodBase> patched_methods;
	}
}
