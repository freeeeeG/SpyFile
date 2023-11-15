using System;
using System.Collections;
using Febucci.UI;
using UnityEngine;

// Token: 0x02000175 RID: 373
public class UI_NpcDialog_Popup : APopupWindow
{
	// Token: 0x060009DC RID: 2524 RVA: 0x0002531F File Offset: 0x0002351F
	private void Update()
	{
		this.UpdatePosition();
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x00025327 File Offset: 0x00023527
	public void SetupContent(string content, float duration, float delay, Vector3 targetPosition, Vector3 offset2D)
	{
		this.duration = duration;
		this.delay = delay;
		this.targetPosition = targetPosition;
		this.offset2D = offset2D;
		this.text_Content.SetText(content);
		this.UpdatePosition();
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x00025359 File Offset: 0x00023559
	protected override void ShowWindowProc()
	{
		base.StartCoroutine(this.ShowWindowForSeconds(this.duration, this.delay));
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x00025374 File Offset: 0x00023574
	private IEnumerator ShowWindowForSeconds(float seconds, float delay)
	{
		yield return new WaitForSeconds(delay);
		this.animator.SetBool("isOn", true);
		yield return new WaitForSeconds(seconds);
		base.CloseWindow();
		yield break;
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00025391 File Offset: 0x00023591
	protected override void CloseWindowProc()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x000253A4 File Offset: 0x000235A4
	private void UpdatePosition()
	{
		base.transform.position = Singleton<CameraManager>.Instance.Calculate2DPosFrom3DPos(this.targetPosition);
		base.transform.localPosition += this.offset2D;
	}

	// Token: 0x040007B0 RID: 1968
	[SerializeField]
	private TextAnimator_TMP text_Content;

	// Token: 0x040007B1 RID: 1969
	private float duration;

	// Token: 0x040007B2 RID: 1970
	private float delay;

	// Token: 0x040007B3 RID: 1971
	private Vector3 targetPosition;

	// Token: 0x040007B4 RID: 1972
	private Vector3 offset2D;
}
