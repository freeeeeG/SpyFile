using System;
using UnityEditor;
using UnityEngine;

namespace CutScenes.Shots
{
	// Token: 0x020001CB RID: 459
	public abstract class Shot : MonoBehaviour
	{
		// Token: 0x06000998 RID: 2456
		public abstract void Run();

		// Token: 0x06000999 RID: 2457
		public abstract void SetNext(Shot next);

		// Token: 0x040007E1 RID: 2017
		public static Type[] types = new Type[]
		{
			typeof(EventInfos),
			typeof(SequenceInfos)
		};

		// Token: 0x020001CC RID: 460
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x0600099C RID: 2460 RVA: 0x0001B3B6 File Offset: 0x000195B6
			public SubcomponentAttribute() : base(true, Shot.types)
			{
			}
		}
	}
}
