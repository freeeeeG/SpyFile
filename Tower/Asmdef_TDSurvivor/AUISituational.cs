using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000134 RID: 308
public abstract class AUISituational : AUI
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0001E7C8 File Offset: 0x0001C9C8
	public bool IsUIActivated
	{
		get
		{
			return this.isUIActivated;
		}
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x0001E7D0 File Offset: 0x0001C9D0
	public void Toggle(bool isOn)
	{
		if (isOn)
		{
			if (this.coroutine_CloseAnimatorInSeconds != null)
			{
				this.doCloseInSeconds = false;
				base.StopCoroutine(this.coroutine_CloseAnimatorInSeconds);
			}
			this.isUIActivated = true;
			if (this.animator != null)
			{
				this.animator.gameObject.SetActive(true);
			}
			this.ToggleOn();
			return;
		}
		this.isUIActivated = false;
		this.ToggleOff();
		this.DeactivateAnimatorInSeconds(this.time_AnimatorDeactivateOnToggleOff);
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x0001E841 File Offset: 0x0001CA41
	private void DeactivateAnimatorInSeconds(float time)
	{
		this.coroutine_CloseAnimatorInSeconds = base.StartCoroutine(this.CR_CloseAnimatorInSeconds(time));
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x0001E856 File Offset: 0x0001CA56
	private IEnumerator CR_CloseAnimatorInSeconds(float time)
	{
		this.doCloseInSeconds = true;
		yield return new WaitForSecondsRealtime(time);
		if (this.animator != null && this.doCloseInSeconds)
		{
			this.animator.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x0001E86C File Offset: 0x0001CA6C
	protected virtual void ToggleOn()
	{
		this.animator.SetBool("isOn", true);
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x0001E87F File Offset: 0x0001CA7F
	protected virtual void ToggleOff()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x04000678 RID: 1656
	[SerializeField]
	protected Animator animator;

	// Token: 0x04000679 RID: 1657
	[SerializeField]
	private float time_AnimatorDeactivateOnToggleOff = 1f;

	// Token: 0x0400067A RID: 1658
	private Coroutine coroutine_CloseAnimatorInSeconds;

	// Token: 0x0400067B RID: 1659
	private bool isUIActivated;

	// Token: 0x0400067C RID: 1660
	private bool doCloseInSeconds;
}
