using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000071 RID: 113
	public class LanguageSetter : MonoBehaviour
	{
		// Token: 0x060004C4 RID: 1220 RVA: 0x00018235 File Offset: 0x00016435
		private void Awake()
		{
			LocalizationSystem.language = (LocalizationSystem.Language)PlayerPrefs.GetInt("Language", 0);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00018247 File Offset: 0x00016447
		public void SetEN()
		{
			LocalizationSystem.language = LocalizationSystem.Language.English;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00018269 File Offset: 0x00016469
		public void SetJP()
		{
			LocalizationSystem.language = LocalizationSystem.Language.Japanese;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001828B File Offset: 0x0001648B
		public void SetCH()
		{
			LocalizationSystem.language = LocalizationSystem.Language.Chinese;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000182AD File Offset: 0x000164AD
		public void SetBR()
		{
			LocalizationSystem.language = LocalizationSystem.Language.BrazilPortuguese;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000182CF File Offset: 0x000164CF
		public void SetTC()
		{
			LocalizationSystem.language = LocalizationSystem.Language.TChinese;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000182F1 File Offset: 0x000164F1
		public void SetFR()
		{
			LocalizationSystem.language = LocalizationSystem.Language.French;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00018314 File Offset: 0x00016514
		public void SetIT()
		{
			LocalizationSystem.language = LocalizationSystem.Language.Italian;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00018337 File Offset: 0x00016537
		public void SetGR()
		{
			LocalizationSystem.language = LocalizationSystem.Language.German;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00018359 File Offset: 0x00016559
		public void SetPL()
		{
			LocalizationSystem.language = LocalizationSystem.Language.Polish;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001837B File Offset: 0x0001657B
		public void SetSP()
		{
			LocalizationSystem.language = LocalizationSystem.Language.Spanish;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001839D File Offset: 0x0001659D
		public void SetRU()
		{
			LocalizationSystem.language = LocalizationSystem.Language.Russian;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x000183BF File Offset: 0x000165BF
		public void SetTR()
		{
			LocalizationSystem.language = LocalizationSystem.Language.Turkish;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x000183E2 File Offset: 0x000165E2
		public void SetKR()
		{
			LocalizationSystem.language = LocalizationSystem.Language.Korean;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00018405 File Offset: 0x00016605
		public void SetHU()
		{
			LocalizationSystem.language = LocalizationSystem.Language.Hungarian;
			PlayerPrefs.SetInt("Language", (int)LocalizationSystem.language);
			this.PostNotification(LanguageSetter.ChangedEvent);
		}

		// Token: 0x040002BB RID: 699
		public static string ChangedEvent = "LanguageSetter.ChangedEvent";
	}
}
