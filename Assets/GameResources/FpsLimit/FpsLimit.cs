using UnityEngine;

namespace GameResources.FpsLimit
{
    public class FpsLimit : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}
