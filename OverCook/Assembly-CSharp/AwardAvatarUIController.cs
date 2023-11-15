using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B20 RID: 2848
public class AwardAvatarUIController : MonoBehaviour
{
	// Token: 0x060039A6 RID: 14758 RVA: 0x0011170C File Offset: 0x0010FB0C
	private void Awake()
	{
		this.m_animator = base.gameObject.RequireComponent<Animator>();
		this.m_chef = base.gameObject.RequireComponentRecursive<FrontendChef>();
		if (this.m_button != null)
		{
			this.m_button.enabled = false;
		}
	}

	// Token: 0x060039A7 RID: 14759 RVA: 0x00111758 File Offset: 0x0010FB58
	public void EnableButton()
	{
		if (this.m_button != null)
		{
			this.m_button.enabled = true;
		}
	}

	// Token: 0x060039A8 RID: 14760 RVA: 0x00111778 File Offset: 0x0010FB78
	public void SetData(ChefAvatarData _chef)
	{
		GameSession.SelectedChefData chefData = new GameSession.SelectedChefData(_chef, this.m_colour);
		this.m_chef.SetShaderMode(FrontendChef.ShaderMode.eUI);
		this.m_chef.SetChefData(chefData, false);
		this.m_chef.SetUIChefAmbientLighting(this.m_AmbientColor);
		this.m_name.text = string.Empty;
		if (this.m_animator)
		{
			this.m_animator.SetTrigger(AwardAvatarUIController.m_iUnlock);
		}
		GameUtils.TriggerAudio(this.m_audioTag, base.gameObject.layer);
	}

	// Token: 0x04002E5B RID: 11867
	[SerializeField]
	private Image m_button;

	// Token: 0x04002E5C RID: 11868
	[SerializeField]
	public float m_awardTimeout = 5f;

	// Token: 0x04002E5D RID: 11869
	[SerializeField]
	private Text m_name;

	// Token: 0x04002E5E RID: 11870
	[SerializeField]
	private ChefColourData m_colour;

	// Token: 0x04002E5F RID: 11871
	[SerializeField]
	private ChefAvatarData m_avatar;

	// Token: 0x04002E60 RID: 11872
	[SerializeField]
	private Color m_AmbientColor;

	// Token: 0x04002E61 RID: 11873
	public T17Text m_WaitingForPlayersText;

	// Token: 0x04002E62 RID: 11874
	private FrontendChef m_chef;

	// Token: 0x04002E63 RID: 11875
	[SerializeField]
	private Animator m_animator;

	// Token: 0x04002E64 RID: 11876
	[SerializeField]
	private GameOneShotAudioTag m_audioTag = GameOneShotAudioTag.Blank;

	// Token: 0x04002E65 RID: 11877
	private static readonly int m_iUnlock = Animator.StringToHash("Unlock");
}
