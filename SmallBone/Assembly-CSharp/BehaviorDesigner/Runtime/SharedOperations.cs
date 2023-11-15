using System;
using Characters.Operations;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001453 RID: 5203
	[Serializable]
	public class SharedOperations : SharedVariable<OperationInfos>
	{
		// Token: 0x060065CC RID: 26060 RVA: 0x001263B1 File Offset: 0x001245B1
		public static explicit operator SharedOperations(OperationInfos value)
		{
			return new SharedOperations
			{
				mValue = value
			};
		}
	}
}
