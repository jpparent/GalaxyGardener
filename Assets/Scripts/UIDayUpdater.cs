using TMPro;
using UnityEngine;

public class UIDayUpdater : MonoBehaviour
{
	public TMP_Text m_dayCountText;

	// Start is called before the first frame update
	private void Start()
	{

	}

	public void UpdateUI( int day )
	{
		m_dayCountText.text = day.ToString();
	}
}
