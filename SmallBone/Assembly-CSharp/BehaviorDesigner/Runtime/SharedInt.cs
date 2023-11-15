using System;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200144E RID: 5198
	[Serializable]
	public class SharedInt : SharedVariable<int>
	{
		// Token: 0x060065C2 RID: 26050 RVA: 0x00126343 File Offset: 0x00124543
		public static implicit operator SharedInt(int value)
		{
			return new SharedInt
			{
				mValue = value
			};
		}
	}
}
