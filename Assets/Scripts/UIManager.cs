using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: rename to PresentationManager ?
public class UIManager : MonoBehaviour
{
	public UIDayUpdater m_dayUpdater;
	public UIHealthUpdater m_healthUpdater;
	public DayManager m_dayManager;
	public GardenManager m_gardenManager;

    // Start is called before the first frame update
    void Start()
    {
		m_dayManager = FindObjectOfType<DayManager>();
		m_gardenManager = FindObjectOfType<GardenManager>();

		m_dayUpdater = FindObjectOfType<UIDayUpdater>();
		// TODO register to end day and fade to black briefly maybe ?
		m_dayManager.BeginDayEvent += ( int x ) => { m_dayUpdater.UpdateUI( x ); };

		m_healthUpdater = FindObjectOfType<UIHealthUpdater>();
		m_gardenManager.HealthPointsChangeEvent += ( int x ) => { m_healthUpdater.UpdateUI( x ); };
	}

    // Update is called once per frame
    void Update()
    {

    }
}
