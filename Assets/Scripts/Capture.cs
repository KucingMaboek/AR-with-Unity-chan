using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Android;

public class Capture : MonoBehaviour
{
    [SerializeField] private AudioSource shutterSound;
    private void Start()
    {
        // Request Storage Permission
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
    }

    public void TakeScreenshot()
    {
        StartCoroutine(ScreenshotProcessing());
    }

    private IEnumerator ScreenshotProcessing()
    {
        yield return null;

        // Wait for buffering is complete
        yield return new WaitForEndOfFrame();

        // Preparing screenshot file name
        string fileName = "Screenshot" + DateTime.Now.Hour + DateTime.Now.Minute +
                          DateTime.Now.Second + ".png";

        // Preparing a new location
        string defaultLocation = Application.persistentDataPath + "/" + fileName;
        string folderLocation = "/storage/emulated/0/DCIM/Camera/";
        string targetLocation = folderLocation + fileName;

        // Check if the new location exist
        if (!Directory.Exists(folderLocation))
        {
            Directory.CreateDirectory(folderLocation);
        }

        // Take a Screenshot
        ScreenCapture.CaptureScreenshot(fileName);
        shutterSound.Play();

        yield return new WaitForSeconds(1);

        // Move Screenshot to new location
        File.Move(defaultLocation, targetLocation);

        // Refresh gallery to show new screenshot
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent",
            "android.intent.action.MEDIA_MOUNTED",
            classUri.CallStatic<AndroidJavaObject>("parse", "file://" + targetLocation));
        objActivity.Call("sendBroadcast", objIntent);
    }
}