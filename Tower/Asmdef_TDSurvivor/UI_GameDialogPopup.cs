using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x02000164 RID: 356
public class UI_GameDialogPopup : MonoBehaviour
{
	// Token: 0x06000962 RID: 2402 RVA: 0x000237F7 File Offset: 0x000219F7
	private void Start()
	{
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x000237F9 File Offset: 0x000219F9
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Y))
		{
			this.node_NPCTween.DOPunchRotation(Vector3.forward * this.strength, this.duration, this.vibrato, this.elasticity);
		}
	}

	// Token: 0x0400076C RID: 1900
	[SerializeField]
	private Transform node_NPCTween;

	// Token: 0x0400076D RID: 1901
	[SerializeField]
	private float duration = 1f;

	// Token: 0x0400076E RID: 1902
	[SerializeField]
	private float strength = 30f;

	// Token: 0x0400076F RID: 1903
	[SerializeField]
	private int vibrato = 1;

	// Token: 0x04000770 RID: 1904
	[SerializeField]
	private float elasticity = 1f;

	// Token: 0x04000771 RID: 1905
	[SerializeField]
	private float randomess = 20f;
}
