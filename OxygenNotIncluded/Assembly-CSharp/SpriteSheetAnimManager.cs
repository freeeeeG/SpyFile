using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000509 RID: 1289
[AddComponentMenu("KMonoBehaviour/scripts/SpriteSheetAnimManager")]
public class SpriteSheetAnimManager : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06001E4C RID: 7756 RVA: 0x000A1D14 File Offset: 0x0009FF14
	public static void DestroyInstance()
	{
		SpriteSheetAnimManager.instance = null;
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x000A1D1C File Offset: 0x0009FF1C
	protected override void OnPrefabInit()
	{
		SpriteSheetAnimManager.instance = this;
	}

	// Token: 0x06001E4E RID: 7758 RVA: 0x000A1D24 File Offset: 0x0009FF24
	protected override void OnSpawn()
	{
		for (int i = 0; i < this.sheets.Length; i++)
		{
			int key = Hash.SDBMLower(this.sheets[i].name);
			this.nameIndexMap[key] = new SpriteSheetAnimator(this.sheets[i]);
		}
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x000A1D78 File Offset: 0x0009FF78
	public void Play(string name, Vector3 pos, Vector2 size, Color32 colour)
	{
		int name_hash = Hash.SDBMLower(name);
		this.Play(name_hash, pos, Quaternion.identity, size, colour);
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x000A1D9C File Offset: 0x0009FF9C
	public void Play(string name, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
	{
		int name_hash = Hash.SDBMLower(name);
		this.Play(name_hash, pos, rotation, size, colour);
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x000A1DBD File Offset: 0x0009FFBD
	public void Play(int name_hash, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
	{
		this.nameIndexMap[name_hash].Play(pos, rotation, size, colour);
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x000A1DDB File Offset: 0x0009FFDB
	public void RenderEveryTick(float dt)
	{
		this.UpdateAnims(dt);
		this.Render();
	}

	// Token: 0x06001E53 RID: 7763 RVA: 0x000A1DEC File Offset: 0x0009FFEC
	public void UpdateAnims(float dt)
	{
		foreach (KeyValuePair<int, SpriteSheetAnimator> keyValuePair in this.nameIndexMap)
		{
			keyValuePair.Value.UpdateAnims(dt);
		}
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x000A1E48 File Offset: 0x000A0048
	public void Render()
	{
		Vector3 zero = Vector3.zero;
		foreach (KeyValuePair<int, SpriteSheetAnimator> keyValuePair in this.nameIndexMap)
		{
			keyValuePair.Value.Render();
		}
	}

	// Token: 0x06001E55 RID: 7765 RVA: 0x000A1EA8 File Offset: 0x000A00A8
	public SpriteSheetAnimator GetSpriteSheetAnimator(HashedString name)
	{
		return this.nameIndexMap[name.HashValue];
	}

	// Token: 0x04001102 RID: 4354
	public const float SECONDS_PER_FRAME = 0.033333335f;

	// Token: 0x04001103 RID: 4355
	[SerializeField]
	private SpriteSheet[] sheets;

	// Token: 0x04001104 RID: 4356
	private Dictionary<int, SpriteSheetAnimator> nameIndexMap = new Dictionary<int, SpriteSheetAnimator>();

	// Token: 0x04001105 RID: 4357
	public static SpriteSheetAnimManager instance;
}
