using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class ParentPool : MonoBehaviour
{
	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060001F9 RID: 505 RVA: 0x00008C9F File Offset: 0x00006E9F
	// (set) Token: 0x060001FA RID: 506 RVA: 0x00008CA7 File Offset: 0x00006EA7
	public Transform currentEffectParent
	{
		get
		{
			return this._currentEffectParent;
		}
		private set
		{
			this._currentEffectParent = value;
		}
	}

	// Token: 0x060001FB RID: 507 RVA: 0x00008CB0 File Offset: 0x00006EB0
	private void Awake()
	{
		foreach (ParentPool.EffectrRangeKeyValue effectrRangeKeyValue in this._parents)
		{
			effectrRangeKeyValue.range.gameObject.SetActive(false);
		}
	}

	// Token: 0x060001FC RID: 508 RVA: 0x00008D0C File Offset: 0x00006F0C
	public Transform GetRandomParent()
	{
		ParentPool.EffectrRangeKeyValue effectrRangeKeyValue = this._parents.Random<ParentPool.EffectrRangeKeyValue>();
		this.currentEffectParent = effectrRangeKeyValue.effect;
		this._currentAttackParent = effectrRangeKeyValue.range;
		this.PickOneAttackRange();
		return this.currentEffectParent;
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00008D4C File Offset: 0x00006F4C
	public Transform GetFirstParent()
	{
		ParentPool.EffectrRangeKeyValue effectrRangeKeyValue = this._parents[0];
		this.currentEffectParent = effectrRangeKeyValue.effect;
		this._currentAttackParent = effectrRangeKeyValue.range;
		this.PickOneAttackRange();
		return this.currentEffectParent;
	}

	// Token: 0x060001FE RID: 510 RVA: 0x00008D8C File Offset: 0x00006F8C
	private void PickOneAttackRange()
	{
		foreach (ParentPool.EffectrRangeKeyValue effectrRangeKeyValue in this._parents)
		{
			Transform range = effectrRangeKeyValue.range;
			if (range != this._currentAttackParent)
			{
				range.gameObject.SetActive(false);
			}
			else
			{
				range.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x040001B5 RID: 437
	[SerializeField]
	private List<ParentPool.EffectrRangeKeyValue> _parents;

	// Token: 0x040001B6 RID: 438
	private Transform _currentEffectParent;

	// Token: 0x040001B7 RID: 439
	private Transform _currentAttackParent;

	// Token: 0x0200006C RID: 108
	[Serializable]
	private class EffectrRangeKeyValue
	{
		// Token: 0x040001B8 RID: 440
		[SerializeField]
		internal Transform effect;

		// Token: 0x040001B9 RID: 441
		[SerializeField]
		internal Transform range;
	}
}
