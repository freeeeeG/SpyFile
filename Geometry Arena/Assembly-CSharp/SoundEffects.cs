using System;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x0200000B RID: 11
[CreateAssetMenu(fileName = "SoundEffects", menuName = "CreateAsset/SoundEffects")]
public class SoundEffects : ScriptableObject
{
	// Token: 0x1700005B RID: 91
	// (get) Token: 0x0600008A RID: 138 RVA: 0x0000455B File Offset: 0x0000275B
	public static SoundEffects Inst
	{
		get
		{
			if (AssetManager.inst != null)
			{
				return AssetManager.inst.soundEffects;
			}
			return Resources.Load<SoundEffects>("Assets/SoundEffects");
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00004580 File Offset: 0x00002780
	public void UpdatePitch()
	{
		if (Player.inst == null)
		{
			return;
		}
		float num = math.clamp(Mathf.Sqrt(Player.inst.unit.playerFactorTotal.fireSpd / GameParameters.Inst.DefaultFactor.fireSpd), 0.9f, 3.9f);
		this.shoot[0].pitch = 0.6f * num;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x000045E8 File Offset: 0x000027E8
	public void InitCanPlay()
	{
		Sound[] array = this.getCoin;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.getUpgrade;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.shoot;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.hit;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.enemyDie;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.playerHurt;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.skill_ShieldOpen;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.skill_ShieldClose;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.skill_Blast;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.skill_Dash;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.ui_PanelOpen;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.ui_PanelClose;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.ui_ButtonEnter;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
		array = this.ui_ButtonClick;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canPlay = true;
		}
	}

	// Token: 0x040000BA RID: 186
	[Header("Items")]
	public Sound[] getCoin = new Sound[1];

	// Token: 0x040000BB RID: 187
	public Sound[] getUpgrade = new Sound[1];

	// Token: 0x040000BC RID: 188
	[Header("Physics")]
	public Sound[] shoot = new Sound[1];

	// Token: 0x040000BD RID: 189
	public Sound[] hit = new Sound[1];

	// Token: 0x040000BE RID: 190
	public Sound[] enemyDie = new Sound[1];

	// Token: 0x040000BF RID: 191
	public Sound[] playerHurt = new Sound[1];

	// Token: 0x040000C0 RID: 192
	[Header("Skills")]
	public Sound[] skill_ShieldOpen = new Sound[1];

	// Token: 0x040000C1 RID: 193
	public Sound[] skill_ShieldClose = new Sound[1];

	// Token: 0x040000C2 RID: 194
	public Sound[] skill_Blast = new Sound[1];

	// Token: 0x040000C3 RID: 195
	public Sound[] skill_Dash = new Sound[1];

	// Token: 0x040000C4 RID: 196
	public Sound[] skill_SwordOut = new Sound[1];

	// Token: 0x040000C5 RID: 197
	public Sound[] skill_SwordBack = new Sound[1];

	// Token: 0x040000C6 RID: 198
	[Header("UI")]
	public Sound[] ui_PanelOpen = new Sound[1];

	// Token: 0x040000C7 RID: 199
	public Sound[] ui_PanelClose = new Sound[1];

	// Token: 0x040000C8 RID: 200
	public Sound[] ui_ButtonEnter = new Sound[1];

	// Token: 0x040000C9 RID: 201
	public Sound[] ui_ButtonClick = new Sound[1];

	// Token: 0x040000CA RID: 202
	[Header("Bullets")]
	public Sound[] bullet_Missle = new Sound[1];

	// Token: 0x040000CB RID: 203
	public Sound[] bullet_Grenade = new Sound[1];

	// Token: 0x040000CC RID: 204
	public Sound[] bullet_Mine = new Sound[1];
}
