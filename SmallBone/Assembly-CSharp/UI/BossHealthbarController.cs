using System;
using Characters;
using UnityEngine;

namespace UI
{
	// Token: 0x02000380 RID: 896
	public class BossHealthbarController : MonoBehaviour
	{
		// Token: 0x06001063 RID: 4195 RVA: 0x0003072C File Offset: 0x0002E92C
		private void Awake()
		{
			this._animators = new EnumArray<BossHealthbarController.Type, HangingPanelAnimator>();
			for (int i = 0; i < this._healthbars.Count; i++)
			{
				this._animators.Array[i] = this._healthbars.Array[i].GetComponent<HangingPanelAnimator>();
			}
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0003077C File Offset: 0x0002E97C
		public void Open(BossHealthbarController.Type type, Character character)
		{
			for (int i = 0; i < this._healthbars.Count; i++)
			{
				this._healthbars.Array[i].gameObject.SetActive(false);
			}
			this._healthbars[type].Initialize(character);
			this._animators[type].Appear();
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000307DC File Offset: 0x0002E9DC
		public void OpenChapter2Phase1(Character left, Character right)
		{
			for (int i = 0; i < this._healthbars.Count; i++)
			{
				this._healthbars.Array[i].gameObject.SetActive(false);
			}
			this._healthbars[BossHealthbarController.Type.Chpater2_Phase1].Initialize(left);
			this._healthbars[BossHealthbarController.Type.Chpater2_Phase1_Right].Initialize(right);
			this._animators[BossHealthbarController.Type.Chpater2_Phase1].Appear();
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0003084C File Offset: 0x0002EA4C
		public void OpenChapter4Phase1(Character left, Character right)
		{
			for (int i = 0; i < this._healthbars.Count; i++)
			{
				this._healthbars.Array[i].gameObject.SetActive(false);
			}
			this._healthbars[BossHealthbarController.Type.Chapter4_Phase1].Initialize(left);
			this._healthbars[BossHealthbarController.Type.Chapter4_Phase1_Right].Initialize(right);
			this._animators[BossHealthbarController.Type.Chapter4_Phase1].Appear();
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x000308C0 File Offset: 0x0002EAC0
		public void OpenVeteranAdventurer(Character character, string nameKey, string titleKey)
		{
			for (int i = 0; i < this._healthbars.Count; i++)
			{
				this._healthbars.Array[i].gameObject.SetActive(false);
			}
			this._veteranHealthbarController.Appear(character, nameKey, titleKey);
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0003090C File Offset: 0x0002EB0C
		public void CloseAll()
		{
			for (int i = 0; i < this._healthbars.Count; i++)
			{
				if (this._healthbars.Array[i].gameObject.activeSelf)
				{
					HangingPanelAnimator hangingPanelAnimator = this._animators.Array[i];
					if (hangingPanelAnimator != null)
					{
						hangingPanelAnimator.Disappear();
					}
				}
			}
			this._veteranHealthbarController.Disappear();
		}

		// Token: 0x04000D65 RID: 3429
		[SerializeField]
		private BossHealthbarController.TypeHealthBarArray _healthbars;

		// Token: 0x04000D66 RID: 3430
		[SerializeField]
		private VeteranHealthbarController _veteranHealthbarController;

		// Token: 0x04000D67 RID: 3431
		private EnumArray<BossHealthbarController.Type, HangingPanelAnimator> _animators;

		// Token: 0x02000381 RID: 897
		[Serializable]
		public class TypeHealthBarArray : EnumArray<BossHealthbarController.Type, CharacterHealthBar>
		{
		}

		// Token: 0x02000382 RID: 898
		public enum Type
		{
			// Token: 0x04000D69 RID: 3433
			Tutorial,
			// Token: 0x04000D6A RID: 3434
			Chpater1_Phase1,
			// Token: 0x04000D6B RID: 3435
			Chapter1_Phase2,
			// Token: 0x04000D6C RID: 3436
			Chpater2_Phase1,
			// Token: 0x04000D6D RID: 3437
			Chpater2_Phase1_Right,
			// Token: 0x04000D6E RID: 3438
			Chapter2_Phase2,
			// Token: 0x04000D6F RID: 3439
			Chpater3_Phase1,
			// Token: 0x04000D70 RID: 3440
			Chapter3_Phase2,
			// Token: 0x04000D71 RID: 3441
			Chapter4_Phase1,
			// Token: 0x04000D72 RID: 3442
			Chapter4_Phase1_Right,
			// Token: 0x04000D73 RID: 3443
			Chapter4_Phase2,
			// Token: 0x04000D74 RID: 3444
			Chapter5_Phase1,
			// Token: 0x04000D75 RID: 3445
			Chapter5_Phase2,
			// Token: 0x04000D76 RID: 3446
			Chapter5_Phase3,
			// Token: 0x04000D77 RID: 3447
			Hardmode_Chapter5_Phase2
		}
	}
}
