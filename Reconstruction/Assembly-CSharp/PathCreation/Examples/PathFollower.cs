using System;
using UnityEngine;

namespace PathCreation.Examples
{
	// Token: 0x020002B5 RID: 693
	public class PathFollower : MonoBehaviour
	{
		// Token: 0x06001108 RID: 4360 RVA: 0x00030104 File Offset: 0x0002E304
		private void Start()
		{
			if (this.pathCreator != null)
			{
				this.pathCreator.pathUpdated += this.OnPathChanged;
			}
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0003012C File Offset: 0x0002E32C
		private void Update()
		{
			if (this.pathCreator != null)
			{
				this.distanceTravelled += this.speed * Time.deltaTime;
				base.transform.position = this.pathCreator.path.GetPointAtDistance(this.distanceTravelled, this.endOfPathInstruction);
				base.transform.rotation = this.pathCreator.path.GetRotationAtDistance(this.distanceTravelled, this.endOfPathInstruction);
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x000301AE File Offset: 0x0002E3AE
		private void OnPathChanged()
		{
			this.distanceTravelled = this.pathCreator.path.GetClosestDistanceAlongPath(base.transform.position);
		}

		// Token: 0x04000944 RID: 2372
		public PathCreator pathCreator;

		// Token: 0x04000945 RID: 2373
		public EndOfPathInstruction endOfPathInstruction;

		// Token: 0x04000946 RID: 2374
		public float speed = 5f;

		// Token: 0x04000947 RID: 2375
		private float distanceTravelled;
	}
}
