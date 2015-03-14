using Android.Media;

namespace KabeDon.XamarinForms.Droid
{
    class AndroidSoundPlayer : ISoundPlayer
    {
        // ���`���l�������Đ��ł���悤�ɂ���H
        MediaPlayer _player = new MediaPlayer();

        public void Play(string path)
        {
            _player.SetDataSource(path);
            _player.Start();
        }
    }
}