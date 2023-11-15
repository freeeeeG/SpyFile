using System;
using System.Collections;
using Characters.Abilities;
using TMPro;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006EF RID: 1775
	public class Dummy : MonoBehaviour
	{
		// Token: 0x060023D7 RID: 9175 RVA: 0x0006B704 File Offset: 0x00069904
		private void Awake()
		{
			this._character.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
			if (this._immuneToCritical)
			{
				this._character.health.immuneToCritical = true;
			}
			if (this._cannotBeKnockbacked)
			{
				this._character.stat.AttachValues(Dummy.cannotBeKnockbacked);
			}
			this._original = base.transform.position;
			if (this._dpsTextEmptyOnStart)
			{
				this._dpsText.text = string.Empty;
				return;
			}
			this._dpsText.text = Dummy.tauntScripts.Random<string>();
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x0006B7A4 File Offset: 0x000699A4
		private void Initialize()
		{
			this._timeText.text = string.Format("{0:0.00}s", this._time);
			this._dpsText.text = string.Format("{0:0.0}\n{1:0.00}", this._totalDamage, this._totalDamage / (double)this._time);
			this._started = false;
			this._time = 0f;
			this._attackCountByAttribute.SetAll(0);
			this._damageByAttribute.SetAll(0.0);
			this._totalAttackCount = 0;
			this._totalDamage = 0.0;
			base.transform.position = this._original;
			this._character.health.Revive();
			this._character.movement.push.Expire();
			this._character.health.ResetToMaximumHealth();
			this._character.ability.Add(this._getInvulnerable);
			base.StopAllCoroutines();
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x0006B8B0 File Offset: 0x00069AB0
		private void Update()
		{
			if (this._started)
			{
				this._timeText.color = Color.yellow;
				this._dpsText.color = Color.yellow;
				return;
			}
			if (this._character.cinematic.value)
			{
				this._timeText.color = Color.red;
				this._dpsText.color = Color.red;
				return;
			}
			this._timeText.color = Color.white;
			this._dpsText.color = Color.white;
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x0006B93C File Offset: 0x00069B3C
		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (this._character.cinematic.value)
			{
				return;
			}
			if (this._character.ability.Contains(this._getInvulnerable))
			{
				return;
			}
			if (!this._started)
			{
				this._started = true;
				base.StartCoroutine(this.CMesure());
			}
			double totalDamage = this._totalDamage;
			Damage damage = tookDamage;
			this._totalDamage = totalDamage + damage.amount;
			EnumArray<Damage.Attribute, double> damageByAttribute = this._damageByAttribute;
			Damage.Attribute attribute = tookDamage.attribute;
			EnumArray<Damage.Attribute, double> enumArray = damageByAttribute;
			Damage.Attribute key = attribute;
			double num = damageByAttribute[attribute];
			damage = tookDamage;
			enumArray[key] = num + damage.amount;
			this._totalAttackCount++;
			EnumArray<Damage.Attribute, int> attackCountByAttribute = this._attackCountByAttribute;
			attribute = tookDamage.attribute;
			int num2 = attackCountByAttribute[attribute];
			attackCountByAttribute[attribute] = num2 + 1;
			if (this._character.health.currentHealth == 0.0)
			{
				this.Initialize();
			}
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x0006BA24 File Offset: 0x00069C24
		private IEnumerator CMesure()
		{
			for (;;)
			{
				yield return null;
				this._time += Chronometer.global.deltaTime;
				this._timeText.text = string.Format("{0:0.00}s", this._time);
				string dpsTextByAttribute = this.GetDpsTextByAttribute(Damage.Attribute.Physical, this._time);
				string dpsTextByAttribute2 = this.GetDpsTextByAttribute(Damage.Attribute.Magic, this._time);
				string dpsTextByAttribute3 = this.GetDpsTextByAttribute(Damage.Attribute.Fixed, this._time);
				string dpsText = Dummy.GetDpsText(this._totalDamage, this._totalAttackCount, this._time);
				this._dpsText.text = string.Concat(new string[]
				{
					Dummy.Colorize(dpsTextByAttribute, "#F25D1C"),
					"\n",
					Dummy.Colorize(dpsTextByAttribute2, "#1787D8"),
					"\n",
					Dummy.Colorize(dpsTextByAttribute3, "#959595"),
					"\n",
					dpsText
				});
			}
			yield break;
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x0006BA33 File Offset: 0x00069C33
		private static string Colorize(string text, string color)
		{
			return string.Concat(new string[]
			{
				"<color=",
				color,
				">",
				text,
				"</color>"
			});
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x0006BA60 File Offset: 0x00069C60
		private string GetDpsTextByAttribute(Damage.Attribute attribute, float time)
		{
			return Dummy.GetDpsText(this._damageByAttribute[attribute], this._attackCountByAttribute[attribute], time);
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x0006BA80 File Offset: 0x00069C80
		private static string GetDpsText(double damage, int attackCount, float time)
		{
			return string.Format("{0}\n{1:0.0}\n{2:0.00}/s", attackCount, damage, damage / (double)time);
		}

		// Token: 0x04001E88 RID: 7816
		private static Stat.Values cannotBeKnockbacked = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.KnockbackResistance, 0.0)
		});

		// Token: 0x04001E89 RID: 7817
		private static readonly string[] tauntScripts = new string[]
		{
			"오징어볶음 다뒤졋다 ㅋㅋ",
			"와 샌즈! 언더테일 아시는구나!",
			"모르면 맞아야죠",
			"오늘 메뉴는 불고기 어떻습니까?",
			"오늘 메뉴는 찜닭 어떻습니까?",
			"오늘 메뉴는 닭도리탕 어떻습니까?",
			"오늘 메뉴는 스테이크 어떻습니까?",
			"오늘 메뉴는 보쌈 어떻습니까?",
			"오늘 메뉴는 족발 어떻습니까?",
			"오늘 메뉴는 삼겹살 어떻습니까?",
			"오늘 메뉴는 쌈밥 어떻습니까?",
			"오늘 메뉴는 치킨 어떻습니까?",
			"오늘 메뉴는 순두부찌개 어떻습니까?",
			"오늘 메뉴는 부대찌개 어떻습니까?",
			"오늘 메뉴는 김치찌개 어떻습니까?",
			"오늘 메뉴는 카레 어떻습니까?",
			"오늘 메뉴는 비빔밥 어떻습니까?",
			"오늘 메뉴는 김치볶음밥 어떻습니까?",
			"오늘 메뉴는 제육볶음 어떻습니까?",
			"오늘 메뉴는 치킨마요 어떻습니까?",
			"오늘 메뉴는 돈부리 어떻습니까?",
			"오늘 메뉴는 국밥 어떻습니까?",
			"오늘 메뉴는 뼈해장국 어떻습니까?",
			"오늘 메뉴는 스파게티 어떻습니까?",
			"오늘 메뉴는 냉면 어떻습니까?",
			"오늘 메뉴는 짜장면 어떻습니까?",
			"오늘 메뉴는 짬뽕 어떻습니까?",
			"오늘 메뉴는 탕수육 어떻습니까?",
			"오늘 메뉴는 꿔바로우 어떻습니까?",
			"오늘 메뉴는 라면 어떻습니까?",
			"오늘 메뉴는 초밥 어떻습니까?",
			"오늘 메뉴는 깐풍기 어떻습니까?",
			"오늘 메뉴는 쌀국수 어떻습니까?",
			"오늘 메뉴는 마파두부 어떻습니까?",
			"오늘 메뉴는 햄버거 어떻습니까?",
			"오늘 메뉴는 팟타이 어떻습니까?",
			"오늘 메뉴는 샌드위치 어떻습니까?",
			"오늘 메뉴는 떢볶이 어떻습니까?",
			"오늘 메뉴는 갈비탕 어떻습니까?",
			"오늘 메뉴는 밥버거 어떻습니까?",
			"오늘 메뉴는 서브웨이 어떻습니까?",
			"오늘 메뉴는 도시락 어떻습니까?",
			"오늘 메뉴는 돈까스 어떻습니까?",
			"오늘 메뉴는 그냥 굶는 게 어떻습니까?"
		};

		// Token: 0x04001E8A RID: 7818
		private readonly GetInvulnerable _getInvulnerable = new GetInvulnerable
		{
			duration = 4f
		};

		// Token: 0x04001E8B RID: 7819
		[SerializeField]
		private bool _immuneToCritical;

		// Token: 0x04001E8C RID: 7820
		[SerializeField]
		private bool _cannotBeKnockbacked;

		// Token: 0x04001E8D RID: 7821
		[SerializeField]
		private bool _dpsTextEmptyOnStart;

		// Token: 0x04001E8E RID: 7822
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04001E8F RID: 7823
		[SerializeField]
		private TMP_Text _timeText;

		// Token: 0x04001E90 RID: 7824
		[SerializeField]
		private TMP_Text _dpsText;

		// Token: 0x04001E91 RID: 7825
		private bool _started;

		// Token: 0x04001E92 RID: 7826
		private float _time;

		// Token: 0x04001E93 RID: 7827
		private EnumArray<Damage.Attribute, int> _attackCountByAttribute = new EnumArray<Damage.Attribute, int>();

		// Token: 0x04001E94 RID: 7828
		private EnumArray<Damage.Attribute, double> _damageByAttribute = new EnumArray<Damage.Attribute, double>();

		// Token: 0x04001E95 RID: 7829
		private int _totalAttackCount;

		// Token: 0x04001E96 RID: 7830
		private double _totalDamage;

		// Token: 0x04001E97 RID: 7831
		private Vector3 _original;
	}
}
