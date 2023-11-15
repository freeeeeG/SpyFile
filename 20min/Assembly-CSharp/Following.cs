using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class Following : MonoBehaviour
{
	// Token: 0x06000014 RID: 20 RVA: 0x00002F54 File Offset: 0x00001154
	private void Start()
	{
		this.followArrow.gameObject.LeanDelayedCall(3f, new Action(this.moveArrow)).setOnStart(new Action(this.moveArrow)).setRepeat(-1);
		LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.localY, 1.1f, -1f);
		LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.localY, 1.1f, -1f, 2f, 0.5f);
		LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.localY, 1.1f, -1f, 2f, 0.5f, 0.9f);
		LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.localY, 1.1f, -1f, 1.5f, 0.8f);
		LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.localY, 50f);
		LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.color, 1.1f, -1f);
		LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.color, 1.1f, -1f, 2f, 0.5f);
		LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.color, 1.1f, -1f, 2f, 0.5f, 0.9f);
		LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.color, 1.1f, -1f, 1.5f, 0.8f);
		LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.color, 0.5f);
		LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.scale, 1.1f, -1f);
		LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.scale, 1.1f, -1f, 2f, 0.5f);
		LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.scale, 1.1f, -1f, 2f, 0.5f, 0.9f);
		LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.scale, 1.1f, -1f, 1.5f, 0.8f);
		LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.scale, 5f);
		Vector3 offset = new Vector3(0f, -20f, -18f);
		LeanTween.followDamp(this.dude1Title, this.dude1, LeanProp.localPosition, 0.6f, -1f).setOffset(offset);
		LeanTween.followSpring(this.dude2Title, this.dude2, LeanProp.localPosition, 0.6f, -1f, 2f, 0.5f).setOffset(offset);
		LeanTween.followBounceOut(this.dude3Title, this.dude3, LeanProp.localPosition, 0.6f, -1f, 2f, 0.5f, 0.9f).setOffset(offset);
		LeanTween.followSpring(this.dude4Title, this.dude4, LeanProp.localPosition, 0.6f, -1f, 1.5f, 0.8f).setOffset(offset);
		LeanTween.followLinear(this.dude5Title, this.dude5, LeanProp.localPosition, 30f).setOffset(offset);
		Vector3 point = Camera.main.transform.InverseTransformPoint(this.planet.transform.position);
		LeanTween.rotateAround(Camera.main.gameObject, Vector3.left, 360f, 300f).setPoint(point).setRepeat(-1);
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000032DC File Offset: 0x000014DC
	private void Update()
	{
		this.fromY = LeanSmooth.spring(this.fromY, this.followArrow.localPosition.y, ref this.velocityY, 1.1f, -1f, -1f, 2f, 0.5f);
		this.fromVec3 = LeanSmooth.spring(this.fromVec3, this.dude5Title.localPosition, ref this.velocityVec3, 1.1f, -1f, -1f, 2f, 0.5f);
		this.fromColor = LeanSmooth.spring(this.fromColor, this.dude1.GetComponent<Renderer>().material.color, ref this.velocityColor, 1.1f, -1f, -1f, 2f, 0.5f);
		Debug.Log(string.Concat(new object[]
		{
			"Smoothed y:",
			this.fromY,
			" vec3:",
			this.fromVec3,
			" color:",
			this.fromColor
		}));
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000033FC File Offset: 0x000015FC
	private void moveArrow()
	{
		LeanTween.moveLocalY(this.followArrow.gameObject, Random.Range(-100f, 100f), 0f);
		Color to = new Color(Random.value, Random.value, Random.value);
		LeanTween.color(this.followArrow.gameObject, to, 0f);
		float d = Random.Range(5f, 10f);
		this.followArrow.localScale = Vector3.one * d;
	}

	// Token: 0x0400000F RID: 15
	public Transform planet;

	// Token: 0x04000010 RID: 16
	public Transform followArrow;

	// Token: 0x04000011 RID: 17
	public Transform dude1;

	// Token: 0x04000012 RID: 18
	public Transform dude2;

	// Token: 0x04000013 RID: 19
	public Transform dude3;

	// Token: 0x04000014 RID: 20
	public Transform dude4;

	// Token: 0x04000015 RID: 21
	public Transform dude5;

	// Token: 0x04000016 RID: 22
	public Transform dude1Title;

	// Token: 0x04000017 RID: 23
	public Transform dude2Title;

	// Token: 0x04000018 RID: 24
	public Transform dude3Title;

	// Token: 0x04000019 RID: 25
	public Transform dude4Title;

	// Token: 0x0400001A RID: 26
	public Transform dude5Title;

	// Token: 0x0400001B RID: 27
	private Color dude1ColorVelocity;

	// Token: 0x0400001C RID: 28
	private Vector3 velocityPos;

	// Token: 0x0400001D RID: 29
	private float fromY;

	// Token: 0x0400001E RID: 30
	private float velocityY;

	// Token: 0x0400001F RID: 31
	private Vector3 fromVec3;

	// Token: 0x04000020 RID: 32
	private Vector3 velocityVec3;

	// Token: 0x04000021 RID: 33
	private Color fromColor;

	// Token: 0x04000022 RID: 34
	private Color velocityColor;
}
