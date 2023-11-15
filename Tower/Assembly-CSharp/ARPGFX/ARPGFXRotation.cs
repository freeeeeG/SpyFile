using System;
using UnityEngine;

namespace ARPGFX
{
	// Token: 0x02000071 RID: 113
	public class ARPGFXRotation : MonoBehaviour
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x00007794 File Offset: 0x00005994
		private void Start()
		{
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007798 File Offset: 0x00005998
		private void Update()
		{
			if (this.rotateSpace == ARPGFXRotation.spaceEnum.Local)
			{
				base.transform.Rotate(this.rotateVector * Time.deltaTime);
			}
			if (this.rotateSpace == ARPGFXRotation.spaceEnum.World)
			{
				base.transform.Rotate(this.rotateVector * Time.deltaTime, Space.World);
			}
		}

		// Token: 0x04000194 RID: 404
		[Header("Rotate axises by degrees per second")]
		public Vector3 rotateVector = Vector3.zero;

		// Token: 0x04000195 RID: 405
		public ARPGFXRotation.spaceEnum rotateSpace;

		// Token: 0x02000102 RID: 258
		public enum spaceEnum
		{
			// Token: 0x040003BF RID: 959
			Local,
			// Token: 0x040003C0 RID: 960
			World
		}
	}
}
