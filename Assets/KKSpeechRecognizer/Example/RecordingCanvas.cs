using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KKSpeech;
using UnityEngine.Android;

public class RecordingCanvas : MonoBehaviour
{

    public Button startRecordingButton;
    public Text resultText;
    private AuthorizationStatus status;
    AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip audioClip2;

    void Start()
    {

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
            switch (status)
            {
                case AuthorizationStatus.Authorized:
                    startRecordingButton.enabled = true;
                    break;
                default:
                    startRecordingButton.enabled = false;
                    resultText.text = "Cannot use Speech Recognition, authorization status is " + status;
                    break;
            }
        }
#endif
        if (SpeechRecognizer.ExistsOnDevice())
        {
            SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
            listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
            listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
            listener.onErrorDuringRecording.AddListener(OnError);
            listener.onErrorOnStartRecording.AddListener(OnError);
            listener.onFinalResults.AddListener(OnFinalResult);
            listener.onPartialResults.AddListener(OnPartialResult);
            listener.onEndOfSpeech.AddListener(OnEndOfSpeech);
            startRecordingButton.enabled = false;
            SpeechRecognizer.RequestAccess();

        }
        else
        {
            resultText.text = "Sorry, but this device doesn't support speech recognition";
            startRecordingButton.enabled = false;
        }

    }

    public void OnFinalResult(string result)
    {
        resultText.text = result;

        if  (result.Equals("hello"))
        {
            GetComponent<AudioSource>().clip = audioClip;
            GetComponent<AudioSource>().Play();
            Debug.Log("started");
        }
        else
        {
            if (result.Equals("good morning"))

                GetComponent<AudioSource>().clip = audioClip2;
                GetComponent<AudioSource>().Play();
                Debug.Log("started");

        }

    }
    public void OnPartialResult(string result)
    {
        resultText.text = result;

        }

    public void OnAvailabilityChange(bool available)
    {
        startRecordingButton.enabled = available;
        if (!available)
        {
            resultText.text = "Speech Recognition not available";
        }
        else
        {
            resultText.text = "Say something :-)";
        }
    }

    public void OnAuthorizationStatusFetched(AuthorizationStatus status)
    {
        switch (status)
        {
            case AuthorizationStatus.Authorized:
                startRecordingButton.enabled = true;
                break;
            default:
                startRecordingButton.enabled = false;
                resultText.text = "Cannot use Speech Recognition, authorization status is " + status;
                break;
        }
    }

    public void OnEndOfSpeech()
    {

        startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
    }

    public void OnError(string error)
    {
        Debug.LogError(error);
        resultText.text = "Something went wrong... Try again! \n [" + error + "]";
        startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
    }

    public void OnStartRecordingPressed()
    {
       
        if (SpeechRecognizer.IsRecording())
        {
            SpeechRecognizer.StopIfRecording();
            startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
        }
        else
        {
            SpeechRecognizer.StartRecording(true);
            startRecordingButton.GetComponentInChildren<Text>().text = "Stop Recording";
            resultText.text = "Say something :-)";





        }
    }
}

