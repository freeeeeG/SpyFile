using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class BGMControl : MonoBehaviour
{
	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000094 RID: 148 RVA: 0x00004A06 File Offset: 0x00002C06
	private AudioSource musicPlayer_SlowShow
	{
		get
		{
			return this.musicPlayers[this.currentShowPlayerIndex];
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000095 RID: 149 RVA: 0x00004A15 File Offset: 0x00002C15
	private AudioSource musicPlayer_SlowFade
	{
		get
		{
			return this.musicPlayers[1 - this.currentShowPlayerIndex];
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00004A26 File Offset: 0x00002C26
	private void Awake()
	{
		BGMControl.inst = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00004A3C File Offset: 0x00002C3C
	private void Update()
	{
		if (TempData.inst == null)
		{
			return;
		}
		if (TempData.inst.currentSceneType == EnumSceneType.BATTLE && Battle.inst.SequalWave == 0)
		{
			return;
		}
		this.UpdateVolume();
		this.Update_FindTarget();
		if (this.ifOnSlow)
		{
			this.InSlow_VolumeControl();
			return;
		}
		if (this.timeCountDown_Left > 0f)
		{
			this.Update_TimeCountDown();
			return;
		}
		this.NotInSlow_TargetDetect();
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00004AA8 File Offset: 0x00002CA8
	public void UpdateVolume()
	{
		float num;
		if (!this.ifOnSlow)
		{
			num = 1f;
		}
		else
		{
			num = this.slowTimeCur / this.slowTimeMax;
			if (num > 1f)
			{
				num = 1f;
			}
		}
		float num2 = 1f - num;
		float volume = num2 * this.slowOutVolume * this.totalVolume * (float)Setting.Inst.SetFloat_BGMVolume * this.scaleVolume;
		float volume2 = (1f - num2) * this.slowInVolume * this.totalVolume * (float)Setting.Inst.SetFloat_BGMVolume * this.scaleVolume;
		this.musicPlayer_SlowFade.volume = volume;
		this.musicPlayer_SlowShow.volume = volume2;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00004B50 File Offset: 0x00002D50
	private void InSlow_VolumeControl()
	{
		float num = this.slowTimeCur / this.slowTimeMax;
		if (num > 1f)
		{
			num = 1f;
		}
		if (num < 1f)
		{
			this.slowTimeCur += Time.unscaledDeltaTime;
			return;
		}
		this.ifOnSlow = false;
		this.slowTimeCur = this.slowTimeMax;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00004BA8 File Offset: 0x00002DA8
	private void NotInSlow_TargetDetect()
	{
		if (this.mode_Current == this.mode_Target && this.index_Current == this.index_Target)
		{
			return;
		}
		float time = 0f;
		if (this.musicPlayer_SlowShow.clip != null)
		{
			time = this.musicPlayer_SlowShow.time;
		}
		this.currentShowPlayerIndex = 1 - this.currentShowPlayerIndex;
		this.index_Old = this.index_Current;
		this.mode_Old = this.mode_Current;
		this.index_Current = this.index_Target;
		this.mode_Current = this.mode_Target;
		if (this.mode_Old >= 10 && this.mode_Current < 10)
		{
			time = 0f;
		}
		this.musicPlayer_SlowShow.volume = 0f;
		this.musicPlayer_SlowShow.clip = this.GetClip(this.mode_Current, this.index_Current);
		Debug.Log(this.musicPlayer_SlowShow.clip);
		this.musicPlayer_SlowShow.Play();
		this.musicPlayer_SlowShow.time = time;
		this.slowInVolume = this.GetVolume(this.mode_Current, this.index_Current);
		if (this.GetBool_IfOldExists())
		{
			this.slowOutVolume = this.GetVolume(this.mode_Old, this.index_Old);
		}
		this.ifOnSlow = true;
		this.slowTimeCur = 0f;
		this.timeCountDown_Left = this.timeCountDown_Max;
		this.InSlow_VolumeControl();
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00004CFF File Offset: 0x00002EFF
	private bool GetBool_IfOldExists()
	{
		return this.mode_Old >= 0 && this.index_Old >= 0;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00004D18 File Offset: 0x00002F18
	private void Update_FindTarget()
	{
		if (TempData.inst.currentSceneType == EnumSceneType.MAINMENU)
		{
			if (this.mode_Current < 10)
			{
				this.SetTargetModeAndIndex(10, 0, 2);
				this.coroutine = base.StartCoroutine(this.MainMenuMusicManager());
				return;
			}
			return;
		}
		else
		{
			if (this.coroutine != null)
			{
				base.StopCoroutine(this.coroutine);
			}
			int num = this.mode_Current;
			int num2 = this.index_Current;
			int forceLevel = 0;
			switch (TempData.inst.modeType)
			{
			case EnumModeType.NORMAL:
				num = Mathf.FloorToInt((float)(Battle.inst.SequalWave - 1) / 5f);
				break;
			case EnumModeType.ENDLESS:
				num = Mathf.FloorToInt((float)(Battle.inst.SequalWave - 1) / 5f);
				break;
			case EnumModeType.WANDER:
				num = Mathf.FloorToInt((float)(Battle.inst.SequalWave - 1) / 5f);
				break;
			}
			num %= 4;
			BattleClipSet battleClipSet = this.battleClipSets[num];
			if (BattleManager.inst.timeStage == EnumTimeStage.REST)
			{
				if (BattleManager.inst.ifGameOver)
				{
					num2 = battleClipSet.index_AttackSpecial_GameOver;
					forceLevel = 1;
				}
				else if (BattleManager.inst.wander_On)
				{
					num2 = battleClipSet.index_Attack1_Enter;
					forceLevel = 1;
				}
				else if (this.mode_Current >= 10)
				{
					num2 = battleClipSet.index_Prepare1_Enter;
					forceLevel = 2;
				}
				else
				{
					num2 = battleClipSet.index_Prepare2_Normal;
					if (this.index_Current > num2)
					{
						forceLevel = 1;
					}
				}
				this.SetTargetModeAndIndex(num, num2, forceLevel);
				return;
			}
			float float_TimePct = BattleManager.inst.GetFloat_TimePct();
			if (BattleManager.inst.ifGameOver)
			{
				num2 = battleClipSet.index_AttackSpecial_GameOver;
				forceLevel = 1;
			}
			else if (BattleManager.inst.GetBool_IfOnBossFight())
			{
				if (this.index_Current <= battleClipSet.index_Attack3_Main)
				{
					int[] array;
					if (Battle.inst.CurrentLevelType == EnumLevelType.FINAL)
					{
						array = battleClipSet.indexs_Boss_Super;
					}
					else
					{
						array = battleClipSet.indexs_Boss_Normal;
					}
					num2 = array[Random.Range(0, array.Length)];
					forceLevel = 1;
				}
				else
				{
					num2 = this.index_Current;
				}
			}
			else if (this.index_Current < battleClipSet.index_Attack1_Enter)
			{
				num2 = battleClipSet.index_Attack1_Enter;
				forceLevel = 1;
			}
			else if (this.index_Current < battleClipSet.index_Attack2_AfterEnter)
			{
				num2 = battleClipSet.index_Attack2_AfterEnter;
			}
			else if (float_TimePct >= battleClipSet.pct_AttackMain)
			{
				num2 = battleClipSet.index_Attack3_Main;
				forceLevel = 1;
			}
			this.SetTargetModeAndIndex(num, num2, forceLevel);
			return;
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00004F2F File Offset: 0x0000312F
	private IEnumerator MainMenuMusicManager()
	{
		float num;
		for (;;)
		{
			if (this.mode_Current == 10 && this.index_Current == 0)
			{
				float time = this.musicPlayer_SlowShow.time;
				num = this.musicPlayer_SlowShow.clip.length - time;
				if (num < 3f)
				{
					break;
				}
			}
			yield return null;
		}
		float time2 = num;
		yield return new WaitForSecondsRealtime(time2);
		if (TempData.inst.currentSceneType != EnumSceneType.MAINMENU)
		{
			Debug.LogError("Error_非主菜单播放主菜单音乐");
		}
		else
		{
			this.mode_Current = 10;
			this.mode_Target = 10;
			this.index_Current = 1;
			this.index_Target = 1;
			this.musicPlayer_SlowShow.clip = this.mainMenuClipSets[1].clip;
			this.musicPlayer_SlowShow.time = 0f;
			this.musicPlayer_SlowShow.Play();
		}
		yield break;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00004F3E File Offset: 0x0000313E
	private AudioClip GetClip(int mode, int index)
	{
		if (mode < 10)
		{
			return this.battleClipSets[mode].clips[index];
		}
		return this.mainMenuClipSets[mode - 10].clip;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00004F65 File Offset: 0x00003165
	private float GetVolume(int mode, int index)
	{
		if (mode < 10)
		{
			return this.battleClipSets[mode].volumes[index];
		}
		return this.mainMenuClipSets[mode - 10].volume;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00004F8C File Offset: 0x0000318C
	private void DebugInts()
	{
		Debug.Log(string.Format("Mode: old {0}  cur {1}  target{2}\nIndex: old {3}  cur {4}  target{5}", new object[]
		{
			this.mode_Old,
			this.mode_Current,
			this.mode_Target,
			this.index_Old,
			this.index_Current,
			this.index_Target
		}));
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00005004 File Offset: 0x00003204
	private void SetTargetModeAndIndex(int mode, int index, int forceLevel)
	{
		if (mode == this.mode_Target && index == this.index_Target)
		{
			return;
		}
		this.mode_Target = mode;
		this.index_Target = index;
		if (forceLevel >= 1)
		{
			this.timeCountDown_Left = 0f;
		}
		if (forceLevel >= 2)
		{
			this.slowTimeCur = this.slowTimeMax;
		}
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00005051 File Offset: 0x00003251
	private void Update_TimeCountDown()
	{
		if (this.timeCountDown_Left > 0f)
		{
			this.timeCountDown_Left -= Time.unscaledDeltaTime;
		}
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00005074 File Offset: 0x00003274
	public void UpdateTimeScale_SetPitch(float speed)
	{
		if (TimeManager.inst && TimeManager.inst.ifPause)
		{
			return;
		}
		float pitch = Mathf.Max(0.81f, Mathf.Sqrt(Mathf.Sqrt(speed)));
		if (TempData.inst.currentSceneType == EnumSceneType.MAINMENU)
		{
			pitch = 1f;
		}
		AudioSource[] array = this.musicPlayers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].pitch = pitch;
		}
		this.UpdateTimeScale_SetVolume(speed);
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x000050E8 File Offset: 0x000032E8
	public void UpdateTimeScale_SetVolume(float speed)
	{
		float num = Mathf.Max(0.81f, Mathf.Sqrt(Mathf.Sqrt(speed)));
		if (TempData.inst.currentSceneType == EnumSceneType.MAINMENU)
		{
			num = 1f;
		}
		else if (TimeManager.inst.ifPause)
		{
			num = 1f;
		}
		this.scaleVolume = num;
	}

	// Token: 0x040000E9 RID: 233
	public static BGMControl inst;

	// Token: 0x040000EA RID: 234
	[SerializeField]
	private float totalVolume = 1f;

	// Token: 0x040000EB RID: 235
	[SerializeField]
	private float scaleVolume = 1f;

	// Token: 0x040000EC RID: 236
	[Header("AudioSource")]
	[SerializeField]
	private AudioSource[] musicPlayers = new AudioSource[2];

	// Token: 0x040000ED RID: 237
	[SerializeField]
	private int currentShowPlayerIndex;

	// Token: 0x040000EE RID: 238
	[Header("SlowChange")]
	[SerializeField]
	private float slowTimeMax = 3f;

	// Token: 0x040000EF RID: 239
	[SerializeField]
	private float slowTimeCur;

	// Token: 0x040000F0 RID: 240
	[SerializeField]
	private bool ifOnSlow;

	// Token: 0x040000F1 RID: 241
	[SerializeField]
	private float slowOutVolume;

	// Token: 0x040000F2 RID: 242
	[SerializeField]
	private float slowInVolume;

	// Token: 0x040000F3 RID: 243
	[Header("TimeCountDown")]
	[SerializeField]
	private float timeCountDown_Max = 3f;

	// Token: 0x040000F4 RID: 244
	[SerializeField]
	private float timeCountDown_Left = 3f;

	// Token: 0x040000F5 RID: 245
	[Header("IndexAndMode")]
	[SerializeField]
	private int index_Current = -1;

	// Token: 0x040000F6 RID: 246
	[SerializeField]
	private int index_Target = -1;

	// Token: 0x040000F7 RID: 247
	[SerializeField]
	private int index_Old = -1;

	// Token: 0x040000F8 RID: 248
	[SerializeField]
	private int mode_Current = -1;

	// Token: 0x040000F9 RID: 249
	[SerializeField]
	private int mode_Target = -1;

	// Token: 0x040000FA RID: 250
	[SerializeField]
	private int mode_Old = -1;

	// Token: 0x040000FB RID: 251
	[Header("Clips")]
	[SerializeField]
	private BattleClipSet[] battleClipSets = new BattleClipSet[1];

	// Token: 0x040000FC RID: 252
	[SerializeField]
	private MainMenuClipSet[] mainMenuClipSets = new MainMenuClipSet[1];

	// Token: 0x040000FD RID: 253
	[Header("MainMenu")]
	[SerializeField]
	private Coroutine coroutine;
}
