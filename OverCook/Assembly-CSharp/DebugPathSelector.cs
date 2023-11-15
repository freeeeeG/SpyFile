using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
[RequireComponent(typeof(GridNavigator))]
public class DebugPathSelector : MonoBehaviour
{
	// Token: 0x0600047D RID: 1149 RVA: 0x0002710D File Offset: 0x0002550D
	private void Awake()
	{
		this.m_gridNavigator = base.gameObject.RequireComponent<GridNavigator>();
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00027120 File Offset: 0x00025520
	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 pos = ray.origin - ray.origin.y * ray.direction / ray.direction.y;
			this.m_gridNavigator.MoveToTarget(pos);
		}
	}

	// Token: 0x040003F9 RID: 1017
	private GridNavigator m_gridNavigator;
}
