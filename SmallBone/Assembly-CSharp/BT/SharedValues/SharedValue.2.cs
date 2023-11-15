using System;

namespace BT.SharedValues
{
	// Token: 0x02001429 RID: 5161
	public class SharedValue<T> : SharedValue
	{
		// Token: 0x06006555 RID: 25941 RVA: 0x001256D8 File Offset: 0x001238D8
		public T GetValue()
		{
			return this._value;
		}

		// Token: 0x06006556 RID: 25942 RVA: 0x001256E0 File Offset: 0x001238E0
		public void SetValue(T value)
		{
			this._value = value;
		}

		// Token: 0x06006557 RID: 25943 RVA: 0x001256E9 File Offset: 0x001238E9
		public SharedValue(T value)
		{
			this._value = value;
		}

		// Token: 0x040051A2 RID: 20898
		protected T _value;
	}
}
