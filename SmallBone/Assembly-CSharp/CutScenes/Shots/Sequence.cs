using System;
using System.Collections;
using System.Linq;
using CutScenes.Shots.Sequences;
using Runnables;
using UnityEditor;

namespace CutScenes.Shots
{
	// Token: 0x020001C3 RID: 451
	public abstract class Sequence : CRunnable
	{
		// Token: 0x040007D1 RID: 2001
		public new static readonly Type[] types = new Type[]
		{
			typeof(CharacterMoveTo),
			typeof(CharacterLootAt),
			typeof(ShowDialog),
			typeof(ShowLine),
			typeof(ShowRandomDialog),
			typeof(ShowEndingGameResult),
			typeof(CutScenes.Shots.Sequences.RunAction),
			typeof(OpenChatSelector),
			typeof(OpenContentSelector),
			typeof(Talk),
			typeof(TalkRaw),
			typeof(TalkRandomly),
			typeof(TalkCacheText),
			typeof(NextTalk),
			typeof(WaitForTransfom)
		};

		// Token: 0x020001C4 RID: 452
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x0600097D RID: 2429 RVA: 0x0001B162 File Offset: 0x00019362
			public SubcomponentAttribute() : base(true, CRunnable.types.Concat(Sequence.types).ToArray<Type>())
			{
			}
		}

		// Token: 0x020001C5 RID: 453
		[Serializable]
		public class Subcomponents : SubcomponentArray<CRunnable>
		{
			// Token: 0x0600097E RID: 2430 RVA: 0x0001B17F File Offset: 0x0001937F
			public IEnumerator CRun()
			{
				int num;
				for (int i = 0; i < base.components.Length; i = num + 1)
				{
					yield return base.components[i].CRun();
					num = i;
				}
				yield break;
			}
		}
	}
}
