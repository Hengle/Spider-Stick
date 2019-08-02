// class: SCR_Tutorial
using UnityEngine;
using UnityEngine.UI;

public class SCR_Tutorial : MonoBehaviour
{
	public Image imgDarken;

	public Image imgTutorial;

	private float startAlphaDarken;

	private float startAlphaTutorial;

	public void Start()
	{
		startAlphaDarken = imgDarken.color.a;
		startAlphaTutorial = imgTutorial.color.a;
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void FadeOut()
	{
		iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.25f, "onupdate", "UpdateAlpha", "oncomplete", "Hide"));
	}

	public void UpdateAlpha(float alpha)
	{
		imgDarken.color = new Color(imgDarken.color.r, imgDarken.color.g, imgDarken.color.b, alpha * startAlphaDarken);
		imgTutorial.color = new Color(imgTutorial.color.r, imgTutorial.color.g, imgTutorial.color.b, alpha * startAlphaTutorial);
	}
}
