using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UMX.Managers;
using UnityEngine.UI;

public class DataManager : Manager
{
    
    #region private Fields
    
    [SerializeField] private InputField m_LoginUsername;
    [SerializeField] private InputField m_LoginPassword;
    
    [SerializeField]private GameObject m_Login;
    
    private string m_Email;
    private string m_UserName;
    private string m_Password;
    
    #endregion
    
    #region Public Fields

    public Text messageText;

    public string UserName
    {
        get => m_UserName;
        set => m_UserName = value;
    }

    public string Password
    {
        get => m_Password;
        set => m_Password = value;
    }

    public string Email
    {
        get => m_Email;
        set => m_Email = value;
    }

    #endregion

    #region Unity Callbacks
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("UserName"))
        {
            UserName = PlayerPrefs.GetString("UserName");
            Password = PlayerPrefs.GetString("Password");
            OnClickLoginButton();
        }
        else
        {
            LoginWithAndroidDevice();
        }
    }
    
    #endregion
    
    #region PlayFab
    
    #region Loging With AndroidDeviceID
    private void LoginWithAndroidDevice()
    {
        var request = new LoginWithAndroidDeviceIDRequest()
        {
            AndroidDeviceId =  SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(request,OnSuccess,OnError);
    }
    #endregion

    #region OnSuccess / OnError

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create!");
        GetAcountInfo();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while  logging  in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    #endregion

    #region Leaderboard

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlateFormScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request,OnLeaderboardUpdate,OnError);
    }
    
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult results)
    {
        Debug.Log("Successful Leader Board Send");
    }
    
    void GetAcountInfo()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request,SuccesssLeaderboard, OnError);
        
    }
    void SuccesssLeaderboard(GetAccountInfoResult result)
    {      
		
        Debug.Log("............................................ "+result.AccountInfo.PlayFabId);
 
    }
    
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest()
        {
            StatisticName = "PlateFormScore",
            StartPosition = 0,
            MaxResultsCount =  10
        };
        PlayFabClientAPI.GetLeaderboard(request,OnLeaderboardGet,OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position+" "+item.PlayFabId+" "+item.StatValue);
        }
        
    }
    
    #endregion

    #region OnRegister
    
    public void OnClickRegisterButton()
    {
        if (Email.Length == 0)
        {
            messageText.text = "Email Address Can Not Be Null";
            return;
        }
        if (UserName.Length == 0)
        {
            messageText.text = "UserName Can Not Be Null";
            return;
        }
        if (Password.Length <= 6)
        {
            messageText.text = "Password Is Too Short";
            return;
        }
        messageText.text = "";
        var add = new AddUsernamePasswordRequest()
        {
            Email = Email, 
            Username =UserName,
            Password = Password, 
        };
        PlayFabClientAPI.AddUsernamePassword(add,OnAddSuccess,OnError);
    }
    
    private void OnAddSuccess(AddUsernamePasswordResult obj)
    {
        Debug.Log("Added Successfully TO Play Fab");
        messageText.text = "!!!!!!! Added Successfully To PlayFab !!!!!!!";
        PlayerPrefs.SetString("UserName",UserName);
        PlayerPrefs.SetString("Password",Password);
    }
    
    #endregion

    #region OnLogin

    public void OnClickLoginButton()
    {
        if (UserName.Length == 0)
        {
            messageText.text = "Enter UserName!!!";
            return;
        }
        if (Password.Length <= 6)
        {
            messageText.text = "Check Password";
            return;
        }
        messageText.text = "";
        m_LoginUsername.text = UserName;
        m_LoginPassword.text = Password;
        var request = new LoginWithPlayFabRequest()
        {
            /*Username = m_LoginUsername.text,
            Password = m_LoginPassword.text*/
            Username = UserName,
            Password = Password
        };
        PlayFabClientAPI.LoginWithPlayFab(request,OnLoginSuccess,OnError);
    }

    private void OnLoginSuccess(LoginResult obj)
    {
            messageText.text = "!!! You Are Logged - in !!!"; 
        m_Login.transform.GetChild(2).GetComponent<Button>().interactable = false;
        PlayerPrefs.SetString("UserName",UserName);
        PlayerPrefs.SetString("Password",Password);
        ExecuteCloud();
    }

    #endregion

    #region CloudScript

    public void ExecuteCloud()
    {
        var request = new ExecuteCloudScriptRequest()
        {
            FunctionName = "helloWorld",
            FunctionParameter = new
            {
                name = UserName
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request,OnExecuteSuccess,OnError);
    }

    private void OnExecuteSuccess(ExecuteCloudScriptResult obj)
    {
        Debug.Log("##### "+obj.FunctionResult + " #####");
    }

    #endregion
    
    #endregion
}

