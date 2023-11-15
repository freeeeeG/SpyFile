using System;
using UnityEngine;

namespace PixelArsenal
{
	// Token: 0x020002B3 RID: 691
	public class PixelArsenalRotation : MonoBehaviour
	{
		// Token: 0x06001103 RID: 4355 RVA: 0x00030056 File Offset: 0x0002E256
		private void Start()
		{
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00030058 File Offset: 0x0002E258
		private void Update()
		{
			if (this.rotateSpace == PixelArsenalRotation.spaceEnum.Local)
			{
				base.transform.Rotate(this.rotateVector * Time.deltaTime);
			}
			if (this.rotateSpace == PixelArsenalRotation.spaceEnum.World)
			{
				base.transform.Rotate(this.rotateVector * Time.deltaTime, Space.World);
			}
		}

		// Token: 0x04000940 RID: 2368
		[Header("Rotate axises by degrees per second")]
		public Vector3 rotateVector = Vector3.zero;

		// Token: 0x04000941 RID: 2369
		public PixelArsenalRotation.spaceEnum rotateSpace;

		// Token: 0x0200030E RID: 782
		public enum spaceEnum
		{
			// Token: 0x04000AA6 RID: 2726
			Local,
			// Token: 0x04000AA7 RID: 2727
			World
		}
	}
}
