using System;
using UnityEngine;

// Token: 0x020003CE RID: 974
[ExecutionDependency(typeof(AudioManager))]
[RequireComponent(typeof(MapAvatarControls), typeof(MapAvatarTransformer))]
public class MapAvatarCosmeticDecisions : MonoBehaviour
{
	// Token: 0x0600120B RID: 4619 RVA: 0x000663F0 File Offset: 0x000647F0
	private void Awake()
	{
		this.m_controls = base.gameObject.RequireComponent<MapAvatarControls>();
		this.m_avatarTransformer = base.gameObject.RequireComponent<MapAvatarTransformer>();
		this.m_audioManager = GameUtils.RequireManager<AudioManager>();
		this.TransitionToVanType(this.m_avatarTransformer.CurrentType);
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x00066430 File Offset: 0x00064830
	private void TransitionToVanType(MapAvatarTransformer.VanType _type)
	{
		this.m_currentVanType = _type;
		if (this.m_sourceTag != GameLoopingAudioTag.COUNT)
		{
			this.m_audioManager.StopAudio(this.m_sourceTag, this.m_audioToken);
			this.m_audioToken = null;
			this.m_source = null;
			this.m_sourceTag = GameLoopingAudioTag.COUNT;
		}
		GameLoopingAudioTag gameLoopingAudioTag = GameLoopingAudioTag.COUNT;
		switch (_type)
		{
		case MapAvatarTransformer.VanType.LAND:
			gameLoopingAudioTag = this.m_engineTags.m_landTag;
			break;
		case MapAvatarTransformer.VanType.WATER:
			gameLoopingAudioTag = this.m_engineTags.m_waterTag;
			break;
		case MapAvatarTransformer.VanType.FLYING:
			gameLoopingAudioTag = this.m_engineTags.m_flyingTag;
			break;
		}
		if (gameLoopingAudioTag != GameLoopingAudioTag.COUNT)
		{
			this.m_audioToken = new MapAvatarCosmeticDecisions.AudioToken(gameLoopingAudioTag, base.gameObject);
			this.m_source = this.m_audioManager.StartAudio(gameLoopingAudioTag, this.m_audioToken, base.gameObject.layer);
			this.m_sourceTag = gameLoopingAudioTag;
		}
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x00066520 File Offset: 0x00064920
	public float GetClampedMovementSpeed()
	{
		return Mathf.Clamp01(this.m_controls.GetUnclampedMovementSpeed());
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x00066534 File Offset: 0x00064934
	private void Update()
	{
		if (this.m_currentVanType != this.m_avatarTransformer.CurrentType)
		{
			this.TransitionToVanType(this.m_avatarTransformer.CurrentType);
		}
		if (this.m_source != null)
		{
			float clampedMovementSpeed = this.GetClampedMovementSpeed();
			float nTargetX = MathUtils.Remap(clampedMovementSpeed, 0f, 1f, this.m_minPitch, this.m_maxPitch);
			float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
			float pitch = this.m_source.pitch;
			MathUtils.AdvanceToTarget_Sinusoidal(ref pitch, ref this.m_pitchGradient, nTargetX, this.m_gradientLimit, this.m_timeToMax, deltaTime);
			this.m_source.pitch = pitch;
		}
	}

	// Token: 0x04000E13 RID: 3603
	[SerializeField]
	private MapAvatarCosmeticDecisions.MapAvatarEngineTags m_engineTags;

	// Token: 0x04000E14 RID: 3604
	[SerializeField]
	private float m_minPitch = 1f;

	// Token: 0x04000E15 RID: 3605
	[SerializeField]
	private float m_maxPitch = 2f;

	// Token: 0x04000E16 RID: 3606
	[SerializeField]
	private float m_gradientLimit = 1f;

	// Token: 0x04000E17 RID: 3607
	[SerializeField]
	private float m_timeToMax = 0.1f;

	// Token: 0x04000E18 RID: 3608
	private MapAvatarControls m_controls;

	// Token: 0x04000E19 RID: 3609
	private MapAvatarTransformer m_avatarTransformer;

	// Token: 0x04000E1A RID: 3610
	private AudioManager m_audioManager;

	// Token: 0x04000E1B RID: 3611
	private AudioSource m_source;

	// Token: 0x04000E1C RID: 3612
	private GameLoopingAudioTag m_sourceTag = GameLoopingAudioTag.COUNT;

	// Token: 0x04000E1D RID: 3613
	private float m_pitchGradient;

	// Token: 0x04000E1E RID: 3614
	private MapAvatarCosmeticDecisions.AudioToken m_audioToken;

	// Token: 0x04000E1F RID: 3615
	private MapAvatarTransformer.VanType m_currentVanType;

	// Token: 0x020003CF RID: 975
	[Serializable]
	public class MapAvatarEngineTags
	{
		// Token: 0x04000E20 RID: 3616
		[SerializeField]
		public GameLoopingAudioTag m_landTag = GameLoopingAudioTag.VanEngine;

		// Token: 0x04000E21 RID: 3617
		[SerializeField]
		public GameLoopingAudioTag m_waterTag = GameLoopingAudioTag.WorldMapBoatEngine;

		// Token: 0x04000E22 RID: 3618
		[SerializeField]
		public GameLoopingAudioTag m_flyingTag = GameLoopingAudioTag.WorldMapPlaneEngine;
	}

	// Token: 0x020003D0 RID: 976
	private class AudioToken
	{
		// Token: 0x06001210 RID: 4624 RVA: 0x000665FC File Offset: 0x000649FC
		public AudioToken(GameLoopingAudioTag _tag, GameObject _gameObject)
		{
			this.m_tag = _tag;
			this.m_gameObject = _gameObject;
		}

		// Token: 0x04000E23 RID: 3619
		public GameLoopingAudioTag m_tag;

		// Token: 0x04000E24 RID: 3620
		public GameObject m_gameObject;
	}
}
