using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class PathSplinePerformance : MonoBehaviour
{
	// Token: 0x06000055 RID: 85 RVA: 0x00006254 File Offset: 0x00004454
	private void Start()
	{
		Application.targetFrameRate = 240;
		List<Vector3> list = new List<Vector3>();
		float num = 0f;
		int num2 = this.trackNodes + 1;
		for (int i = 0; i < num2; i++)
		{
			float x = Mathf.Cos(num * 0.017453292f) * this.circleLength + Random.Range(0f, this.randomRange);
			float z = Mathf.Sin(num * 0.017453292f) * this.circleLength + Random.Range(0f, this.randomRange);
			list.Add(new Vector3(x, 1f, z));
			num += 360f / (float)this.trackNodes;
		}
		list[0] = list[list.Count - 1];
		list.Add(list[1]);
		list.Add(list[2]);
		this.track = new LTSpline(list.ToArray());
		this.carAdd = this.carSpeed / this.track.distance;
		this.tracerSpeed = this.track.distance / (this.carSpeed * 1.2f);
		LeanTween.moveSpline(this.trackTrailRenderers, this.track, this.tracerSpeed).setOrientToPath(true).setRepeat(-1);
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00006398 File Offset: 0x00004598
	private void Update()
	{
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
		this.track.place(this.car.transform, this.trackPosition);
		this.trackPosition += Time.deltaTime * this.carAdd;
		if (this.trackPosition > 1f)
		{
			this.trackPosition = 0f;
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00006477 File Offset: 0x00004677
	private void OnDrawGizmos()
	{
		if (this.track != null)
		{
			this.track.drawGizmo(Color.red);
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00006494 File Offset: 0x00004694
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

	// Token: 0x0400006E RID: 110
	public GameObject trackTrailRenderers;

	// Token: 0x0400006F RID: 111
	public GameObject car;

	// Token: 0x04000070 RID: 112
	public GameObject carInternal;

	// Token: 0x04000071 RID: 113
	public float circleLength = 10f;

	// Token: 0x04000072 RID: 114
	public float randomRange = 1f;

	// Token: 0x04000073 RID: 115
	public int trackNodes = 30;

	// Token: 0x04000074 RID: 116
	public float carSpeed = 30f;

	// Token: 0x04000075 RID: 117
	public float tracerSpeed = 2f;

	// Token: 0x04000076 RID: 118
	private LTSpline track;

	// Token: 0x04000077 RID: 119
	private int trackIter = 1;

	// Token: 0x04000078 RID: 120
	private float carAdd;

	// Token: 0x04000079 RID: 121
	private float trackPosition;
}
