using System;
using UnityEngine;

// Token: 0x02000448 RID: 1096
public class KAnimLink
{
	// Token: 0x060017A1 RID: 6049 RVA: 0x00079B57 File Offset: 0x00077D57
	public KAnimLink(KAnimControllerBase master, KAnimControllerBase slave)
	{
		this.slave = slave;
		this.master = master;
		this.Register();
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x00079B7C File Offset: 0x00077D7C
	private void Register()
	{
		this.master.OnOverlayColourChanged += this.OnOverlayColourChanged;
		KAnimControllerBase kanimControllerBase = this.master;
		kanimControllerBase.OnTintChanged = (Action<Color>)Delegate.Combine(kanimControllerBase.OnTintChanged, new Action<Color>(this.OnTintColourChanged));
		KAnimControllerBase kanimControllerBase2 = this.master;
		kanimControllerBase2.OnHighlightChanged = (Action<Color>)Delegate.Combine(kanimControllerBase2.OnHighlightChanged, new Action<Color>(this.OnHighlightColourChanged));
		this.master.onLayerChanged += this.slave.SetLayer;
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x00079C0C File Offset: 0x00077E0C
	public void Unregister()
	{
		if (this.master != null)
		{
			this.master.OnOverlayColourChanged -= this.OnOverlayColourChanged;
			KAnimControllerBase kanimControllerBase = this.master;
			kanimControllerBase.OnTintChanged = (Action<Color>)Delegate.Remove(kanimControllerBase.OnTintChanged, new Action<Color>(this.OnTintColourChanged));
			KAnimControllerBase kanimControllerBase2 = this.master;
			kanimControllerBase2.OnHighlightChanged = (Action<Color>)Delegate.Remove(kanimControllerBase2.OnHighlightChanged, new Action<Color>(this.OnHighlightColourChanged));
			if (this.slave != null)
			{
				this.master.onLayerChanged -= this.slave.SetLayer;
			}
		}
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x00079CBA File Offset: 0x00077EBA
	private void OnOverlayColourChanged(Color32 c)
	{
		if (this.slave != null)
		{
			this.slave.OverlayColour = c;
		}
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x00079CDB File Offset: 0x00077EDB
	private void OnTintColourChanged(Color c)
	{
		if (this.syncTint && this.slave != null)
		{
			this.slave.TintColour = c;
		}
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x00079D04 File Offset: 0x00077F04
	private void OnHighlightColourChanged(Color c)
	{
		if (this.slave != null)
		{
			this.slave.HighlightColour = c;
		}
	}

	// Token: 0x04000D16 RID: 3350
	public bool syncTint = true;

	// Token: 0x04000D17 RID: 3351
	private KAnimControllerBase master;

	// Token: 0x04000D18 RID: 3352
	private KAnimControllerBase slave;
}
