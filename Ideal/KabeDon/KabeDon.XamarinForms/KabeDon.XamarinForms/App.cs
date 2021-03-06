﻿using KabeDon.Engine;
using KabeDon.Packaging;
using KabeDon.Sound;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading;

using Xamarin.Forms;

namespace KabeDon.XamarinForms
{
    public class App : Application
    {
        private string _serverUrl;
        private IStorage _root;
        private ISoundPlayerFactory _soundFactory;

        public App(string serverUrl = null, IStorage root = null, ISoundPlayerFactory soundFactory = null)
        {
            _serverUrl = serverUrl;
            _root = root;
            _soundFactory = soundFactory;
        }

        CancellationTokenSource _endOfGame = new CancellationTokenSource();

        protected override async void OnStart()
        {
            var m = await PackageManager.LoadAsync(_serverUrl, _root, _soundFactory);
            var vm = new KabeDonViewModel(m);

            var startButton = new Button()
            {
                Text = "開始",
            };
            startButton.Clicked += (sender, e) => vm.StartCommand.Execute(null);
            SetBinding(vm.Engine, nameof(vm.Engine.IsRunning), startButton, (s, t) => t.IsVisible = !s.IsRunning);

            var remainderTimelabel = new Label();
            SetBinding(vm.Engine, nameof(vm.Engine.RemainderTime), remainderTimelabel, (s, t) => t.Text = $"残り時間 {s.RemainderTime.Minutes:00}:{s.RemainderTime.Seconds:00}");

            var scorelabel = new Label();
            SetBinding(vm.Engine, nameof(vm.Engine.Score), scorelabel, (s, t) => t.Text = $"スコア {s.Score}");

            var image = new TouchEventImage();
            image.Touch.Subscribe(p =>
            {
                var pos = new DataModels.Point(
                    (int)(1080 * p.X),
                    (int)(1920 * p.Y));
                vm.TapCommand.Execute(pos);
            });

            var grid = new Grid
            {
                Children =
                    {
                        image,
                        new StackLayout
                        {
                            Children =
                            {
                                startButton,
                                remainderTimelabel,
                                scorelabel,
                            },
                        },
                    },
            };

            var page = new ContentPage { Content = grid };

            MainPage = page;

            SetBinding(vm, nameof(vm.Image), image, (s, t) => t.Source = ImageSource.FromFile(s.Image));

            try
            {
                while (!_endOfGame.IsCancellationRequested)
                {
                    await vm.Engine.ExecuteAsync(_endOfGame.Token);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private static void SetBinding<TSource, TTarget>(TSource vm, string sourceName, TTarget target, Action<TSource, TTarget> bind)
            where TSource : INotifyPropertyChanged
        {
            bind(vm, target);

            vm.PropertyChanged += (sender, arg) =>
            {
                if (arg.PropertyName == sourceName) bind(vm, target);
            };
        }

        protected override void OnSleep()
        {
            // 今のところ、resume 機能なし。スリープした瞬間終了。
            _endOfGame.Cancel();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
