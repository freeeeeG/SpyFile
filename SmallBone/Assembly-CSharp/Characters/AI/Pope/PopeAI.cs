using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011F6 RID: 4598
	public sealed class PopeAI : AIController
	{
		// Token: 0x06005A43 RID: 23107 RVA: 0x0010C14A File Offset: 0x0010A34A
		private new void OnEnable()
		{
			base.OnEnable();
		}

		// Token: 0x06005A44 RID: 23108 RVA: 0x0010C152 File Offset: 0x0010A352
		private void OnDestroy()
		{
			this._idle.Dispose();
		}

		// Token: 0x06005A45 RID: 23109 RVA: 0x0010C15F File Offset: 0x0010A35F
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			for (;;)
			{
				this._sequenceCoroutine = this._sequence.CRun(this);
				yield return this._sequenceCoroutine;
				yield return Chronometer.global.WaitForSeconds(1f);
			}
			yield break;
		}

		// Token: 0x06005A46 RID: 23110 RVA: 0x000F0D27 File Offset: 0x000EEF27
		public void StartCombat()
		{
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06005A47 RID: 23111 RVA: 0x0010C16E File Offset: 0x0010A36E
		public void NextPhase()
		{
			base.StopAllCoroutines();
			this._idle.SetNextClip();
			this._sequence.NextPhase();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(base.CCheckStun());
		}

		// Token: 0x040048E7 RID: 18663
		[SerializeField]
		private PopeAI.Animation _idle;

		// Token: 0x040048E8 RID: 18664
		[SerializeField]
		[Subcomponent(typeof(Sequence))]
		private Sequence _sequence;

		// Token: 0x040048E9 RID: 18665
		private IEnumerator _sequenceCoroutine;

		// Token: 0x020011F7 RID: 4599
		[Serializable]
		private class Animation
		{
			// Token: 0x06005A49 RID: 23113 RVA: 0x0010C1A6 File Offset: 0x0010A3A6
			public void SetNextClip()
			{
				if (this._characterAnimation == null)
				{
					Debug.LogError("Character Animation is null");
					return;
				}
				this._characterAnimation.SetIdle(this._phase2IdleClip);
				this._characterAnimation.SetWalk(this._phase2IdleClip);
			}

			// Token: 0x06005A4A RID: 23114 RVA: 0x0010C1E3 File Offset: 0x0010A3E3
			public void Dispose()
			{
				this._phase2IdleClip = null;
				this._characterAnimation = null;
			}

			// Token: 0x040048EA RID: 18666
			[SerializeField]
			private AnimationClip _phase2IdleClip;

			// Token: 0x040048EB RID: 18667
			[SerializeField]
			private CharacterAnimation _characterAnimation;
		}
	}
}
