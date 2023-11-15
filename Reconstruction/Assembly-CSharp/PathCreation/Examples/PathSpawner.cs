using System;
using UnityEngine;

namespace PathCreation.Examples
{
	// Token: 0x020002B8 RID: 696
	public class PathSpawner : MonoBehaviour
	{
		// Token: 0x06001117 RID: 4375 RVA: 0x000303A4 File Offset: 0x0002E5A4
		private void Start()
		{
			foreach (Transform transform in this.spawnPoints)
			{
				PathCreator pathCreator = Object.Instantiate<PathCreator>(this.pathPrefab, transform.position, transform.rotation);
				Object.Instantiate<PathFollower>(this.followerPrefab).pathCreator = pathCreator;
				pathCreator.transform.parent = transform;
			}
		}

		// Token: 0x0400094F RID: 2383
		public PathCreator pathPrefab;

		// Token: 0x04000950 RID: 2384
		public PathFollower followerPrefab;

		// Token: 0x04000951 RID: 2385
		public Transform[] spawnPoints;
	}
}
