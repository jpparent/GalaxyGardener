using TMPro;
using UnityEngine;

public class UIDayUpdater : MonoBehaviour
{
	public TMP_Text m_dayCountText;

	public void UpdateUI( int day )
	{
		m_dayCountText.text = day.ToString();
	}
}
