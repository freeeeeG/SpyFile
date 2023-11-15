using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000EF RID: 239
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_astar_smooth_follow2.php")]
	public class AstarSmoothFollow2 : MonoBehaviour
	{
		// Token: 0x06000A04 RID: 2564 RVA: 0x00041E54 File Offset: 0x00040054
		private void LateUpdate()
		{
			Vector3 b;
			if (this.staticOffset)
			{
				b = this.target.position + new Vector3(0f, this.height, this.distance);
			}
			else if (this.followBehind)
			{
				b = this.target.TransformPoint(0f, this.height, -this.distance);
			}
			else
			{
				b = this.target.TransformPoint(0f, this.height, this.distance);
			}
			base.transform.position = Vector3.Lerp(base.transform.position, b, Time.deltaTime * this.damping);
			if (this.smoothRotation)
			{
				Quaternion b2 = Quaternion.LookRotation(this.target.position - base.transform.position, this.target.up);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b2, Time.deltaTime * this.rotationDamping);
				return;
			}
			base.transform.LookAt(this.target, this.target.up);
		}

		// Token: 0x04000624 RID: 1572
		public Transform target;

		// Token: 0x04000625 RID: 1573
		public float distance = 3f;

		// Token: 0x04000626 RID: 1574
		public float height = 3f;

		// Token: 0x04000627 RID: 1575
		public float damping = 5f;

		// Token: 0x04000628 RID: 1576
		public bool smoothRotation = true;

		// Token: 0x04000629 RID: 1577
		public bool followBehind = true;

		// Token: 0x0400062A RID: 1578
		public float rotationDamping = 10f;

		// Token: 0x0400062B RID: 1579
		public bool staticOffset;
	}
}
