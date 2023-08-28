using RPGCreateNow_Local.UseCase;
using UnityEngine;

namespace RPGCreateNow_Local.System
{
    public class StatusSetthingButtonMark : MonoBehaviour, IStatusSetthingButtonMark
    {
        [SerializeField]
        int statusNumber;

        public int MyNumber()
        {
            return statusNumber;
        }
    }
}