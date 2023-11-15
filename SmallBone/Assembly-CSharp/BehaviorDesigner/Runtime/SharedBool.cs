using System;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001444 RID: 5188
	[Serializable]
	public class SharedBool : SharedVariable<bool>
	{
		// Token: 0x060065AE RID: 26030 RVA: 0x00126267 File Offset: 0x00124467
		public static implicit operator SharedBool(bool value)
		{
			return new SharedBool
			{
				mValue = value
			};
		}
	}
}
