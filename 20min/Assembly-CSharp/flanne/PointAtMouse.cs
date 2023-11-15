using System;
using flanne.Core;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000AD RID: 173
	public class PointAtMouse : MonoBehaviour
	{
		// Token: 0x060005B3 RID: 1459 RVA: 0x0001B064 File Offset: 0x00019264
		private void Start()
		{
			this.SC = ShootingCursor.Instance;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001B074 File Offset: 0x00019274
		private void Update()
		{
			if (!PauseController.isPaused)
			{
				Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
				Vector2 b = base.transform.position;
				Vector2 vector = a - b;
				float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				base.transform.rotation = Quaternion.AngleAxis(num, Vector3.forward);
				if (num <= 90f && num > -90f)
				{
					base.transform.localScale = new Vector3(1f, 1f, 1f);
					return;
				}
				base.transform.localScale = new Vector3(1f, -1f, 1f);
			}
		}

		// Token: 0x04000398 RID: 920
		private ShootingCursor SC;
	}
}
