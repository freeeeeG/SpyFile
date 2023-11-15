using System;
using UnityEngine;

namespace TwoDLaserPack
{
	// Token: 0x0200165C RID: 5724
	public class DemoToggleLaserActive : MonoBehaviour
	{
		// Token: 0x06006D16 RID: 27926 RVA: 0x00002191 File Offset: 0x00000391
		private void Start()
		{
		}

		// Token: 0x06006D17 RID: 27927 RVA: 0x00137DD0 File Offset: 0x00135FD0
		private void OnMouseOver()
		{
			if (this.lineLaserRef != null && Input.GetMouseButtonDown(0))
			{
				this.lineLaserRef.SetLaserState(!this.lineLaserRef.laserActive);
			}
			if (this.spriteLaserRef != null && Input.GetMouseButtonDown(0))
			{
				this.spriteLaserRef.SetLaserState(!this.spriteLaserRef.laserActive);
			}
		}

		// Token: 0x06006D18 RID: 27928 RVA: 0x00002191 File Offset: 0x00000391
		private void Update()
		{
		}

		// Token: 0x040058D8 RID: 22744
		public LineBasedLaser lineLaserRef;

		// Token: 0x040058D9 RID: 22745
		public SpriteBasedLaser spriteLaserRef;
	}
}
