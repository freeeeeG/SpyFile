using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002C5 RID: 709
	public class Touch
	{
		// Token: 0x06000E82 RID: 3714 RVA: 0x00046BC7 File Offset: 0x00044FC7
		internal Touch(int fingerId)
		{
			this.fingerId = fingerId;
			this.phase = TouchPhase.Ended;
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00046BE0 File Offset: 0x00044FE0
		internal void SetWithTouchData(Touch touch, ulong updateTick, float deltaTime)
		{
			this.phase = touch.phase;
			this.tapCount = touch.tapCount;
			Vector2 a = touch.position;
			if (a.x < 0f)
			{
				a.x = (float)Screen.width + a.x;
			}
			if (this.phase == TouchPhase.Began)
			{
				this.deltaPosition = Vector2.zero;
				this.lastPosition = a;
				this.position = a;
			}
			else
			{
				if (this.phase == TouchPhase.Stationary)
				{
					this.phase = TouchPhase.Moved;
				}
				this.deltaPosition = a - this.lastPosition;
				this.lastPosition = this.position;
				this.position = a;
			}
			this.deltaTime = deltaTime;
			this.updateTick = updateTick;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00046CA4 File Offset: 0x000450A4
		internal bool SetWithMouseData(ulong updateTick, float deltaTime)
		{
			if (Input.touchCount > 0)
			{
				return false;
			}
			Vector2 a = new Vector2(Mathf.Round(Input.mousePosition.x), Mathf.Round(Input.mousePosition.y));
			if (Input.GetMouseButtonDown(0))
			{
				this.phase = TouchPhase.Began;
				this.tapCount = 1;
				this.deltaPosition = Vector2.zero;
				this.lastPosition = a;
				this.position = a;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.phase = TouchPhase.Ended;
				this.tapCount = 1;
				this.deltaPosition = a - this.lastPosition;
				this.lastPosition = this.position;
				this.position = a;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			if (Input.GetMouseButton(0))
			{
				this.phase = TouchPhase.Moved;
				this.tapCount = 1;
				this.deltaPosition = a - this.lastPosition;
				this.lastPosition = this.position;
				this.position = a;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			return false;
		}

		// Token: 0x04000B47 RID: 2887
		public int fingerId;

		// Token: 0x04000B48 RID: 2888
		public TouchPhase phase;

		// Token: 0x04000B49 RID: 2889
		public int tapCount;

		// Token: 0x04000B4A RID: 2890
		public Vector2 position;

		// Token: 0x04000B4B RID: 2891
		public Vector2 deltaPosition;

		// Token: 0x04000B4C RID: 2892
		public Vector2 lastPosition;

		// Token: 0x04000B4D RID: 2893
		public float deltaTime;

		// Token: 0x04000B4E RID: 2894
		public ulong updateTick;
	}
}
