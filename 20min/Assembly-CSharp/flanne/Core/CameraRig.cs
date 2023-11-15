using System;
using UnityEngine;

namespace flanne.Core
{
	// Token: 0x020001FF RID: 511
	public class CameraRig : MonoBehaviour
	{
		// Token: 0x06000B7E RID: 2942 RVA: 0x0002B194 File Offset: 0x00029394
		private void Awake()
		{
			this.parent = base.transform.parent;
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0002B1A7 File Offset: 0x000293A7
		private void Start()
		{
			this.SC = ShootingCursor.Instance;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0002B1B4 File Offset: 0x000293B4
		private void Update()
		{
			if (PauseController.isPaused)
			{
				return;
			}
			if (this.SC.autoAim || this.SC.usingGamepad)
			{
				base.transform.localPosition = Vector3.zero;
				return;
			}
			Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
			Vector2 b = this.parent.position;
			Vector2 vector = a - b;
			vector /= 12f;
			if (this.maxLookDistance < vector.magnitude)
			{
				vector = vector.normalized;
				vector = this.maxLookDistance * vector;
			}
			base.transform.localPosition = vector;
		}

		// Token: 0x040007E8 RID: 2024
		[SerializeField]
		private float maxLookDistance;

		// Token: 0x040007E9 RID: 2025
		private Transform parent;

		// Token: 0x040007EA RID: 2026
		private ShootingCursor SC;
	}
}
