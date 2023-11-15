using System;
using Characters.Abilities.Weapons.DavyJones;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
	// Token: 0x02000465 RID: 1125
	public sealed class DavyJonesHud : MonoBehaviour
	{
		// Token: 0x0600156F RID: 5487 RVA: 0x00043689 File Offset: 0x00041889
		public void ShowHUD()
		{
			this._container.gameObject.SetActive(true);
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0004369C File Offset: 0x0004189C
		public void HideAll()
		{
			this._container.gameObject.SetActive(false);
			this.HideAllCannonBall();
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x000436B8 File Offset: 0x000418B8
		public void HideAllCannonBall()
		{
			Image[] cannonImages = this._cannonImages;
			for (int i = 0; i < cannonImages.Length; i++)
			{
				cannonImages[i].gameObject.SetActive(false);
			}
			Animator[] effects = this._effects;
			for (int i = 0; i < effects.Length; i++)
			{
				effects[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0004370C File Offset: 0x0004190C
		public void SetCannonBall(int index, CannonBallType type)
		{
			this._cannonImages[index].sprite = this._sprites[type];
			this._cannonImages[index].gameObject.SetActive(true);
			if (Convert.ToInt32(type) % 2 == 1)
			{
				this._effects[index].gameObject.SetActive(true);
				this._effects[index].runtimeAnimatorController = this._effectAnimators[type];
			}
		}

		// Token: 0x040012BE RID: 4798
		[SerializeField]
		private GameObject _container;

		// Token: 0x040012BF RID: 4799
		[Header("탄환")]
		[SerializeField]
		private Image[] _cannonImages;

		// Token: 0x040012C0 RID: 4800
		[SerializeField]
		private float[] _hueValue;

		// Token: 0x040012C1 RID: 4801
		[SerializeField]
		private EnumArray<CannonBallType, Sprite> _sprites;

		// Token: 0x040012C2 RID: 4802
		[SerializeField]
		private Animator[] _effects;

		// Token: 0x040012C3 RID: 4803
		[SerializeField]
		private EnumArray<CannonBallType, RuntimeAnimatorController> _effectAnimators;
	}
}
