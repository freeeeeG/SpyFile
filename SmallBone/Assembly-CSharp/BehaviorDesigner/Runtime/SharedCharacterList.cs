using System;
using System.Collections.Generic;
using Characters;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001447 RID: 5191
	[Serializable]
	public class SharedCharacterList : SharedVariable<List<Character>>
	{
		// Token: 0x060065B4 RID: 26036 RVA: 0x001262A9 File Offset: 0x001244A9
		public static implicit operator SharedCharacterList(List<Character> value)
		{
			return new SharedCharacterList
			{
				mValue = value
			};
		}
	}
}
