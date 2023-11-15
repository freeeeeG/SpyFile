using System;

namespace Pathfinding
{
	// Token: 0x02000051 RID: 81
	internal interface IPathInternals
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003F7 RID: 1015
		PathHandler PathHandler { get; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060003F8 RID: 1016
		// (set) Token: 0x060003F9 RID: 1017
		bool Pooled { get; set; }

		// Token: 0x060003FA RID: 1018
		void AdvanceState(PathState s);

		// Token: 0x060003FB RID: 1019
		void OnEnterPool();

		// Token: 0x060003FC RID: 1020
		void Reset();

		// Token: 0x060003FD RID: 1021
		void ReturnPath();

		// Token: 0x060003FE RID: 1022
		void PrepareBase(PathHandler handler);

		// Token: 0x060003FF RID: 1023
		void Prepare();

		// Token: 0x06000400 RID: 1024
		void Initialize();

		// Token: 0x06000401 RID: 1025
		void Cleanup();

		// Token: 0x06000402 RID: 1026
		void CalculateStep(long targetTick);

		// Token: 0x06000403 RID: 1027
		string DebugString(PathLog logMode);
	}
}
