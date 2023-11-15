using System;
using System.Collections.Generic;
using Pathfinding.Serialization;

namespace Pathfinding
{
	// Token: 0x02000059 RID: 89
	public interface IGraphInternals
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000432 RID: 1074
		// (set) Token: 0x06000433 RID: 1075
		string SerializedEditorSettings { get; set; }

		// Token: 0x06000434 RID: 1076
		void OnDestroy();

		// Token: 0x06000435 RID: 1077
		void DestroyAllNodes();

		// Token: 0x06000436 RID: 1078
		IEnumerable<Progress> ScanInternal();

		// Token: 0x06000437 RID: 1079
		void SerializeExtraInfo(GraphSerializationContext ctx);

		// Token: 0x06000438 RID: 1080
		void DeserializeExtraInfo(GraphSerializationContext ctx);

		// Token: 0x06000439 RID: 1081
		void PostDeserialization(GraphSerializationContext ctx);

		// Token: 0x0600043A RID: 1082
		void DeserializeSettingsCompatibility(GraphSerializationContext ctx);
	}
}
