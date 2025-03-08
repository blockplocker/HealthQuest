using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.Models
{
    internal class Quest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Reps { get; set; }
        public int RepsDone { get; set; }
        public string Stat { get; set; }


        //public int UserId { get; set; }

        // Navigation property
        //public User User { get; set; }
    }
}
