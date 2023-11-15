using System;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public static class AnimatorExtensions
{
	// Token: 0x06000619 RID: 1561 RVA: 0x0001772B File Offset: 0x0001592B
	public static AnimatorStateInfo GetCurrentAnimatorStateInfo(this Animator animator, string layerName)
	{
		return animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex(layerName));
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0001773A File Offset: 0x0001593A
	public static AnimatorStateInfo GetNextAnimatorStateInfo(this Animator animator, string layerName)
	{
		return animator.GetNextAnimatorStateInfo(animator.GetLayerIndex(layerName));
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0001774C File Offset: 0x0001594C
	public static bool IsCurrentStateName(this Animator animator, string layerName, string stateName)
	{
		return animator.GetCurrentAnimatorStateInfo(layerName).IsName(stateName);
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0001776C File Offset: 0x0001596C
	public static bool IsCurrentStateName(this Animator animator, int layerIndex, string stateName)
	{
		return animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
	}
}
