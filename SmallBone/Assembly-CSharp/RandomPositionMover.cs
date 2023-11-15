using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class RandomPositionMover : MonoBehaviour
{
	// Token: 0x06000004 RID: 4 RVA: 0x000020D4 File Offset: 0x000002D4
	private void Start()
	{
		if (this.pickerInterval == 0f)
		{
			this.pickerInterval = 3f;
		}
		this.randomPointInCircle = Vector2.zero;
		base.InvokeRepeating("PickRandomPointInCircle", UnityEngine.Random.Range(0f, this.pickerInterval), this.pickerInterval);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002128 File Offset: 0x00000328
	private void PickRandomPointInCircle()
	{
		base.transform.position = this.player.transform.position;
		this.randomPointInCircle = base.transform.localPosition + UnityEngine.Random.insideUnitCircle * this.radius;
		base.transform.localPosition = this.randomPointInCircle;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002191 File Offset: 0x00000391
	private void Update()
	{
	}

	// Token: 0x04000003 RID: 3
	public float pickerInterval;

	// Token: 0x04000004 RID: 4
	public float radius;

	// Token: 0x04000005 RID: 5
	public GameObject player;

	// Token: 0x04000006 RID: 6
	public Vector2 randomPointInCircle;
}
