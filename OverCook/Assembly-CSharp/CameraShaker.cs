using System;
using UnityEngine;

// Token: 0x02000792 RID: 1938
public class CameraShaker : MonoBehaviour
{
	// Token: 0x0600257D RID: 9597 RVA: 0x000B151C File Offset: 0x000AF91C
	private float GetProp(float[] _phases, float _frequency, float _timer)
	{
		float num = 0f;
		if (_phases.Length > 0)
		{
			for (int i = 0; i < _phases.Length; i++)
			{
				num += Mathf.Sin(_frequency * _phases[i] * _timer);
			}
			num /= (float)_phases.Length;
		}
		return num;
	}

	// Token: 0x0600257E RID: 9598 RVA: 0x000B1564 File Offset: 0x000AF964
	private void Update()
	{
		float prop = this.GetProp(this.PhasesX, this.FrequencyX, this.m_timer);
		float prop2 = this.GetProp(this.PhasesY, this.FrequencyY, this.m_timer);
		float prop3 = this.GetProp(this.PhasesZ, this.FrequencyZ, this.m_timer);
		Vector3 localPosition = this.Power * new Vector3(this.DistanceX * prop, this.DistanceY * prop2, this.DistanceZ * prop3);
		base.transform.localPosition = localPosition;
		this.m_timer += TimeManager.GetDeltaTime(base.gameObject);
		this.m_timer %= 3141592.8f;
	}

	// Token: 0x04001D02 RID: 7426
	[Range(0f, 1f)]
	public float Power = 1f;

	// Token: 0x04001D03 RID: 7427
	[Header("X")]
	public float DistanceX = 0.03f;

	// Token: 0x04001D04 RID: 7428
	public float FrequencyX = 1f;

	// Token: 0x04001D05 RID: 7429
	public float[] PhasesX = new float[0];

	// Token: 0x04001D06 RID: 7430
	[Header("Y")]
	public float DistanceY = 0.03f;

	// Token: 0x04001D07 RID: 7431
	public float FrequencyY = 1f;

	// Token: 0x04001D08 RID: 7432
	public float[] PhasesY = new float[]
	{
		1f
	};

	// Token: 0x04001D09 RID: 7433
	[Header("Z")]
	public float DistanceZ = 0.03f;

	// Token: 0x04001D0A RID: 7434
	public float FrequencyZ = 1f;

	// Token: 0x04001D0B RID: 7435
	public float[] PhasesZ = new float[0];

	// Token: 0x04001D0C RID: 7436
	private float m_timer;
}
