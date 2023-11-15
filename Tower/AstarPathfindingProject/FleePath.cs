using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200008E RID: 142
	public class FleePath : RandomPath
	{
		// Token: 0x060006DA RID: 1754 RVA: 0x000292F2 File Offset: 0x000274F2
		public static FleePath Construct(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback = null)
		{
			FleePath path = PathPool.GetPath<FleePath>();
			path.Setup(start, avoid, searchLength, callback);
			return path;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00029303 File Offset: 0x00027503
		protected void Setup(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback)
		{
			base.Setup(start, searchLength, callback);
			this.aim = start - (avoid - start) * 10f;
		}
	}
}
