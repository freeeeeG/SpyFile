using System;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x020002C1 RID: 705
	public class ETFXRotation : MonoBehaviour
	{
		// Token: 0x0600113B RID: 4411 RVA: 0x000311F0 File Offset: 0x0002F3F0
		private void Start()
		{
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000311F4 File Offset: 0x0002F3F4
		private void Update()
		{
			if (this.rotateSpace == ETFXRotation.spaceEnum.Local)
			{
				base.transform.Rotate(this.rotateVector * Time.deltaTime);
			}
			if (this.rotateSpace == ETFXRotation.spaceEnum.World)
			{
				base.transform.Rotate(this.rotateVector * Time.deltaTime, Space.World);
			}
		}

		// Token: 0x04000986 RID: 2438
		[Header("Rotate axises by degrees per second")]
		public Vector3 rotateVector = Vector3.zero;

		// Token: 0x04000987 RID: 2439
		public ETFXRotation.spaceEnum rotateSpace;

		// Token: 0x02000311 RID: 785
		public enum spaceEnum
		{
			// Token: 0x04000AB0 RID: 2736
			Local,
			// Token: 0x04000AB1 RID: 2737
			World
		}
	}
}
