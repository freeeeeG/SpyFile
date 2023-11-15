using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x020000C8 RID: 200
	public interface IMovementPlane
	{
		// Token: 0x06000876 RID: 2166
		Vector2 ToPlane(Vector3 p);

		// Token: 0x06000877 RID: 2167
		Vector2 ToPlane(Vector3 p, out float elevation);

		// Token: 0x06000878 RID: 2168
		Vector3 ToWorld(Vector2 p, float elevation = 0f);
	}
}
