using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x020000C9 RID: 201
	public interface ITransform
	{
		// Token: 0x06000879 RID: 2169
		Vector3 Transform(Vector3 position);

		// Token: 0x0600087A RID: 2170
		Vector3 InverseTransform(Vector3 position);
	}
}
