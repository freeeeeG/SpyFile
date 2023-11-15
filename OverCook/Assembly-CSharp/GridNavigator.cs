using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class GridNavigator : MonoBehaviour
{
	// Token: 0x06000480 RID: 1152 RVA: 0x000271AE File Offset: 0x000255AE
	private void Awake()
	{
		this.m_gridNavSpace = GameUtils.GetGridNavSpace();
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x000271BB File Offset: 0x000255BB
	public void SetSpeedModifier(float _modifier)
	{
		this.m_speedModifier = _modifier;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x000271C4 File Offset: 0x000255C4
	public void MoveToTarget(Vector3 _pos)
	{
		this.ClearTarget();
		this.m_isMoving = true;
		base.StartCoroutine(this.MovingCoroutine(_pos));
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x000271E1 File Offset: 0x000255E1
	public void ClearTarget()
	{
		base.StopAllCoroutines();
		this.m_isMoving = false;
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x000271F0 File Offset: 0x000255F0
	public bool HasCompletedRoute()
	{
		return !this.m_isMoving;
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x000271FC File Offset: 0x000255FC
	private IEnumerator MovingCoroutine(Vector3 _target)
	{
		Point2 startPoint = this.m_gridNavSpace.GetNavPoint(base.transform.position);
		Point2 targetPoint = this.m_gridNavSpace.GetNavPoint(_target);
		List<Vector3> path = this.m_gridNavSpace.FindPath(startPoint, targetPoint);
		if (path.Count > 0)
		{
			float distanceToMove = 0f;
			for (int i = 0; i < path.Count; i++)
			{
				for (;;)
				{
					Vector3 nextPos = path[i];
					if (distanceToMove > 0f)
					{
						float magnitude = (nextPos - base.transform.position).magnitude;
						float num = Mathf.Min(magnitude, distanceToMove);
						base.transform.position += (nextPos - base.transform.position).SafeNormalised(Vector3.zero) * num;
						base.transform.rotation = Quaternion.LookRotation((nextPos - base.transform.position).SafeNormalised(base.transform.forward), base.transform.up);
						distanceToMove -= num;
					}
					if (distanceToMove != 0f)
					{
						break;
					}
					yield return null;
					distanceToMove += TimeManager.GetDeltaTime(base.gameObject) * this.m_speed * this.m_speedModifier;
				}
			}
		}
		this.m_isMoving = false;
		yield break;
	}

	// Token: 0x040003FA RID: 1018
	[SerializeField]
	private float m_speed = 4.5f;

	// Token: 0x040003FB RID: 1019
	private GridNavSpace m_gridNavSpace;

	// Token: 0x040003FC RID: 1020
	private bool m_isMoving;

	// Token: 0x040003FD RID: 1021
	private float m_speedModifier = 1f;
}
