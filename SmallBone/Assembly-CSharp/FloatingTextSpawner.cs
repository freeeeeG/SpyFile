using System;
using System.Collections;
using Characters;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

// Token: 0x020000A0 RID: 160
public class FloatingTextSpawner : MonoBehaviour
{
	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000325 RID: 805 RVA: 0x0000BD64 File Offset: 0x00009F64
	private string breakShieldText
	{
		get
		{
			return Localization.GetLocalizedString("floating/breakShield");
		}
	}

	// Token: 0x06000326 RID: 806 RVA: 0x0000BD70 File Offset: 0x00009F70
	private void Awake()
	{
		ColorUtility.TryParseHtmlString("#FF0D06", out this._playerTakingDamageColor);
		ColorUtility.TryParseHtmlString("#E3FF00", out this._criticalPhysicalAttackColor);
		ColorUtility.TryParseHtmlString("#17FFDB", out this._criticalMagicAttackColor);
		ColorUtility.TryParseHtmlString("#FF8000", out this._physicalAttackColor);
		ColorUtility.TryParseHtmlString("#17C4FF", out this._magicAttackColor);
		ColorUtility.TryParseHtmlString("#F2F2F2", out this._fixedAttackColor);
		ColorUtility.TryParseHtmlString("#18FF00", out this._healColor);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0000BDF4 File Offset: 0x00009FF4
	public FloatingText Spawn(string text, Vector3 position)
	{
		FloatingText floatingText = this.floatingTextPrefab.Spawn();
		floatingText.Initialize(text, position);
		return floatingText;
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0000BE0C File Offset: 0x0000A00C
	public void SpawnPlayerTakingDamage(in Damage damage)
	{
		Damage damage2 = damage;
		this.SpawnPlayerTakingDamage(damage2.amount, damage.hitPoint);
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0000BE34 File Offset: 0x0000A034
	public void SpawnPlayerTakingDamage(double amount, Vector2 position)
	{
		if (Scene<GameBase>.instance.uiManager.hideOption == UIManager.HideOption.HideAll)
		{
			return;
		}
		if (amount < 1.0)
		{
			return;
		}
		FloatingText floatingText = this.Spawn(amount.ToString("0"), position + new Vector2(0f, 0.5f));
		floatingText.color = this._playerTakingDamageColor;
		floatingText.Modify(GameObjectModifier.TranslateBySpeedAndAcc(9f, -12f, 2.5f));
		floatingText.sortingOrder = 500;
		floatingText.transform.localScale = this._playerTakingDamageScale;
		if (MMMaths.RandomBool())
		{
			floatingText.Modify(GameObjectModifier.TranslateUniformMotion(0.2f, 0f, 0f));
		}
		else
		{
			floatingText.Modify(GameObjectModifier.TranslateUniformMotion(-0.2f, 0f, 0f));
		}
		floatingText.Despawn(0.6f);
	}

	// Token: 0x0600032A RID: 810 RVA: 0x0000BF18 File Offset: 0x0000A118
	public void SpawnTakingDamage(in Damage damage)
	{
		if (Scene<GameBase>.instance.uiManager.hideOption == UIManager.HideOption.HideAll)
		{
			return;
		}
		Damage damage2 = damage;
		if (damage2.amount < 1.0)
		{
			return;
		}
		damage2 = damage;
		FloatingText floatingText = this.Spawn(damage2.ToString(), damage.hitPoint + new Vector2(0f, 0.5f));
		switch (damage.attribute)
		{
		case Damage.Attribute.Physical:
			floatingText.color = this._physicalAttackColor;
			floatingText.sortingOrder = 100;
			break;
		case Damage.Attribute.Magic:
			floatingText.color = this._magicAttackColor;
			floatingText.sortingOrder = 200;
			break;
		case Damage.Attribute.Fixed:
			floatingText.color = this._fixedAttackColor;
			floatingText.sortingOrder = 100;
			break;
		}
		if (damage.critical)
		{
			floatingText.Modify(GameObjectModifier.LerpScale(1.4f, 1.6f, 0.4f));
			Damage.Attribute attribute = damage.attribute;
			if (attribute != Damage.Attribute.Physical)
			{
				if (attribute == Damage.Attribute.Magic)
				{
					floatingText.color = this._criticalMagicAttackColor;
					floatingText.sortingOrder = 400;
				}
			}
			else
			{
				floatingText.color = this._criticalPhysicalAttackColor;
				floatingText.sortingOrder = 300;
			}
		}
		floatingText.Modify(GameObjectModifier.TranslateBySpeedAndAcc(10f, -17f, 3f));
		if (MMMaths.RandomBool())
		{
			floatingText.Modify(GameObjectModifier.TranslateUniformMotion(0.2f, 0f, 0f));
		}
		else
		{
			floatingText.Modify(GameObjectModifier.TranslateUniformMotion(-0.2f, 0f, 0f));
		}
		floatingText.Despawn(0.5f);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0000C0B0 File Offset: 0x0000A2B0
	public void SpawnHeal(double amount, Vector3 position)
	{
		if (Scene<GameBase>.instance.uiManager.hideOption == UIManager.HideOption.HideAll)
		{
			return;
		}
		if (amount <= 0.0)
		{
			return;
		}
		FloatingText floatingText = this.Spawn(amount.ToString("0"), position);
		floatingText.color = this._healColor;
		floatingText.Modify(GameObjectModifier.Scale(1.1f));
		floatingText.Modify(GameObjectModifier.TranslateBySpeedAndAcc(7.5f, -7.5f, 1f));
		floatingText.sortingOrder = 1;
		if (MMMaths.RandomBool())
		{
			floatingText.Modify(GameObjectModifier.TranslateUniformMotion(0.2f, 0f, 0f));
		}
		else
		{
			floatingText.Modify(GameObjectModifier.TranslateUniformMotion(-0.2f, 0f, 0f));
		}
		floatingText.Despawn(0.5f);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0000C178 File Offset: 0x0000A378
	public void SpawnBuff(string text, Vector3 position, string colorValue = "#F2F2F2")
	{
		if (Scene<GameBase>.instance.uiManager.hideOption == UIManager.HideOption.HideAll)
		{
			return;
		}
		Color color;
		ColorUtility.TryParseHtmlString(colorValue, out color);
		BuffText buffText = this._buffTextPrefab.Spawn();
		buffText.Initialize(text, position);
		buffText.color = color;
		buffText.sortingOrder = 1;
		base.StartCoroutine(this.CMove(buffText.transform, 0.5f, 2f));
		base.StartCoroutine(this.CFadeOut(buffText, 1.5f, 0.5f));
		buffText.Despawn(2f);
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0000C204 File Offset: 0x0000A404
	public void SpawnStatus(string text, Vector3 position, string colorValue)
	{
		if (Scene<GameBase>.instance.uiManager.hideOption == UIManager.HideOption.HideAll)
		{
			return;
		}
		Color color;
		ColorUtility.TryParseHtmlString(colorValue, out color);
		FloatingText floatingText = this.Spawn(text, position);
		floatingText.color = color;
		floatingText.sortingOrder = 1;
		floatingText.Modify(GameObjectModifier.Scale(0.75f));
		base.StartCoroutine(this.CMove(floatingText.transform, 0.5f, 1f));
		base.StartCoroutine(this.CFadeOut(floatingText, 0.5f, 0.5f));
		floatingText.Despawn(1f);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x0000C294 File Offset: 0x0000A494
	public void SpawnEvade(string text, Vector3 position, string colorValue)
	{
		if (Scene<GameBase>.instance.uiManager.hideOption == UIManager.HideOption.HideAll)
		{
			return;
		}
		Color color;
		ColorUtility.TryParseHtmlString(colorValue, out color);
		FloatingText floatingText = this.Spawn(text, position);
		floatingText.color = color;
		floatingText.sortingOrder = 1;
		floatingText.Modify(GameObjectModifier.Scale(1f));
		base.StartCoroutine(this.CMove(floatingText.transform, 0.5f, 1f));
		base.StartCoroutine(this.CFadeOut(floatingText, 0.5f, 0.5f));
		floatingText.Despawn(1f);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x0000C324 File Offset: 0x0000A524
	public void SpawnBreakShield(Vector3 position, string colorValue = "#F2F2F2")
	{
		if (Scene<GameBase>.instance.uiManager.hideOption == UIManager.HideOption.HideAll)
		{
			return;
		}
		Color color;
		ColorUtility.TryParseHtmlString(colorValue, out color);
		BuffText buffText = this._buffTextPrefab.Spawn();
		buffText.Initialize(this.breakShieldText, position);
		buffText.color = color;
		buffText.sortingOrder = 1;
		base.StartCoroutine(this.CMove(buffText.transform, 0.5f, 2f));
		base.StartCoroutine(this.CFadeOut(buffText, 1.5f, 0.5f));
		buffText.Despawn(2f);
	}

	// Token: 0x06000330 RID: 816 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
	private IEnumerator CMove(Transform transform, float distance, float duration)
	{
		float elapsed = 0f;
		float speed = distance / duration;
		while (elapsed <= duration)
		{
			float deltaTime = Chronometer.global.deltaTime;
			float d = speed * deltaTime;
			elapsed += deltaTime;
			transform.Translate(Vector2.up * d);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000331 RID: 817 RVA: 0x0000C3D1 File Offset: 0x0000A5D1
	private IEnumerator CFadeOut(FloatingText spawned, float delay, float duration)
	{
		yield return Chronometer.global.WaitForSeconds(delay);
		spawned.FadeOut(duration);
		yield break;
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0000C3EE File Offset: 0x0000A5EE
	private IEnumerator CFadeOut(BuffText spawned, float delay, float duration)
	{
		yield return Chronometer.global.WaitForSeconds(delay);
		spawned.FadeOut(duration);
		yield break;
	}

	// Token: 0x0400028A RID: 650
	private const int _countLimit = 100;

	// Token: 0x0400028B RID: 651
	private const string _buffDefaultColorString = "#F2F2F2";

	// Token: 0x0400028C RID: 652
	public FloatingText floatingTextPrefab;

	// Token: 0x0400028D RID: 653
	private const int _buffTextLimit = 10;

	// Token: 0x0400028E RID: 654
	public BuffText _buffTextPrefab;

	// Token: 0x0400028F RID: 655
	private Vector3 _playerTakingDamageScale = new Vector3(1.25f, 1.25f, 1f);

	// Token: 0x04000290 RID: 656
	private Color _playerTakingDamageColor;

	// Token: 0x04000291 RID: 657
	private Color _criticalPhysicalAttackColor;

	// Token: 0x04000292 RID: 658
	private Color _criticalMagicAttackColor;

	// Token: 0x04000293 RID: 659
	private Color _physicalAttackColor;

	// Token: 0x04000294 RID: 660
	private Color _magicAttackColor;

	// Token: 0x04000295 RID: 661
	private Color _fixedAttackColor;

	// Token: 0x04000296 RID: 662
	private Color _healColor;
}
