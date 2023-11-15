using System;
using System.Collections.Generic;
using Characters;
using GameResources;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x02000394 RID: 916
	public sealed class DarkEnemyHealthbarController : MonoBehaviour
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060010C9 RID: 4297 RVA: 0x0003188F File Offset: 0x0002FA8F
		public IDictionary<Character, string> attached
		{
			get
			{
				return this._attached;
			}
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00031898 File Offset: 0x0002FA98
		public void Open(Character character, string abilityName)
		{
			this._healthBar.Initialize(character);
			this._name.text = Localization.GetLocalizedString(string.Format("enemy/name/{0}", character.key));
			this._ability.text = abilityName;
			this._animator.gameObject.SetActive(true);
			this._animator.Appear();
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(player.onGaveDamage, new GaveDamageDelegate(this.HandelOnGaveDamage));
			Character player2 = Singleton<Service>.Instance.levelManager.player;
			player2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(player2.onGaveDamage, new GaveDamageDelegate(this.HandelOnGaveDamage));
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0003195E File Offset: 0x0002FB5E
		public void AddTarget(Character character, string abilityName)
		{
			if (!this._attached.ContainsKey(character))
			{
				this._attached.Add(character, abilityName);
				return;
			}
			this._attached[character] = abilityName;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00031989 File Offset: 0x0002FB89
		public void RemoveTarget(Character character)
		{
			this._attached.Remove(character);
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00031998 File Offset: 0x0002FB98
		private void HandelOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			Character character = target.character;
			if (character == null)
			{
				return;
			}
			if (character.type == Character.Type.Named && !character.health.dead)
			{
				this.Set(character);
			}
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x000319D4 File Offset: 0x0002FBD4
		public void Set(Character character)
		{
			this._name.text = Localization.GetLocalizedString(string.Format("enemy/name/{0}", character.key));
			this._ability.text = this.attached[character];
			this._healthBar.Initialize(character);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00031A2C File Offset: 0x0002FC2C
		public void Close()
		{
			if (Singleton<Service>.Instance.levelManager.player != null)
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				player.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(player.onGaveDamage, new GaveDamageDelegate(this.HandelOnGaveDamage));
			}
			if (this._healthBar.gameObject.activeSelf)
			{
				this._animator.Disappear();
			}
		}

		// Token: 0x04000DC1 RID: 3521
		[SerializeField]
		private CharacterHealthBar _healthBar;

		// Token: 0x04000DC2 RID: 3522
		[SerializeField]
		private HangingPanelAnimator _animator;

		// Token: 0x04000DC3 RID: 3523
		[SerializeField]
		private TextMeshProUGUI _name;

		// Token: 0x04000DC4 RID: 3524
		[SerializeField]
		private TextMeshProUGUI _ability;

		// Token: 0x04000DC5 RID: 3525
		private IDictionary<Character, string> _attached = new Dictionary<Character, string>();

		// Token: 0x02000395 RID: 917
		private struct Target
		{
			// Token: 0x060010D1 RID: 4305 RVA: 0x00031AB0 File Offset: 0x0002FCB0
			public Target(Character character, string abilityName)
			{
				this.character = character;
				this.abilityName = abilityName;
			}

			// Token: 0x04000DC6 RID: 3526
			public Character character;

			// Token: 0x04000DC7 RID: 3527
			public string abilityName;
		}
	}
}
