using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BCA RID: 3018
	public sealed class DarkAbilityAttacher : MonoBehaviour
	{
		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06003E1C RID: 15900 RVA: 0x000B497C File Offset: 0x000B2B7C
		public bool attached
		{
			get
			{
				return this._attached;
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06003E1D RID: 15901 RVA: 0x000B4984 File Offset: 0x000B2B84
		// (set) Token: 0x06003E1E RID: 15902 RVA: 0x000B498C File Offset: 0x000B2B8C
		public DarkAbilityGauge gauge { get; set; }

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06003E1F RID: 15903 RVA: 0x000B4998 File Offset: 0x000B2B98
		public string displayName
		{
			get
			{
				if (!string.IsNullOrEmpty(this._cachedDisplayName))
				{
					return this._cachedDisplayName;
				}
				StringBuilder stringBuilder = new StringBuilder();
				foreach (DarkAbility darkAbility in this._abilities)
				{
					stringBuilder.Append(darkAbility.displayName);
				}
				this._cachedDisplayName = stringBuilder.ToString();
				return this._cachedDisplayName;
			}
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x000B4A18 File Offset: 0x000B2C18
		public void Initialize(Character owner)
		{
			this._owner = owner;
			this._darkAbilityContainer.Initialize(owner);
			this._initialized = true;
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x000B4A34 File Offset: 0x000B2C34
		public void StartAttach()
		{
			base.StartCoroutine(this.CWaitForInitialize());
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x000B4A44 File Offset: 0x000B2C44
		public void Attach()
		{
			this._abilities = this._darkAbilityContainer.GetDarkAbility();
			foreach (DarkAbility darkAbility in this._abilities)
			{
				darkAbility.AttachTo(this._owner, this);
			}
			this._attached = true;
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x000B4AB0 File Offset: 0x000B2CB0
		public void Detach()
		{
			foreach (DarkAbility darkAbility in this._abilities)
			{
				darkAbility.RemoveFrom(this._owner);
			}
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x000B4B00 File Offset: 0x000B2D00
		private IEnumerator CWaitForInitialize()
		{
			while (!this._initialized)
			{
				yield return null;
			}
			this.Attach();
			yield break;
		}

		// Token: 0x04002FFD RID: 12285
		[Subcomponent(typeof(DarkAbilityContainer))]
		[SerializeField]
		private DarkAbilityContainer _darkAbilityContainer;

		// Token: 0x04002FFE RID: 12286
		private Character _owner;

		// Token: 0x04002FFF RID: 12287
		private ICollection<DarkAbility> _abilities;

		// Token: 0x04003000 RID: 12288
		private bool _initialized;

		// Token: 0x04003001 RID: 12289
		private bool _attached;

		// Token: 0x04003003 RID: 12291
		private string _cachedDisplayName;
	}
}
