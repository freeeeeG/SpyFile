using System;
using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002C2 RID: 706
	public class InvokeOnCharacterHit : MonoBehaviour
	{
		// Token: 0x06000E4D RID: 3661 RVA: 0x0002D0BC File Offset: 0x0002B2BC
		private void Awake()
		{
			if (this._owner == null)
			{
				this._owner = base.GetComponentInParent<Character>();
			}
			foreach (IHitEvent hitEvent in this._hitEvents)
			{
				this._owner.health.onTookDamage += new TookDamageDelegate(this.ExecuteHitEvents);
			}
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0002D118 File Offset: 0x0002B318
		private void OnDestroy()
		{
			foreach (IHitEvent hitEvent in this._hitEvents)
			{
				this._owner.health.onTookDamage -= new TookDamageDelegate(this.ExecuteHitEvents);
			}
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0002D15C File Offset: 0x0002B35C
		private void ExecuteHitEvents(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (this._trigger.IsSatisfied())
			{
				IHitEvent[] hitEvents = this._hitEvents;
				for (int i = 0; i < hitEvents.Length; i++)
				{
					hitEvents[i].OnHit(originalDamage, tookDamage, damageDealt);
				}
			}
		}

		// Token: 0x04000BE5 RID: 3045
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BE6 RID: 3046
		[SerializeField]
		private Character _owner;

		// Token: 0x04000BE7 RID: 3047
		[SerializeReference]
		[SubclassSelector]
		private IHitEvent[] _hitEvents;
	}
}
