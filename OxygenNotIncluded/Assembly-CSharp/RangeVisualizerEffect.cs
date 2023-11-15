using System;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000A3F RID: 2623
public class RangeVisualizerEffect : MonoBehaviour
{
	// Token: 0x06004F07 RID: 20231 RVA: 0x001BE5CF File Offset: 0x001BC7CF
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/Range"));
	}

	// Token: 0x06004F08 RID: 20232 RVA: 0x001BE5E8 File Offset: 0x001BC7E8
	private void OnPostRender()
	{
		RangeVisualizer rangeVisualizer = null;
		Vector2I u = new Vector2I(0, 0);
		if (SelectTool.Instance.selected != null)
		{
			Grid.PosToXY(SelectTool.Instance.selected.transform.GetPosition(), out u.x, out u.y);
			rangeVisualizer = SelectTool.Instance.selected.GetComponent<RangeVisualizer>();
		}
		if (rangeVisualizer == null && BuildTool.Instance.visualizer != null)
		{
			Grid.PosToXY(BuildTool.Instance.visualizer.transform.GetPosition(), out u.x, out u.y);
			rangeVisualizer = BuildTool.Instance.visualizer.GetComponent<RangeVisualizer>();
		}
		if (rangeVisualizer != null)
		{
			if (this.OcclusionTex == null)
			{
				this.OcclusionTex = new Texture2D(64, 64, TextureFormat.Alpha8, false);
				this.OcclusionTex.filterMode = FilterMode.Point;
				this.OcclusionTex.wrapMode = TextureWrapMode.Clamp;
			}
			Vector2I vector2I;
			Vector2I vector2I2;
			this.FindWorldBounds(out vector2I, out vector2I2);
			Vector2I rangeMin = rangeVisualizer.RangeMin;
			Vector2I rangeMax = rangeVisualizer.RangeMax;
			Vector2I vector2I3 = rangeVisualizer.OriginOffset;
			Rotatable rotatable;
			if (rangeVisualizer.TryGetComponent<Rotatable>(out rotatable))
			{
				vector2I3 = rotatable.GetRotatedOffset(vector2I3);
				Vector2I rotatedOffset = rotatable.GetRotatedOffset(rangeMin);
				Vector2I rotatedOffset2 = rotatable.GetRotatedOffset(rangeMax);
				rangeMin.x = ((rotatedOffset.x < rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				rangeMin.y = ((rotatedOffset.y < rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
				rangeMax.x = ((rotatedOffset.x > rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				rangeMax.y = ((rotatedOffset.y > rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
			}
			Vector2I vector2I4 = u + vector2I3;
			int width = this.OcclusionTex.width;
			NativeArray<byte> pixelData = this.OcclusionTex.GetPixelData<byte>(0);
			int num = 0;
			if (rangeVisualizer.TestLineOfSight)
			{
				for (int i = 0; i <= rangeMax.y - rangeMin.y; i++)
				{
					int num2 = vector2I4.y + rangeMin.y + i;
					for (int j = 0; j <= rangeMax.x - rangeMin.x; j++)
					{
						int num3 = vector2I4.x + rangeMin.x + j;
						Grid.XYToCell(num3, num2);
						bool flag = num3 > vector2I.x && num3 < vector2I2.x && num2 > vector2I.y && (num2 < vector2I2.y || rangeVisualizer.AllowLineOfSightInvalidCells) && Grid.TestLineOfSight(vector2I4.x, vector2I4.y, num3, num2, rangeVisualizer.BlockingCb, rangeVisualizer.BlockingTileVisible, rangeVisualizer.AllowLineOfSightInvalidCells);
						pixelData[i * width + j] = (flag ? byte.MaxValue : 0);
						if (flag)
						{
							num++;
						}
					}
				}
			}
			else
			{
				for (int k = 0; k <= rangeMax.y - rangeMin.y; k++)
				{
					int num4 = vector2I4.y + rangeMin.y + k;
					for (int l = 0; l <= rangeMax.x - rangeMin.x; l++)
					{
						int num5 = vector2I4.x + rangeMin.x + l;
						int arg = Grid.XYToCell(num5, num4);
						bool flag2 = num5 > vector2I.x && num5 < vector2I2.x && num4 > vector2I.y && num4 < vector2I2.y && rangeVisualizer.BlockingCb(arg);
						pixelData[k * width + l] = (flag2 ? 0 : byte.MaxValue);
						if (!flag2)
						{
							num++;
						}
					}
				}
			}
			this.OcclusionTex.Apply(false, false);
			Vector2I vector2I5 = rangeMin + vector2I4;
			Vector2I vector2I6 = rangeMax + vector2I4;
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
			vector.x = point.x;
			vector.y = point.y;
			ray = this.myCamera.ViewportPointToRay(Vector3.one);
			distance = Mathf.Abs(ray.origin.z / ray.direction.z);
			point = ray.GetPoint(distance);
			vector.z = point.x - vector.x;
			vector.w = point.y - vector.y;
			this.material.SetVector("_UVOffsetScale", vector);
			Vector4 value;
			value.x = (float)vector2I5.x;
			value.y = (float)vector2I5.y;
			value.z = (float)(vector2I6.x + 1);
			value.w = (float)(vector2I6.y + 1);
			this.material.SetVector("_RangeParams", value);
			this.material.SetColor("_HighlightColor", this.highlightColor);
			Vector4 value2;
			value2.x = 1f / (float)this.OcclusionTex.width;
			value2.y = 1f / (float)this.OcclusionTex.height;
			value2.z = 0f;
			value2.w = 0f;
			this.material.SetVector("_OcclusionParams", value2);
			this.material.SetTexture("_OcclusionTex", this.OcclusionTex);
			Vector4 value3;
			value3.x = (float)Grid.WidthInCells;
			value3.y = (float)Grid.HeightInCells;
			value3.z = 1f / (float)Grid.WidthInCells;
			value3.w = 1f / (float)Grid.HeightInCells;
			this.material.SetVector("_WorldParams", value3);
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.LoadOrtho();
			GL.Begin(5);
			GL.Color(Color.white);
			GL.Vertex3(0f, 0f, 0f);
			GL.Vertex3(0f, 1f, 0f);
			GL.Vertex3(1f, 0f, 0f);
			GL.Vertex3(1f, 1f, 0f);
			GL.End();
			GL.PopMatrix();
			if (this.LastVisibleTileCount != num)
			{
				SoundEvent.PlayOneShot(GlobalAssets.GetSound("RangeVisualization_movement", false), rangeVisualizer.transform.GetPosition(), 1f);
				this.LastVisibleTileCount = num;
			}
		}
	}

	// Token: 0x06004F09 RID: 20233 RVA: 0x001BECDC File Offset: 0x001BCEDC
	private void FindWorldBounds(out Vector2I world_min, out Vector2I world_max)
	{
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			world_min = activeWorld.WorldOffset;
			world_max = activeWorld.WorldOffset + activeWorld.WorldSize;
			return;
		}
		world_min.x = 0;
		world_min.y = 0;
		world_max.x = Grid.WidthInCells;
		world_max.y = Grid.HeightInCells;
	}

	// Token: 0x04003367 RID: 13159
	private Material material;

	// Token: 0x04003368 RID: 13160
	private Camera myCamera;

	// Token: 0x04003369 RID: 13161
	public Color highlightColor = new Color(0f, 1f, 0.8f, 1f);

	// Token: 0x0400336A RID: 13162
	private Texture2D OcclusionTex;

	// Token: 0x0400336B RID: 13163
	private int LastVisibleTileCount;
}
