using System;
using UnityEditor;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x020000FA RID: 250
	public abstract class Event : MonoBehaviour
	{
		// Token: 0x060004D3 RID: 1235
		public abstract void Run();

		// Token: 0x040003B7 RID: 951
		public static Type[] types = new Type[]
		{
			typeof(ExecuteRunnable),
			typeof(SaveSkulStoryData)
		};

		// Token: 0x020000FB RID: 251
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x060004D6 RID: 1238 RVA: 0x0000F679 File Offset: 0x0000D879
			public SubcomponentAttribute() : base(true, Event.types)
			{
			}
		}

		// Token: 0x020000FC RID: 252
		[Serializable]
		public class Subcomponents : SubcomponentArray<Event>
		{
			// Token: 0x060004D7 RID: 1239 RVA: 0x0000F688 File Offset: 0x0000D888
			public void Run()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Run();
				}
			}
		}
	}
}
