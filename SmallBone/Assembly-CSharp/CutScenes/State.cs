using System;
using UnityEditor;
using UnityEngine;

namespace CutScenes
{
	// Token: 0x020001B0 RID: 432
	public abstract class State : MonoBehaviour
	{
		// Token: 0x06000933 RID: 2355
		public abstract void Attach();

		// Token: 0x06000934 RID: 2356
		public abstract void Detach();

		// Token: 0x04000792 RID: 1938
		protected static object key = new object();

		// Token: 0x04000793 RID: 1939
		public static readonly Type[] types = new Type[]
		{
			typeof(PlayerInputBlock),
			typeof(PlayerMovementBlock),
			typeof(CharacterInvulnerable)
		};

		// Token: 0x020001B1 RID: 433
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06000937 RID: 2359 RVA: 0x0001A526 File Offset: 0x00018726
			public SubcomponentAttribute() : base(true, State.types)
			{
			}
		}
	}
}
