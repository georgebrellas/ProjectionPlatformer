using UnityEngine.UI;
using UnityEngine;

public class DebugToggles : MonoBehaviour
{
    public KeyCode FollowCameraToggle;
    public KeyCode FPSCounterToggle;
    public KeyCode QuitGame;
    private FollowScript Follow;
    private bool FPSCounter;
    private int frameCounter= 0;
    private float timeCounter = 0f;
    private float lastFramerate = 0f;
    private float refreshTime = 1f;
    void Start()
    {
        if (!Debug.isDebugBuild)
        {
            GameObject.Find("DebugUI").SetActive(false);
        }
        Follow = GetComponent<FollowScript>();
    }
    void Update()
    {
        if (Input.GetKeyDown(QuitGame))
        {
            Application.Quit();
        }

        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(FPSCounterToggle))
            { 
                if (FPSCounter)
                {
                    FPSCounter = false;
                    GameObject.Find("FPSValue").GetComponent<Text>().text = "Paused";
                } else
                {
                    FPSCounter = true;
                }
            }

            if (Input.GetKeyDown(FollowCameraToggle))
            {
                Follow.enabled = !Follow.enabled;
                if (Follow.enabled)
                {
                    GameObject.Find("FollowCameraValue").GetComponent<Text>().text = "1";
                } else
                {
                    GameObject.Find("FollowCameraValue").GetComponent<Text>().text = "0";
                }
            }

            if (FPSCounter)
            {
                if (timeCounter < refreshTime)
                {
                    timeCounter += Time.deltaTime;
                    frameCounter++;
                }
                else
                {
                    lastFramerate = (float)frameCounter / timeCounter;
                    frameCounter = 0;
                    timeCounter = 0;
                }
                GameObject.Find("FPSValue").GetComponent<Text>().text = lastFramerate.ToString();
            }
        }
    }
}
