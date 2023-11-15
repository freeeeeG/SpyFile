using System;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x0200006A RID: 106
	public class ETFXRotation : MonoBehaviour
	{
		// Token: 0x06000180 RID: 384 RVA: 0x00007138 File Offset: 0x00005338
		private void Start()
		{
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000713C File Offset: 0x0000533C
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

		// Token: 0x04000178 RID: 376
		[Header("Rotate axises by degrees per second")]
		public Vector3 rotateVector = Vector3.zero;

		// Token: 0x04000179 RID: 377
		public ETFXRotation.spaceEnum rotateSpace;

		// Token: 0x020000FF RID: 255
		public enum spaceEnum
		{
			// Token: 0x040003B5 RID: 949
			Local,
			// Token: 0x040003B6 RID: 950
			World
		}
	}
}
