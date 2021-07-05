using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesApi.Data.Entities
{
    public class Hero
    {
        public Guid Id { get; set; }
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public bool IsAttacker { get; set; }
        public bool IsDefender { get; set; }
        public DateTime? FirstTrainingDate { get; set; }
        public DateTime? LastTrainingDate { get; set; }
        public int NumTrainingAtLastDate { get; set; }
        public int NumTrainingToday { get; set; }
        public string SuitPart1Color { get; set; }
        public string SuitPart2Color { get; set; }
        public string SuitPart3Color { get; set; }
        public double StartingPower { get; set; }
        public double CurrentPower { get; set; }
    }
}
