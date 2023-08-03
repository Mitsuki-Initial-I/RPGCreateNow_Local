using UnityEngine.SceneManagement;
using RPGCreateNow_Local.UseCase;

namespace RPGCreateNow_Local.System
{
    public class SceneChangeSystem
    {
        // �󂯎�����V�[���ԍ��̃V�[���ɐ؂�ւ���
        public void SceneChange(SceneNameS sceneName)
        {
            SceneManager.LoadScene((int)sceneName);
        }
    }
}