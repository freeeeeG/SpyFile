using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class rotateCamera : MonoBehaviour
{
	// Token: 0x06000011 RID: 17 RVA: 0x0000297C File Offset: 0x00000B7C
	private void Update()
	{
		if (this.left)
		{
			if (this.count >= this.maxCount)
			{
				base.transform.Rotate(Vector3.up, -this.turnSpeed * Time.deltaTime);
				this.count = 0;
				this.left = false;
				return;
			}
			base.transform.Rotate(Vector3.up, this.turnSpeed * Time.deltaTime);
			this.count++;
			return;
		}
		else
		{
			if (this.count >= this.maxCount)
			{
				base.transform.Rotate(Vector3.up, this.turnSpeed * Time.deltaTime);
				this.count = 0;
				this.left = true;
				return;
			}
			base.transform.Rotate(Vector3.up, -this.turnSpeed * Time.deltaTime);
			this.count++;
			return;
		}
	}

	// Token: 0x04000010 RID: 16
	public float turnSpeed = 50f;

	// Token: 0x04000011 RID: 17
	public int count;

	// Token: 0x04000012 RID: 18
	public int maxCount;

	// Token: 0x04000013 RID: 19
	public bool left;
}
