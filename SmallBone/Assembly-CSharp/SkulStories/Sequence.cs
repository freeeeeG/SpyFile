using System;
using System.Collections;
using Scenes;
using UnityEditor;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x0200011D RID: 285
	public abstract class Sequence : MonoBehaviour
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x00010D3C File Offset: 0x0000EF3C
		private void Awake()
		{
			this._narration = Scene<GameBase>.instance.uiManager.narration;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00010D53 File Offset: 0x0000EF53
		public IEnumerator CCheckWait()
		{
			if (this._wait && !this._narration.skipped)
			{
				yield return this.CRun();
			}
			else
			{
				base.StartCoroutine(this.CRun());
			}
			yield break;
		}

		// Token: 0x0600059A RID: 1434
		public abstract IEnumerator CRun();

		// Token: 0x04000445 RID: 1093
		public static readonly Type[] types = new Type[]
		{
			typeof(PlaceText),
			typeof(FadeInOut),
			typeof(ShowTexts),
			typeof(PlayNarration),
			typeof(WaitInput),
			typeof(ChangeScene),
			typeof(ScrollImage),
			typeof(RunCutSceneSequence),
			typeof(RunOperation)
		};

		// Token: 0x04000446 RID: 1094
		[SerializeField]
		private bool _wait;

		// Token: 0x04000447 RID: 1095
		protected Narration _narration;

		// Token: 0x0200011E RID: 286
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x0600059D RID: 1437 RVA: 0x00010DF2 File Offset: 0x0000EFF2
			public SubcomponentAttribute() : base(true, Sequence.types)
			{
			}
		}
	}
}
