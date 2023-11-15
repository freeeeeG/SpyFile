using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class MapNodePathLine : MonoBehaviour
{
	// Token: 0x06000373 RID: 883 RVA: 0x0000E191 File Offset: 0x0000C391
	public void SetPathPoints(List<Vector3> path)
	{
		this.list_PathPoints = path;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0000E19A File Offset: 0x0000C39A
	public void SetLineType(MapNodePathLine.eLineType lineType)
	{
		if (lineType == MapNodePathLine.eLineType.AVALIABLE_PATH)
		{
			this.lineRenderer.sortingOrder = 2;
			return;
		}
		if (lineType != MapNodePathLine.eLineType.DISABLED_PATH)
		{
			return;
		}
		this.lineRenderer.sortingOrder = 1;
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0000E1BD File Offset: 0x0000C3BD
	public void SetColorGradient(Gradient gradient)
	{
		this.lineRenderer.colorGradient = gradient;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0000E1CB File Offset: 0x0000C3CB
	public void SetMaterial(Material material)
	{
		this.lineRenderer.material = material;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0000E1D9 File Offset: 0x0000C3D9
	public void SetColor(Color color)
	{
		this.lineRenderer_FogMask.startColor = color;
		this.lineRenderer_FogMask.endColor = color;
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0000E1F4 File Offset: 0x0000C3F4
	public void SetShowPercentage(float percentage)
	{
		if (percentage <= 0f)
		{
			this.ToggleLine(false);
			this.ToggleFogMaskLine(false);
			return;
		}
		this.ToggleLine(true);
		this.ToggleFogMaskLine(true);
		int num = Mathf.CeilToInt((float)this.list_PathPoints.Count * percentage);
		this.lineRenderer.positionCount = num;
		this.lineRenderer_FogMask.positionCount = num;
		for (int i = 0; i < num; i++)
		{
			this.lineRenderer.SetPosition(i, this.list_PathPoints[i]);
			this.lineRenderer_FogMask.SetPosition(i, this.list_PathPoints[i]);
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x0000E28E File Offset: 0x0000C48E
	private void ToggleLine(bool isOn)
	{
		this.isLineOn = isOn;
		this.lineRenderer.enabled = isOn;
		this.collider.enabled = isOn;
	}

	// Token: 0x0600037A RID: 890 RVA: 0x0000E2AF File Offset: 0x0000C4AF
	public void ToggleFogMaskLine(bool isOn)
	{
		this.lineRenderer_FogMask.enabled = isOn;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x0000E2C0 File Offset: 0x0000C4C0
	public void SetupCollider(Vector3 start, Vector3 end)
	{
		float num = Vector3.SignedAngle((end - start).normalized, Vector3.right, Vector3.forward);
		float x = Vector3.Distance(start, end);
		this.collider.enabled = true;
		this.collider.transform.localPosition = (start + end) / 2f;
		this.collider.transform.localRotation = Quaternion.Euler(0f, 0f, -1f * num);
		this.collider.size = this.collider.size.WithX(x);
	}

	// Token: 0x040003A7 RID: 935
	[SerializeField]
	private LineRenderer lineRenderer;

	// Token: 0x040003A8 RID: 936
	[SerializeField]
	private LineRenderer lineRenderer_FogMask;

	// Token: 0x040003A9 RID: 937
	[SerializeField]
	private BoxCollider collider;

	// Token: 0x040003AA RID: 938
	[SerializeField]
	private List<Vector3> list_PathPoints;

	// Token: 0x040003AB RID: 939
	private bool isLineOn;

	// Token: 0x02000203 RID: 515
	public enum eLineType
	{
		// Token: 0x04000A36 RID: 2614
		AVALIABLE_PATH,
		// Token: 0x04000A37 RID: 2615
		DISABLED_PATH
	}
}
