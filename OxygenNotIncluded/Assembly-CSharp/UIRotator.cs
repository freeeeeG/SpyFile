using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
[AddComponentMenu("KMonoBehaviour/prefabs/UIRotator")]
public class UIRotator : KMonoBehaviour
{
	// Token: 0x06000021 RID: 33 RVA: 0x000028FF File Offset: 0x00000AFF
	protected override void OnPrefabInit()
	{
		this.rotationSpeed = UnityEngine.Random.Range(this.minRotationSpeed, this.maxRotationSpeed);
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002918 File Offset: 0x00000B18
	private void Update()
	{
		base.GetComponent<RectTransform>().Rotate(0f, 0f, this.rotationSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x0400001E RID: 30
	public float minRotationSpeed = 1f;

	// Token: 0x0400001F RID: 31
	public float maxRotationSpeed = 1f;

	// Token: 0x04000020 RID: 32
	public float rotationSpeed = 1f;
}
