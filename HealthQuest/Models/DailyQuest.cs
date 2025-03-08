using System.ComponentModel;

namespace HealthQuest.Models
{
    class DailyQuest : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Id { get; set; }
        private int _pushups;
        public int Pushups
        {
            get { return _pushups; }
            set
            {
                _pushups = value;
                OnPropertyChanged(nameof(Pushups));
            }
        }

        private int _sitUps;
        public int SitUps
        {
            get { return _sitUps; }
            set
            {
                _sitUps = value;
                OnPropertyChanged(nameof(SitUps));
            }
        }

        private int _squats;
        public int Squats
        {
            get { return _squats; }
            set
            {
                _squats = value;
                OnPropertyChanged(nameof(Squats));
            }
        }

        private int _walk;
        public int Walk
        {
            get { return _walk; }
            set
            {
                _walk = value;
                OnPropertyChanged(nameof(Walk));
            }
        }
    }
}
