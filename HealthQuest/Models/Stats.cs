using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.Models
{
    class Stats : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Id { get; set; }

        private int _hp;
        public int Hp
        {
            get { return _hp; }
            set
            {
                _hp = value;
                OnPropertyChanged(nameof(Hp));
            }
        }

        private int _stamina;
        public int Stamina
        {
            get { return _stamina; }
            set
            {
                _stamina = value;
                OnPropertyChanged(nameof(Stamina));
            }
        }

        private int _strenght;
        public int Strenght
        {
            get { return _strenght; }
            set
            {
                _strenght = value;
                OnPropertyChanged(nameof(Strenght));
            }
        }

        private int _agility;
        public int Agility
        {
            get { return _agility; }
            set
            {
                _agility = value;
                OnPropertyChanged(nameof(Agility));
            }
        }

        private int _vigor;
        public int Vigor
        {
            get { return _vigor; }
            set
            {
                _vigor = value;
                OnPropertyChanged(nameof(Vigor));
            }
        }
    }
}
