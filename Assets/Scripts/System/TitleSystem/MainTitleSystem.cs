using UnityEngine;

namespace RPGCreateNow_Local.System
{
    public class MainTitleSystem : MonoBehaviour
    {
        private void Update()
        {
            // クリックされたらシーンの切り替えを行う
            if (Input.GetMouseButtonDown(0))
            {
                SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();
                sceneChangeSystem.SceneChange(UseCase.SceneNameS.Loading);
            }
        }
    }
}