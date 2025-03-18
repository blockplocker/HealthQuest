using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.Models
{
    public class Quest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Reps { get; set; }
        public int RepsDone { get; set; }
        public string Stat { get; set; }


    }
}
