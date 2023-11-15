using System;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001459 RID: 5209
	[Serializable]
	public class SharedUInt : SharedVariable<uint>
	{
		// Token: 0x060065D8 RID: 26072 RVA: 0x00126435 File Offset: 0x00124635
		public static implicit operator SharedUInt(uint value)
		{
			return new SharedUInt
			{
				mValue = value
			};
		}
	}
}
