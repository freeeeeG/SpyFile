using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects
{
	// Token: 0x020008EF RID: 2287
	public abstract class QuintessenceEffect : MonoBehaviour
	{
		// Token: 0x060030E5 RID: 12517 RVA: 0x000923EC File Offset: 0x000905EC
		public void Invoke(Quintessence quintessence)
		{
			this.OnInvoke(quintessence);
		}

		// Token: 0x060030E6 RID: 12518
		protected abstract void OnInvoke(Quintessence quintessence);

		// Token: 0x0400284B RID: 10315
		public static readonly Type[] types = new Type[]
		{
			typeof(AttachAbility),
			typeof(AttachAbilityToTargetOnGaveStatus),
			typeof(RunOperations),
			typeof(RunAction)
		};

		// Token: 0x020008F0 RID: 2288
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x060030E9 RID: 12521 RVA: 0x00092444 File Offset: 0x00090644
			public SubcomponentAttribute() : base(true, QuintessenceEffect.types)
			{
			}
		}

		// Token: 0x020008F1 RID: 2289
		[Serializable]
		public class Subcomponents : SubcomponentArray<QuintessenceEffect>
		{
			// Token: 0x060030EA RID: 12522 RVA: 0x00092454 File Offset: 0x00090654
			public void Invoke(Quintessence quintessence)
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Invoke(quintessence);
				}
			}
		}
	}
}
