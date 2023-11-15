using System;
using System.Collections;
using Characters.AI.Behaviours;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011FD RID: 4605
	public class Sequence : MonoBehaviour, ISequence
	{
		// Token: 0x06005A63 RID: 23139 RVA: 0x0010C5B8 File Offset: 0x0010A7B8
		private void Awake()
		{
			this._phases = new Sequence.IPhase[]
			{
				this._phase1,
				this._phase2
			};
		}

		// Token: 0x06005A64 RID: 23140 RVA: 0x0010C5D8 File Offset: 0x0010A7D8
		public IEnumerator CRun(AIController controller)
		{
			this.reference = this._phases[this._currentPhase].CRun(controller);
			yield return this.reference;
			yield break;
		}

		// Token: 0x06005A65 RID: 23141 RVA: 0x0010C5EE File Offset: 0x0010A7EE
		public void Stop()
		{
			base.StopCoroutine(this.reference);
		}

		// Token: 0x06005A66 RID: 23142 RVA: 0x0010C5FC File Offset: 0x0010A7FC
		public void NextPhase()
		{
			if (this._currentPhase + 1 >= this._phases.Length)
			{
				Debug.LogError("Index of out of range in Pope's phase");
				return;
			}
			this._currentPhase++;
		}

		// Token: 0x040048FD RID: 18685
		[SerializeField]
		[Header("Phase1")]
		private Sequence.Phase1 _phase1;

		// Token: 0x040048FE RID: 18686
		[Space]
		[Header("Phase2")]
		[SerializeField]
		private Sequence.Phase2 _phase2;

		// Token: 0x040048FF RID: 18687
		private Sequence.IPhase[] _phases;

		// Token: 0x04004900 RID: 18688
		private int _currentPhase;

		// Token: 0x04004901 RID: 18689
		private IEnumerator reference;

		// Token: 0x020011FE RID: 4606
		public interface IPhase
		{
			// Token: 0x06005A68 RID: 23144
			IEnumerator CRun(AIController controller);
		}

		// Token: 0x020011FF RID: 4607
		[Serializable]
		private class Phase1 : Sequence.IPhase
		{
			// Token: 0x06005A69 RID: 23145 RVA: 0x0010C629 File Offset: 0x0010A829
			public IEnumerator CRun(AIController controller)
			{
				yield return this._behaviours.CRun(controller);
				yield break;
			}

			// Token: 0x04004902 RID: 18690
			[SerializeField]
			[Characters.AI.Behaviours.Behaviour.SubcomponentAttribute(true)]
			private Characters.AI.Behaviours.Behaviour _behaviours;
		}

		// Token: 0x02001201 RID: 4609
		[Serializable]
		private class Phase2 : Sequence.IPhase
		{
			// Token: 0x06005A71 RID: 23153 RVA: 0x0010C6AB File Offset: 0x0010A8AB
			public IEnumerator CRun(AIController controller)
			{
				yield return this._sequence.CRun(controller);
				yield break;
			}

			// Token: 0x04004907 RID: 18695
			[SerializeField]
			private Characters.AI.Behaviours.Behaviour _sequence;
		}
	}
}
