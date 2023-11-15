using System;

namespace InControl
{
	// Token: 0x020002B1 RID: 689
	public struct InputControlState
	{
		// Token: 0x06000D31 RID: 3377 RVA: 0x00043065 File Offset: 0x00041465
		public void Reset()
		{
			this.State = false;
			this.Value = 0f;
			this.RawValue = 0f;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00043084 File Offset: 0x00041484
		public void Set(float value)
		{
			this.Value = value;
			this.State = Utility.IsNotZero(value);
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00043099 File Offset: 0x00041499
		public void Set(float value, float threshold)
		{
			this.Value = value;
			this.State = Utility.AbsoluteIsOverThreshold(value, threshold);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x000430AF File Offset: 0x000414AF
		public void Set(bool state)
		{
			this.State = state;
			this.Value = ((!state) ? 0f : 1f);
			this.RawValue = this.Value;
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x000430DF File Offset: 0x000414DF
		public static implicit operator bool(InputControlState state)
		{
			return state.State;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x000430E8 File Offset: 0x000414E8
		public static implicit operator float(InputControlState state)
		{
			return state.Value;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x000430F1 File Offset: 0x000414F1
		public static bool operator ==(InputControlState a, InputControlState b)
		{
			return a.State == b.State && Utility.Approximately(a.Value, b.Value);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0004311B File Offset: 0x0004151B
		public static bool operator !=(InputControlState a, InputControlState b)
		{
			return a.State != b.State || !Utility.Approximately(a.Value, b.Value);
		}

		// Token: 0x04000A35 RID: 2613
		public bool State;

		// Token: 0x04000A36 RID: 2614
		public float Value;

		// Token: 0x04000A37 RID: 2615
		public float RawValue;
	}
}
