using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007FD RID: 2045
public interface IGameObjectEffectDescriptor
{
	// Token: 0x06003A64 RID: 14948
	List<Descriptor> GetDescriptors(GameObject go);
}
