using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BC1 RID: 3009
	[Serializable]
	public sealed class Quarantine : Ability
	{
		// Token: 0x06003DFB RID: 15867 RVA: 0x000B43A1 File Offset: 0x000B25A1
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Quarantine.Instance(owner, this);
		}

		// Token: 0x04002FE6 RID: 12262
		[SerializeField]
		private GameObject _quarantineObject;

		// Token: 0x04002FE7 RID: 12263
		[SerializeField]
		private OperationInfos _operationInfos;

		// Token: 0x04002FE8 RID: 12264
		[SerializeField]
		private float _maxSpeed;

		// Token: 0x04002FE9 RID: 12265
		[SerializeField]
		private float _readyTime;

		// Token: 0x02000BC2 RID: 3010
		public sealed class Instance : AbilityInstance<Quarantine>
		{
			// Token: 0x06003DFD RID: 15869 RVA: 0x000B43AA File Offset: 0x000B25AA
			public Instance(Character owner, Quarantine ability) : base(owner, ability)
			{
			}

			// Token: 0x06003DFE RID: 15870 RVA: 0x000B43B4 File Offset: 0x000B25B4
			protected override void OnAttach()
			{
				this._reaminReadyTime = this.ability._readyTime;
				this.owner.StartCoroutine(this.CLoad());
			}

			// Token: 0x06003DFF RID: 15871 RVA: 0x000B43D9 File Offset: 0x000B25D9
			protected override void OnDetach()
			{
				this.ability._quarantineObject.gameObject.SetActive(false);
				this.ability._operationInfos.Stop();
				this.ability.soundOnAttach.Dispose();
			}

			// Token: 0x06003E00 RID: 15872 RVA: 0x000B4411 File Offset: 0x000B2611
			private IEnumerator CLoad()
			{
				yield return null;
				this.ability._operationInfos.Initialize();
				this.Activate();
				yield break;
			}

			// Token: 0x06003E01 RID: 15873 RVA: 0x000B4420 File Offset: 0x000B2620
			private void Activate()
			{
				if (this._active)
				{
					return;
				}
				this.ability._quarantineObject.SetActive(true);
				this.ability._operationInfos.gameObject.SetActive(true);
				if (this.ability._operationInfos.gameObject.activeSelf)
				{
					this.ability._operationInfos.Run(this.owner);
				}
				this._active = true;
			}

			// Token: 0x06003E02 RID: 15874 RVA: 0x000B4491 File Offset: 0x000B2691
			private void Deactivate()
			{
				if (!this._active)
				{
					return;
				}
				this.ability._quarantineObject.gameObject.SetActive(false);
				this.ability._operationInfos.Stop();
				this._active = false;
			}

			// Token: 0x04002FEA RID: 12266
			private float _reaminReadyTime;

			// Token: 0x04002FEB RID: 12267
			private bool _active;
		}
	}
}
