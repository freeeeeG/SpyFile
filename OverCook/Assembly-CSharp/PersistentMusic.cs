using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000528 RID: 1320
[RequireComponent(typeof(AudioSource))]
public class PersistentMusic : MonoBehaviour
{
	// Token: 0x060018B5 RID: 6325 RVA: 0x0007D7DC File Offset: 0x0007BBDC
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.m_audioSource = base.gameObject.RequireComponent<AudioSource>();
		this.m_audioSource.volume = PersistentMusic.s_volume;
		this.m_alive = true;
		this.m_levelOfOrigin = SceneManager.GetActiveScene().name;
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		for (int i = 0; i < PersistentMusic.s_allmusic.Length; i++)
		{
			PersistentMusic.s_allmusic[i].m_lifeTime += 0.071071f;
		}
		ArrayUtils.PushBack<PersistentMusic>(ref PersistentMusic.s_allmusic, this);
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x0007D87C File Offset: 0x0007BC7C
	private void Start()
	{
		if (this.m_alive)
		{
			for (int i = 0; i < PersistentMusic.s_allmusic.Length; i++)
			{
				if (PersistentMusic.s_allmusic[i] != this)
				{
					PersistentMusic.s_allmusic[i].OnMusicAdded(this);
				}
			}
		}
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x0007D8CB File Offset: 0x0007BCCB
	private void OnDestroy()
	{
		PersistentMusic.s_allmusic = PersistentMusic.s_allmusic.AllRemoved_Predicate(new Predicate<PersistentMusic>(this.Equals));
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x0007D8FC File Offset: 0x0007BCFC
	private void Update()
	{
		if (this.m_dead)
		{
			this.Kill();
		}
		if (this.m_alive && this.m_markedForDeath != PersistentMusic.DeathType.None)
		{
			this.m_alive = false;
			PersistentMusic.DeathType markedForDeath = this.m_markedForDeath;
			if (markedForDeath != PersistentMusic.DeathType.FadeOut)
			{
				if (markedForDeath == PersistentMusic.DeathType.Sudden)
				{
					this.Kill();
				}
			}
			else
			{
				base.StartCoroutine(this.KillMeSoftly());
			}
		}
		this.m_lifeTime += TimeManager.GetDeltaTime(base.gameObject);
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x0007D986 File Offset: 0x0007BD86
	public bool IsAlive()
	{
		return this.m_alive;
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x0007D990 File Offset: 0x0007BD90
	private void OnMusicAdded(PersistentMusic _otherMusic)
	{
		if (!this.m_alive)
		{
			return;
		}
		PersistentMusic.DeathType deathType = this.BattleOfTheBands(_otherMusic, false);
		if (deathType == PersistentMusic.DeathType.None)
		{
			return;
		}
		PersistentMusic.DeathType markedForDeath = this.m_markedForDeath;
		if (markedForDeath == PersistentMusic.DeathType.Sudden)
		{
			return;
		}
		if (markedForDeath == PersistentMusic.DeathType.FadeOut)
		{
			if (deathType == PersistentMusic.DeathType.Sudden)
			{
				this.m_markedForDeath = deathType;
			}
			return;
		}
		if (markedForDeath != PersistentMusic.DeathType.None)
		{
			return;
		}
		this.m_markedForDeath = deathType;
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x0007D9F4 File Offset: 0x0007BDF4
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (!this.m_alive)
		{
			return;
		}
		if (mode == LoadSceneMode.Additive)
		{
			return;
		}
		PersistentMusic[] array = PersistentMusic.s_allmusic.FindAll((PersistentMusic x) => x.IsAlive());
		if (array.Length > 2)
		{
			if (Debug.isDebugBuild)
			{
				string name = SceneManager.GetActiveScene().name;
				string text = "{\n";
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"     name: ",
						base.name,
						" level: ",
						array[i].m_levelOfOrigin,
						" track: ",
						array[i].ClipName(),
						" lifetime: ",
						array[i].m_lifeTime,
						"\n"
					});
				}
				text += "}";
			}
			Array.Sort<PersistentMusic>(array, (PersistentMusic x, PersistentMusic y) => x.m_lifeTime.CompareTo(y.m_lifeTime));
			for (int j = 2; j < array.Length; j++)
			{
				array[j].Kill();
			}
			Array.Resize<PersistentMusic>(ref array, 2);
		}
		PersistentMusic otherMusic = array.TryAtIndex(array.FindIndex_Predicate((PersistentMusic x) => x != this));
		this.m_markedForDeath = this.BattleOfTheBands(otherMusic, true);
		if (this.m_markedForDeath == PersistentMusic.DeathType.None && !this.m_audioSource.isPlaying)
		{
			this.m_audioSource.Play();
		}
	}

	// Token: 0x060018BC RID: 6332 RVA: 0x0007DB94 File Offset: 0x0007BF94
	private PersistentMusic.DeathType BattleOfTheBands(PersistentMusic _otherMusic, bool _newLevel = false)
	{
		if (_newLevel)
		{
			string name = SceneManager.GetActiveScene().name;
			if (this.m_levelOfOrigin != name)
			{
				if (_otherMusic == null || _otherMusic.m_audioSource.clip != this.m_audioSource.clip)
				{
					return PersistentMusic.DeathType.FadeOut;
				}
			}
			else if (_otherMusic != null)
			{
				if (_otherMusic.m_levelOfOrigin == this.m_levelOfOrigin)
				{
					if (this.m_lifeTime <= _otherMusic.m_lifeTime)
					{
						return PersistentMusic.DeathType.Sudden;
					}
				}
				else if (_otherMusic.m_audioSource.clip == this.m_audioSource.clip)
				{
					return PersistentMusic.DeathType.Sudden;
				}
			}
			else if (this.m_lifeTime > 0f)
			{
				return PersistentMusic.DeathType.Sudden;
			}
		}
		else
		{
			if (_otherMusic == null || _otherMusic.m_audioSource.clip != this.m_audioSource.clip)
			{
				return PersistentMusic.DeathType.FadeOut;
			}
			if (this.m_lifeTime <= _otherMusic.m_lifeTime)
			{
				return PersistentMusic.DeathType.Sudden;
			}
		}
		return PersistentMusic.DeathType.None;
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x0007DCB4 File Offset: 0x0007C0B4
	private IEnumerator KillMeSoftly()
	{
		float volumeStart = this.m_audioSource.volume;
		while (this.m_audioSource.volume > 0f)
		{
			yield return null;
			float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
			this.m_audioSource.volume = Mathf.Clamp01(this.m_audioSource.volume - volumeStart / this.m_fadeTime * deltaTime);
		}
		this.Kill();
		yield break;
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x0007DCCF File Offset: 0x0007C0CF
	private void Kill()
	{
		this.m_alive = false;
		this.m_dead = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060018BF RID: 6335 RVA: 0x0007DCEC File Offset: 0x0007C0EC
	private string ClipName()
	{
		if (this.m_audioSource.clip != null)
		{
			return this.m_audioSource.clip.name + this.m_audioSource.playOnAwake + this.m_audioSource.isPlaying;
		}
		return "null";
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x0007DD4A File Offset: 0x0007C14A
	public AudioSource GetAudioSource()
	{
		return this.m_audioSource;
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x0007DD52 File Offset: 0x0007C152
	public void StopMusic(bool _kill)
	{
		this.m_markedForDeath = ((!_kill) ? this.BattleOfTheBands(null, false) : PersistentMusic.DeathType.Sudden);
	}

	// Token: 0x040013DB RID: 5083
	[SerializeField]
	private float m_fadeTime = 2f;

	// Token: 0x040013DC RID: 5084
	private static PersistentMusic[] s_allmusic = new PersistentMusic[0];

	// Token: 0x040013DD RID: 5085
	private static float s_volume = 0.65f;

	// Token: 0x040013DE RID: 5086
	private AudioSource m_audioSource;

	// Token: 0x040013DF RID: 5087
	private string m_levelOfOrigin;

	// Token: 0x040013E0 RID: 5088
	private PersistentMusic.DeathType m_markedForDeath;

	// Token: 0x040013E1 RID: 5089
	private bool m_alive;

	// Token: 0x040013E2 RID: 5090
	private bool m_dead;

	// Token: 0x040013E3 RID: 5091
	private float m_lifeTime;

	// Token: 0x02000529 RID: 1321
	private enum DeathType
	{
		// Token: 0x040013E7 RID: 5095
		None,
		// Token: 0x040013E8 RID: 5096
		Sudden,
		// Token: 0x040013E9 RID: 5097
		FadeOut
	}
}
