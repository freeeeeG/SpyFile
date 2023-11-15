using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B84 RID: 2948
public class NotificationAnimator : MonoBehaviour
{
	// Token: 0x06005B90 RID: 23440 RVA: 0x002193BA File Offset: 0x002175BA
	public void Begin(bool startOffset = true)
	{
		this.Reset();
		this.animating = true;
		if (startOffset)
		{
			this.layoutElement.minWidth = 100f;
			return;
		}
		this.layoutElement.minWidth = 1f;
		this.speed = -10f;
	}

	// Token: 0x06005B91 RID: 23441 RVA: 0x002193F8 File Offset: 0x002175F8
	private void Reset()
	{
		this.bounceCount = 2;
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.layoutElement.minWidth = 0f;
		this.speed = 1f;
	}

	// Token: 0x06005B92 RID: 23442 RVA: 0x00219428 File Offset: 0x00217628
	public void Stop()
	{
		this.Reset();
		this.animating = false;
	}

	// Token: 0x06005B93 RID: 23443 RVA: 0x00219438 File Offset: 0x00217638
	private void LateUpdate()
	{
		if (!this.animating)
		{
			return;
		}
		this.layoutElement.minWidth -= this.speed;
		this.speed += 0.5f;
		if (this.layoutElement.minWidth <= 0f)
		{
			if (this.bounceCount > 0)
			{
				this.bounceCount--;
				this.speed = -this.speed / Mathf.Pow(2f, (float)(2 - this.bounceCount));
				this.layoutElement.minWidth = -this.speed;
				return;
			}
			this.layoutElement.minWidth = 0f;
			this.Stop();
		}
	}

	// Token: 0x04003DB4 RID: 15796
	private const float START_SPEED = 1f;

	// Token: 0x04003DB5 RID: 15797
	private const float ACCELERATION = 0.5f;

	// Token: 0x04003DB6 RID: 15798
	private const float BOUNCE_DAMPEN = 2f;

	// Token: 0x04003DB7 RID: 15799
	private const int BOUNCE_COUNT = 2;

	// Token: 0x04003DB8 RID: 15800
	private const float OFFSETX = 100f;

	// Token: 0x04003DB9 RID: 15801
	private float speed = 1f;

	// Token: 0x04003DBA RID: 15802
	private int bounceCount = 2;

	// Token: 0x04003DBB RID: 15803
	private LayoutElement layoutElement;

	// Token: 0x04003DBC RID: 15804
	[SerializeField]
	private bool animating = true;
}
