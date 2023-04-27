using UnityEngine;
using UnityEngine.EventSystems;

namespace RPGCreateNow_Local.System
{
    public class MainHomeSystem : MonoBehaviour
    {
        [SerializeField]
        GameObject SelectBox;
        [SerializeField]
        EventSystem eventSystem;
        HomeButtonSystem homeButtonSystem = new HomeButtonSystem();

        private void Start()
        {
            homeButtonSystem.SelectBox = SelectBox;
            homeButtonSystem.eventSystem = eventSystem;
            homeButtonSystem.SetSelectHomeButton();
        }
    }
}