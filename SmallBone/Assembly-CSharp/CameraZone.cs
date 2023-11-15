using System;
using Scenes;
using UI;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class CameraZone : MonoBehaviour
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000040 RID: 64 RVA: 0x0000324B File Offset: 0x0000144B
	// (set) Token: 0x06000041 RID: 65 RVA: 0x00003253 File Offset: 0x00001453
	public bool hasCeil
	{
		get
		{
			return this._hasCeil;
		}
		set
		{
			this._hasCeil = value;
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0000325C File Offset: 0x0000145C
	private void Awake()
	{
		if (this._zone != null)
		{
			this.bounds = this._zone.bounds;
			UnityEngine.Object.Destroy(this._zone);
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003288 File Offset: 0x00001488
	public Vector3 GetClampedPosition(Camera camera, Vector3 position)
	{
		float orthographicSize = camera.orthographicSize;
		UIManager uiManager = Scene<GameBase>.instance.uiManager;
		Vector2 sizeDelta = uiManager.rectTransform.sizeDelta;
		Vector2 sizeDelta2 = uiManager.content.sizeDelta;
		float num = sizeDelta2.x / sizeDelta2.y;
		float num2 = orthographicSize * 2f * sizeDelta2.y / sizeDelta.y;
		float num3 = num2 * num;
		Vector3 max = this.bounds.max;
		max.x -= num3 * 0.5f;
		if (this._hasCeil)
		{
			max.y -= num2 * 0.5f;
		}
		else
		{
			max.y = float.PositiveInfinity;
		}
		Vector3 min = this.bounds.min;
		min.x += num3 * 0.5f;
		min.y += num2 * 0.5f;
		float z = position.z;
		position = Vector3.Max(Vector3.Min(position, max), min);
		position.z = z;
		if (this.bounds.size.x < num3)
		{
			switch (this._horizontalAlign)
			{
			case CameraZone.HorizontalAlign.Center:
				position.x = this.bounds.center.x;
				break;
			case CameraZone.HorizontalAlign.Left:
				position.x = min.x;
				break;
			case CameraZone.HorizontalAlign.Right:
				position.x = max.x;
				break;
			}
		}
		if (this.bounds.size.y < num2)
		{
			switch (this._verticalAlign)
			{
			case CameraZone.VerticalAlign.Center:
				position.y = this.bounds.center.y;
				break;
			case CameraZone.VerticalAlign.Bottom:
				position.y = min.y;
				break;
			case CameraZone.VerticalAlign.Top:
				position.y = max.y;
				break;
			}
		}
		return position;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003450 File Offset: 0x00001650
	public void ClampPosition(Camera camera)
	{
		camera.transform.position = this.GetClampedPosition(camera, camera.transform.position);
	}

	// Token: 0x0400003F RID: 63
	[GetComponent]
	[SerializeField]
	private BoxCollider2D _zone;

	// Token: 0x04000040 RID: 64
	[SerializeField]
	private CameraZone.HorizontalAlign _horizontalAlign;

	// Token: 0x04000041 RID: 65
	[SerializeField]
	private CameraZone.VerticalAlign _verticalAlign = CameraZone.VerticalAlign.Bottom;

	// Token: 0x04000042 RID: 66
	[SerializeField]
	private bool _hasCeil;

	// Token: 0x04000043 RID: 67
	[NonSerialized]
	public Bounds bounds;

	// Token: 0x02000011 RID: 17
	private enum HorizontalAlign
	{
		// Token: 0x04000045 RID: 69
		Center,
		// Token: 0x04000046 RID: 70
		Left,
		// Token: 0x04000047 RID: 71
		Right
	}

	// Token: 0x02000012 RID: 18
	private enum VerticalAlign
	{
		// Token: 0x04000049 RID: 73
		Center,
		// Token: 0x0400004A RID: 74
		Bottom,
		// Token: 0x0400004B RID: 75
		Top
	}
}
