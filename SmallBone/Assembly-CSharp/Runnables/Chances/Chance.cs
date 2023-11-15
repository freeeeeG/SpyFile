using System;
using UnityEditor;
using UnityEngine;

namespace Runnables.Chances
{
	// Token: 0x02000344 RID: 836
	public abstract class Chance : MonoBehaviour
	{
		// Token: 0x06000FD0 RID: 4048
		public abstract bool IsTrue();

		// Token: 0x04000CF8 RID: 3320
		public static readonly Type[] types = new Type[]
		{
			typeof(Constant),
			typeof(ByValueComponent)
		};

		// Token: 0x02000345 RID: 837
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06000FD3 RID: 4051 RVA: 0x0002F999 File Offset: 0x0002DB99
			public SubcomponentAttribute() : base(true, Chance.types)
			{
			}
		}
	}
}
