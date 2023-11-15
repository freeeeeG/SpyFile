using System;
using UnityEngine;

// Token: 0x020008EF RID: 2287
public class FogOfWarPostFX : MonoBehaviour
{
	// Token: 0x0600422F RID: 16943 RVA: 0x001720E0 File Offset: 0x001702E0
	private void Awake()
	{
		if (this.shader != null)
		{
			this.material = new Material(this.shader);
		}
	}

	// Token: 0x06004230 RID: 16944 RVA: 0x00172101 File Offset: 0x00170301
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.SetupUVs();
		Graphics.Blit(source, destination, this.material, 0);
	}

	// Token: 0x06004231 RID: 16945 RVA: 0x00172118 File Offset: 0x00170318
	private void SetupUVs()
	{
		if (this.myCamera == null)
		{
			this.myCamera = base.GetComponent<Camera>();
			if (this.myCamera == null)
			{
				return;
			}
		}
		Ray ray = this.myCamera.ViewportPointToRay(Vector3.zero);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector4 vector;
		vector.x = point.x / Grid.WidthInMeters;
		vector.y = point.y / Grid.HeightInMeters;
		ray = this.myCamera.ViewportPointToRay(Vector3.one);
		distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		point = ray.GetPoint(distance);
		vector.z = point.x / Grid.WidthInMeters - vector.x;
		vector.w = point.y / Grid.HeightInMeters - vector.y;
		this.material.SetVector("_UVOffsetScale", vector);
	}

	// Token: 0x04002B39 RID: 11065
	[SerializeField]
	private Shader shader;

	// Token: 0x04002B3A RID: 11066
	private Material material;

	// Token: 0x04002B3B RID: 11067
	private Camera myCamera;
}
