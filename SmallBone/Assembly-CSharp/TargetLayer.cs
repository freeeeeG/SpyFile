using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
[Serializable]
public class TargetLayer
{
	// Token: 0x060002C1 RID: 705 RVA: 0x0000AFA3 File Offset: 0x000091A3
	public static bool IsPlayer(int layer)
	{
		return !TargetLayer.IsMonster(layer);
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0000AFAE File Offset: 0x000091AE
	public static bool IsMonster(int layer)
	{
		return layer == 10 || layer == 16;
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0000AFBC File Offset: 0x000091BC
	public TargetLayer()
	{
		this._rawMask = 0;
		this._allyBody = false;
		this._foeBody = true;
		this._allyProjectile = false;
		this._foeProjectile = false;
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x0000AFEC File Offset: 0x000091EC
	public TargetLayer(LayerMask rawMask, bool allyBody, bool foeBody, bool allyProjectile, bool foeProjectile)
	{
		this._rawMask = rawMask;
		this._allyBody = allyBody;
		this._foeBody = foeBody;
		this._allyProjectile = allyProjectile;
		this._foeProjectile = foeProjectile;
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0000B01C File Offset: 0x0000921C
	public LayerMask Evaluate(GameObject owner)
	{
		LayerMask layerMask = this._rawMask;
		if (TargetLayer.IsMonster(owner.layer))
		{
			if (this._allyBody)
			{
				layerMask |= 1024;
			}
			if (this._foeBody)
			{
				layerMask |= 512;
			}
			if (this._allyProjectile)
			{
				layerMask |= 65536;
			}
			if (this._foeProjectile)
			{
				layerMask |= 32768;
			}
		}
		else
		{
			if (this._allyBody)
			{
				layerMask |= 512;
			}
			if (this._foeBody)
			{
				layerMask |= 1024;
			}
			if (this._allyProjectile)
			{
				layerMask |= 32768;
			}
			if (this._foeProjectile)
			{
				layerMask |= 65536;
			}
		}
		return layerMask;
	}

	// Token: 0x0400024E RID: 590
	[SerializeField]
	private LayerMask _rawMask;

	// Token: 0x0400024F RID: 591
	[SerializeField]
	private bool _allyBody;

	// Token: 0x04000250 RID: 592
	[SerializeField]
	private bool _foeBody;

	// Token: 0x04000251 RID: 593
	[SerializeField]
	private bool _allyProjectile;

	// Token: 0x04000252 RID: 594
	[SerializeField]
	private bool _foeProjectile;
}
