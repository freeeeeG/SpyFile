﻿using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class LogoCinematic : MonoBehaviour
{
	// Token: 0x0600003F RID: 63 RVA: 0x00002F51 File Offset: 0x00001151
	private void Awake()
	{
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00005668 File Offset: 0x00003868
	private void Start()
	{
		this.tween.transform.localPosition += -Vector3.right * 15f;
		LeanTween.moveLocalX(this.tween, this.tween.transform.localPosition.x + 15f, 0.4f).setEase(LeanTweenType.linear).setDelay(0f).setOnComplete(new Action(this.playBoom));
		this.tween.transform.RotateAround(this.tween.transform.position, Vector3.forward, -30f);
		LeanTween.rotateAround(this.tween, Vector3.forward, 30f, 0.4f).setEase(LeanTweenType.easeInQuad).setDelay(0.4f).setOnComplete(new Action(this.playBoom));
		this.lean.transform.position += Vector3.up * 5.1f;
		LeanTween.moveY(this.lean, this.lean.transform.position.y - 5.1f, 0.6f).setEase(LeanTweenType.easeInQuad).setDelay(0.6f).setOnComplete(new Action(this.playBoom));
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000057D0 File Offset: 0x000039D0
	private void playBoom()
	{
		AnimationCurve volume = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1.163155f, 0f, -1f),
			new Keyframe(0.3098361f, 0f, 0f, 0f),
			new Keyframe(0.5f, 0.003524712f, 0f, 0f)
		});
		AnimationCurve frequency = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0.000819672f, 0.007666667f, 0f, 0f),
			new Keyframe(0.01065573f, 0.002424242f, 0f, 0f),
			new Keyframe(0.02704918f, 0.007454545f, 0f, 0f),
			new Keyframe(0.03770492f, 0.002575758f, 0f, 0f),
			new Keyframe(0.052459f, 0.007090909f, 0f, 0f),
			new Keyframe(0.06885245f, 0.002939394f, 0f, 0f),
			new Keyframe(0.0819672f, 0.006727273f, 0f, 0f),
			new Keyframe(0.1040983f, 0.003181818f, 0f, 0f),
			new Keyframe(0.1188525f, 0.006212121f, 0f, 0f),
			new Keyframe(0.145082f, 0.004151515f, 0f, 0f),
			new Keyframe(0.1893443f, 0.005636364f, 0f, 0f)
		});
		LeanAudio.play(LeanAudio.createAudio(volume, frequency, LeanAudio.options().setVibrato(new Vector3[]
		{
			new Vector3(0.1f, 0f, 0f)
		}).setFrequency(11025)));
	}

	// Token: 0x0400004E RID: 78
	public GameObject lean;

	// Token: 0x0400004F RID: 79
	public GameObject tween;
}
