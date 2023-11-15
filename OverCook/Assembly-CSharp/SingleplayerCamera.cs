using System;
using UnityEngine;

// Token: 0x02000445 RID: 1093
[ExecutionDependency(typeof(PlayerControls))]
[ExecutionDependency(typeof(PlayerSwitchingManager))]
public class SingleplayerCamera : MonoBehaviour
{
	// Token: 0x06001426 RID: 5158 RVA: 0x0006E1CB File Offset: 0x0006C5CB
	private void Awake()
	{
		this.m_followCamera = base.gameObject.AddComponent<FollowCamera>();
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x0006E1E0 File Offset: 0x0006C5E0
	private void Start()
	{
		this.m_playerSwitchingManager = GameUtils.RequireManager<PlayerSwitchingManager>();
		this.m_playerSwitchingManager.AvatarSelectChangeCallback += this.OnAvatarSelectionChanged;
		this.m_followCamera.Target = this.m_playerSwitchingManager.SelectedAvatar(PlayerInputLookup.Player.One).gameObject;
		Vector3 b = base.transform.rotation * (-this.m_idealDistance * Vector3.forward);
		this.m_followCamera.IdealOffset = this.m_targetOffset + b;
		this.SetMode(SingleplayerCamera.Mode.Normal);
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x0006E26B File Offset: 0x0006C66B
	private void OnAvatarSelectionChanged(PlayerInputLookup.Player _player, PlayerControls _controls)
	{
		this.m_followCamera.Target = _controls.gameObject;
		this.SetMode(SingleplayerCamera.Mode.Switching);
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x0006E288 File Offset: 0x0006C688
	private void SetMode(SingleplayerCamera.Mode _mode)
	{
		if (_mode != SingleplayerCamera.Mode.Normal)
		{
			if (_mode == SingleplayerCamera.Mode.Switching)
			{
				this.m_followCamera.GradientLimit = this.m_switchFollowSpeed;
				this.m_followCamera.TimeToMax = this.m_switchAccelTime;
			}
		}
		else
		{
			this.UpdateNormalMode();
		}
		this.m_mode = _mode;
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x0006E2E0 File Offset: 0x0006C6E0
	private void UpdateNormalMode()
	{
		PlayerControls playerControls = this.m_followCamera.Target.RequireComponent<PlayerControls>();
		float num = Mathf.Max(1f, playerControls.GetUnclampedMovementSpeed());
		this.m_followCamera.GradientLimit = num * this.m_normalFollowSpeed;
		this.m_followCamera.TimeToMax = this.m_normalAccelTime / num;
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x0006E338 File Offset: 0x0006C738
	private void Update()
	{
		if (this.m_mode == SingleplayerCamera.Mode.Switching)
		{
			float sqrMagnitude = (this.m_followCamera.GetIdealLocation() - base.transform.position).sqrMagnitude;
			float num = this.m_switchCompleteDistance * this.m_switchCompleteDistance;
			if (sqrMagnitude < num)
			{
				this.SetMode(SingleplayerCamera.Mode.Normal);
			}
		}
		else
		{
			this.UpdateNormalMode();
		}
	}

	// Token: 0x04000F8F RID: 3983
	[SerializeField]
	private float m_idealDistance = 5f;

	// Token: 0x04000F90 RID: 3984
	[SerializeField]
	private Vector3 m_targetOffset = new Vector3(0f, 1f, 0f);

	// Token: 0x04000F91 RID: 3985
	[SerializeField]
	private float m_normalAccelTime = 1f;

	// Token: 0x04000F92 RID: 3986
	[SerializeField]
	private float m_normalFollowSpeed = 5f;

	// Token: 0x04000F93 RID: 3987
	[SerializeField]
	private float m_switchAccelTime = 1f;

	// Token: 0x04000F94 RID: 3988
	[SerializeField]
	private float m_switchFollowSpeed = 10f;

	// Token: 0x04000F95 RID: 3989
	[SerializeField]
	private float m_switchCompleteDistance = 0.1f;

	// Token: 0x04000F96 RID: 3990
	private SingleplayerCamera.Mode m_mode;

	// Token: 0x04000F97 RID: 3991
	private FollowCamera m_followCamera;

	// Token: 0x04000F98 RID: 3992
	private PlayerSwitchingManager m_playerSwitchingManager;

	// Token: 0x02000446 RID: 1094
	private enum Mode
	{
		// Token: 0x04000F9A RID: 3994
		Normal,
		// Token: 0x04000F9B RID: 3995
		Switching
	}
}
