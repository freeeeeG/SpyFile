using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class TurretDetector : MonoBehaviour
{
	// Token: 0x1700026A RID: 618
	// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
	// (set) Token: 0x060005B7 RID: 1463 RVA: 0x0000F9DC File Offset: 0x0000DBDC
	public List<TurretContent> Turrets
	{
		get
		{
			return this.turrets;
		}
		set
		{
			this.turrets = value;
		}
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0000F9E8 File Offset: 0x0000DBE8
	private void OnTriggerEnter2D(Collider2D collision)
	{
		TurretContent component = collision.GetComponent<TurretContent>();
		if (!this.Turrets.Contains(component))
		{
			this.Turrets.Add(component);
		}
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0000FA18 File Offset: 0x0000DC18
	private void OnTriggerExit2D(Collider2D collision)
	{
		TurretContent component = collision.GetComponent<TurretContent>();
		this.Turrets.Remove(component);
	}

	// Token: 0x04000274 RID: 628
	private List<TurretContent> turrets = new List<TurretContent>();
}
