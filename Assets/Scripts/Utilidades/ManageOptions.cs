using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManageOptions : MonoBehaviour {

	public Text ResolutionText;
	public Text QualityText;
	public GameObject ButtonPrefab;
	public GameObject ParentPanelResolution;
	public GameObject ParentPanelQuality;
	public GameObject FullscreenCB;
	private Resolution MyResolution;
	private Resolution[] Resolutions;
	private string[] QualityNames;

	// Use this for initialization
	void Start () 
	{
		this.ResolutionText.text = Screen.width + "x" + Screen.height;
		this.Resolutions = Screen.resolutions;

		this.QualityNames = QualitySettings.names;
		this.QualityText.text = this.QualityNames[QualitySettings.GetQualityLevel()];

		this.FullscreenCB.GetComponent<Toggle>().isOn = Screen.fullScreen;

		for(int i = 0; i < this.Resolutions.Length; i++)
		{
			GameObject button = Instantiate(this.ButtonPrefab) as GameObject;
			button.GetComponentInChildren<Text>().text = this.ResolutionToString(this.Resolutions[i]);
			
			int index = i;
			button.GetComponent<Button>().onClick.AddListener( () => { this.SetResolution(index);});
			
			button.transform.parent = this.ParentPanelResolution.transform;
		}

		for(int i = 0; i < this.QualityNames.Length; i++)
		{
			GameObject button = Instantiate(this.ButtonPrefab) as GameObject;
			button.GetComponentInChildren<Text>().text = this.QualityNames[i];

			int index = i;
			button.GetComponent<Button>().onClick.AddListener( () => { this.SetQuality(index);});

			button.transform.parent = this.ParentPanelQuality.transform;
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetResolution(int index)
	{
		Screen.SetResolution(Resolutions[index].width, Resolutions[index].height, Screen.fullScreen);
		this.ResolutionText.text = this.ResolutionToString(this.Resolutions[index]);
	}

	void SetQuality (int index)
	{
		QualitySettings.SetQualityLevel(index);
		this.QualityText.text = this.QualityNames[index];

	}
	
	string ResolutionToString(Resolution res)
	{
		return res.width + "x" + res.height;
	}

	public void SwitchFullscreen()
	{
		Screen.fullScreen = this.FullscreenCB.GetComponent<Toggle>().isOn;
	}
}
