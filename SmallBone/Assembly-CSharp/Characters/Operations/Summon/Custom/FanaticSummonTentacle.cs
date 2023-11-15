using System;
using Level;
using UnityEngine;

namespace Characters.Operations.Summon.Custom
{
	// Token: 0x02000F65 RID: 3941
	[Serializable]
	public class FanaticSummonTentacle : IBDCharacterSetting
	{
		// Token: 0x06004C89 RID: 19593 RVA: 0x000E311C File Offset: 0x000E131C
		public void ApplyTo(Character character)
		{
			SpriteRenderer corpseSpriteRenderer = character.attach.GetComponentInChildren<SpriteRenderer>();
			corpseSpriteRenderer.sprite = this._corpseSprite;
			corpseSpriteRenderer.flipX = (this._summoner.lookingDirection == Character.LookingDirection.Left);
			character.health.onDied += delegate()
			{
				corpseSpriteRenderer.transform.SetParent(Map.Instance.transform);
				corpseSpriteRenderer.sortingOrder = character.sortingGroup.sortingOrder;
			};
		}

		// Token: 0x04003C30 RID: 15408
		[SerializeField]
		private Character _summoner;

		// Token: 0x04003C31 RID: 15409
		[SerializeField]
		private Sprite _corpseSprite;
	}
}
