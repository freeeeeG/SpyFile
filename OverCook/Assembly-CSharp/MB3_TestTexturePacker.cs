using System;
using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class MB3_TestTexturePacker : MonoBehaviour
{
	// Token: 0x06000014 RID: 20 RVA: 0x00002CDC File Offset: 0x000010DC
	[ContextMenu("Generate List Of Images To Add")]
	public void GenerateListOfImagesToAdd()
	{
		this.imgsToAdd = new List<Vector2>();
		for (int i = 0; i < this.numTex; i++)
		{
			Vector2 item = new Vector2((float)Mathf.RoundToInt((float)UnityEngine.Random.Range(this.min, this.max) * this.xMult), (float)Mathf.RoundToInt((float)UnityEngine.Random.Range(this.min, this.max) * this.yMult));
			if (this.imgsMustBePowerOfTwo)
			{
				item.x = (float)MB2_TexturePacker.RoundToNearestPositivePowerOfTwo((int)item.x);
				item.y = (float)MB2_TexturePacker.RoundToNearestPositivePowerOfTwo((int)item.y);
			}
			this.imgsToAdd.Add(item);
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002D94 File Offset: 0x00001194
	[ContextMenu("Run")]
	public void RunTestHarness()
	{
		this.texturePacker = new MB2_TexturePacker();
		this.texturePacker.doPowerOfTwoTextures = this.doPowerOfTwoTextures;
		this.texturePacker.LOG_LEVEL = this.logLevel;
		this.rs = this.texturePacker.GetRects(this.imgsToAdd, this.maxDim, this.padding, this.doMultiAtlas);
		if (this.rs != null)
		{
			Debug.Log("NumAtlas= " + this.rs.Length);
			for (int i = 0; i < this.rs.Length; i++)
			{
				for (int j = 0; j < this.rs[i].rects.Length; j++)
				{
					Rect rect = this.rs[i].rects[j];
					rect.x *= (float)this.rs[i].atlasX;
					rect.y *= (float)this.rs[i].atlasY;
					rect.width *= (float)this.rs[i].atlasX;
					rect.height *= (float)this.rs[i].atlasY;
					Debug.Log(rect.ToString("f5"));
				}
				Debug.Log("===============");
			}
			this.res = string.Concat(new object[]
			{
				"mxX= ",
				this.rs[0].atlasX,
				" mxY= ",
				this.rs[0].atlasY
			});
		}
		else
		{
			this.res = "ERROR: PACKING FAILED";
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002F58 File Offset: 0x00001358
	private void OnDrawGizmos()
	{
		if (this.rs != null)
		{
			for (int i = 0; i < this.rs.Length; i++)
			{
				Vector2 vector = new Vector2((float)i * 1.5f * (float)this.maxDim, 0f);
				AtlasPackingResult atlasPackingResult = this.rs[i];
				Vector2 v = new Vector2(vector.x + (float)(atlasPackingResult.atlasX / 2), vector.y + (float)(atlasPackingResult.atlasY / 2));
				Vector2 v2 = new Vector2((float)atlasPackingResult.atlasX, (float)atlasPackingResult.atlasY);
				Gizmos.DrawWireCube(v, v2);
				for (int j = 0; j < this.rs[i].rects.Length; j++)
				{
					Rect rect = this.rs[i].rects[j];
					Gizmos.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
					v = new Vector2(vector.x + (rect.x + rect.width / 2f) * (float)this.rs[i].atlasX, vector.y + (rect.y + rect.height / 2f) * (float)this.rs[i].atlasY);
					Vector2 v3 = new Vector2(rect.width * (float)this.rs[i].atlasX, rect.height * (float)this.rs[i].atlasY);
					Gizmos.DrawCube(v, v3);
				}
			}
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000030FC File Offset: 0x000014FC
	[ContextMenu("Test1")]
	private void Test1()
	{
		this.texturePacker = new MB2_TexturePacker();
		this.texturePacker.doPowerOfTwoTextures = true;
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(450f, 200f));
		list.Add(new Vector2(450f, 200f));
		list.Add(new Vector2(450f, 80f));
		this.texturePacker.LOG_LEVEL = this.logLevel;
		this.rs = this.texturePacker.GetRects(list, 512, 8, true);
		Debug.Log("Success! ");
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000319C File Offset: 0x0000159C
	[ContextMenu("Test2")]
	private void Test2()
	{
		this.texturePacker = new MB2_TexturePacker();
		this.texturePacker.doPowerOfTwoTextures = true;
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(200f, 450f));
		list.Add(new Vector2(200f, 450f));
		list.Add(new Vector2(80f, 450f));
		this.texturePacker.LOG_LEVEL = this.logLevel;
		this.rs = this.texturePacker.GetRects(list, 512, 8, true);
		Debug.Log("Success! ");
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000323C File Offset: 0x0000163C
	[ContextMenu("Test3")]
	private void Test3()
	{
		this.texturePacker = new MB2_TexturePacker();
		this.texturePacker.doPowerOfTwoTextures = false;
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(450f, 200f));
		list.Add(new Vector2(450f, 200f));
		list.Add(new Vector2(450f, 80f));
		this.texturePacker.LOG_LEVEL = this.logLevel;
		this.rs = this.texturePacker.GetRects(list, 512, 8, true);
		Debug.Log("Success! ");
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000032DC File Offset: 0x000016DC
	[ContextMenu("Test4")]
	private void Test4()
	{
		this.texturePacker = new MB2_TexturePacker();
		this.texturePacker.doPowerOfTwoTextures = false;
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(200f, 450f));
		list.Add(new Vector2(200f, 450f));
		list.Add(new Vector2(80f, 450f));
		this.texturePacker.LOG_LEVEL = this.logLevel;
		this.rs = this.texturePacker.GetRects(list, 512, 8, true);
		Debug.Log("Success! ");
	}

	// Token: 0x04000025 RID: 37
	private MB2_TexturePacker texturePacker;

	// Token: 0x04000026 RID: 38
	public int numTex = 32;

	// Token: 0x04000027 RID: 39
	public int min = 126;

	// Token: 0x04000028 RID: 40
	public int max = 2046;

	// Token: 0x04000029 RID: 41
	public float xMult = 1f;

	// Token: 0x0400002A RID: 42
	public float yMult = 1f;

	// Token: 0x0400002B RID: 43
	public bool imgsMustBePowerOfTwo;

	// Token: 0x0400002C RID: 44
	public List<Vector2> imgsToAdd = new List<Vector2>();

	// Token: 0x0400002D RID: 45
	public int padding = 1;

	// Token: 0x0400002E RID: 46
	public int maxDim = 4096;

	// Token: 0x0400002F RID: 47
	public bool doPowerOfTwoTextures = true;

	// Token: 0x04000030 RID: 48
	public bool doMultiAtlas;

	// Token: 0x04000031 RID: 49
	public MB2_LogLevel logLevel;

	// Token: 0x04000032 RID: 50
	public string res;

	// Token: 0x04000033 RID: 51
	public AtlasPackingResult[] rs;
}
