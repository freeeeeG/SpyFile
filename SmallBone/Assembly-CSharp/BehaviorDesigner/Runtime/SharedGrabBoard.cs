using System;
using Utils;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200144D RID: 5197
	[Serializable]
	public class SharedGrabBoard : SharedVariable<GrabBoard>
	{
		// Token: 0x060065C0 RID: 26048 RVA: 0x0012632D File Offset: 0x0012452D
		public static implicit operator SharedGrabBoard(GrabBoard value)
		{
			return new SharedGrabBoard
			{
				mValue = value
			};
		}
	}
}
