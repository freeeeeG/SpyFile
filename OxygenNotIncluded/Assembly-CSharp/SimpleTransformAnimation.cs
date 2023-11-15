using System;
using UnityEngine;

// Token: 0x02000C61 RID: 3169
public class SimpleTransformAnimation : MonoBehaviour
{
	// Token: 0x060064CB RID: 25803 RVA: 0x0025587A File Offset: 0x00253A7A
	private void Start()
	{
	}

	// Token: 0x060064CC RID: 25804 RVA: 0x0025587C File Offset: 0x00253A7C
	private void Update()
	{
		base.transform.Rotate(this.rotationSpeed * Time.unscaledDeltaTime);
		base.transform.Translate(this.translateSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x040044ED RID: 17645
	[SerializeField]
	private Vector3 rotationSpeed;

	// Token: 0x040044EE RID: 17646
	[SerializeField]
	private Vector3 translateSpeed;
}
