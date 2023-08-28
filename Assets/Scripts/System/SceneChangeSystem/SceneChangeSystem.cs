using UnityEngine.SceneManagement;
using RPGCreateNow_Local.UseCase;

namespace RPGCreateNow_Local.System
{
    public class SceneChangeSystem
    {
        // 受け取ったシーン番号のシーンに切り替える
        public void SceneChange(SceneNames sceneName)
        {
            SceneManager.LoadScene((int)sceneName);
        }
    }
}