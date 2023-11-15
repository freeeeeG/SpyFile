using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETFXPEL
{
	// Token: 0x0200005D RID: 93
	public class UICanvasManager : MonoBehaviour
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00005D2B File Offset: 0x00003F2B
		private void Awake()
		{
			UICanvasManager.GlobalAccess = this;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005D33 File Offset: 0x00003F33
		private void Start()
		{
			if (this.PENameText != null)
			{
				this.PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005D58 File Offset: 0x00003F58
		private void Update()
		{
			if (!this.MouseOverButton && Input.GetMouseButtonUp(0))
			{
				this.SpawnCurrentParticleEffect();
			}
			if (Input.GetKeyUp(KeyCode.A))
			{
				this.SelectPreviousPE();
			}
			if (Input.GetKeyUp(KeyCode.D))
			{
				this.SelectNextPE();
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005D8E File Offset: 0x00003F8E
		public void UpdateToolTip(ButtonTypes toolTipType)
		{
			if (this.ToolTipText != null)
			{
				if (toolTipType == ButtonTypes.Previous)
				{
					this.ToolTipText.text = "Select Previous Particle Effect";
					return;
				}
				if (toolTipType == ButtonTypes.Next)
				{
					this.ToolTipText.text = "Select Next Particle Effect";
				}
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005DC7 File Offset: 0x00003FC7
		public void ClearToolTip()
		{
			if (this.ToolTipText != null)
			{
				this.ToolTipText.text = "";
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005DE7 File Offset: 0x00003FE7
		private void SelectPreviousPE()
		{
			ParticleEffectsLibrary.GlobalAccess.PreviousParticleEffect();
			if (this.PENameText != null)
			{
				this.PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005E16 File Offset: 0x00004016
		private void SelectNextPE()
		{
			ParticleEffectsLibrary.GlobalAccess.NextParticleEffect();
			if (this.PENameText != null)
			{
				this.PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005E45 File Offset: 0x00004045
		private void SpawnCurrentParticleEffect()
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.rayHit))
			{
				ParticleEffectsLibrary.GlobalAccess.SpawnParticleEffect(this.rayHit.point);
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005E78 File Offset: 0x00004078
		public void UIButtonClick(ButtonTypes buttonTypeClicked)
		{
			if (buttonTypeClicked == ButtonTypes.Previous)
			{
				this.SelectPreviousPE();
				return;
			}
			if (buttonTypeClicked != ButtonTypes.Next)
			{
				return;
			}
			this.SelectNextPE();
		}

		// Token: 0x0400011A RID: 282
		public static UICanvasManager GlobalAccess;

		// Token: 0x0400011B RID: 283
		public bool MouseOverButton;

		// Token: 0x0400011C RID: 284
		public Text PENameText;

		// Token: 0x0400011D RID: 285
		public Text ToolTipText;

		// Token: 0x0400011E RID: 286
		private RaycastHit rayHit;
	}
}
