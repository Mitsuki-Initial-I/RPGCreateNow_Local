using UnityEngine;

namespace RPGCreateNow_Local.System
{
    public class MainTitleSystem : MonoBehaviour
    {
        private void Update()
        {
            // �N���b�N���ꂽ��V�[���̐؂�ւ����s��
            if (Input.GetMouseButtonDown(0))
            {
                SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();
                sceneChangeSystem.SceneChange(UseCase.SceneNameS.Loading);
            }
        }
    }
}