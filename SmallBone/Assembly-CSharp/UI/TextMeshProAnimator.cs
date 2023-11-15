using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x020003D9 RID: 985
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextMeshProAnimator : MonoBehaviour
	{
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001253 RID: 4691 RVA: 0x00035D19 File Offset: 0x00033F19
		// (set) Token: 0x06001254 RID: 4692 RVA: 0x00035D24 File Offset: 0x00033F24
		public string text
		{
			get
			{
				return this.customText;
			}
			set
			{
				this.customText = value;
				if (this.useCustomText)
				{
					if (this.TMProGUI == null)
					{
						this.TMProGUI = base.GetComponent<TextMeshProUGUI>();
					}
					this.TMProGUI.text = this.ParseText(value);
					this.SyncToTextMesh();
				}
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001255 RID: 4693 RVA: 0x00035D72 File Offset: 0x00033F72
		public int totalChars
		{
			get
			{
				return this.TMProGUI.textInfo.characterCount;
			}
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00035D84 File Offset: 0x00033F84
		public void SyncToTextMesh()
		{
			this.TMProGUI.ForceMeshUpdate(false, false);
			this.vertex_Base = new Vector3[this.TMProGUI.textInfo.meshInfo.Length][];
			int num = 0;
			for (int i = 0; i < this.TMProGUI.textInfo.meshInfo.Length; i++)
			{
				this.vertex_Base[i] = new Vector3[this.TMProGUI.textInfo.meshInfo[i].vertices.Length];
				if (num < this.vertex_Base[i].Length)
				{
					num = this.vertex_Base[i].Length;
				}
				Array.Copy(this.TMProGUI.textInfo.meshInfo[i].vertices, this.vertex_Base[i], this.TMProGUI.textInfo.meshInfo[i].vertices.Length);
			}
			this.wigglePrevPos = new float[num * 2];
			this.wiggleTargetPos = new float[num * 2];
			this.wiggleTimeCurrent = new float[num * 2];
			this.wiggleTimeTotal = new float[num * 2];
			this.TMProGUI.ForceMeshUpdate(false, false);
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00035EAC File Offset: 0x000340AC
		public void UpdateText(string text = null)
		{
			if (this.TMProGUI == null)
			{
				this.TMProGUI = base.gameObject.GetComponent<TextMeshProUGUI>();
			}
			if (!this.useCustomText)
			{
				this.TMProGUI.text = this.ParseText(this.TMProGUI.text);
				this.SyncToTextMesh();
				return;
			}
			if (text == null)
			{
				this.text = this.text;
				return;
			}
			this.text = text;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00035F1C File Offset: 0x0003411C
		private string ParseText(string inputText)
		{
			List<bool> list = new List<bool>();
			List<float> list2 = new List<float>();
			List<TextMeshAnimator_IndependencyType> list3 = new List<TextMeshAnimator_IndependencyType>();
			bool item = false;
			float item2 = 1f;
			TextMeshAnimator_IndependencyType item3 = this.shakeIndependency;
			List<bool> list4 = new List<bool>();
			List<float> list5 = new List<float>();
			List<float> list6 = new List<float>();
			List<float> list7 = new List<float>();
			List<TextMeshAnimator_IndependencyType> list8 = new List<TextMeshAnimator_IndependencyType>();
			bool item4 = false;
			float item5 = 1f;
			float item6 = 1f;
			float item7 = 1f;
			TextMeshAnimator_IndependencyType item8 = this.waveIndependency;
			List<bool> list9 = new List<bool>();
			List<float> list10 = new List<float>();
			List<float> list11 = new List<float>();
			List<TextMeshAnimator_IndependencyType> list12 = new List<TextMeshAnimator_IndependencyType>();
			bool item9 = false;
			float item10 = 1f;
			float item11 = 1f;
			TextMeshAnimator_IndependencyType item12 = this.wiggleIndependency;
			List<int> list13 = new List<int>();
			int item13 = 2;
			string text = "";
			for (int i = 0; i < inputText.Length; i++)
			{
				if (inputText[i] == '<')
				{
					int num = i;
					while (i < inputText.Length)
					{
						if (inputText[i++] == '>')
						{
							string text2 = inputText.Substring(num, i - num);
							if (text2.ToUpper().Contains("COLOR") || text2.ToUpper().Contains("SIZE"))
							{
								Debug.Log("THE TAG IS " + text2);
								text += text2;
								break;
							}
							if (text2.ToUpper().Contains("SPRITE"))
							{
								text += text2;
								break;
							}
							if (text2.ToUpper().Contains("/SHAKE"))
							{
								item = false;
								item2 = 1f;
								break;
							}
							if (text2.ToUpper().Contains("SHAKE"))
							{
								item = true;
								string text3 = "INTENSITY=";
								if (text2.ToUpper().Contains(text3))
								{
									int num2 = text2.ToUpper().IndexOf(text3) + text3.Length;
									int num3 = num2;
									while (num3 < text2.Length && (char.IsDigit(text2[num3]) || text2[num3] == '.'))
									{
										num3++;
									}
									string text4 = text2.Substring(num2, num3 - num2);
									if (!float.TryParse(text4, out item2))
									{
										Debug.LogError(string.Format("'{0}' is not a valid value for shake amount.", text4));
									}
								}
								if (text2.ToUpper().Contains("UNITED"))
								{
									item3 = TextMeshAnimator_IndependencyType.United;
								}
								if (text2.ToUpper().Contains("WORD"))
								{
									item3 = TextMeshAnimator_IndependencyType.Word;
								}
								if (text2.ToUpper().Contains("CHARACTER"))
								{
									item3 = TextMeshAnimator_IndependencyType.Character;
								}
								if (text2.ToUpper().Contains("VERTEX"))
								{
									item3 = TextMeshAnimator_IndependencyType.Vertex;
									break;
								}
								break;
							}
							else
							{
								if (text2.ToUpper().Contains("/WAVE"))
								{
									item4 = false;
									item5 = 1f;
									item6 = 1f;
									item7 = 1f;
									break;
								}
								if (text2.ToUpper().Contains("WAVE"))
								{
									item4 = true;
									string text5 = "INTENSITY=";
									if (text2.ToUpper().Contains(text5))
									{
										int num4 = text2.ToUpper().IndexOf(text5) + text5.Length;
										int num5 = num4;
										while (num5 < text2.Length && (char.IsDigit(text2[num5]) || text2[num5] == '.'))
										{
											num5++;
										}
										string text6 = text2.Substring(num4, num5 - num4);
										if (!float.TryParse(text6, out item5))
										{
											Debug.LogError(string.Format("'{0}' is not a valid value for wave amount.", text6));
										}
									}
									string text7 = "SPEED=";
									if (text2.ToUpper().Contains(text7))
									{
										int num6 = text2.ToUpper().IndexOf(text7) + text7.Length;
										int num7 = num6;
										while (num7 < text2.Length && (char.IsDigit(text2[num7]) || text2[num7] == '.'))
										{
											num7++;
										}
										string text8 = text2.Substring(num6, num7 - num6);
										if (!float.TryParse(text8, out item6))
										{
											Debug.LogError(string.Format("'{0}' is not a valid value for wave speed.", text8));
										}
									}
									string text9 = "SEPARATION=";
									if (text2.ToUpper().Contains(text9))
									{
										int num8 = text2.ToUpper().IndexOf(text9) + text9.Length;
										int num9 = num8;
										while (num9 < text2.Length && (char.IsDigit(text2[num9]) || text2[num9] == '.'))
										{
											num9++;
										}
										string text10 = text2.Substring(num8, num9 - num8);
										if (!float.TryParse(text10, out item7))
										{
											Debug.LogError(string.Format("'{0}' is not a valid value for wave separation.", text10));
										}
									}
									if (text2.ToUpper().Contains("UNITED"))
									{
										item8 = TextMeshAnimator_IndependencyType.United;
									}
									if (text2.ToUpper().Contains("WORD"))
									{
										item8 = TextMeshAnimator_IndependencyType.Word;
									}
									if (text2.ToUpper().Contains("CHARACTER"))
									{
										item8 = TextMeshAnimator_IndependencyType.Character;
									}
									if (text2.ToUpper().Contains("VERTEX"))
									{
										item8 = TextMeshAnimator_IndependencyType.Vertex;
										break;
									}
									break;
								}
								else
								{
									if (text2.ToUpper().Contains("/WIGGLE"))
									{
										item9 = false;
										item10 = 1f;
										item11 = 1f;
										break;
									}
									if (text2.ToUpper().Contains("WIGGLE"))
									{
										item9 = true;
										string text11 = "INTENSITY=";
										if (text2.ToUpper().Contains(text11))
										{
											int num10 = text2.ToUpper().IndexOf(text11) + text11.Length;
											int num11 = num10;
											while (num11 < text2.Length && (char.IsDigit(text2[num11]) || text2[num11] == '.'))
											{
												num11++;
											}
											string text12 = text2.Substring(num10, num11 - num10);
											if (!float.TryParse(text12, out item10))
											{
												Debug.LogError(string.Format("'{0}' is not a valid value for wiggle amount.", text12));
											}
										}
										string text13 = "SPEED=";
										if (text2.ToUpper().Contains(text13))
										{
											int num12 = text2.ToUpper().IndexOf(text13) + text13.Length;
											int num13 = num12;
											while (num13 < text2.Length && (char.IsDigit(text2[num13]) || text2[num13] == '.'))
											{
												num13++;
											}
											string text14 = text2.Substring(num12, num13 - num12);
											if (!float.TryParse(text14, out item11))
											{
												Debug.LogError(string.Format("'{0}' is not a valid value for wiggle speed.", text14));
											}
										}
										if (text2.ToUpper().Contains("UNITED"))
										{
											item12 = TextMeshAnimator_IndependencyType.United;
										}
										if (text2.ToUpper().Contains("WORD"))
										{
											item12 = TextMeshAnimator_IndependencyType.Word;
										}
										if (text2.ToUpper().Contains("CHARACTER"))
										{
											item12 = TextMeshAnimator_IndependencyType.Character;
										}
										if (text2.ToUpper().Contains("VERTEX"))
										{
											item12 = TextMeshAnimator_IndependencyType.Vertex;
											break;
										}
										break;
									}
									else
									{
										if (text2.ToUpper().Contains("/SPEED"))
										{
											item13 = 3;
											break;
										}
										if (text2.ToUpper().Contains("SPEED"))
										{
											string text15 = "AMT=";
											int num14 = text2.ToUpper().IndexOf(text15) + text15.Length;
											int num15 = num14;
											while (num15 < text2.Length && (char.IsDigit(text2[num15]) || text2[num15] == '.'))
											{
												num15++;
											}
											item13 = int.Parse(text2.Substring(num14, num15 - num14));
											break;
										}
										break;
									}
								}
							}
						}
					}
				}
				if (i < inputText.Length)
				{
					if (!char.IsControl(inputText[i]) && inputText[i] != ' ')
					{
						list.Add(item);
						list2.Add(item2);
						list3.Add(item3);
						list4.Add(item4);
						list5.Add(item5);
						list6.Add(item6);
						list7.Add(item7);
						list8.Add(item8);
						list9.Add(item9);
						list10.Add(item10);
						list11.Add(item11);
						list12.Add(item12);
						list13.Add(item13);
					}
					text += inputText[i].ToString();
				}
			}
			this.shakesEnabled = list.ToArray();
			this.shakeVelocities = list2.ToArray();
			this.shakeIndependencies = list3.ToArray();
			this.wavesEnabled = list4.ToArray();
			this.waveVelocities = list5.ToArray();
			this.waveSpeeds = list6.ToArray();
			this.waveSeparations = list7.ToArray();
			this.waveIndependencies = list8.ToArray();
			this.wigglesEnabled = list9.ToArray();
			this.wiggleVelocities = list10.ToArray();
			this.wiggleSpeeds = list11.ToArray();
			this.wiggleIndependencies = list12.ToArray();
			this.scrollSpeeds = list13.ToArray();
			return text;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x000367D3 File Offset: 0x000349D3
		private void Start()
		{
			this.UpdateText(null);
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x000367DC File Offset: 0x000349DC
		public void BeginAnimation(string text = null)
		{
			this.UpdateText(text);
			this.currentFrame = 0;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x000367EC File Offset: 0x000349EC
		private Vector3 ShakeVector(float amount)
		{
			return new Vector3(UnityEngine.Random.Range(-amount, amount), UnityEngine.Random.Range(-amount, amount));
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00036803 File Offset: 0x00034A03
		private Vector3 WaveVector(float amount, float time)
		{
			return new Vector3(0f, Mathf.Sin(time) * amount);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00036818 File Offset: 0x00034A18
		private Vector3 WiggleVector(float amount, float speed, ref int i)
		{
			this.wiggleTimeCurrent[i * 2] += speed;
			if (this.wiggleTimeTotal[i * 2] == 0f || this.wiggleTimeCurrent[i * 2] / this.wiggleTimeTotal[i * 2] >= 1f)
			{
				this.wiggleTimeCurrent[i * 2] -= this.wiggleTimeTotal[i * 2];
				this.wiggleTimeTotal[i * 2] = UnityEngine.Random.Range(this.wiggleMinimumDuration, 1f);
				this.wigglePrevPos[i * 2] = this.wiggleTargetPos[i * 2];
				this.wiggleTargetPos[i * 2] = UnityEngine.Random.Range(-amount, amount);
			}
			this.wiggleTimeCurrent[i * 2 + 1] += speed;
			if (this.wiggleTimeTotal[i * 2 + 1] == 0f || this.wiggleTimeCurrent[i * 2 + 1] / this.wiggleTimeTotal[i * 2 + 1] >= 1f)
			{
				this.wiggleTimeCurrent[i * 2 + 1] -= this.wiggleTimeTotal[i * 2 + 1];
				this.wiggleTimeTotal[i * 2 + 1] = UnityEngine.Random.Range(this.wiggleMinimumDuration, 1f);
				this.wigglePrevPos[i * 2 + 1] = this.wiggleTargetPos[i * 2 + 1];
				this.wiggleTargetPos[i * 2 + 1] = UnityEngine.Random.Range(-amount, amount);
			}
			Vector3 result = new Vector3(Mathf.SmoothStep(this.wigglePrevPos[i * 2], this.wiggleTargetPos[i * 2], this.wiggleTimeCurrent[i * 2] / this.wiggleTimeTotal[i * 2]), Mathf.SmoothStep(this.wigglePrevPos[i * 2 + 1], this.wiggleTargetPos[i * 2 + 1], this.wiggleTimeCurrent[i * 2 + 1] / this.wiggleTimeTotal[i * 2 + 1]));
			i++;
			return result;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x000369FC File Offset: 0x00034BFC
		private void Update()
		{
			Vector3 a = default(Vector3);
			Vector3 a2 = default(Vector3);
			Vector3 a3 = default(Vector3);
			for (int i = 0; i < this.TMProGUI.textInfo.meshInfo.Length; i++)
			{
				int num = 0;
				float d = 1f;
				if (this.shakeVelocities.Length > num)
				{
					d = this.shakeVelocities[num];
				}
				int num2 = 0;
				TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType = this.shakeIndependency;
				if (textMeshAnimator_IndependencyType == TextMeshAnimator_IndependencyType.United)
				{
					a = this.ShakeVector(this.shakeAmount);
				}
				float d2 = 1f;
				if (this.waveVelocities.Length > num)
				{
					d2 = this.waveVelocities[num];
				}
				float num3 = 1f;
				if (this.waveSpeeds.Length > num)
				{
					num3 = this.waveSpeeds[num];
				}
				int num4 = 0;
				TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType2 = this.waveIndependency;
				if (textMeshAnimator_IndependencyType2 == TextMeshAnimator_IndependencyType.United)
				{
					a2 = this.WaveVector(this.waveAmount, (float)this.currentFrame * (this.waveSpeed * num3));
				}
				float d3 = 1f;
				if (this.wiggleVelocities.Length > num)
				{
					d3 = this.wiggleVelocities[num];
				}
				float num5 = 1f;
				if (this.wiggleSpeeds.Length > num)
				{
					num5 = this.wiggleSpeeds[num];
				}
				int num6 = 0;
				int num7 = 0;
				TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType3 = this.wiggleIndependency;
				if (textMeshAnimator_IndependencyType3 == TextMeshAnimator_IndependencyType.United)
				{
					a3 = this.WiggleVector(this.wiggleAmount, this.wiggleSpeed * num5, ref num7);
				}
				int j = 0;
				while (j < this.TMProGUI.textInfo.meshInfo[i].vertices.Length)
				{
					for (byte b = 0; b < 4; b += 1)
					{
						this.TMProGUI.textInfo.meshInfo[i].vertices[j + (int)b] = this.vertex_Base[i][j + (int)b];
					}
					TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType4 = textMeshAnimator_IndependencyType;
					if (num < this.shakeIndependencies.Length)
					{
						textMeshAnimator_IndependencyType = this.shakeIndependencies[num];
					}
					if (num >= 1 && num < this.shakeIndependencies.Length + 1)
					{
						textMeshAnimator_IndependencyType4 = this.shakeIndependencies[num - 1];
					}
					if (textMeshAnimator_IndependencyType == TextMeshAnimator_IndependencyType.Word && num2 < this.TMProGUI.text.Length && (this.TMProGUI.text[num2] == ' ' || char.IsControl(this.TMProGUI.text[num2]) || textMeshAnimator_IndependencyType4 != TextMeshAnimator_IndependencyType.Word || num2 == 0))
					{
						a = this.ShakeVector(this.shakeAmount);
						if (this.TMProGUI.text[num2] == ' ' || char.IsControl(this.TMProGUI.text[num2]))
						{
							num2++;
						}
					}
					num2++;
					bool flag = false;
					if (this.shakesEnabled.Length > num)
					{
						flag = this.shakesEnabled[num];
					}
					if (flag)
					{
						if (this.shakeVelocities.Length > num)
						{
							d = this.shakeVelocities[num];
						}
						if (textMeshAnimator_IndependencyType == TextMeshAnimator_IndependencyType.Character)
						{
							a = this.ShakeVector(this.shakeAmount);
						}
						for (byte b2 = 0; b2 < 4; b2 += 1)
						{
							if (textMeshAnimator_IndependencyType == TextMeshAnimator_IndependencyType.Vertex)
							{
								a = this.ShakeVector(this.shakeAmount);
							}
							this.TMProGUI.textInfo.meshInfo[i].vertices[j + (int)b2] += a * d;
						}
					}
					if (this.waveSpeeds.Length > num)
					{
						num3 = this.waveSpeeds[num];
					}
					float num8 = this.waveSeparation;
					if (this.waveSeparations.Length > num)
					{
						num8 = this.waveSeparations[num];
					}
					TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType5 = textMeshAnimator_IndependencyType2;
					if (num < this.waveIndependencies.Length)
					{
						textMeshAnimator_IndependencyType2 = this.waveIndependencies[num];
					}
					if (num >= 1 && num < this.waveIndependencies.Length + 1)
					{
						textMeshAnimator_IndependencyType5 = this.waveIndependencies[num - 1];
					}
					if (textMeshAnimator_IndependencyType2 == TextMeshAnimator_IndependencyType.Word && num4 < this.TMProGUI.text.Length && (this.TMProGUI.text[num4] == ' ' || char.IsControl(this.TMProGUI.text[num4]) || textMeshAnimator_IndependencyType5 != TextMeshAnimator_IndependencyType.Word || num4 == 0))
					{
						a2 = this.WaveVector(this.waveAmount, (float)this.currentFrame * (this.waveSpeed * num3) + this.waveSpeed * num3 + this.TMProGUI.textInfo.meshInfo[i].vertices[j].x / (this.waveSeparation * num8));
						if (this.TMProGUI.text[num4] == ' ' || char.IsControl(this.TMProGUI.text[num4]))
						{
							num4++;
						}
					}
					num4++;
					bool flag2 = false;
					if (this.wavesEnabled.Length > num)
					{
						flag2 = this.wavesEnabled[num];
					}
					if (flag2)
					{
						if (this.waveVelocities.Length > num)
						{
							d2 = this.waveVelocities[num];
						}
						if (textMeshAnimator_IndependencyType2 == TextMeshAnimator_IndependencyType.Character)
						{
							a2 = this.WaveVector(this.waveAmount, (float)this.currentFrame * (this.waveSpeed * num3) + this.TMProGUI.textInfo.meshInfo[i].vertices[j].x / (this.waveSeparation * num8));
						}
						for (byte b3 = 0; b3 < 4; b3 += 1)
						{
							if (textMeshAnimator_IndependencyType2 == TextMeshAnimator_IndependencyType.Vertex)
							{
								a2 = this.WaveVector(this.waveAmount, (float)this.currentFrame * (this.waveSpeed * num3) + this.TMProGUI.textInfo.meshInfo[i].vertices[j + (int)b3].x / (this.waveSeparation * num8));
							}
							this.TMProGUI.textInfo.meshInfo[i].vertices[j + (int)b3] += a2 * d2;
						}
					}
					num5 = this.wiggleSpeed;
					if (this.wiggleSpeeds.Length > num)
					{
						num5 = this.wiggleSpeeds[num];
					}
					TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType6 = textMeshAnimator_IndependencyType3;
					if (num < this.wiggleIndependencies.Length)
					{
						textMeshAnimator_IndependencyType3 = this.wiggleIndependencies[num];
					}
					if (num >= 1 && num < this.wiggleIndependencies.Length + 1)
					{
						textMeshAnimator_IndependencyType6 = this.wiggleIndependencies[num - 1];
					}
					if (textMeshAnimator_IndependencyType3 == TextMeshAnimator_IndependencyType.Word && num6 < this.TMProGUI.text.Length && (this.TMProGUI.text[num6] == ' ' || char.IsControl(this.TMProGUI.text[num6]) || textMeshAnimator_IndependencyType6 != TextMeshAnimator_IndependencyType.Word || num6 == 0))
					{
						a3 = this.WiggleVector(this.wiggleAmount, this.wiggleSpeed * num5, ref num7);
						if (this.TMProGUI.text[num6] == ' ' || char.IsControl(this.TMProGUI.text[num6]))
						{
							num6++;
						}
					}
					num6++;
					bool flag3 = false;
					if (this.wigglesEnabled.Length > num)
					{
						flag3 = this.wigglesEnabled[num];
					}
					if (flag3)
					{
						if (this.wiggleVelocities.Length > num)
						{
							d3 = this.wiggleVelocities[num];
						}
						if (textMeshAnimator_IndependencyType3 == TextMeshAnimator_IndependencyType.Character)
						{
							a3 = this.WiggleVector(this.wiggleAmount, this.wiggleSpeed * num5, ref num7);
						}
						for (byte b4 = 0; b4 < 4; b4 += 1)
						{
							if (textMeshAnimator_IndependencyType3 == TextMeshAnimator_IndependencyType.Vertex)
							{
								a3 = this.WiggleVector(this.wiggleAmount, this.wiggleSpeed * num5, ref num7);
							}
							this.TMProGUI.textInfo.meshInfo[i].vertices[j + (int)b4] += a3 * d3;
						}
					}
					if (j / 4 + 1 > this.charsVisible)
					{
						for (int k = 0; k < 4; k++)
						{
							this.TMProGUI.textInfo.meshInfo[i].vertices[j + k] = Vector3.zero;
						}
					}
					j += 4;
					num++;
				}
				this.TMProGUI.UpdateVertexData();
			}
			this.currentFrame++;
		}

		// Token: 0x04000F2F RID: 3887
		public int currentFrame;

		// Token: 0x04000F30 RID: 3888
		public bool useCustomText;

		// Token: 0x04000F31 RID: 3889
		public string customText;

		// Token: 0x04000F32 RID: 3890
		public float shakeAmount = 1f;

		// Token: 0x04000F33 RID: 3891
		public TextMeshAnimator_IndependencyType shakeIndependency = TextMeshAnimator_IndependencyType.Character;

		// Token: 0x04000F34 RID: 3892
		public float waveAmount = 1f;

		// Token: 0x04000F35 RID: 3893
		public float waveSpeed = 1f;

		// Token: 0x04000F36 RID: 3894
		public float waveSeparation = 1f;

		// Token: 0x04000F37 RID: 3895
		public TextMeshAnimator_IndependencyType waveIndependency = TextMeshAnimator_IndependencyType.Vertex;

		// Token: 0x04000F38 RID: 3896
		public float wiggleAmount = 1f;

		// Token: 0x04000F39 RID: 3897
		public float wiggleSpeed = 0.125f;

		// Token: 0x04000F3A RID: 3898
		public float wiggleMinimumDuration = 0.5f;

		// Token: 0x04000F3B RID: 3899
		public TextMeshAnimator_IndependencyType wiggleIndependency = TextMeshAnimator_IndependencyType.Character;

		// Token: 0x04000F3C RID: 3900
		private TextMeshProUGUI TMProGUI;

		// Token: 0x04000F3D RID: 3901
		private Vector3[][] vertex_Base;

		// Token: 0x04000F3E RID: 3902
		private bool[] shakesEnabled;

		// Token: 0x04000F3F RID: 3903
		private float[] shakeVelocities;

		// Token: 0x04000F40 RID: 3904
		private TextMeshAnimator_IndependencyType[] shakeIndependencies;

		// Token: 0x04000F41 RID: 3905
		private bool[] wavesEnabled;

		// Token: 0x04000F42 RID: 3906
		private float[] waveVelocities;

		// Token: 0x04000F43 RID: 3907
		private float[] waveSpeeds;

		// Token: 0x04000F44 RID: 3908
		private float[] waveSeparations;

		// Token: 0x04000F45 RID: 3909
		private TextMeshAnimator_IndependencyType[] waveIndependencies;

		// Token: 0x04000F46 RID: 3910
		private bool[] wigglesEnabled;

		// Token: 0x04000F47 RID: 3911
		private float[] wiggleVelocities;

		// Token: 0x04000F48 RID: 3912
		private float[] wiggleSpeeds;

		// Token: 0x04000F49 RID: 3913
		private float[] wigglePrevPos;

		// Token: 0x04000F4A RID: 3914
		private float[] wiggleTargetPos;

		// Token: 0x04000F4B RID: 3915
		private float[] wiggleTimeCurrent;

		// Token: 0x04000F4C RID: 3916
		private float[] wiggleTimeTotal;

		// Token: 0x04000F4D RID: 3917
		private TextMeshAnimator_IndependencyType[] wiggleIndependencies;

		// Token: 0x04000F4E RID: 3918
		[SerializeField]
		public int charsVisible;

		// Token: 0x04000F4F RID: 3919
		[SerializeField]
		public int[] scrollSpeeds;

		// Token: 0x020003DA RID: 986
		public struct TextSpeedItem
		{
			// Token: 0x04000F50 RID: 3920
			public int speed;

			// Token: 0x04000F51 RID: 3921
			public int index;
		}
	}
}
