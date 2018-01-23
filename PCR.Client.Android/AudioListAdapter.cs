using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PCR.Common;

namespace PCR.Client.Android 
{
    public class AudioListAdapter : BaseAdapter<AppDetails>
    {
        Activity _activity;
        List<AppDetails> _appDefinitions;
        private string _appUrl;

        public AudioListAdapter(Activity activity, List<AppDetails> appDefinitions, string url):base()
        {
            this._activity = activity;
            this._appDefinitions = appDefinitions;
            this._appUrl = url;
        }

        public override int Count
        {
            get { return _appDefinitions.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override AppDetails this[int index]
        {
            get { return _appDefinitions[index]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.ListItemRow, parent, false);

            var item = this[position];
            view.FindViewById<TextView>(Resource.Id.MixerText).Text = item.App;
            var volume = view.FindViewById<TextView>(Resource.Id.VolumeOutput);
            volume.Text = item.Volume.ToString();
            var seekBarMixer = view.FindViewById<SeekBar>(Resource.Id.MixerVolume);
            seekBarMixer.Progress = item.Volume;
            seekBarMixer.Tag = item.ProcessId;
            var speakerButton = view.FindViewById<ImageView>(Resource.Id.speakerButton);
            speakerButton.Tag = item.ProcessId;
            var speakerButtonMute = view.FindViewById<ImageView>(Resource.Id.speakerButtonMute);
            speakerButtonMute.Tag = item.ProcessId;

            if (item.Mute)
            {
                speakerButtonMute.Visibility = ViewStates.Visible;
                speakerButton.Visibility = ViewStates.Invisible;
            }
            else
            {
                speakerButton.Visibility = ViewStates.Visible;
                speakerButtonMute.Visibility = ViewStates.Invisible;
            }

            speakerButton.Click += (sender, e) =>
            {
                var image = (ImageView)sender;
                var processId = (uint)image.Tag;
                speakerButtonMute.Visibility = ViewStates.Visible;
                speakerButton.Visibility = ViewStates.Invisible;
                Audio.MuteApp(_appUrl, processId);
            };

            speakerButtonMute.Click += (sender, e) =>
            {
                var image = (ImageView)sender;
                var processId = (uint)image.Tag;
                speakerButtonMute.Visibility = ViewStates.Invisible;
                speakerButton.Visibility = ViewStates.Visible;
                Audio.MuteApp(_appUrl, processId);
            };

            seekBarMixer.ProgressChanged += (sender, e) =>
            {
                var mixer = (SeekBar)sender;
                var processId = (uint)mixer.Tag;
                var setVolume = e.SeekBar.Progress;
                volume.Text = setVolume.ToString();

                var message = new AppDetails()
                {
                    ProcessId = processId,
                    Volume = setVolume
                };
                
                Audio.UpdateAppVolume<AppDetails>(_appUrl, message);
            };

            return view;
        }
    }
}