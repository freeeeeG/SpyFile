using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000007 RID: 7
[AddComponentMenu("KMonoBehaviour/scripts/UICurvePath")]
public class UICurvePath : KMonoBehaviour
{
	// Token: 0x06000019 RID: 25 RVA: 0x0000253C File Offset: 0x0000073C
	protected override void OnSpawn()
	{
		this.Init();
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		this.OnResize();
		this.startDelay = (float)UnityEngine.Random.Range(0, 8);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000258C File Offset: 0x0000078C
	private void OnResize()
	{
		this.A = this.startPoint.position;
		this.B = this.controlPointStart.position;
		this.C = this.controlPointEnd.position;
		this.D = this.endPoint.position;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000025DD File Offset: 0x000007DD
	protected override void OnCleanUp()
	{
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
		base.OnCleanUp();
	}

	// Token: 0x0600001C RID: 28 RVA: 0x0000260C File Offset: 0x0000080C
	private void Update()
	{
		this.startDelay -= Time.unscaledDeltaTime;
		this.sprite.gameObject.SetActive(this.startDelay < 0f);
		if (this.startDelay > 0f)
		{
			return;
		}
		this.tick += Time.unscaledDeltaTime * this.moveSpeed;
		this.sprite.transform.position = this.DeCasteljausAlgorithm(this.tick);
		this.sprite.SetAlpha(Mathf.Min(this.sprite.color.a + this.tick / 2f, 1f));
		if (this.animateScale)
		{
			float num = Mathf.Min(this.sprite.transform.localScale.x + Time.unscaledDeltaTime * this.moveSpeed, 1f);
			this.sprite.transform.localScale = new Vector3(num, num, 1f);
		}
		if (this.loop && this.tick > 1f)
		{
			this.Init();
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000272C File Offset: 0x0000092C
	private void Init()
	{
		this.sprite.transform.position = this.startPoint.position;
		this.tick = 0f;
		if (this.animateScale)
		{
			this.sprite.transform.localScale = this.initialScale;
		}
		this.sprite.SetAlpha(this.initialAlpha);
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002790 File Offset: 0x00000990
	private void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			this.A = this.startPoint.position;
			this.B = this.controlPointStart.position;
			this.C = this.controlPointEnd.position;
			this.D = this.endPoint.position;
		}
		Gizmos.color = Color.white;
		Vector3 a = this.A;
		float num = 0.02f;
		int num2 = Mathf.FloorToInt(1f / num);
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i * num;
			this.DeCasteljausAlgorithm(t);
		}
		Gizmos.color = Color.green;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002830 File Offset: 0x00000A30
	private Vector3 DeCasteljausAlgorithm(float t)
	{
		float d = 1f - t;
		Vector3 a = d * this.A + t * this.B;
		Vector3 a2 = d * this.B + t * this.C;
		Vector3 a3 = d * this.C + t * this.D;
		Vector3 a4 = d * a + t * a2;
		Vector3 a5 = d * a2 + t * a3;
		return d * a4 + t * a5;
	}

	// Token: 0x0400000B RID: 11
	public Transform startPoint;

	// Token: 0x0400000C RID: 12
	public Transform endPoint;

	// Token: 0x0400000D RID: 13
	public Transform controlPointStart;

	// Token: 0x0400000E RID: 14
	public Transform controlPointEnd;

	// Token: 0x0400000F RID: 15
	public Image sprite;

	// Token: 0x04000010 RID: 16
	public bool loop = true;

	// Token: 0x04000011 RID: 17
	public bool animateScale;

	// Token: 0x04000012 RID: 18
	public Vector3 initialScale;

	// Token: 0x04000013 RID: 19
	private float startDelay;

	// Token: 0x04000014 RID: 20
	public float initialAlpha = 0.5f;

	// Token: 0x04000015 RID: 21
	public float moveSpeed = 0.1f;

	// Token: 0x04000016 RID: 22
	private float tick;

	// Token: 0x04000017 RID: 23
	private Vector3 A;

	// Token: 0x04000018 RID: 24
	private Vector3 B;

	// Token: 0x04000019 RID: 25
	private Vector3 C;

	// Token: 0x0400001A RID: 26
	private Vector3 D;
}
