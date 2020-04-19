using UnityEngine;
using UnityEngine.Events;

public class DayManager : MonoBehaviour
{
	public event UnityAction<int> EndDayEvent;
	public event UnityAction<int> BeginDayEvent;

	private int m_currentDay = 0;

	// Start is called before the first frame update
	private void Start()
	{

	}

	// Update is called once per frame
	private void Update()
	{

	}

	private void EndDay()
	{
		EndDayEvent?.Invoke(m_currentDay);

		// TODO: fade to black or something

		m_currentDay++;

		BeginDayEvent?.Invoke(m_currentDay);
	}
}
