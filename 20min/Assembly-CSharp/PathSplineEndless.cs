using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class PathSplineEndless : MonoBehaviour
{
	// Token: 0x0600004D RID: 77 RVA: 0x00005D7C File Offset: 0x00003F7C
	private void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			this.addRandomTrackPoint();
		}
		this.refreshSpline();
		LeanTween.value(base.gameObject, 0f, 0.3f, 2f).setOnUpdate(delegate(float val)
		{
			this.pushTrackAhead = val;
		});
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00005DD0 File Offset: 0x00003FD0
	private void Update()
	{
		if (this.trackPts[this.trackPts.Count - 1].z - base.transform.position.z < 200f)
		{
			this.addRandomTrackPoint();
			this.refreshSpline();
		}
		this.track.place(this.car.transform, this.carIter);
		this.carIter += this.carAdd * Time.deltaTime;
		this.track.place(this.trackTrailRenderers.transform, this.carIter + this.pushTrackAhead);
		float axis = Input.GetAxis("Horizontal");
		if (Input.anyKeyDown)
		{
			if (axis < 0f && this.trackIter > 0)
			{
				this.trackIter--;
				this.playSwish();
			}
			else if (axis > 0f && this.trackIter < 2)
			{
				this.trackIter++;
				this.playSwish();
			}
			LeanTween.moveLocalX(this.carInternal, (float)(this.trackIter - 1) * 6f, 0.3f).setEase(LeanTweenType.easeOutBack);
		}
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00005EFB File Offset: 0x000040FB
	private GameObject objectQueue(GameObject[] arr, ref int lastIter)
	{
		lastIter = ((lastIter >= arr.Length - 1) ? 0 : (lastIter + 1));
		arr[lastIter].transform.localScale = Vector3.one;
		arr[lastIter].transform.rotation = Quaternion.identity;
		return arr[lastIter];
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00005F3C File Offset: 0x0000413C
	private void addRandomTrackPoint()
	{
		float num = Mathf.PerlinNoise(0f, this.randomIter);
		this.randomIter += this.randomIterWidth;
		Vector3 vector = new Vector3((num - 0.5f) * 20f, 0f, (float)this.zIter * 40f);
		this.objectQueue(this.cubes, ref this.cubesIter).transform.position = vector;
		GameObject gameObject = this.objectQueue(this.trees, ref this.treesIter);
		float num2 = (this.zIter % 2 == 0) ? -15f : 15f;
		gameObject.transform.position = new Vector3(vector.x + num2, 0f, (float)this.zIter * 40f);
		LeanTween.rotateAround(gameObject, Vector3.forward, 0f, 1f).setFrom((this.zIter % 2 == 0) ? 180f : -180f).setEase(LeanTweenType.easeOutBack);
		this.trackPts.Add(vector);
		if (this.trackPts.Count > this.trackMaxItems)
		{
			this.trackPts.RemoveAt(0);
		}
		this.zIter++;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00006074 File Offset: 0x00004274
	private void refreshSpline()
	{
		this.track = new LTSpline(this.trackPts.ToArray());
		this.carIter = this.track.ratioAtPoint(this.car.transform.position);
		this.carAdd = 40f / this.track.distance;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000060D0 File Offset: 0x000042D0
	private void playSwish()
	{
		AnimationCurve volume = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.005464481f, 1.83897f, 0f),
			new Keyframe(0.1114856f, 2.281785f, 0f, 0f),
			new Keyframe(0.2482903f, 2.271654f, 0f, 0f),
			new Keyframe(0.3f, 0.01670286f, 0f, 0f)
		});
		AnimationCurve frequency = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.00136725f, 0f, 0f),
			new Keyframe(0.1482391f, 0.005405405f, 0f, 0f),
			new Keyframe(0.2650336f, 0.002480127f, 0f, 0f)
		});
		LeanAudio.play(LeanAudio.createAudio(volume, frequency, LeanAudio.options().setVibrato(new Vector3[]
		{
			new Vector3(0.2f, 0.5f, 0f)
		}).setWaveNoise().setWaveNoiseScale(1000f)));
	}

	// Token: 0x0400005D RID: 93
	public GameObject trackTrailRenderers;

	// Token: 0x0400005E RID: 94
	public GameObject car;

	// Token: 0x0400005F RID: 95
	public GameObject carInternal;

	// Token: 0x04000060 RID: 96
	public GameObject[] cubes;

	// Token: 0x04000061 RID: 97
	private int cubesIter;

	// Token: 0x04000062 RID: 98
	public GameObject[] trees;

	// Token: 0x04000063 RID: 99
	private int treesIter;

	// Token: 0x04000064 RID: 100
	public float randomIterWidth = 0.1f;

	// Token: 0x04000065 RID: 101
	private LTSpline track;

	// Token: 0x04000066 RID: 102
	private List<Vector3> trackPts = new List<Vector3>();

	// Token: 0x04000067 RID: 103
	private int zIter;

	// Token: 0x04000068 RID: 104
	private float carIter;

	// Token: 0x04000069 RID: 105
	private float carAdd;

	// Token: 0x0400006A RID: 106
	private int trackMaxItems = 15;

	// Token: 0x0400006B RID: 107
	private int trackIter = 1;

	// Token: 0x0400006C RID: 108
	private float pushTrackAhead;

	// Token: 0x0400006D RID: 109
	private float randomIter;
}
