using System;
using System.Collections;
using Characters.Actions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Quintessences
{
	// Token: 0x020008DD RID: 2269
	public class RunAction : UseQuintessence
	{
		// Token: 0x06003071 RID: 12401 RVA: 0x000912DF File Offset: 0x0008F4DF
		protected override void Awake()
		{
			base.Awake();
			this._character.gameObject.SetActive(false);
			this._character.transform.parent = null;
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x00091309 File Offset: 0x0008F509
		private void OnEnable()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.Disable;
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x00091326 File Offset: 0x0008F526
		private void OnDisable()
		{
			this.DetachEvents();
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.Disable;
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x00091349 File Offset: 0x0008F549
		private void Disable()
		{
			this.DetachEvents();
			this._character.gameObject.SetActive(false);
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x00091364 File Offset: 0x0008F564
		private void AttachEvents()
		{
			Character character = this._character;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(this.OnKilled));
			this._character.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.OnGiveDamage));
			Character character2 = this._character;
			character2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character2.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			Character character3 = this._character;
			character3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(character3.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x00091408 File Offset: 0x0008F608
		private void DetachEvents()
		{
			Character character = this._character;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(this.OnKilled));
			this._character.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			Character character2 = this._character;
			character2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character2.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			Character character3 = this._character;
			character3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(character3.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x000914A8 File Offset: 0x0008F6A8
		protected override void OnUse()
		{
			if (!this._character.gameObject.activeSelf)
			{
				if (this._flipObject != null)
				{
					if (this._quintessence.owner.lookingDirection == Character.LookingDirection.Right)
					{
						this._flipObject.localScale = new Vector2(1f, 1f);
					}
					else
					{
						this._flipObject.localScale = new Vector2(-1f, 1f);
					}
				}
				this._quintessence.owner.StartCoroutine(this.CUse());
			}
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x0009153E File Offset: 0x0008F73E
		private IEnumerator CUse()
		{
			this._character.stat.getDamageOverridingStat = this._quintessence.owner.stat;
			this.AttachEvents();
			this._character.ForceToLookAt(this._quintessence.owner.desiringLookingDirection);
			this._character.transform.position = base.transform.position;
			this._character.gameObject.SetActive(true);
			this._action.TryStart();
			while (this._action.running)
			{
				yield return null;
			}
			this.Disable();
			yield break;
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x0009154D File Offset: 0x0008F74D
		private void OnDestroy()
		{
			if (!Service.quitting)
			{
				UnityEngine.Object.Destroy(this._character.gameObject);
			}
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x00091566 File Offset: 0x0008F766
		private void OnKilled(ITarget target, ref Damage damage)
		{
			Character.OnKilledDelegate onKilled = this._quintessence.owner.onKilled;
			if (onKilled == null)
			{
				return;
			}
			onKilled(target, ref damage);
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x00091584 File Offset: 0x0008F784
		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			return this._quintessence.owner.onGiveDamage.Invoke(target, ref damage);
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x0009159D File Offset: 0x0008F79D
		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			GaveDamageDelegate onGaveDamage = this._quintessence.owner.onGaveDamage;
			if (onGaveDamage == null)
			{
				return;
			}
			onGaveDamage(target, originalDamage, gaveDamage, damageDealt);
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x000915BE File Offset: 0x0008F7BE
		private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
		{
			Character.OnGaveStatusDelegate onGaveStatus = this._quintessence.owner.onGaveStatus;
			if (onGaveStatus == null)
			{
				return;
			}
			onGaveStatus(target, applyInfo, result);
		}

		// Token: 0x04002814 RID: 10260
		[SerializeField]
		private Character _character;

		// Token: 0x04002815 RID: 10261
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04002816 RID: 10262
		[SerializeField]
		private Transform _flipObject;
	}
}
