using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class TestingRigidbodyCS : MonoBehaviour
{
	// Token: 0x06000011 RID: 17 RVA: 0x00002EE4 File Offset: 0x000010E4
	private void Start()
	{
		this.ball1 = GameObject.Find("Sphere1");
		LeanTween.rotateAround(this.ball1, Vector3.forward, -90f, 1f);
		LeanTween.move(this.ball1, new Vector3(2f, 0f, 7f), 1f).setDelay(1f).setRepeat(-1);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002F51 File Offset: 0x00001151
	private void Update()
	{
	}

	// Token: 0x0400000E RID: 14
	private GameObject ball1;
}
