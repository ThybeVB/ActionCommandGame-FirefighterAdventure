﻿using ActionCommandGame.Model.Abstractions;

namespace ActionCommandGame.Model
{
    public class NegativeGameEvent : IIdentifiable, IHasProbability
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefenseWithGearDescription { get; set; }
        public string DefenseWithoutGearDescription { get; set; }
        public int DefenseLoss { get; set; }
        public int Probability { get; set; }
    }
}
