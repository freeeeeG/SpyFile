using System;
using Characters.Actions;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001446 RID: 5190
	[Serializable]
	public class SharedCharacterAction : SharedVariable<Characters.Actions.Action>
	{
		// Token: 0x060065B2 RID: 26034 RVA: 0x00126293 File Offset: 0x00124493
		public static explicit operator SharedCharacterAction(Characters.Actions.Action value)
		{
			return new SharedCharacterAction
			{
				mValue = value
			};
		}
	}
}
