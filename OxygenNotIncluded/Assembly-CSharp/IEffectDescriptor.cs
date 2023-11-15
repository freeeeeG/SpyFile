using System;
using System.Collections.Generic;

// Token: 0x020007FE RID: 2046
[Obsolete("No longer used. Use IGameObjectEffectDescriptor instead", false)]
public interface IEffectDescriptor
{
	// Token: 0x06003A65 RID: 14949
	List<Descriptor> GetDescriptors(BuildingDef def);
}
