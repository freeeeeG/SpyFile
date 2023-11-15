using System;
using Pathfinding.RVO;
using UnityEngine;

namespace Pathfinding.Legacy
{
	// Token: 0x020000A4 RID: 164
	[AddComponentMenu("Pathfinding/Legacy/Local Avoidance/Legacy RVO Controller")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_legacy_1_1_legacy_r_v_o_controller.php")]
	public class LegacyRVOController : RVOController
	{
		// Token: 0x0600077D RID: 1917 RVA: 0x0002E2A4 File Offset: 0x0002C4A4
		public void Update()
		{
			if (base.rvoAgent == null)
			{
				return;
			}
			Vector3 a = this.tr.position + base.CalculateMovementDelta(Time.deltaTime);
			RaycastHit raycastHit;
			if (this.mask != 0 && Physics.Raycast(a + Vector3.up * base.height * 0.5f, Vector3.down, out raycastHit, float.PositiveInfinity, this.mask))
			{
				a.y = raycastHit.point.y;
			}
			else
			{
				a.y = 0f;
			}
			this.tr.position = a + Vector3.up * (base.height * 0.5f - base.center);
			if (this.enableRotation && base.velocity != Vector3.zero)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.LookRotation(base.velocity), Time.deltaTime * this.rotationSpeed * Mathf.Min(base.velocity.magnitude, 0.2f));
			}
		}

		// Token: 0x04000447 RID: 1095
		[Tooltip("Layer mask for the ground. The RVOController will raycast down to check for the ground to figure out where to place the agent")]
		public new LayerMask mask = -1;

		// Token: 0x04000448 RID: 1096
		public new bool enableRotation = true;

		// Token: 0x04000449 RID: 1097
		public new float rotationSpeed = 30f;
	}
}
