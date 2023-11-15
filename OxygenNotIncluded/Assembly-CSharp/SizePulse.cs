using System;
using UnityEngine;

// Token: 0x02000AA5 RID: 2725
public class SizePulse : MonoBehaviour
{
	// Token: 0x06005332 RID: 21298 RVA: 0x001DD114 File Offset: 0x001DB314
	private void Start()
	{
		if (base.GetComponents<SizePulse>().Length > 1)
		{
			UnityEngine.Object.Destroy(this);
		}
		RectTransform rectTransform = (RectTransform)base.transform;
		this.from = rectTransform.localScale;
		this.cur = this.from;
		this.to = this.from * this.multiplier;
	}

	// Token: 0x06005333 RID: 21299 RVA: 0x001DD174 File Offset: 0x001DB374
	private void Update()
	{
		float num = this.updateWhenPaused ? Time.unscaledDeltaTime : Time.deltaTime;
		num *= this.speed;
		SizePulse.State state = this.state;
		if (state != SizePulse.State.Up)
		{
			if (state == SizePulse.State.Down)
			{
				this.cur = Vector2.Lerp(this.cur, this.from, num);
				if ((this.from - this.cur).sqrMagnitude < 0.0001f)
				{
					this.cur = this.from;
					this.state = SizePulse.State.Finished;
					if (this.onComplete != null)
					{
						this.onComplete();
					}
				}
			}
		}
		else
		{
			this.cur = Vector2.Lerp(this.cur, this.to, num);
			if ((this.to - this.cur).sqrMagnitude < 0.0001f)
			{
				this.cur = this.to;
				this.state = SizePulse.State.Down;
			}
		}
		((RectTransform)base.transform).localScale = new Vector3(this.cur.x, this.cur.y, 1f);
	}

	// Token: 0x04003760 RID: 14176
	public System.Action onComplete;

	// Token: 0x04003761 RID: 14177
	public Vector2 from = Vector2.one;

	// Token: 0x04003762 RID: 14178
	public Vector2 to = Vector2.one;

	// Token: 0x04003763 RID: 14179
	public float multiplier = 1.25f;

	// Token: 0x04003764 RID: 14180
	public float speed = 1f;

	// Token: 0x04003765 RID: 14181
	public bool updateWhenPaused;

	// Token: 0x04003766 RID: 14182
	private Vector2 cur;

	// Token: 0x04003767 RID: 14183
	private SizePulse.State state;

	// Token: 0x020019BB RID: 6587
	private enum State
	{
		// Token: 0x0400772D RID: 30509
		Up,
		// Token: 0x0400772E RID: 30510
		Down,
		// Token: 0x0400772F RID: 30511
		Finished
	}
}
