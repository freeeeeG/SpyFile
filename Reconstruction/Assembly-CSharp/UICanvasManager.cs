using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000008 RID: 8
public class UICanvasManager : MonoBehaviour
{
	// Token: 0x06000050 RID: 80 RVA: 0x000038EF File Offset: 0x00001AEF
	private void Awake()
	{
		UICanvasManager.GlobalAccess = this;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000038F7 File Offset: 0x00001AF7
	private void Start()
	{
		if (this.PENameText != null)
		{
			this.PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x0000391C File Offset: 0x00001B1C
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

	// Token: 0x06000053 RID: 83 RVA: 0x00003952 File Offset: 0x00001B52
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

	// Token: 0x06000054 RID: 84 RVA: 0x0000398B File Offset: 0x00001B8B
	public void ClearToolTip()
	{
		if (this.ToolTipText != null)
		{
			this.ToolTipText.text = "";
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000039AB File Offset: 0x00001BAB
	private void SelectPreviousPE()
	{
		ParticleEffectsLibrary.GlobalAccess.PreviousParticleEffect();
		if (this.PENameText != null)
		{
			this.PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000039DA File Offset: 0x00001BDA
	private void SelectNextPE()
	{
		ParticleEffectsLibrary.GlobalAccess.NextParticleEffect();
		if (this.PENameText != null)
		{
			this.PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00003A09 File Offset: 0x00001C09
	private void SpawnCurrentParticleEffect()
	{
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.rayHit))
		{
			ParticleEffectsLibrary.GlobalAccess.SpawnParticleEffect(this.rayHit.point);
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00003A3C File Offset: 0x00001C3C
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

	// Token: 0x0400002A RID: 42
	public static UICanvasManager GlobalAccess;

	// Token: 0x0400002B RID: 43
	public bool MouseOverButton;

	// Token: 0x0400002C RID: 44
	public Text PENameText;

	// Token: 0x0400002D RID: 45
	public Text ToolTipText;

	// Token: 0x0400002E RID: 46
	private RaycastHit rayHit;
}
