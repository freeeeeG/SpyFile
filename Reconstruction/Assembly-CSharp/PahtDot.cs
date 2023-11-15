using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public class PahtDot : MonoBehaviour
{
	// Token: 0x06000BA1 RID: 2977 RVA: 0x0001E4E1 File Offset: 0x0001C6E1
	private void Update()
	{
		this.MoveToNext();
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x0001E4EC File Offset: 0x0001C6EC
	private void MoveToNext()
	{
		base.transform.position = Vector2.MoveTowards(base.transform.position, this.m_Path[this.index], this.Speed * Time.deltaTime);
		if ((base.transform.position - this.m_Path[this.index]).sqrMagnitude < 0.001f)
		{
			this.index++;
			if (this.index >= this.m_Path.Count)
			{
				base.transform.position = this.m_Path[0];
				this.index = 1;
			}
			this.RotateToward();
		}
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x0001E5BA File Offset: 0x0001C7BA
	private void RotateToward()
	{
		base.transform.rotation = Quaternion.LookRotation(this.m_Path[this.index] - base.transform.position);
	}

	// Token: 0x040005CD RID: 1485
	public List<Vector2> m_Path;

	// Token: 0x040005CE RID: 1486
	public float Speed = 0.5f;

	// Token: 0x040005CF RID: 1487
	public int index;
}
