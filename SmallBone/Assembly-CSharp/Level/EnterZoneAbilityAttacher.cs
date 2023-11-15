using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace Level
{
	// Token: 0x020004D7 RID: 1239
	[RequireComponent(typeof(Collider2D))]
	public class EnterZoneAbilityAttacher : MonoBehaviour
	{
		// Token: 0x06001824 RID: 6180 RVA: 0x0004BBE1 File Offset: 0x00049DE1
		private void Awake()
		{
			this._abilityComponent.Initialize();
			this._enteredCharacters = new List<Character>();
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0004BBFC File Offset: 0x00049DFC
		private void OnTriggerEnter2D(Collider2D collision)
		{
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			if (this._types != null && !this._types.Contains(component.type))
			{
				return;
			}
			this.AttachTo(component);
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0004BC40 File Offset: 0x00049E40
		private void OnTriggerExit2D(Collider2D collision)
		{
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			if (this._types != null && !this._types.Contains(component.type))
			{
				return;
			}
			this.DetachFrom(component);
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0004BC81 File Offset: 0x00049E81
		private void AttachTo(Character who)
		{
			who.ability.Add(this._abilityComponent.ability);
			this._enteredCharacters.Add(who);
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0004BCA6 File Offset: 0x00049EA6
		private void DetachFrom(Character who)
		{
			who.ability.Remove(this._abilityComponent.ability);
			this._enteredCharacters.Remove(who);
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0004BCCC File Offset: 0x00049ECC
		private void DetachAll()
		{
			foreach (Character who in this._enteredCharacters)
			{
				this.DetachFrom(who);
			}
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0004BD1C File Offset: 0x00049F1C
		private void OnDestroy()
		{
			this.DetachAll();
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0004BD1C File Offset: 0x00049F1C
		private void OnDisable()
		{
			this.DetachAll();
		}

		// Token: 0x04001509 RID: 5385
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x0400150A RID: 5386
		[SerializeField]
		private Character.Type[] _types;

		// Token: 0x0400150B RID: 5387
		private ICollection<Character> _enteredCharacters;
	}
}
