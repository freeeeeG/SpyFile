using System;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001456 RID: 5206
	[Serializable]
	public class SharedString : SharedVariable<string>
	{
		// Token: 0x060065D2 RID: 26066 RVA: 0x001263F3 File Offset: 0x001245F3
		public static implicit operator SharedString(string value)
		{
			return new SharedString
			{
				mValue = value
			};
		}
	}
}
