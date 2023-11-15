using System;
using System.Collections.Generic;
using System.IO;
using Characters;
using GameResources;
using Level;
using UnityEngine;

// Token: 0x0200005D RID: 93
[CreateAssetMenu(fileName = "EnemyTable", menuName = "ScriptableObjects/EnemyTable", order = 1)]
public class EnemyTable : ScriptableObject
{
	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000831A File Offset: 0x0000651A
	public static EnemyTable instance
	{
		get
		{
			if (EnemyTable._instance == null)
			{
				EnemyTable._instance = Resources.Load<EnemyTable>("HardmodeSetting/EnemyTable");
				EnemyTable._instance.Initialize();
			}
			return EnemyTable._instance;
		}
	}

	// Token: 0x060001BA RID: 442 RVA: 0x00008347 File Offset: 0x00006547
	private void Initialize()
	{
		this._common = new Dictionary<Key, Character>();
		this._chapterCommon = new Dictionary<Key, Character>();
		this._enemies = new Dictionary<Key, Character>();
		this.LoadCommon();
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00008370 File Offset: 0x00006570
	public void Dispose()
	{
		this._common.Clear();
		this._chapterCommon.Clear();
		this._enemies.Clear();
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00008394 File Offset: 0x00006594
	public Character Get(Key key)
	{
		Character result;
		if (this._common.TryGetValue(key, out result))
		{
			return result;
		}
		if (this._chapterCommon.TryGetValue(key, out result))
		{
			return result;
		}
		this._enemies.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x000083D8 File Offset: 0x000065D8
	private void LoadCommon()
	{
		Character[] array = CommonResource.LoadAll<Character>("Assets/Enemies/Hardmode/Common", "*.prefab", SearchOption.TopDirectoryOnly);
		for (int i = 0; i < array.Length; i++)
		{
			this._common.Add(array[i].key, array[i]);
		}
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000841C File Offset: 0x0000661C
	private void LoadChapterCommon(Chapter.Type type)
	{
		this._chapterCommon.Clear();
		Chapter.Type type2 = type;
		switch (type)
		{
		case Chapter.Type.HardmodeChapter1:
			type2 = Chapter.Type.Chapter1;
			break;
		case Chapter.Type.HardmodeChapter2:
			type2 = Chapter.Type.Chapter2;
			break;
		case Chapter.Type.HardmodeChapter3:
			type2 = Chapter.Type.Chapter3;
			break;
		case Chapter.Type.HardmodeChapter4:
			type2 = Chapter.Type.Chapter4;
			break;
		case Chapter.Type.HardmodeChapter5:
			type2 = Chapter.Type.Chapter5;
			break;
		}
		Character[] array = CommonResource.LoadAll<Character>("Assets/Enemies/" + type2.ToString(), "*.prefab", SearchOption.TopDirectoryOnly);
		Debug.Log("Assets/Enemies/" + type2.ToString());
		for (int i = 0; i < array.Length; i++)
		{
			if (!this._chapterCommon.ContainsKey(array[i].key))
			{
				this._chapterCommon.Add(array[i].key, array[i]);
			}
		}
	}

	// Token: 0x04000184 RID: 388
	[SerializeField]
	[MinMaxSlider(1f, 10f)]
	private Vector2 _groupB;

	// Token: 0x04000185 RID: 389
	private const string assets = "Assets";

	// Token: 0x04000186 RID: 390
	private const string enemy = "Enemies";

	// Token: 0x04000187 RID: 391
	private const string hardmode = "Hardmode";

	// Token: 0x04000188 RID: 392
	private const string common = "Common";

	// Token: 0x04000189 RID: 393
	private const string normalEnemyPath = "Assets/Enemies";

	// Token: 0x0400018A RID: 394
	private const string hardmodeEnemyPath = "Assets/Enemies/Hardmode";

	// Token: 0x0400018B RID: 395
	private IDictionary<Key, Character> _common;

	// Token: 0x0400018C RID: 396
	private IDictionary<Key, Character> _chapterCommon;

	// Token: 0x0400018D RID: 397
	private IDictionary<Key, Character> _enemies;

	// Token: 0x0400018E RID: 398
	private static EnemyTable _instance;
}
