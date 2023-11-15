using System;
using System.Collections.Generic;
using Characters.AI.Hardmode.Chapter3;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.FindOptions.Custom
{
	// Token: 0x02000EAE RID: 3758
	[Serializable]
	public class FarFromTotem : ICondition
	{
		// Token: 0x060049F4 RID: 18932 RVA: 0x000D7D6C File Offset: 0x000D5F6C
		static FarFromTotem()
		{
			FarFromTotem._overlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x000D7D94 File Offset: 0x000D5F94
		public bool Satisfied(Character character)
		{
			FarFromTotem._overlapper.OverlapBox(character.transform.position, this._searchRange.bounds.size, 0f);
			using (IEnumerator<Collider2D> enumerator = FarFromTotem._overlapper.results.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetComponent<Totem>() != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0400392D RID: 14637
		[SerializeField]
		private BoxCollider2D _searchRange;

		// Token: 0x0400392E RID: 14638
		private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(31);
	}
}
