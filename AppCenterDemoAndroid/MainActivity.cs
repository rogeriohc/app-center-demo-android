using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using System;
using System.Collections.Generic;

namespace AppCenterDemoAndroid
{
    [Activity(Label = "App Center Demo Android", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //App Center Analytics and Crashes 
            //AppCenter.Start("88d2c395-566e-46e1-a5ef-a8ce69e2c7f2", typeof(Analytics), typeof(Crashes));

            //App Center Push
            if (!AppCenter.Configured)
            {
                Push.PushNotificationReceived += (sender, e) =>
                {
                    // Add the notification message and title to the message
                    var summary = $"Push notification received:" +
                                        $"\n\tNotification title: {e.Title}" +
                                        $"\n\tMessage: {e.Message}";

                    // If there is custom data associated with the notification,
                    // print the entries
                    if (e.CustomData != null)
                    {
                        summary += "\n\tCustom data:\n";
                        foreach (var key in e.CustomData.Keys)
                        {
                            summary += $"\t\t{key} : {e.CustomData[key]}\n";
                        }
                    }

                    TextView text = FindViewById<TextView>(Resource.Id.textPush);
                    text.Text = summary;

                    // Send the notification summary to debug output
                    System.Diagnostics.Debug.WriteLine(summary);

                };
            }

            AppCenter.Start("88d2c395-566e-46e1-a5ef-a8ce69e2c7f2", typeof(Push), typeof(Analytics), typeof(Crashes));

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);
            button.Click += delegate { button.Text = $"{count++} clicks!"; Analytics.TrackEvent($"{count++} clicks!"); };

            Button buttonEvent = FindViewById<Button>(Resource.Id.buttonEvent);
            buttonEvent.Click += delegate { Analytics.TrackEvent("buttonEvent acionado!"); };

            Button buttonGenerateTestCrash = FindViewById<Button>(Resource.Id.buttonGenerateTestCrash);
            buttonGenerateTestCrash.Click += delegate { Crashes.GenerateTestCrash(); };

            Button buttonCrash = FindViewById<Button>(Resource.Id.buttonCrash);
            Exception exception = new Exception("Crash!");
            var properties = new Dictionary<string, string> {{ "Category", "Music" },{ "Wifi", "On" }};
            buttonCrash.Click += delegate { Crashes.TrackError(exception,properties);};

            

        }
    }
}

