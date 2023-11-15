using System;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public class Visibility : MonoBehaviour
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x0600038E RID: 910 RVA: 0x0000CF40 File Offset: 0x0000B140
	// (remove) Token: 0x0600038F RID: 911 RVA: 0x0000CF78 File Offset: 0x0000B178
	public event Action<bool> onChanged;

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000390 RID: 912 RVA: 0x0000CFAD File Offset: 0x0000B1AD
	// (set) Token: 0x06000391 RID: 913 RVA: 0x0000CFB5 File Offset: 0x0000B1B5
	public bool visible { get; private set; } = true;

	// Token: 0x06000392 RID: 914 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
	public void SetVisible(bool visible)
	{
		if (this.visible == visible)
		{
			return;
		}
		this.visible = visible;
		Vector3 position = base.transform.position;
		position.z = (visible ? 0f : -100f);
		base.transform.position = position;
		Action<bool> action = this.onChanged;
		if (action == null)
		{
			return;
		}
		action(visible);
	}

	// Token: 0x040002D6 RID: 726
	private const float outOfCameraZPosition = -100f;
}
