using System;
using Characters;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001445 RID: 5189
	[Serializable]
	public class SharedCharacter : SharedVariable<Character>
	{
		// Token: 0x060065B0 RID: 26032 RVA: 0x0012627D File Offset: 0x0012447D
		public static explicit operator SharedCharacter(Character value)
		{
			return new SharedCharacter
			{
				mValue = value
			};
		}
	}
}
