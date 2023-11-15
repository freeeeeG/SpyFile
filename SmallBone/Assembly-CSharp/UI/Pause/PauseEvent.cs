using System;
using UnityEditor;
using UnityEngine;

namespace UI.Pause
{
	// Token: 0x02000420 RID: 1056
	public abstract class PauseEvent : MonoBehaviour
	{
		// Token: 0x06001404 RID: 5124
		public abstract void Invoke();

		// Token: 0x04001102 RID: 4354
		public static Type[] types = new Type[]
		{
			typeof(PauseMenuPopUp),
			typeof(StorySkip),
			typeof(CreditExit),
			typeof(Empty)
		};

		// Token: 0x02000421 RID: 1057
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06001407 RID: 5127 RVA: 0x0003D3A0 File Offset: 0x0003B5A0
			public SubcomponentAttribute() : base(true, PauseEvent.types)
			{
			}
		}
	}
}
