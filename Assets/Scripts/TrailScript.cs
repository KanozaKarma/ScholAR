using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
#if UNITY_EDITOR
// Set up touch input propagation while using Instant Preview in the editor.
using Input = GoogleARCore.InstantPreviewInput;
#endif


public class TrailScript : MonoBehaviour
{

    public GameObject gameObject;
    public GameObject camera;
    public Button DrawButton;
    public List<GameObject> Points = new List<GameObject>();

    bool resume = false;

    private bool m_IsQuitting = false;

    // Update is called once per frame
    void Update()
    {
        _UpdateApplicationLifecycle();

        DrawButton.onClick.AddListener(EnableDrawing);

        if (resume == true)
        {
            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                Debug.Log("Touched");
                Vector3 camPos = camera.transform.position;
                Vector3 camDirection = camera.transform.forward;
                Quaternion camRotation = camera.transform.rotation;
                float spawnDistance = 2;
                Debug.Log("Touched" + camPos.x + " " + camPos.y + " " + camPos.z);
                Vector3 spawnPos = camPos + (camDirection * spawnDistance);
                GameObject cur = Instantiate(gameObject, spawnPos, camRotation);
                cur.transform.SetParent(this.transform);
            }
        }
    }

    public void EnableDrawing()
    {
        if (resume == false)
        {
            resume = true;
        }
        else
        {
            resume = false;
        }
        Update();
    }

    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                    message, 0);
                toastObject.Call("show");
            }));
        }
    }

    private void _UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (m_IsQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage("ARCore encountered a problem connecting. Please start the app again.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void _DoQuit()
    {
        Application.Quit();
    }
}