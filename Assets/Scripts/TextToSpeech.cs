using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static DataModel;

public class TextToSpeech
{
    private CultureInfo ci = new CultureInfo("en-us");

    private SpeechConfig config = SpeechConfig.FromSubscription("af3f455ea5f84082b13a2344cdd30128", "westeurope");
    private SpeechSynthesizer synthesizer;

    private float rate;
    private string pitch;

    public TextToSpeech(string voice, string pitch, float rate)
    {
        config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw16Khz16BitMonoPcm);
        config.SpeechSynthesisVoiceName = voice;
        synthesizer = new SpeechSynthesizer(config, null);
        this.rate = rate;
        this.pitch = pitch;
    }

    private string TextToSSML(string text)
    {
        return
            "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\">" +
                "<voice name=\"" + config.SpeechSynthesisVoiceName + "\">" +
                    "<prosody pitch=\"" + pitch + "\" rate=\"" + rate.ToString("F1", ci) + "\">" +
                        text +
                    "</prosody>" +
                "</voice>" +
            "</speak>";
    }

    public void SyntetizeInstruction(string instruction, AudioSource audioSource, UnityAction ready, UnityAction call = null)
    {
        TaskCompletionSource<SpeechSynthesisResult> tcs = new TaskCompletionSource<SpeechSynthesisResult>();
        Task.Run(() => tcs.SetResult(synthesizer.SpeakSsmlAsync(TextToSSML(instruction)).Result));
        tcs.Task.GetAwaiter().OnCompleted(() =>
        {
            SpeechSynthesisResult result = tcs.Task.Result;

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                var sampleCount = result.AudioData.Length / 2;
                var audioData = new float[sampleCount];
                for (var i = 0; i < sampleCount; ++i)
                {
                    audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8 | result.AudioData[i * 2]) / 32768.0F;
                }

                AudioClip audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
                audioClip.SetData(audioData, 0);

                ready.Invoke();
                AudioManager.instance.PlayAudioAndWait(audioSource, audioClip, call);
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                Debug.Log($"CANCELED:\nReason=[{cancellation.Reason}]\nErrorDetails=[{cancellation.ErrorDetails}]\nDid you update the subscription info?");
                SyntetizeInstruction(instruction, audioSource, call);
            }
        });
    }
}


//public class TextToSpeech
//{
//    private AmazonPollyClient client;
//    private SynthesizeSpeechRequest request;

//    private float speechSpeed;

//    async public static Task<TextToSpeech> CreateTextToSpeech(string selectedVoice)
//    {
//        AmazonPollyClient client = new AmazonPollyClient("AKIAWYXBLMVAQIKLH47C", "YajXu4JQr1/knF27btdyM4WOEHuPsCSylNeeOjzC", RegionEndpoint.EUCentral1);
//        DescribeVoicesRequest describeVoicesRequest = new DescribeVoicesRequest();
//        DescribeVoicesResponse describeVoicesResult = await client.DescribeVoicesAsync(describeVoicesRequest);
//        List<Voice> voices = describeVoicesResult.Voices;
//        Voice voice = voices.Find(x => x.Id == VoiceId.Justin);
//        return new TextToSpeech(client, voice);
//    }

//    private TextToSpeech(AmazonPollyClient client, Voice voice)
//    {
//        this.client = client;
//        request = new SynthesizeSpeechRequest
//        {
//            VoiceId = voice.Id,
//            TextType = TextType.Ssml,
//            OutputFormat = Amazon.Polly.OutputFormat.Pcm
//        };
//        speechSpeed = 0.85f;//PlayerPrefs.GetFloat("SpeechSpeed");
//    }

//    private string TextToSSML(string text)
//    {
//        return 
//            "<speak>" +
//                "<prosody rate = \"" + speechSpeed*10 + "%\">" +
//                    text + 
//                "</prosody>" +
//            "</speak>";

//    }

//    public void SyntetizeInstruction(string instruction, AudioSource audioSource, UnityAction call = null)
//    {
//        request.Text = TextToSSML(instruction);

//        TaskCompletionSource<SynthesizeSpeechResponse> tcs = new TaskCompletionSource<SynthesizeSpeechResponse>();
//        Task.Run(() => tcs.SetResult(client.SynthesizeSpeechAsync(request).GetAwaiter().GetResult()));
//        tcs.Task.GetAwaiter().OnCompleted(() =>
//        {
//            SynthesizeSpeechResponse response = tcs.Task.Result;
//            var result = ReadStream(response.AudioStream);

//            int sampleCount = result.Length / 2;
//            float[] audioData = new float[sampleCount];
//            for (var i = 0; i < sampleCount; ++i)
//            {
//                audioData[i] = (short)(result[i * 2 + 1] << 8 | result[i * 2]) / 32768.0F;
//            }

//            AudioClip audioClip = AudioClip.Create("SynthesizedAudio", audioData.Length, 1, 16000, false);
//            audioClip.SetData(audioData, 0);

//            VirtualAssistantManager.instance.ActivateBubble(instruction);
//            AudioManager.instance.PlayAudioAndWait(audioSource, audioClip, call);
//        });
//    }

//    private byte[] ReadStream(Stream input)
//    {
//        using (MemoryStream ms = new MemoryStream())
//        {
//            input.CopyTo(ms);
//            return ms.ToArray();
//        }
//    }
//}