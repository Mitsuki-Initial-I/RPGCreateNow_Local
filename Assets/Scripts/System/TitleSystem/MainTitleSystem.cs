using UnityEngine;

namespace RPGCreateNow_Local.System
{
    public class MainTitleSystem : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();
                sceneChangeSystem.SceneChange(UseCase.SceneNameS.Loading);
            }
        }
    }
}