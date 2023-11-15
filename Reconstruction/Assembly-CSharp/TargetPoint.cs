using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class TargetPoint : MonoBehaviour
{
	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000668 RID: 1640 RVA: 0x0001155A File Offset: 0x0000F75A
	public Vector2 Position
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000669 RID: 1641 RVA: 0x0001156C File Offset: 0x0000F76C
	// (set) Token: 0x0600066A RID: 1642 RVA: 0x00011574 File Offset: 0x0000F774
	public IDamage Enemy { get; set; }

	// Token: 0x0600066B RID: 1643 RVA: 0x0001157D File Offset: 0x0000F77D
	private void Awake()
	{
		this.SetEnemy();
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00011585 File Offset: 0x0000F785
	protected virtual void SetEnemy()
	{
		this.Enemy = base.transform.root.GetComponent<IDamage>();
	}
}
