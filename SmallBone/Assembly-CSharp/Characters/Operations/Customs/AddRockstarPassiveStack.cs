using System;
using Characters.Abilities.Customs;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FC4 RID: 4036
	public class AddRockstarPassiveStack : Operation
	{
		// Token: 0x06004E27 RID: 20007 RVA: 0x000E9E5C File Offset: 0x000E805C
		public override void Run()
		{
			if (this._rockstarPassive == null)
			{
				WeaponInventory weapon = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
				this._rockstarPassive = weapon.polymorphOrCurrent.GetComponent<RockstarPassiveComponent>();
				if (this._rockstarPassive == null)
				{
					return;
				}
			}
			this._rockstarPassive.AddStack(this._amount);
		}

		// Token: 0x04003E2D RID: 15917
		[SerializeField]
		[Tooltip("비워두면 자동으로 찾고, 못찾으면 그냥 실행 안함(서먼으로 사용할 때 유용)")]
		private RockstarPassiveComponent _rockstarPassive;

		// Token: 0x04003E2E RID: 15918
		[SerializeField]
		private int _amount;
	}
}
