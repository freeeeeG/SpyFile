using System;
using UnityEngine;

// Token: 0x0200019D RID: 413
public class Func_PerlinNoiseFloat : MonoBehaviour
{
	// Token: 0x06000AF7 RID: 2807 RVA: 0x0002A454 File Offset: 0x00028654
	private void Start()
	{
		this.ResetStartPos();
		this.perlinOffset = (base.transform.position.x + base.transform.position.y) * this.perlinOffsetMultiplier + Random.Range(0f, this.randomPerlinOffsetMultiplier);
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0002A4A8 File Offset: 0x000286A8
	private void Update()
	{
		this.time = Time.timeSinceLevelLoad;
		if (this.isLocalPosition)
		{
			base.transform.localPosition = this.startPos + new Vector3(Mathf.PerlinNoise(this.time * this.offsetSpeedMultiplier * this.externalSpeedMultiplier + this.perlinOffset, 0f) - 0.5f, Mathf.PerlinNoise(0f, this.time * this.offsetSpeedMultiplier * this.externalSpeedMultiplier + this.perlinOffset) - 0.5f) * this.offsetMultiplier;
		}
		else
		{
			base.transform.position = this.startPos + new Vector3(Mathf.PerlinNoise(this.time * this.offsetSpeedMultiplier * this.externalSpeedMultiplier + this.perlinOffset, 0f) - 0.5f, Mathf.PerlinNoise(0f, this.time * this.offsetSpeedMultiplier * this.externalSpeedMultiplier + this.perlinOffset) - 0.5f) * this.offsetMultiplier;
		}
		base.transform.localRotation = this.startRot * Quaternion.Euler(0f, 0f, (Mathf.PerlinNoise(this.time * this.rotateSpeedMultiplier * this.externalSpeedMultiplier + this.perlinOffset, 0f) - 0.5f) * this.rotationAngle);
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0002A619 File Offset: 0x00028819
	public void ResetStartPos()
	{
		this.startPos = (this.isLocalPosition ? base.transform.localPosition : base.transform.position);
		this.startRot = base.transform.localRotation;
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0002A652 File Offset: 0x00028852
	public void SetExternalSpeedMultiplier(float value)
	{
		this.externalSpeedMultiplier = value;
	}

	// Token: 0x040008E2 RID: 2274
	[SerializeField]
	private bool isLocalPosition;

	// Token: 0x040008E3 RID: 2275
	[SerializeField]
	private float offsetSpeedMultiplier = 1f;

	// Token: 0x040008E4 RID: 2276
	[SerializeField]
	private float offsetMultiplier = 1f;

	// Token: 0x040008E5 RID: 2277
	[SerializeField]
	private float rotateSpeedMultiplier = 1f;

	// Token: 0x040008E6 RID: 2278
	[SerializeField]
	private float rotationAngle = 30f;

	// Token: 0x040008E7 RID: 2279
	[SerializeField]
	private float perlinOffsetMultiplier = 1f;

	// Token: 0x040008E8 RID: 2280
	[SerializeField]
	private float randomPerlinOffsetMultiplier = 1f;

	// Token: 0x040008E9 RID: 2281
	private float perlinOffset;

	// Token: 0x040008EA RID: 2282
	private Vector3 startPos;

	// Token: 0x040008EB RID: 2283
	private Quaternion startRot;

	// Token: 0x040008EC RID: 2284
	private float time;

	// Token: 0x040008ED RID: 2285
	private float externalSpeedMultiplier = 1f;
}
