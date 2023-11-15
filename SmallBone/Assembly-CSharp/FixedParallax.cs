using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class FixedParallax : MonoBehaviour
{
	// Token: 0x06000178 RID: 376 RVA: 0x000075F5 File Offset: 0x000057F5
	private void Awake()
	{
		this._origin = base.transform.position;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00007608 File Offset: 0x00005808
	private void LateUpdate()
	{
		this.SetPosition();
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00007610 File Offset: 0x00005810
	private void SetPosition()
	{
		Vector2 vector = this._origin;
		Vector2 b = (Camera.main.transform.position - (this._origin + this._offset)) * this._positionRatio;
		vector += b;
		base.transform.position = vector;
	}

	// Token: 0x0400013E RID: 318
	[Tooltip("카메라 위치로 인해 변경되는 오브젝트의 중심점을 움직입니다.\n카메라의 중심이 정확히 이 오브젝트의 위치 + offset에 위치할 때 이 오브젝트가 에디터상에서 배치한 그 위치에 보여집니다.")]
	[SerializeField]
	private Vector2 _offset;

	// Token: 0x0400013F RID: 319
	[SerializeField]
	[Tooltip("카메라로부터 떨어진 거리에 이 값을 곱한 만큼 움직입니다. 0이면 움직이지 않고, 1이면 항상 카메라에 붙어 다니게 됩니다. 값의 범위에 제한은 없습니다.")]
	private Vector2 _positionRatio;

	// Token: 0x04000140 RID: 320
	private Vector3 _origin;
}
