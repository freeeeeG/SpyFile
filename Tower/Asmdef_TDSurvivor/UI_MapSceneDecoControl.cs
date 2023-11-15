using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016A RID: 362
public class UI_MapSceneDecoControl : MonoBehaviour
{
	// Token: 0x06000981 RID: 2433 RVA: 0x00023D22 File Offset: 0x00021F22
	private void Start()
	{
		base.StartCoroutine(this.CR_StartAnim());
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00023D31 File Offset: 0x00021F31
	private IEnumerator CR_StartAnim()
	{
		yield return new WaitForSeconds(1.75f);
		this.animator.SetBool("isOn", true);
		yield break;
	}

	// Token: 0x0400077A RID: 1914
	[SerializeField]
	private Animator animator;
}
