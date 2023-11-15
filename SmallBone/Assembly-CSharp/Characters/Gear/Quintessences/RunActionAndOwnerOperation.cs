using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences
{
	// Token: 0x020008DF RID: 2271
	public class RunActionAndOwnerOperation : UseQuintessence
	{
		// Token: 0x06003085 RID: 12421 RVA: 0x000916D0 File Offset: 0x0008F8D0
		protected override void Awake()
		{
			base.Awake();
			this._quintessence.onEquipped += this._operations.Initialize;
			this._character.gameObject.SetActive(false);
			this._character.transform.parent = null;
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x00091724 File Offset: 0x0008F924
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

		// Token: 0x06003087 RID: 12423 RVA: 0x000917C8 File Offset: 0x0008F9C8
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

		// Token: 0x06003088 RID: 12424 RVA: 0x00091867 File Offset: 0x0008FA67
		private void Disable()
		{
			if (this._character == null)
			{
				return;
			}
			this.DetachEvents();
			this._operations.StopAll();
			this._character.gameObject.SetActive(false);
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x0009189A File Offset: 0x0008FA9A
		private void OnEnable()
		{
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += this.Disable;
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x000918B7 File Offset: 0x0008FAB7
		private void OnDisable()
		{
			this.Disable();
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= this.Disable;
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x000918DA File Offset: 0x0008FADA
		protected override void OnUse()
		{
			if (!this._character.gameObject.activeSelf)
			{
				this._quintessence.owner.StartCoroutine(this.CUse());
			}
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x00091905 File Offset: 0x0008FB05
		private IEnumerator CUse()
		{
			this._character.stat.getDamageOverridingStat = this._quintessence.owner.stat;
			this.AttachEvents();
			this._character.ForceToLookAt(this._quintessence.owner.desiringLookingDirection);
			this._character.transform.position = base.transform.position;
			this._character.gameObject.SetActive(true);
			base.StartCoroutine(this._operations.CRun(this._quintessence.owner));
			this._action.TryStart();
			while (this._action.running)
			{
				yield return null;
			}
			this.Disable();
			yield break;
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x00091566 File Offset: 0x0008F766
		private void OnKilled(ITarget target, ref Damage damage)
		{
			Character.OnKilledDelegate onKilled = this._quintessence.owner.onKilled;
			if (onKilled == null)
			{
				return;
			}
			onKilled(target, ref damage);
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x00091584 File Offset: 0x0008F784
		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			return this._quintessence.owner.onGiveDamage.Invoke(target, ref damage);
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x0009159D File Offset: 0x0008F79D
		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			GaveDamageDelegate onGaveDamage = this._quintessence.owner.onGaveDamage;
			if (onGaveDamage == null)
			{
				return;
			}
			onGaveDamage(target, originalDamage, gaveDamage, damageDealt);
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x000915BE File Offset: 0x0008F7BE
		private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
		{
			Character.OnGaveStatusDelegate onGaveStatus = this._quintessence.owner.onGaveStatus;
			if (onGaveStatus == null)
			{
				return;
			}
			onGaveStatus(target, applyInfo, result);
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x00091914 File Offset: 0x0008FB14
		private void OnDestroy()
		{
			if (!Service.quitting)
			{
				UnityEngine.Object.Destroy(this._character.gameObject);
			}
		}

		// Token: 0x0400281A RID: 10266
		[SerializeField]
		private Character _character;

		// Token: 0x0400281B RID: 10267
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x0400281C RID: 10268
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
