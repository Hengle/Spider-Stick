// class: SCR_ProgressBar
using UnityEngine;
using UnityEngine.UI;

public class SCR_ProgressBar : MonoBehaviour
{
	public Image imgCurrentLevel;

	public Image imgNextLevel;

	public Text txtCurrentLevel;

	public Text txtNextLevel;

	public Image imgProgressBG;

	public Image imgProgressFG;

	public void SetLevel(int level)
	{
		txtCurrentLevel.text = level.ToString();
		txtNextLevel.text = (level + 1).ToString();
	}

	public void SetProgress(float t)
	{
		t = Mathf.Clamp(t, 0f, 1f);
		imgProgressFG.fillAmount = t;
	}

	public void Finish()
	{
		imgNextLevel.color = imgCurrentLevel.color;
	}
}
