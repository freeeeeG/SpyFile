using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200128F RID: 4751
	public abstract class Behaviour : MonoBehaviour
	{
		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06005E3E RID: 24126 RVA: 0x001150AA File Offset: 0x001132AA
		// (set) Token: 0x06005E3F RID: 24127 RVA: 0x001150B2 File Offset: 0x001132B2
		public Behaviour.Result result { get; set; }

		// Token: 0x06005E40 RID: 24128 RVA: 0x001150BC File Offset: 0x001132BC
		private void Stop()
		{
			if (this.result.Equals(Behaviour.Result.Doing))
			{
				this.result = Behaviour.Result.Done;
			}
		}

		// Token: 0x06005E41 RID: 24129 RVA: 0x001150EC File Offset: 0x001132EC
		protected IEnumerator CExpire(AIController controller, Vector2 durationMinMax)
		{
			float seconds = UnityEngine.Random.Range(durationMinMax.x, durationMinMax.y);
			yield return controller.character.chronometer.master.WaitForSeconds(seconds);
			this.Stop();
			yield break;
		}

		// Token: 0x06005E42 RID: 24130 RVA: 0x00115109 File Offset: 0x00113309
		protected IEnumerator CExpire(AIController controller, float duration)
		{
			yield return controller.character.chronometer.master.WaitForSeconds(duration);
			this.Stop();
			yield break;
		}

		// Token: 0x06005E43 RID: 24131
		public abstract IEnumerator CRun(AIController controller);

		// Token: 0x06005E44 RID: 24132 RVA: 0x00115128 File Offset: 0x00113328
		public void StopPropagation()
		{
			this.result = Behaviour.Result.Done;
			if (this._childs == null)
			{
				return;
			}
			foreach (Behaviour behaviour in this._childs)
			{
				if (behaviour != null)
				{
					behaviour.StopPropagation();
				}
			}
		}

		// Token: 0x06005E45 RID: 24133 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04004BB8 RID: 19384
		public Action onStart;

		// Token: 0x04004BB9 RID: 19385
		public Action onEnd;

		// Token: 0x04004BBA RID: 19386
		protected List<Behaviour> _childs;

		// Token: 0x02001290 RID: 4752
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06005E47 RID: 24135 RVA: 0x00115190 File Offset: 0x00113390
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, Behaviour.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04004BBC RID: 19388
			public new static readonly Type[] types = new Type[]
			{
				typeof(Selector),
				typeof(Sequence),
				typeof(Height),
				typeof(Conditional),
				typeof(Count),
				typeof(Chance),
				typeof(CoolTime),
				typeof(UniformSelector),
				typeof(WeightedSelector),
				typeof(Repeat),
				typeof(RandomBehaviour),
				typeof(InfiniteLoop),
				typeof(Idle),
				typeof(SkipableIdle),
				typeof(TimeLoop)
			};
		}

		// Token: 0x02001291 RID: 4753
		[Serializable]
		public class Subcomponents : SubcomponentArray<Behaviour>
		{
		}

		// Token: 0x02001292 RID: 4754
		public enum Result
		{
			// Token: 0x04004BBE RID: 19390
			Fail,
			// Token: 0x04004BBF RID: 19391
			Doing,
			// Token: 0x04004BC0 RID: 19392
			Success,
			// Token: 0x04004BC1 RID: 19393
			Done
		}
	}
}
