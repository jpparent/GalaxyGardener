using TMPro;
using UnityEngine;

public class UIStomachUpdater : MonoBehaviour
{
	public TMP_Text m_stomachCountText;

	public void UpdateUI( int count )
	{
		m_stomachCountText.text = count.ToString();
	}
}
