using System;
using Services;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects
{
	// Token: 0x020008F2 RID: 2290
	public sealed class QuintessenceEventAdapter : MonoBehaviour
	{
		// Token: 0x060030EC RID: 12524 RVA: 0x0009248A File Offset: 0x0009068A
		private void Awake()
		{
			this.Connect();
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x00092492 File Offset: 0x00090692
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			if (this._quintessence == null)
			{
				return;
			}
			this.Disconnect();
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x000924B4 File Offset: 0x000906B4
		private void Connect()
		{
			this._quintessence.onEquipped += this.AttachEvents;
			this._quintessence.onDropped += this.DetachEvents;
			this._quintessence.onDiscard += this.DetachEvents;
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x00092508 File Offset: 0x00090708
		private void Disconnect()
		{
			this._quintessence.onEquipped -= this.AttachEvents;
			this._quintessence.onDropped -= this.DetachEvents;
			this._quintessence.onDiscard -= this.DetachEvents;
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x0009255C File Offset: 0x0009075C
		private void AttachEvents()
		{
			Character quintessenceCharacter = this._quintessenceCharacter;
			quintessenceCharacter.onKilled = (Character.OnKilledDelegate)Delegate.Combine(quintessenceCharacter.onKilled, new Character.OnKilledDelegate(this.OnKilled));
			this._quintessenceCharacter.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.OnGiveDamage));
			Character quintessenceCharacter2 = this._quintessenceCharacter;
			quintessenceCharacter2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(quintessenceCharacter2.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			Character quintessenceCharacter3 = this._quintessenceCharacter;
			quintessenceCharacter3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(quintessenceCharacter3.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000925FF File Offset: 0x000907FF
		private void DetachEvents(Gear gear)
		{
			this.DetachEvents();
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x00092608 File Offset: 0x00090808
		private void DetachEvents()
		{
			Character quintessenceCharacter = this._quintessenceCharacter;
			quintessenceCharacter.onKilled = (Character.OnKilledDelegate)Delegate.Remove(quintessenceCharacter.onKilled, new Character.OnKilledDelegate(this.OnKilled));
			this._quintessenceCharacter.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			Character quintessenceCharacter2 = this._quintessenceCharacter;
			quintessenceCharacter2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(quintessenceCharacter2.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			Character quintessenceCharacter3 = this._quintessenceCharacter;
			quintessenceCharacter3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(quintessenceCharacter3.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x000926A7 File Offset: 0x000908A7
		private void OnKilled(ITarget target, ref Damage damage)
		{
			Character.OnKilledDelegate onKilled = this._quintessence.owner.onKilled;
			if (onKilled == null)
			{
				return;
			}
			onKilled(target, ref damage);
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x000926C5 File Offset: 0x000908C5
		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			return this._quintessence.owner.onGiveDamage.Invoke(target, ref damage);
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x000926DE File Offset: 0x000908DE
		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			GaveDamageDelegate onGaveDamage = this._quintessence.owner.onGaveDamage;
			if (onGaveDamage == null)
			{
				return;
			}
			onGaveDamage(target, originalDamage, gaveDamage, damageDealt);
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x000926FF File Offset: 0x000908FF
		private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
		{
			Character.OnGaveStatusDelegate onGaveStatus = this._quintessence.owner.onGaveStatus;
			if (onGaveStatus == null)
			{
				return;
			}
			onGaveStatus(target, applyInfo, result);
		}

		// Token: 0x0400284C RID: 10316
		[SerializeField]
		private Quintessence _quintessence;

		// Token: 0x0400284D RID: 10317
		[SerializeField]
		private Character _quintessenceCharacter;
	}
}
