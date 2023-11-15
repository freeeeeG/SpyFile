using System;
using StylizedWater;
using UnityEngine;

// Token: 0x02000003 RID: 3
[RequireComponent(typeof(Rigidbody))]
public class BuoyantObject : MonoBehaviour
{
	// Token: 0x06000004 RID: 4 RVA: 0x0000207C File Offset: 0x0000027C
	private void Awake()
	{
		this.steepness = this.water.GetWaveSteepness();
		this.wavelength = this.water.GetWaveLength();
		this.speed = this.water.GetWaveSpeed();
		this.directions = this.water.GetWaveDirections();
		this.rb = base.GetComponent<Rigidbody>();
		this.rb.useGravity = false;
		this.effectorProjections = new Vector3[this.effectors.Length];
		for (int i = 0; i < this.effectors.Length; i++)
		{
			this.effectorProjections[i] = this.effectors[i].position;
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002124 File Offset: 0x00000324
	private void OnDisable()
	{
		this.rb.useGravity = true;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002134 File Offset: 0x00000334
	private void FixedUpdate()
	{
		int num = this.effectors.Length;
		for (int i = 0; i < num; i++)
		{
			Vector3 position = this.effectors[i].position;
			this.effectorProjections[i] = position;
			this.effectorProjections[i].y = this.water.transform.position.y + GerstnerWaveDisplacement.GetWaveDisplacement(position, this.steepness, this.wavelength, this.speed, this.directions).y;
			this.rb.AddForceAtPosition(Physics.gravity / (float)num, position, ForceMode.Acceleration);
			float y = this.effectorProjections[i].y;
			float y2 = position.y;
			if (y2 < y)
			{
				float num2 = Mathf.Clamp01(y - y2) / this.objectDepth;
				float d = Mathf.Abs(Physics.gravity.y) * num2 * this.strength;
				this.rb.AddForceAtPosition(Vector3.up * d, position, ForceMode.Acceleration);
				this.rb.AddForce(-this.rb.velocity * this.velocityDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
				this.rb.AddTorque(-this.rb.angularVelocity * this.angularDrag * Time.fixedDeltaTime, ForceMode.Impulse);
			}
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000022A4 File Offset: 0x000004A4
	private void OnDrawGizmos()
	{
		if (this.effectors == null)
		{
			return;
		}
		for (int i = 0; i < this.effectors.Length; i++)
		{
			if (!Application.isPlaying && this.effectors[i] != null)
			{
				Gizmos.color = this.green;
				Gizmos.DrawSphere(this.effectors[i].position, 0.06f);
			}
			else
			{
				if (this.effectors[i] == null)
				{
					return;
				}
				if (this.effectors[i].position.y < this.effectorProjections[i].y)
				{
					Gizmos.color = this.red;
				}
				else
				{
					Gizmos.color = this.green;
				}
				Gizmos.DrawSphere(this.effectors[i].position, 0.06f);
				Gizmos.color = this.orange;
				Gizmos.DrawSphere(this.effectorProjections[i], 0.06f);
				Gizmos.color = this.blue;
				Gizmos.DrawLine(this.effectors[i].position, this.effectorProjections[i]);
			}
		}
	}

	// Token: 0x04000001 RID: 1
	private Color red = new Color(0.92f, 0.25f, 0.2f);

	// Token: 0x04000002 RID: 2
	private Color green = new Color(0.2f, 0.92f, 0.51f);

	// Token: 0x04000003 RID: 3
	private Color blue = new Color(0.2f, 0.67f, 0.92f);

	// Token: 0x04000004 RID: 4
	private Color orange = new Color(0.97f, 0.79f, 0.26f);

	// Token: 0x04000005 RID: 5
	private float steepness;

	// Token: 0x04000006 RID: 6
	private float wavelength;

	// Token: 0x04000007 RID: 7
	private float speed;

	// Token: 0x04000008 RID: 8
	private float[] directions = new float[4];

	// Token: 0x04000009 RID: 9
	[Header("Water Object")]
	public StylizedWaterURP water;

	// Token: 0x0400000A RID: 10
	[Header("Buoyancy")]
	[Range(1f, 5f)]
	public float strength = 1f;

	// Token: 0x0400000B RID: 11
	[Range(0.2f, 5f)]
	public float objectDepth = 1f;

	// Token: 0x0400000C RID: 12
	public float velocityDrag = 0.99f;

	// Token: 0x0400000D RID: 13
	public float angularDrag = 0.5f;

	// Token: 0x0400000E RID: 14
	[Header("Effectors")]
	public Transform[] effectors;

	// Token: 0x0400000F RID: 15
	private Rigidbody rb;

	// Token: 0x04000010 RID: 16
	private Vector3[] effectorProjections;
}
