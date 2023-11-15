using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000464 RID: 1124
[AddComponentMenu("KMonoBehaviour/scripts/AudioEventManager")]
public class AudioEventManager : KMonoBehaviour
{
	// Token: 0x060018A0 RID: 6304 RVA: 0x00080028 File Offset: 0x0007E228
	public static AudioEventManager Get()
	{
		if (AudioEventManager.instance == null)
		{
			if (App.IsExiting)
			{
				return null;
			}
			GameObject gameObject = GameObject.Find("/AudioEventManager");
			if (gameObject == null)
			{
				gameObject = new GameObject();
				gameObject.name = "AudioEventManager";
			}
			AudioEventManager.instance = gameObject.GetComponent<AudioEventManager>();
			if (AudioEventManager.instance == null)
			{
				AudioEventManager.instance = gameObject.AddComponent<AudioEventManager>();
			}
		}
		return AudioEventManager.instance;
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x00080098 File Offset: 0x0007E298
	protected override void OnSpawn()
	{
		base.OnPrefabInit();
		this.spatialSplats.Reset(Grid.WidthInCells, Grid.HeightInCells, 16, 16);
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x000800B9 File Offset: 0x0007E2B9
	public static float LoudnessToDB(float loudness)
	{
		if (loudness <= 0f)
		{
			return 0f;
		}
		return 10f * Mathf.Log10(loudness);
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x000800D5 File Offset: 0x0007E2D5
	public static float DBToLoudness(float src_db)
	{
		return Mathf.Pow(10f, src_db / 10f);
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x000800E8 File Offset: 0x0007E2E8
	public float GetDecibelsAtCell(int cell)
	{
		return Mathf.Round(AudioEventManager.LoudnessToDB(Grid.Loudness[cell]) * 2f) / 2f;
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x00080108 File Offset: 0x0007E308
	public static string GetLoudestNoisePolluterAtCell(int cell)
	{
		float negativeInfinity = float.NegativeInfinity;
		string result = null;
		AudioEventManager audioEventManager = AudioEventManager.Get();
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2 pos = new Vector2((float)vector2I.x, (float)vector2I.y);
		foreach (object obj in audioEventManager.spatialSplats.GetAllIntersecting(pos))
		{
			NoiseSplat noiseSplat = (NoiseSplat)obj;
			if (noiseSplat.GetLoudness(cell) > negativeInfinity)
			{
				result = noiseSplat.GetProvider().GetName();
			}
		}
		return result;
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x000801AC File Offset: 0x0007E3AC
	public void ClearNoiseSplat(NoiseSplat splat)
	{
		if (this.splats.Contains(splat))
		{
			this.splats.Remove(splat);
			this.spatialSplats.Remove(splat);
		}
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x000801D5 File Offset: 0x0007E3D5
	public void AddSplat(NoiseSplat splat)
	{
		this.splats.Add(splat);
		this.spatialSplats.Add(splat);
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x000801F0 File Offset: 0x0007E3F0
	public NoiseSplat CreateNoiseSplat(Vector2 pos, int dB, int radius, string name, GameObject go)
	{
		Polluter polluter = this.GetPolluter(radius);
		polluter.SetAttributes(pos, dB, go, name);
		NoiseSplat noiseSplat = new NoiseSplat(polluter, 0f);
		polluter.SetSplat(noiseSplat);
		return noiseSplat;
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x00080224 File Offset: 0x0007E424
	public List<AudioEventManager.PolluterDisplay> GetPollutersForCell(int cell)
	{
		this.polluters.Clear();
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2 pos = new Vector2((float)vector2I.x, (float)vector2I.y);
		foreach (object obj in this.spatialSplats.GetAllIntersecting(pos))
		{
			NoiseSplat noiseSplat = (NoiseSplat)obj;
			float loudness = noiseSplat.GetLoudness(cell);
			if (loudness > 0f)
			{
				AudioEventManager.PolluterDisplay item = default(AudioEventManager.PolluterDisplay);
				item.name = noiseSplat.GetName();
				item.value = AudioEventManager.LoudnessToDB(loudness);
				item.provider = noiseSplat.GetProvider();
				this.polluters.Add(item);
			}
		}
		return this.polluters;
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x000802FC File Offset: 0x0007E4FC
	private void RemoveExpiredSplats()
	{
		if (this.removeTime.Count > 1)
		{
			this.removeTime.Sort((Pair<float, NoiseSplat> a, Pair<float, NoiseSplat> b) => a.first.CompareTo(b.first));
		}
		int num = -1;
		int num2 = 0;
		while (num2 < this.removeTime.Count && this.removeTime[num2].first <= Time.time)
		{
			NoiseSplat second = this.removeTime[num2].second;
			if (second != null)
			{
				IPolluter provider = second.GetProvider();
				this.FreePolluter(provider as Polluter);
			}
			num = num2;
			num2++;
		}
		for (int i = num; i >= 0; i--)
		{
			this.removeTime.RemoveAt(i);
		}
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x000803B8 File Offset: 0x0007E5B8
	private void Update()
	{
		this.RemoveExpiredSplats();
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x000803C0 File Offset: 0x0007E5C0
	private Polluter GetPolluter(int radius)
	{
		if (!this.freePool.ContainsKey(radius))
		{
			this.freePool.Add(radius, new List<Polluter>());
		}
		Polluter polluter;
		if (this.freePool[radius].Count > 0)
		{
			polluter = this.freePool[radius][0];
			this.freePool[radius].RemoveAt(0);
		}
		else
		{
			polluter = new Polluter(radius);
		}
		if (!this.inusePool.ContainsKey(radius))
		{
			this.inusePool.Add(radius, new List<Polluter>());
		}
		this.inusePool[radius].Add(polluter);
		return polluter;
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x00080464 File Offset: 0x0007E664
	private void FreePolluter(Polluter pol)
	{
		if (pol != null)
		{
			pol.Clear();
			global::Debug.Assert(this.inusePool[pol.radius].Contains(pol));
			this.inusePool[pol.radius].Remove(pol);
			this.freePool[pol.radius].Add(pol);
		}
	}

	// Token: 0x060018AE RID: 6318 RVA: 0x000804C8 File Offset: 0x0007E6C8
	public void PlayTimedOnceOff(Vector2 pos, int dB, int radius, string name, GameObject go, float time = 1f)
	{
		if (dB > 0 && radius > 0 && time > 0f)
		{
			Polluter polluter = this.GetPolluter(radius);
			polluter.SetAttributes(pos, dB, go, name);
			this.AddTimedInstance(polluter, time);
		}
	}

	// Token: 0x060018AF RID: 6319 RVA: 0x00080504 File Offset: 0x0007E704
	private void AddTimedInstance(Polluter p, float time)
	{
		NoiseSplat noiseSplat = new NoiseSplat(p, time + Time.time);
		p.SetSplat(noiseSplat);
		this.removeTime.Add(new Pair<float, NoiseSplat>(time + Time.time, noiseSplat));
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x0008053E File Offset: 0x0007E73E
	private static void SoundLog(long itemId, string message)
	{
		global::Debug.Log(" [" + itemId.ToString() + "] \t" + message);
	}

	// Token: 0x04000D96 RID: 3478
	public const float NO_NOISE_EFFECTORS = 0f;

	// Token: 0x04000D97 RID: 3479
	public const float MIN_LOUDNESS_THRESHOLD = 1f;

	// Token: 0x04000D98 RID: 3480
	private static AudioEventManager instance;

	// Token: 0x04000D99 RID: 3481
	private List<Pair<float, NoiseSplat>> removeTime = new List<Pair<float, NoiseSplat>>();

	// Token: 0x04000D9A RID: 3482
	private Dictionary<int, List<Polluter>> freePool = new Dictionary<int, List<Polluter>>();

	// Token: 0x04000D9B RID: 3483
	private Dictionary<int, List<Polluter>> inusePool = new Dictionary<int, List<Polluter>>();

	// Token: 0x04000D9C RID: 3484
	private HashSet<NoiseSplat> splats = new HashSet<NoiseSplat>();

	// Token: 0x04000D9D RID: 3485
	private UniformGrid<NoiseSplat> spatialSplats = new UniformGrid<NoiseSplat>();

	// Token: 0x04000D9E RID: 3486
	private List<AudioEventManager.PolluterDisplay> polluters = new List<AudioEventManager.PolluterDisplay>();

	// Token: 0x020010E7 RID: 4327
	public enum NoiseEffect
	{
		// Token: 0x04005A8A RID: 23178
		Peaceful,
		// Token: 0x04005A8B RID: 23179
		Quiet = 36,
		// Token: 0x04005A8C RID: 23180
		TossAndTurn = 45,
		// Token: 0x04005A8D RID: 23181
		WakeUp = 60,
		// Token: 0x04005A8E RID: 23182
		Passive = 80,
		// Token: 0x04005A8F RID: 23183
		Active = 106
	}

	// Token: 0x020010E8 RID: 4328
	public struct PolluterDisplay
	{
		// Token: 0x04005A90 RID: 23184
		public string name;

		// Token: 0x04005A91 RID: 23185
		public float value;

		// Token: 0x04005A92 RID: 23186
		public IPolluter provider;
	}
}
