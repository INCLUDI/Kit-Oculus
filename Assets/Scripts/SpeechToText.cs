using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;
using System.Collections;

public class SpeechToText
{
    private SpeechConfig config = SpeechConfig.FromSubscription("af3f455ea5f84082b13a2344cdd30128", "westeurope");
    private SpeechRecognizer recognizer;

    public SpeechToText()
    {
        recognizer = new SpeechRecognizer(config);
    }

    public void StartListening()
    {
        TaskCompletionSource<SpeechRecognitionResult> tcs = new TaskCompletionSource<SpeechRecognitionResult>();
        Task.Run(async () =>
        {
            tcs.SetResult(await recognizer.RecognizeOnceAsync().ConfigureAwait(false));
        });
        tcs.Task.GetAwaiter().OnCompleted(() =>
        {
            SpeechRecognitionResult result = tcs.Task.Result;

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                Debug.Log(result.Text);
                ActivityManager.instance.checkCorrectAction(result.Text);
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                Debug.Log("NOMATCH: Speech could not be recognized.");
                StartListening();
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                CancellationDetails cancellation = CancellationDetails.FromResult(result);
                Debug.Log($"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}");
                StartListening();
            }
        });
    }
}