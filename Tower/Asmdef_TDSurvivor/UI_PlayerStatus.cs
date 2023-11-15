using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000182 RID: 386
public class UI_PlayerStatus : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000A34 RID: 2612 RVA: 0x000262CC File Offset: 0x000244CC
	private void OnEnable()
	{
		EventMgr.Register<int>(eGameEvents.OnHpChanged, new Action<int>(this.OnHPChanged));
		EventMgr.Register<int>(eGameEvents.OnCoinChanged, new Action<int>(this.OnCoinChanged));
		EventMgr.Register<int>(eGameEvents.OnEnergyChanged, new Action<int>(this.OnEnergyChanged));
		EventMgr.Register<AMonsterBase>(eGameEvents.MonsterDealDamageToPlayer, new Action<AMonsterBase>(this.OnMonsterDealDamageToPlayer));
		EventMgr.Register<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Register(eGameEvents.OnPlayerVictory, new Action(this.OnPlayerVictory));
		EventMgr.Register(eGameEvents.OnPlayerDefeat, new Action(this.OnPlayerDefeat));
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x00026384 File Offset: 0x00024584
	private void OnDisable()
	{
		EventMgr.Remove<int>(eGameEvents.OnHpChanged, new Action<int>(this.OnHPChanged));
		EventMgr.Remove<int>(eGameEvents.OnCoinChanged, new Action<int>(this.OnCoinChanged));
		EventMgr.Remove<int>(eGameEvents.OnEnergyChanged, new Action<int>(this.OnEnergyChanged));
		EventMgr.Remove<AMonsterBase>(eGameEvents.MonsterDealDamageToPlayer, new Action<AMonsterBase>(this.OnMonsterDealDamageToPlayer));
		EventMgr.Remove<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Remove(eGameEvents.OnPlayerVictory, new Action(this.OnPlayerVictory));
		EventMgr.Register(eGameEvents.OnPlayerDefeat, new Action(this.OnPlayerDefeat));
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x00026439 File Offset: 0x00024639
	private void Update()
	{
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x0002643B File Offset: 0x0002463B
	private void OnHPChanged(int value)
	{
		this.text_HP.text = value.ToString();
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x00026450 File Offset: 0x00024650
	private void OnCoinChanged(int value)
	{
		if (value == this.curCoinVaue)
		{
			return;
		}
		if (this.coroutine_CoinChange != null)
		{
			base.StopCoroutine(this.coroutine_CoinChange);
		}
		if (this.curCoinVaue == -1)
		{
			this.text_Coin.text = value.ToString();
			this.curCoinVaue = value;
			return;
		}
		this.coroutine_CoinChange = base.StartCoroutine(this.CR_CoinLerpValue(value));
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x000264B1 File Offset: 0x000246B1
	private IEnumerator CR_CoinLerpValue(int value)
	{
		float duration = 0.33f;
		float timer = 0f;
		int startValue = this.curCoinVaue;
		while (timer < duration)
		{
			this.curCoinVaue = (int)Mathf.Lerp((float)startValue, (float)value, Mathf.Clamp01(timer / duration));
			this.text_Coin.text = this.curCoinVaue.ToString();
			yield return new WaitForSecondsRealtime(0.05f);
			timer += 0.05f;
		}
		this.curCoinVaue = value;
		this.text_Coin.text = this.curCoinVaue.ToString();
		this.coroutine_CoinChange = null;
		yield break;
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x000264C7 File Offset: 0x000246C7
	private void OnEnergyChanged(int value)
	{
		this.text_Energy.text = value.ToString();
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x000264DB File Offset: 0x000246DB
	private void OnMonsterDealDamageToPlayer(AMonsterBase @base)
	{
		this.animator.SetTrigger("onHitTrigger");
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x000264ED File Offset: 0x000246ED
	private void OnRoundStart(int index, int totalRound)
	{
		if (index == 1)
		{
			this.animator.SetBool("isOn", true);
		}
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00026504 File Offset: 0x00024704
	private void OnPlayerVictory()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00026517 File Offset: 0x00024717
	private void OnPlayerDefeat()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0002652C File Offset: 0x0002472C
	public void OnPointerEnter(PointerEventData eventData)
	{
		string @string = LocalizationManager.Instance.GetString("UI", "PLAYER_STAT_HP_INFO", Array.Empty<object>());
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
		EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, "", @string);
		EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._2D, base.transform, Vector3.up * 50f);
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x000265A0 File Offset: 0x000247A0
	public void OnPointerExit(PointerEventData eventData)
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
	}

	// Token: 0x040007E1 RID: 2017
	[SerializeField]
	private Animator animator;

	// Token: 0x040007E2 RID: 2018
	[SerializeField]
	private TMP_Text text_HP;

	// Token: 0x040007E3 RID: 2019
	[SerializeField]
	private TMP_Text text_Coin;

	// Token: 0x040007E4 RID: 2020
	[SerializeField]
	private TMP_Text text_Energy;

	// Token: 0x040007E5 RID: 2021
	[SerializeField]
	private ParticleSystem particle_Fire;

	// Token: 0x040007E6 RID: 2022
	private int curCoinVaue = -1;

	// Token: 0x040007E7 RID: 2023
	private Coroutine coroutine_CoinChange;
}
