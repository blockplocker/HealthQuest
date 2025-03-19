using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.Models
{
    public class Quest : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Reps { get; set; }
        private int _repsDone;
        public int RepsDone
        {
            get => _repsDone;
            set
            {
                if (_repsDone != value)
                {
                    _repsDone = value;
                    OnPropertyChanged(nameof(RepsDone));
                    OnPropertyChanged(nameof(CompletionStatus));
                }
            }
        }
        public string Stat { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public string CompletionStatus => RepsDone >= Reps ? "Complete" : "Incomplete";

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
