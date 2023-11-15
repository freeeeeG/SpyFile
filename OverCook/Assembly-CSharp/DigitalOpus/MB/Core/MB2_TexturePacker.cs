using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200001F RID: 31
	public class MB2_TexturePacker
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00004654 File Offset: 0x00002A54
		private static void printTree(MB2_TexturePacker.Node r, string spc)
		{
			Debug.Log(string.Concat(new object[]
			{
				spc,
				"Nd img=",
				r.img != null,
				" r=",
				r.r
			}));
			if (r.child[0] != null)
			{
				MB2_TexturePacker.printTree(r.child[0], spc + "      ");
			}
			if (r.child[1] != null)
			{
				MB2_TexturePacker.printTree(r.child[1], spc + "      ");
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000046EC File Offset: 0x00002AEC
		private static void flattenTree(MB2_TexturePacker.Node r, List<MB2_TexturePacker.Image> putHere)
		{
			if (r.img != null)
			{
				r.img.x = r.r.x;
				r.img.y = r.r.y;
				putHere.Add(r.img);
			}
			if (r.child[0] != null)
			{
				MB2_TexturePacker.flattenTree(r.child[0], putHere);
			}
			if (r.child[1] != null)
			{
				MB2_TexturePacker.flattenTree(r.child[1], putHere);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004774 File Offset: 0x00002B74
		private static void drawGizmosNode(MB2_TexturePacker.Node r)
		{
			Vector3 size = new Vector3((float)r.r.w, (float)r.r.h, 0f);
			Vector3 center = new Vector3((float)r.r.x + size.x / 2f, (float)(-(float)r.r.y) - size.y / 2f, 0f);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(center, size);
			if (r.img != null)
			{
				Gizmos.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
				size = new Vector3((float)r.img.w, (float)r.img.h, 0f);
				center = new Vector3((float)r.r.x + size.x / 2f, (float)(-(float)r.r.y) - size.y / 2f, 0f);
				Gizmos.DrawCube(center, size);
			}
			if (r.child[0] != null)
			{
				Gizmos.color = Color.red;
				MB2_TexturePacker.drawGizmosNode(r.child[0]);
			}
			if (r.child[1] != null)
			{
				Gizmos.color = Color.green;
				MB2_TexturePacker.drawGizmosNode(r.child[1]);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000048D0 File Offset: 0x00002CD0
		private static Texture2D createFilledTex(Color c, int w, int h)
		{
			Texture2D texture2D = new Texture2D(w, h);
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					texture2D.SetPixel(i, j, c);
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000491C File Offset: 0x00002D1C
		public void DrawGizmos()
		{
			if (this.bestRoot != null)
			{
				MB2_TexturePacker.drawGizmosNode(this.bestRoot.root);
				Gizmos.color = Color.yellow;
				Vector3 size = new Vector3((float)this.bestRoot.outW, (float)(-(float)this.bestRoot.outH), 0f);
				Vector3 center = new Vector3(size.x / 2f, size.y / 2f, 0f);
				Gizmos.DrawWireCube(center, size);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000049A0 File Offset: 0x00002DA0
		private bool ProbeSingleAtlas(MB2_TexturePacker.Image[] imgsToAdd, int idealAtlasW, int idealAtlasH, float imgArea, int maxAtlasDim, MB2_TexturePacker.ProbeResult pr)
		{
			MB2_TexturePacker.Node node = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.maxDim);
			node.r = new MB2_TexturePacker.PixRect(0, 0, idealAtlasW, idealAtlasH);
			for (int i = 0; i < imgsToAdd.Length; i++)
			{
				if (node.Insert(imgsToAdd[i], false) == null)
				{
					return false;
				}
				if (i == imgsToAdd.Length - 1)
				{
					int num = 0;
					int num2 = 0;
					this.GetExtent(node, ref num, ref num2);
					int num3 = num;
					int num4 = num2;
					bool flag;
					float num8;
					float num9;
					if (this.doPowerOfTwoTextures)
					{
						num3 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(num), maxAtlasDim);
						num4 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(num2), maxAtlasDim);
						if (num4 < num3 / 2)
						{
							num4 = num3 / 2;
						}
						if (num3 < num4 / 2)
						{
							num3 = num4 / 2;
						}
						flag = (num <= maxAtlasDim && num2 <= maxAtlasDim);
						float num5 = Mathf.Max(1f, (float)num / (float)maxAtlasDim);
						float num6 = Mathf.Max(1f, (float)num2 / (float)maxAtlasDim);
						float num7 = (float)num3 * num5 * (float)num4 * num6;
						num8 = 1f - (num7 - imgArea) / num7;
						num9 = 1f;
					}
					else
					{
						num8 = 1f - ((float)(num * num2) - imgArea) / (float)(num * num2);
						if (num < num2)
						{
							num9 = (float)num / (float)num2;
						}
						else
						{
							num9 = (float)num2 / (float)num;
						}
						flag = (num <= maxAtlasDim && num2 <= maxAtlasDim);
					}
					pr.Set(num, num2, num3, num4, node, flag, num8, num9);
					if (this.LOG_LEVEL >= MB2_LogLevel.debug)
					{
						MB2_Log.LogDebug(string.Concat(new object[]
						{
							"Probe success efficiency w=",
							num,
							" h=",
							num2,
							" e=",
							num8,
							" sq=",
							num9,
							" fits=",
							flag
						}), new object[0]);
					}
					return true;
				}
			}
			Debug.LogError("Should never get here.");
			return false;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004BA8 File Offset: 0x00002FA8
		private bool ProbeMultiAtlas(MB2_TexturePacker.Image[] imgsToAdd, int idealAtlasW, int idealAtlasH, float imgArea, int maxAtlasDim, MB2_TexturePacker.ProbeResult pr)
		{
			int num = 0;
			MB2_TexturePacker.Node node = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.maxDim);
			node.r = new MB2_TexturePacker.PixRect(0, 0, idealAtlasW, idealAtlasH);
			for (int i = 0; i < imgsToAdd.Length; i++)
			{
				if (node.Insert(imgsToAdd[i], false) == null)
				{
					if (imgsToAdd[i].x > idealAtlasW && imgsToAdd[i].y > idealAtlasH)
					{
						return false;
					}
					MB2_TexturePacker.Node node2 = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.Container);
					node2.r = new MB2_TexturePacker.PixRect(0, 0, node.r.w + idealAtlasW, idealAtlasH);
					MB2_TexturePacker.Node node3 = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.maxDim);
					node3.r = new MB2_TexturePacker.PixRect(node.r.w, 0, idealAtlasW, idealAtlasH);
					node2.child[1] = node3;
					node2.child[0] = node;
					node = node2;
					MB2_TexturePacker.Node node4 = node.Insert(imgsToAdd[i], false);
					num++;
				}
			}
			pr.numAtlases = num;
			pr.root = node;
			pr.totalAtlasArea = (float)(num * maxAtlasDim * maxAtlasDim);
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				MB2_Log.LogDebug(string.Concat(new object[]
				{
					"Probe success efficiency numAtlases=",
					num,
					" totalArea=",
					pr.totalAtlasArea
				}), new object[0]);
			}
			return true;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004CE8 File Offset: 0x000030E8
		private void GetExtent(MB2_TexturePacker.Node r, ref int x, ref int y)
		{
			if (r.img != null)
			{
				if (r.r.x + r.img.w > x)
				{
					x = r.r.x + r.img.w;
				}
				if (r.r.y + r.img.h > y)
				{
					y = r.r.y + r.img.h;
				}
			}
			if (r.child[0] != null)
			{
				this.GetExtent(r.child[0], ref x, ref y);
			}
			if (r.child[1] != null)
			{
				this.GetExtent(r.child[1], ref x, ref y);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004DA8 File Offset: 0x000031A8
		private int StepWidthHeight(int oldVal, int step, int maxDim)
		{
			if (this.doPowerOfTwoTextures && oldVal < maxDim)
			{
				return oldVal * 2;
			}
			int num = oldVal + step;
			if (num > maxDim && oldVal < maxDim)
			{
				num = maxDim;
			}
			return num;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004DE0 File Offset: 0x000031E0
		public static int RoundToNearestPositivePowerOfTwo(int x)
		{
			int num = (int)Mathf.Pow(2f, (float)Mathf.RoundToInt(Mathf.Log((float)x) / Mathf.Log(2f)));
			if (num == 0 || num == 1)
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004E24 File Offset: 0x00003224
		public static int CeilToNearestPowerOfTwo(int x)
		{
			int num = (int)Mathf.Pow(2f, Mathf.Ceil(Mathf.Log((float)x) / Mathf.Log(2f)));
			if (num == 0 || num == 1)
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004E64 File Offset: 0x00003264
		public AtlasPackingResult[] GetRects(List<Vector2> imgWidthHeights, int maxDimension, int padding)
		{
			return this.GetRects(imgWidthHeights, maxDimension, padding, false);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004E70 File Offset: 0x00003270
		public AtlasPackingResult[] GetRects(List<Vector2> imgWidthHeights, int maxDimension, int padding, bool doMultiAtlas)
		{
			if (doMultiAtlas)
			{
				return this._GetRectsMultiAtlas(imgWidthHeights, maxDimension, padding, 2 + padding * 2, 2 + padding * 2, 2 + padding * 2, 2 + padding * 2);
			}
			AtlasPackingResult atlasPackingResult = this._GetRectsSingleAtlas(imgWidthHeights, maxDimension, padding, 2 + padding * 2, 2 + padding * 2, 2 + padding * 2, 2 + padding * 2, 0);
			if (atlasPackingResult == null)
			{
				return null;
			}
			return new AtlasPackingResult[]
			{
				atlasPackingResult
			};
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004ED4 File Offset: 0x000032D4
		private AtlasPackingResult _GetRectsSingleAtlas(List<Vector2> imgWidthHeights, int maxDimension, int padding, int minImageSizeX, int minImageSizeY, int masterImageSizeX, int masterImageSizeY, int recursionDepth)
		{
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				Debug.Log(string.Format("_GetRects numImages={0}, maxDimension={1}, padding={2}, minImageSizeX={3}, minImageSizeY={4}, masterImageSizeX={5}, masterImageSizeY={6}, recursionDepth={7}", new object[]
				{
					imgWidthHeights.Count,
					maxDimension,
					padding,
					minImageSizeX,
					minImageSizeY,
					masterImageSizeX,
					masterImageSizeY,
					recursionDepth
				}));
			}
			if (recursionDepth > 10)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.error)
				{
					Debug.LogError("Maximum recursion depth reached. Couldn't find packing for these textures.");
				}
				return null;
			}
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			MB2_TexturePacker.Image[] array = new MB2_TexturePacker.Image[imgWidthHeights.Count];
			for (int i = 0; i < array.Length; i++)
			{
				int tw = (int)imgWidthHeights[i].x;
				int th = (int)imgWidthHeights[i].y;
				MB2_TexturePacker.Image image = array[i] = new MB2_TexturePacker.Image(i, tw, th, padding, minImageSizeX, minImageSizeY);
				num += (float)(image.w * image.h);
				num2 = Mathf.Max(num2, image.w);
				num3 = Mathf.Max(num3, image.h);
			}
			if ((float)num3 / (float)num2 > 2f)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug("Using height Comparer", new object[0]);
				}
				Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageHeightComparer());
			}
			else if ((double)((float)num3 / (float)num2) < 0.5)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug("Using width Comparer", new object[0]);
				}
				Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageWidthComparer());
			}
			else
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug("Using area Comparer", new object[0]);
				}
				Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageAreaComparer());
			}
			int num4 = (int)Mathf.Sqrt(num);
			int num6;
			int num5;
			if (this.doPowerOfTwoTextures)
			{
				num5 = (num6 = MB2_TexturePacker.RoundToNearestPositivePowerOfTwo(num4));
				if (num2 > num6)
				{
					num6 = MB2_TexturePacker.CeilToNearestPowerOfTwo(num6);
				}
				if (num3 > num5)
				{
					num5 = MB2_TexturePacker.CeilToNearestPowerOfTwo(num5);
				}
			}
			else
			{
				num6 = num4;
				num5 = num4;
				if (num2 > num4)
				{
					num6 = num2;
					num5 = Mathf.Max(Mathf.CeilToInt(num / (float)num2), num3);
				}
				if (num3 > num4)
				{
					num6 = Mathf.Max(Mathf.CeilToInt(num / (float)num3), num2);
					num5 = num3;
				}
			}
			if (num6 == 0)
			{
				num6 = 4;
			}
			if (num5 == 0)
			{
				num5 = 4;
			}
			int num7 = (int)((float)num6 * 0.15f);
			int num8 = (int)((float)num5 * 0.15f);
			if (num7 == 0)
			{
				num7 = 1;
			}
			if (num8 == 0)
			{
				num8 = 1;
			}
			int num9 = 2;
			int num10 = num5;
			while (num9 >= 1 && num10 < num4 * 1000)
			{
				bool flag = false;
				num9 = 0;
				int num11 = num6;
				while (!flag && num11 < num4 * 1000)
				{
					MB2_TexturePacker.ProbeResult probeResult = new MB2_TexturePacker.ProbeResult();
					if (this.LOG_LEVEL >= MB2_LogLevel.trace)
					{
						Debug.Log(string.Concat(new object[]
						{
							"Probing h=",
							num10,
							" w=",
							num11
						}));
					}
					if (this.ProbeSingleAtlas(array, num11, num10, num, maxDimension, probeResult))
					{
						flag = true;
						if (this.bestRoot == null)
						{
							this.bestRoot = probeResult;
						}
						else if (probeResult.GetScore(this.doPowerOfTwoTextures) > this.bestRoot.GetScore(this.doPowerOfTwoTextures))
						{
							this.bestRoot = probeResult;
						}
					}
					else
					{
						num9++;
						num11 = this.StepWidthHeight(num11, num7, maxDimension);
						if (this.LOG_LEVEL >= MB2_LogLevel.trace)
						{
							MB2_Log.LogDebug(string.Concat(new object[]
							{
								"increasing Width h=",
								num10,
								" w=",
								num11
							}), new object[0]);
						}
					}
				}
				num10 = this.StepWidthHeight(num10, num8, maxDimension);
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug(string.Concat(new object[]
					{
						"increasing Height h=",
						num10,
						" w=",
						num11
					}), new object[0]);
				}
			}
			if (this.bestRoot == null)
			{
				return null;
			}
			int num12;
			int num13;
			if (this.doPowerOfTwoTextures)
			{
				num12 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(this.bestRoot.w), maxDimension);
				num13 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(this.bestRoot.h), maxDimension);
				if (num13 < num12 / 2)
				{
					num13 = num12 / 2;
				}
				if (num12 < num13 / 2)
				{
					num12 = num13 / 2;
				}
			}
			else
			{
				num12 = Mathf.Min(this.bestRoot.w, maxDimension);
				num13 = Mathf.Min(this.bestRoot.h, maxDimension);
			}
			this.bestRoot.outW = num12;
			this.bestRoot.outH = num13;
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Best fit found: atlasW=",
					num12,
					" atlasH",
					num13,
					" w=",
					this.bestRoot.w,
					" h=",
					this.bestRoot.h,
					" efficiency=",
					this.bestRoot.efficiency,
					" squareness=",
					this.bestRoot.squareness,
					" fits in max dimension=",
					this.bestRoot.largerOrEqualToMaxDim
				}));
			}
			List<MB2_TexturePacker.Image> list = new List<MB2_TexturePacker.Image>();
			MB2_TexturePacker.flattenTree(this.bestRoot.root, list);
			list.Sort(new MB2_TexturePacker.ImgIDComparer());
			AtlasPackingResult result = this.ScaleAtlasToFitMaxDim(this.bestRoot, imgWidthHeights, list, maxDimension, padding, minImageSizeX, minImageSizeY, masterImageSizeX, masterImageSizeY, num12, num13, recursionDepth);
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				MB2_Log.LogDebug(string.Format("Done GetRects atlasW={0} atlasH={1}", this.bestRoot.w, this.bestRoot.h), new object[0]);
			}
			return result;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005530 File Offset: 0x00003930
		private AtlasPackingResult ScaleAtlasToFitMaxDim(MB2_TexturePacker.ProbeResult root, List<Vector2> imgWidthHeights, List<MB2_TexturePacker.Image> images, int maxDimension, int padding, int minImageSizeX, int minImageSizeY, int masterImageSizeX, int masterImageSizeY, int outW, int outH, int recursionDepth)
		{
			int minImageSizeX2 = minImageSizeX;
			int minImageSizeY2 = minImageSizeY;
			bool flag = false;
			float num = (float)padding / (float)outW;
			if (root.w > maxDimension)
			{
				num = (float)padding / (float)maxDimension;
				float num2 = (float)maxDimension / (float)root.w;
				if (this.LOG_LEVEL >= MB2_LogLevel.warn)
				{
					Debug.LogWarning("Packing exceeded atlas width shrinking to " + num2);
				}
				for (int i = 0; i < images.Count; i++)
				{
					MB2_TexturePacker.Image image = images[i];
					if ((float)image.w * num2 < (float)masterImageSizeX)
					{
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							Debug.Log("Small images are being scaled to zero. Will need to redo packing with larger minTexSizeX.");
						}
						flag = true;
						minImageSizeX2 = Mathf.CeilToInt((float)minImageSizeX / num2);
					}
					int num3 = (int)((float)(image.x + image.w) * num2);
					image.x = (int)(num2 * (float)image.x);
					image.w = num3 - image.x;
				}
				outW = maxDimension;
			}
			float num4 = (float)padding / (float)outH;
			if (root.h > maxDimension)
			{
				num4 = (float)padding / (float)maxDimension;
				float num5 = (float)maxDimension / (float)root.h;
				if (this.LOG_LEVEL >= MB2_LogLevel.warn)
				{
					Debug.LogWarning("Packing exceeded atlas height shrinking to " + num5);
				}
				for (int j = 0; j < images.Count; j++)
				{
					MB2_TexturePacker.Image image2 = images[j];
					if ((float)image2.h * num5 < (float)masterImageSizeY)
					{
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							Debug.Log("Small images are being scaled to zero. Will need to redo packing with larger minTexSizeY.");
						}
						flag = true;
						minImageSizeY2 = Mathf.CeilToInt((float)minImageSizeY / num5);
					}
					int num6 = (int)((float)(image2.y + image2.h) * num5);
					image2.y = (int)(num5 * (float)image2.y);
					image2.h = num6 - image2.y;
				}
				outH = maxDimension;
			}
			if (!flag)
			{
				AtlasPackingResult atlasPackingResult = new AtlasPackingResult();
				atlasPackingResult.rects = new Rect[images.Count];
				atlasPackingResult.srcImgIdxs = new int[images.Count];
				atlasPackingResult.atlasX = outW;
				atlasPackingResult.atlasY = outH;
				atlasPackingResult.usedW = -1;
				atlasPackingResult.usedH = -1;
				for (int k = 0; k < images.Count; k++)
				{
					MB2_TexturePacker.Image image3 = images[k];
					Rect rect = atlasPackingResult.rects[k] = new Rect((float)image3.x / (float)outW + num, (float)image3.y / (float)outH + num4, (float)image3.w / (float)outW - num * 2f, (float)image3.h / (float)outH - num4 * 2f);
					atlasPackingResult.srcImgIdxs[k] = image3.imgId;
					if (this.LOG_LEVEL >= MB2_LogLevel.debug)
					{
						MB2_Log.LogDebug(string.Concat(new object[]
						{
							"Image: ",
							k,
							" imgID=",
							image3.imgId,
							" x=",
							rect.x * (float)outW,
							" y=",
							rect.y * (float)outH,
							" w=",
							rect.width * (float)outW,
							" h=",
							rect.height * (float)outH,
							" padding=",
							padding
						}), new object[0]);
					}
				}
				return atlasPackingResult;
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				Debug.Log("==================== REDOING PACKING ================");
			}
			root = null;
			return this._GetRectsSingleAtlas(imgWidthHeights, maxDimension, padding, minImageSizeX2, minImageSizeY2, masterImageSizeX, masterImageSizeY, recursionDepth + 1);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005904 File Offset: 0x00003D04
		private AtlasPackingResult[] _GetRectsMultiAtlas(List<Vector2> imgWidthHeights, int maxDimensionPassed, int padding, int minImageSizeX, int minImageSizeY, int masterImageSizeX, int masterImageSizeY)
		{
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				Debug.Log(string.Format("_GetRects numImages={0}, maxDimension={1}, padding={2}, minImageSizeX={3}, minImageSizeY={4}, masterImageSizeX={5}, masterImageSizeY={6}", new object[]
				{
					imgWidthHeights.Count,
					maxDimensionPassed,
					padding,
					minImageSizeX,
					minImageSizeY,
					masterImageSizeX,
					masterImageSizeY
				}));
			}
			float num = 0f;
			int a = 0;
			int a2 = 0;
			MB2_TexturePacker.Image[] array = new MB2_TexturePacker.Image[imgWidthHeights.Count];
			int num2 = maxDimensionPassed;
			if (this.doPowerOfTwoTextures)
			{
				num2 = MB2_TexturePacker.RoundToNearestPositivePowerOfTwo(num2);
			}
			for (int i = 0; i < array.Length; i++)
			{
				int num3 = (int)imgWidthHeights[i].x;
				int num4 = (int)imgWidthHeights[i].y;
				num3 = Mathf.Min(num3, num2 - padding * 2);
				num4 = Mathf.Min(num4, num2 - padding * 2);
				MB2_TexturePacker.Image image = array[i] = new MB2_TexturePacker.Image(i, num3, num4, padding, minImageSizeX, minImageSizeY);
				num += (float)(image.w * image.h);
				a = Mathf.Max(a, image.w);
				a2 = Mathf.Max(a2, image.h);
			}
			int num5;
			int num6;
			if (this.doPowerOfTwoTextures)
			{
				num5 = MB2_TexturePacker.RoundToNearestPositivePowerOfTwo(num2);
				num6 = MB2_TexturePacker.RoundToNearestPositivePowerOfTwo(num2);
			}
			else
			{
				num5 = num2;
				num6 = num2;
			}
			if (num6 == 0)
			{
				num6 = 4;
			}
			if (num5 == 0)
			{
				num5 = 4;
			}
			MB2_TexturePacker.ProbeResult probeResult = new MB2_TexturePacker.ProbeResult();
			Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageHeightComparer());
			if (this.ProbeMultiAtlas(array, num6, num5, num, num2, probeResult))
			{
				this.bestRoot = probeResult;
			}
			Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageWidthComparer());
			if (this.ProbeMultiAtlas(array, num6, num5, num, num2, probeResult) && probeResult.totalAtlasArea < this.bestRoot.totalAtlasArea)
			{
				this.bestRoot = probeResult;
			}
			Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageAreaComparer());
			if (this.ProbeMultiAtlas(array, num6, num5, num, num2, probeResult) && probeResult.totalAtlasArea < this.bestRoot.totalAtlasArea)
			{
				this.bestRoot = probeResult;
			}
			if (this.bestRoot == null)
			{
				return null;
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Best fit found: w=",
					this.bestRoot.w,
					" h=",
					this.bestRoot.h,
					" efficiency=",
					this.bestRoot.efficiency,
					" squareness=",
					this.bestRoot.squareness,
					" fits in max dimension=",
					this.bestRoot.largerOrEqualToMaxDim
				}));
			}
			List<AtlasPackingResult> list = new List<AtlasPackingResult>();
			List<MB2_TexturePacker.Node> list2 = new List<MB2_TexturePacker.Node>();
			Stack<MB2_TexturePacker.Node> stack = new Stack<MB2_TexturePacker.Node>();
			for (MB2_TexturePacker.Node node = this.bestRoot.root; node != null; node = node.child[0])
			{
				stack.Push(node);
			}
			while (stack.Count > 0)
			{
				MB2_TexturePacker.Node node = stack.Pop();
				if (node.isFullAtlas == MB2_TexturePacker.NodeType.maxDim)
				{
					list2.Add(node);
				}
				if (node.child[1] != null)
				{
					for (node = node.child[1]; node != null; node = node.child[0])
					{
						stack.Push(node);
					}
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				List<MB2_TexturePacker.Image> list3 = new List<MB2_TexturePacker.Image>();
				MB2_TexturePacker.flattenTree(list2[j], list3);
				Rect[] array2 = new Rect[list3.Count];
				int[] array3 = new int[list3.Count];
				for (int k = 0; k < list3.Count; k++)
				{
					array2[k] = new Rect((float)(list3[k].x - list2[j].r.x), (float)list3[k].y, (float)list3[k].w, (float)list3[k].h);
					array3[k] = list3[k].imgId;
				}
				AtlasPackingResult atlasPackingResult = new AtlasPackingResult();
				this.GetExtent(list2[j], ref atlasPackingResult.usedW, ref atlasPackingResult.usedH);
				atlasPackingResult.usedW -= list2[j].r.x;
				int num7 = list2[j].r.w;
				int num8 = list2[j].r.h;
				if (this.doPowerOfTwoTextures)
				{
					num7 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(atlasPackingResult.usedW), list2[j].r.w);
					num8 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(atlasPackingResult.usedH), list2[j].r.h);
					if (num8 < num7 / 2)
					{
						num8 = num7 / 2;
					}
					if (num7 < num8 / 2)
					{
						num7 = num8 / 2;
					}
				}
				else
				{
					num7 = atlasPackingResult.usedW;
					num8 = atlasPackingResult.usedH;
				}
				atlasPackingResult.atlasY = num8;
				atlasPackingResult.atlasX = num7;
				atlasPackingResult.rects = array2;
				atlasPackingResult.srcImgIdxs = array3;
				list.Add(atlasPackingResult);
				this.normalizeRects(atlasPackingResult, padding);
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug(string.Format("Done GetRects ", new object[0]), new object[0]);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005EE0 File Offset: 0x000042E0
		private void normalizeRects(AtlasPackingResult rr, int padding)
		{
			for (int i = 0; i < rr.rects.Length; i++)
			{
				rr.rects[i].x = (rr.rects[i].x + (float)padding) / (float)rr.atlasX;
				rr.rects[i].y = (rr.rects[i].y + (float)padding) / (float)rr.atlasY;
				rr.rects[i].width = (rr.rects[i].width - (float)(padding * 2)) / (float)rr.atlasX;
				rr.rects[i].height = (rr.rects[i].height - (float)(padding * 2)) / (float)rr.atlasY;
			}
		}

		// Token: 0x04000064 RID: 100
		public MB2_LogLevel LOG_LEVEL = MB2_LogLevel.info;

		// Token: 0x04000065 RID: 101
		private MB2_TexturePacker.ProbeResult bestRoot;

		// Token: 0x04000066 RID: 102
		public int atlasY;

		// Token: 0x04000067 RID: 103
		public bool doPowerOfTwoTextures = true;

		// Token: 0x02000020 RID: 32
		private enum NodeType
		{
			// Token: 0x04000069 RID: 105
			Container,
			// Token: 0x0400006A RID: 106
			maxDim,
			// Token: 0x0400006B RID: 107
			regular
		}

		// Token: 0x02000021 RID: 33
		private class PixRect
		{
			// Token: 0x06000096 RID: 150 RVA: 0x00005FBE File Offset: 0x000043BE
			public PixRect()
			{
			}

			// Token: 0x06000097 RID: 151 RVA: 0x00005FC6 File Offset: 0x000043C6
			public PixRect(int xx, int yy, int ww, int hh)
			{
				this.x = xx;
				this.y = yy;
				this.w = ww;
				this.h = hh;
			}

			// Token: 0x06000098 RID: 152 RVA: 0x00005FEC File Offset: 0x000043EC
			public override string ToString()
			{
				return string.Format("x={0},y={1},w={2},h={3}", new object[]
				{
					this.x,
					this.y,
					this.w,
					this.h
				});
			}

			// Token: 0x0400006C RID: 108
			public int x;

			// Token: 0x0400006D RID: 109
			public int y;

			// Token: 0x0400006E RID: 110
			public int w;

			// Token: 0x0400006F RID: 111
			public int h;
		}

		// Token: 0x02000022 RID: 34
		private class Image
		{
			// Token: 0x06000099 RID: 153 RVA: 0x00006041 File Offset: 0x00004441
			public Image(int id, int tw, int th, int padding, int minImageSizeX, int minImageSizeY)
			{
				this.imgId = id;
				this.w = Mathf.Max(tw + padding * 2, minImageSizeX);
				this.h = Mathf.Max(th + padding * 2, minImageSizeY);
			}

			// Token: 0x0600009A RID: 154 RVA: 0x00006078 File Offset: 0x00004478
			public Image(MB2_TexturePacker.Image im)
			{
				this.imgId = im.imgId;
				this.w = im.w;
				this.h = im.h;
				this.x = im.x;
				this.y = im.y;
			}

			// Token: 0x04000070 RID: 112
			public int imgId;

			// Token: 0x04000071 RID: 113
			public int w;

			// Token: 0x04000072 RID: 114
			public int h;

			// Token: 0x04000073 RID: 115
			public int x;

			// Token: 0x04000074 RID: 116
			public int y;
		}

		// Token: 0x02000023 RID: 35
		private class ImgIDComparer : IComparer<MB2_TexturePacker.Image>
		{
			// Token: 0x0600009C RID: 156 RVA: 0x000060CF File Offset: 0x000044CF
			public int Compare(MB2_TexturePacker.Image x, MB2_TexturePacker.Image y)
			{
				if (x.imgId > y.imgId)
				{
					return 1;
				}
				if (x.imgId == y.imgId)
				{
					return 0;
				}
				return -1;
			}
		}

		// Token: 0x02000024 RID: 36
		private class ImageHeightComparer : IComparer<MB2_TexturePacker.Image>
		{
			// Token: 0x0600009E RID: 158 RVA: 0x00006100 File Offset: 0x00004500
			public int Compare(MB2_TexturePacker.Image x, MB2_TexturePacker.Image y)
			{
				if (x.h > y.h)
				{
					return -1;
				}
				if (x.h == y.h)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x02000025 RID: 37
		private class ImageWidthComparer : IComparer<MB2_TexturePacker.Image>
		{
			// Token: 0x060000A0 RID: 160 RVA: 0x00006131 File Offset: 0x00004531
			public int Compare(MB2_TexturePacker.Image x, MB2_TexturePacker.Image y)
			{
				if (x.w > y.w)
				{
					return -1;
				}
				if (x.w == y.w)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x02000026 RID: 38
		private class ImageAreaComparer : IComparer<MB2_TexturePacker.Image>
		{
			// Token: 0x060000A2 RID: 162 RVA: 0x00006164 File Offset: 0x00004564
			public int Compare(MB2_TexturePacker.Image x, MB2_TexturePacker.Image y)
			{
				int num = x.w * x.h;
				int num2 = y.w * y.h;
				if (num > num2)
				{
					return -1;
				}
				if (num == num2)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x02000027 RID: 39
		private class ProbeResult
		{
			// Token: 0x060000A4 RID: 164 RVA: 0x000061A8 File Offset: 0x000045A8
			public void Set(int ww, int hh, int outw, int outh, MB2_TexturePacker.Node r, bool fits, float e, float sq)
			{
				this.w = ww;
				this.h = hh;
				this.outW = outw;
				this.outH = outh;
				this.root = r;
				this.largerOrEqualToMaxDim = fits;
				this.efficiency = e;
				this.squareness = sq;
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x000061E8 File Offset: 0x000045E8
			public float GetScore(bool doPowerOfTwoScore)
			{
				float num = (!this.largerOrEqualToMaxDim) ? 0f : 1f;
				if (doPowerOfTwoScore)
				{
					return num * 2f + this.efficiency;
				}
				return this.squareness + 2f * this.efficiency + num;
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x0000623A File Offset: 0x0000463A
			public void PrintTree()
			{
				MB2_TexturePacker.printTree(this.root, "  ");
			}

			// Token: 0x04000075 RID: 117
			public int w;

			// Token: 0x04000076 RID: 118
			public int h;

			// Token: 0x04000077 RID: 119
			public int outW;

			// Token: 0x04000078 RID: 120
			public int outH;

			// Token: 0x04000079 RID: 121
			public MB2_TexturePacker.Node root;

			// Token: 0x0400007A RID: 122
			public bool largerOrEqualToMaxDim;

			// Token: 0x0400007B RID: 123
			public float efficiency;

			// Token: 0x0400007C RID: 124
			public float squareness;

			// Token: 0x0400007D RID: 125
			public float totalAtlasArea;

			// Token: 0x0400007E RID: 126
			public int numAtlases;
		}

		// Token: 0x02000028 RID: 40
		private class Node
		{
			// Token: 0x060000A7 RID: 167 RVA: 0x0000624C File Offset: 0x0000464C
			public Node(MB2_TexturePacker.NodeType rootType)
			{
				this.isFullAtlas = rootType;
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x00006267 File Offset: 0x00004667
			private bool isLeaf()
			{
				return this.child[0] == null || this.child[1] == null;
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x00006288 File Offset: 0x00004688
			public MB2_TexturePacker.Node Insert(MB2_TexturePacker.Image im, bool handed)
			{
				int num;
				int num2;
				if (handed)
				{
					num = 0;
					num2 = 1;
				}
				else
				{
					num = 1;
					num2 = 0;
				}
				if (!this.isLeaf())
				{
					MB2_TexturePacker.Node node = this.child[num].Insert(im, handed);
					if (node != null)
					{
						return node;
					}
					return this.child[num2].Insert(im, handed);
				}
				else
				{
					if (this.img != null)
					{
						return null;
					}
					if (this.r.w < im.w || this.r.h < im.h)
					{
						return null;
					}
					if (this.r.w == im.w && this.r.h == im.h)
					{
						this.img = im;
						return this;
					}
					this.child[num] = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.regular);
					this.child[num2] = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.regular);
					int num3 = this.r.w - im.w;
					int num4 = this.r.h - im.h;
					if (num3 > num4)
					{
						this.child[num].r = new MB2_TexturePacker.PixRect(this.r.x, this.r.y, im.w, this.r.h);
						this.child[num2].r = new MB2_TexturePacker.PixRect(this.r.x + im.w, this.r.y, this.r.w - im.w, this.r.h);
					}
					else
					{
						this.child[num].r = new MB2_TexturePacker.PixRect(this.r.x, this.r.y, this.r.w, im.h);
						this.child[num2].r = new MB2_TexturePacker.PixRect(this.r.x, this.r.y + im.h, this.r.w, this.r.h - im.h);
					}
					return this.child[num].Insert(im, handed);
				}
			}

			// Token: 0x0400007F RID: 127
			public MB2_TexturePacker.NodeType isFullAtlas;

			// Token: 0x04000080 RID: 128
			public MB2_TexturePacker.Node[] child = new MB2_TexturePacker.Node[2];

			// Token: 0x04000081 RID: 129
			public MB2_TexturePacker.PixRect r;

			// Token: 0x04000082 RID: 130
			public MB2_TexturePacker.Image img;
		}
	}
}
