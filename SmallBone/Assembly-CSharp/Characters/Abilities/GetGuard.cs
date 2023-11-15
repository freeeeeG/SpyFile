using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Characters.Abilities
{
	// Token: 0x02000A35 RID: 2613
	[Serializable]
	public sealed class GetGuard : Ability
	{
		// Token: 0x0600370D RID: 14093 RVA: 0x000A2BA4 File Offset: 0x000A0DA4
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GetGuard.Instance(owner, this);
		}

		// Token: 0x04002BF3 RID: 11251
		[SerializeField]
		private AssetReference _guardReference;

		// Token: 0x02000A36 RID: 2614
		public sealed class Instance : AbilityInstance<GetGuard>
		{
			// Token: 0x0600370F RID: 14095 RVA: 0x000A2BAD File Offset: 0x000A0DAD
			public Instance(Character owner, GetGuard ability) : base(owner, ability)
			{
			}

			// Token: 0x06003710 RID: 14096 RVA: 0x000A2BB8 File Offset: 0x000A0DB8
			protected override void OnAttach()
			{
				this._guardInstance = Guard.Create(this.ability._guardReference);
				this._guardInstance.Initialize(this.owner);
				this._guardInstance.transform.localPosition = Vector3.zero;
				this._guardInstance.GuardUp();
			}

			// Token: 0x06003711 RID: 14097 RVA: 0x000A2C0C File Offset: 0x000A0E0C
			protected override void OnDetach()
			{
				this._guardInstance.GuardDown();
				UnityEngine.Object.Destroy(this._guardInstance);
				this._guardInstance = null;
			}

			// Token: 0x04002BF4 RID: 11252
			private Guard _guardInstance;
		}
	}
}
