using System;
using UnityEditor;
using UnityEngine;

namespace BT.Conditions
{
	// Token: 0x0200142C RID: 5164
	public abstract class Condition : MonoBehaviour
	{
		// Token: 0x0600655C RID: 25948
		protected abstract bool Check(Context context);

		// Token: 0x0600655D RID: 25949 RVA: 0x0012571F File Offset: 0x0012391F
		public bool IsSatisfied(Context context)
		{
			return this._inverter ^ this.Check(context);
		}

		// Token: 0x040051A5 RID: 20901
		[SerializeField]
		private bool _inverter;

		// Token: 0x0200142D RID: 5165
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x0600655F RID: 25951 RVA: 0x0012572F File Offset: 0x0012392F
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, Condition.SubcomponentAttribute.types)
			{
			}

			// Token: 0x040051A6 RID: 20902
			public new static readonly Type[] types = new Type[]
			{
				typeof(ActionCoolDown),
				typeof(Chance),
				typeof(CoolDown),
				typeof(PlayerInRange),
				typeof(TargetOnOwnerPlatform),
				typeof(HasTarget)
			};
		}
	}
}
