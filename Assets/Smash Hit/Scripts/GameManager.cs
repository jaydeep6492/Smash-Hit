using System.Collections.Generic;
using UMX.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Smash_Hit.Scripts
{
    public class GameManager : Manager
    {
        #region Private Field

        [SerializeField] private GameObject m_GameOver;
        [SerializeField] private GameObject m_MainMenu;
        [SerializeField] private GameObject m_Pyramid;
        
        [SerializeField] private Transform m_Level;

        [SerializeField] private Text m_HighScore;
        [SerializeField] private Text m_PlayerScore;
        [SerializeField] private Text m_FPS;
        [SerializeField] private Text m_BallCountText;
        [SerializeField] private Text m_RewardText;
        
        [SerializeField] private CameraWork m_Camera;
        [SerializeField] public BallShoot m_Ball;

        [SerializeField] public Vector3 m_BallIntialize;
        
        

        private int m_NumberOfBall;
        private float m_HardnessTime;

        #endregion

        #region Public Field

        public int score;

        public float ballSpeedMultiply;

        public List<Vector3> obstaclesPosition;

        public static bool m_IsMainMenu = true;

        #endregion

        #region Getter/Setter

        public int NumberOfBall
        {
            get => m_NumberOfBall;
            set => m_NumberOfBall = value;
        }

        #endregion

        #region Unity Callbacks

        public override void Awake()
        {
            base.Awake();
            // Application.targetFrameRate = 60;
            NumberOfBall = 15;
            m_HighScore.text += PlayerPrefs.GetInt("Highscore");
        }

        private void Start()
        {
            m_MainMenu.SetActive(m_IsMainMenu);
            ShowBalls();
        }

        // Update is called once per frame
        void Update()
        {
            if (!m_Camera.followCamera)
                return;
            if (m_HardnessTime > 10f)
            {
                ballSpeedMultiply += 0.2f;
                m_Camera.cameraSpeed += 0.5f;
                m_HardnessTime = 0;
                score += 2;
            }
            if (Input.GetMouseButtonDown(0) && NumberOfBall > 0)
            {
                NumberOfBall -= 1;
                var ball = Instantiate(m_Ball, m_Camera.transform.position + m_BallIntialize, Quaternion.identity);
                ball.GetComponent<BallShoot>().SetPath(Input.mousePosition, ballSpeedMultiply);
                ShowBalls();
            }
            m_HardnessTime += Time.deltaTime;
            m_FPS.text = "FPS - " + 1 / Time.deltaTime;
        }

        #endregion

        #region Private Methods

        public void ShowBalls()
        {
            m_BallCountText.text = m_NumberOfBall.ToString();
        }

        private void RewardAnimation(string points)
        {
            if (m_RewardText.gameObject.activeSelf)
            {
                m_RewardText.gameObject.SetActive(false);
            }

            m_RewardText.text = points + " Balls";
            m_RewardText.gameObject.SetActive(true);
            GameManager.ShowBalls();
        }

        public void GameOver()
        {
            if (score > PlayerPrefs.GetInt("Highscore"))
            {
                PlayerPrefs.SetInt("Highscore", score);
                m_HighScore.text += PlayerPrefs.GetInt("Highscore");
            }
            
            m_Camera.followCamera = false;
           DataManager.SendLeaderboard(score);
           Camera.main.GetComponent<SphereCollider>().enabled = false;
            m_PlayerScore.text += score;
            m_GameOver.SetActive(true);
        }

        #endregion

        #region Public Methods

        public void Points(int points)
        {
            if (NumberOfBall <= points*(-1))
            {
                Debug.Log("Called Minus");
                NumberOfBall = 0;
                ShowBalls();
                GameOver();
                return;
            }

            NumberOfBall += points;
            if (points == -10)
            {
                RewardAnimation(points.ToString());
            }
            else
            {
                RewardAnimation("+" + points);
            }
        }

        public void FillObstacles()
        {
            if (obstaclesPosition.Count <= 0)
                return;
            var obstacles = Random.Range(0, obstaclesPosition.Count);
            var pos = obstaclesPosition;
            for (var i = 0; i <= obstacles; i++)
            {
                Instantiate(m_Pyramid, obstaclesPosition[i], Quaternion.identity, m_Level.transform);
                pos.RemoveAt(i);
                obstacles = Random.Range(0, obstaclesPosition.Count);
            }

            obstaclesPosition = pos;
        }

        #region UI Button Methods

        public void PlayGame(GameObject btn)
        {
            btn.transform.parent.gameObject.SetActive(false);
            m_Camera.followCamera = true;
        }

        public void PauseGame()
        {
            m_Camera.followCamera = false;
            m_MainMenu.transform.GetChild(3).gameObject.SetActive(true);
            m_MainMenu.SetActive(true);
        }

        public void RestartGame()
        {
            m_IsMainMenu = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
        
        #endregion
    
        #endregion
    }
}
