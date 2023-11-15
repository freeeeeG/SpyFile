using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace flanne
{
	// Token: 0x0200013E RID: 318
	public class Unlockable : MonoBehaviour
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x00023049 File Offset: 0x00021249
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00023054 File Offset: 0x00021254
		private void Awake()
		{
			this._originalMaterial = this.targetSprite.material;
			if (!this.unbuyable)
			{
				this.unlockCostTMP.text = (("Unlock<br>" + this.unlockCost) ?? "");
			}
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000230A3 File Offset: 0x000212A3
		public void Lock()
		{
			this.targetSelectable.interactable = false;
			this.targetSprite.material = this.lockMaterial;
			this.lockSprite.enabled = true;
			this._isLocked = true;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x000230D8 File Offset: 0x000212D8
		public void UnlockWithPoints()
		{
			if (PointsTracker.pts < this.unlockCost)
			{
				return;
			}
			SoundEffectSO soundEffectSO = this.unlockSFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			PointsTracker.pts -= this.unlockCost;
			this.targetSelectable.Select();
			this.Unlock();
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00023128 File Offset: 0x00021328
		public void Unlock()
		{
			this.targetSelectable.interactable = true;
			this.targetSprite.material = this._originalMaterial;
			this.lockSprite.enabled = false;
			this._isLocked = false;
			if (this.unlockButton)
			{
				this.unlockButton.gameObject.SetActive(false);
			}
		}

		// Token: 0x04000623 RID: 1571
		[SerializeField]
		private int unlockCost;

		// Token: 0x04000624 RID: 1572
		[SerializeField]
		private Selectable targetSelectable;

		// Token: 0x04000625 RID: 1573
		[SerializeField]
		private Image targetSprite;

		// Token: 0x04000626 RID: 1574
		[SerializeField]
		private Image lockSprite;

		// Token: 0x04000627 RID: 1575
		[SerializeField]
		private Material lockMaterial;

		// Token: 0x04000628 RID: 1576
		[SerializeField]
		private bool unbuyable;

		// Token: 0x04000629 RID: 1577
		[SerializeField]
		private Button unlockButton;

		// Token: 0x0400062A RID: 1578
		[SerializeField]
		private TMP_Text unlockCostTMP;

		// Token: 0x0400062B RID: 1579
		[SerializeField]
		private SoundEffectSO unlockSFX;

		// Token: 0x0400062C RID: 1580
		private Material _originalMaterial;

		// Token: 0x0400062D RID: 1581
		private bool _isLocked;
	}
}
