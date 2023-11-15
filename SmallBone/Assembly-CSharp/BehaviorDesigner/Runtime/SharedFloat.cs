using System;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200144A RID: 5194
	[Serializable]
	public class SharedFloat : SharedVariable<float>
	{
		// Token: 0x060065BA RID: 26042 RVA: 0x001262EB File Offset: 0x001244EB
		public static implicit operator SharedFloat(float value)
		{
			return new SharedFloat
			{
				Value = value
			};
		}
	}
}
