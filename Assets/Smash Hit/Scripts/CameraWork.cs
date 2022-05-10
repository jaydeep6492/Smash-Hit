using UMX.Managers;
using UnityEngine;

namespace Smash_Hit.Scripts
{
    public class CameraWork : MonoBehaviour
    {

        #region Public Fields

        public float cameraSpeed;
        
        public bool followCamera;
    
        #endregion

        #region Unity Callbacks

        private void Start()
        {
            followCamera = !GameManager.m_IsMainMenu;
            transform.position = new Vector3(-3.2f, 4.67f, -2.18f);
        }
        
        private void Update()
        {
            if (!followCamera)
                return;
            if (transform.position.z > 290)
            {
                Manager.GameManager.FillObstacles();
                transform.position = new Vector3(-3.20f, 4.67f, -7.92f);
            }
            transform.position += new Vector3(0, 0, cameraSpeed) * Time.deltaTime;
        }

        #endregion
        
    }
}
