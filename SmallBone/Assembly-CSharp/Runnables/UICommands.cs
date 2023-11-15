using System;
using UnityEditor;

namespace Runnables
{
	// Token: 0x020002E5 RID: 741
	public abstract class UICommands : Runnable
	{
		// Token: 0x04000C2D RID: 3117
		public new static readonly Type[] types = new Type[]
		{
			typeof(OpenUIHealthBar),
			typeof(CloseAllUIHealthBar),
			typeof(CompleteUIConversation),
			typeof(SetHeadUpDisplay)
		};

		// Token: 0x020002E6 RID: 742
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06000EB6 RID: 3766 RVA: 0x0002DA18 File Offset: 0x0002BC18
			public SubcomponentAttribute() : base(true, UICommands.types)
			{
			}
		}
	}
}
