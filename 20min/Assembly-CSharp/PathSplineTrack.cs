using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class PathSplineTrack : MonoBehaviour
{
	// Token: 0x0600005A RID: 90 RVA: 0x00006630 File Offset: 0x00004830
	private void Start()
	{
		this.track = new LTSpline(new Vector3[]
		{
			this.trackOnePoints[0].position,
			this.trackOnePoints[1].position,
			this.trackOnePoints[2].position,
			this.trackOnePoints[3].position,
			this.trackOnePoints[4].position,
			this.trackOnePoints[5].position,
			this.trackOnePoints[6].position
		});
		LeanTween.moveSpline(this.trackTrailRenderers, this.track, 2f).setOrientToPath(true).setRepeat(-1);
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00006700 File Offset: 0x00004900
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
		this.trackPosition += Time.deltaTime * 0.03f;
		if (this.trackPosition < 0f)
		{
			this.trackPosition = 1f;
			return;
		}
		if (this.trackPosition > 1f)
		{
			this.trackPosition = 0f;
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000067F7 File Offset: 0x000049F7
	private void OnDrawGizmos()
	{
		LTSpline.drawGizmo(this.trackOnePoints, Color.red);
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0000680C File Offset: 0x00004A0C
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

	// Token: 0x0400007A RID: 122
	public GameObject car;

	// Token: 0x0400007B RID: 123
	public GameObject carInternal;

	// Token: 0x0400007C RID: 124
	public GameObject trackTrailRenderers;

	// Token: 0x0400007D RID: 125
	public Transform[] trackOnePoints;

	// Token: 0x0400007E RID: 126
	private LTSpline track;

	// Token: 0x0400007F RID: 127
	private int trackIter = 1;

	// Token: 0x04000080 RID: 128
	private float trackPosition;
}
