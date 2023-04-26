using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCreateNow_Local.UseCase
{
    public interface ISetPlayerData
    {
        public void SetPlayerStatusData(PlayerStatus_Structure setData);
    }
}
