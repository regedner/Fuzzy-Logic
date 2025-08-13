using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fuzzy_logic
{
    public static class FuzzyData
    {
        public static List<FuzzySet> SensitivitySets = new List<FuzzySet>
        {
            new FuzzySet("Strong", -4, -1.5, 2, 4),
            new FuzzySet("Medium", 3, 5, 7),
            new FuzzySet("Sensitive", 5.5, 8, 12.5, 14)
        };

        public static List<FuzzySet> AmountSets = new List<FuzzySet>
        {
            new FuzzySet("Small", -4, -1.5, 2, 4),
            new FuzzySet("Medium", 3, 5, 7),
            new FuzzySet("Large", 5.5, 8, 12.5, 14)
        };

        public static List<FuzzySet> DirtinessSets = new List<FuzzySet>
        {
            new FuzzySet("Small", -4.5, -2.5, 2, 4.5),
            new FuzzySet("Medium", 3, 5, 7),
            new FuzzySet("Large", 5.5, 8, 12.5, 15)
        };

        public static List<FuzzySet> SpinSpeedSets = new List<FuzzySet>
        {
            new FuzzySet("Sensitive", -5.8, -2.8, 0.5, 1.5),
            new FuzzySet("Normal-Sensitive", 0.5, 2.75, 5),
            new FuzzySet("Medium", 2.75, 5, 7.25),
            new FuzzySet("Normal-Strong", 5, 7.25, 9.5),
            new FuzzySet("Strong", 8.5, 9.5, 12.8, 15.2)
        };

        public static List<FuzzySet> DurationSets = new List<FuzzySet>
        {
            new FuzzySet("Short", -46.5, -25.28, 22.3, 39.9),
            new FuzzySet("Normal-Short", 22.3, 39.9, 57.5),
            new FuzzySet("Medium", 39.9, 57.5, 75.1),
            new FuzzySet("Normal-Long", 57.5, 75.1, 92.7),
            new FuzzySet("Long", 75, 92.7, 111.6, 130)
        };

        public static List<FuzzySet> DetergentSets = new List<FuzzySet>
        {
            new FuzzySet("Very Low", 0, 0, 20, 85),
            new FuzzySet("Low", 20, 85, 150),
            new FuzzySet("Medium", 85, 150, 215),
            new FuzzySet("High", 150, 215, 280),
            new FuzzySet("Very High", 215, 280, 300, 300)
        };

        public static List<FuzzyRule> rules = new List<FuzzyRule>
        {
            new FuzzyRule { Sensitivity = "Sensitive", Amount = "Small", Dirtiness = "Small", SpinSpeed = "Sensitive", Duration = "Short", Detergent = "Very Low" },
            new FuzzyRule { Sensitivity = "Sensitive", Amount = "Small", Dirtiness = "Medium", SpinSpeed = "Normal-Sensitive", Duration = "Short", Detergent = "Low" },
            new FuzzyRule { Sensitivity = "Sensitive", Amount = "Small", Dirtiness = "Large", SpinSpeed = "Medium", Duration = "Normal-Short", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Sensitive", Amount = "Medium", Dirtiness = "Small", SpinSpeed = "Sensitive", Duration = "Short", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Sensitive", Amount = "Medium", Dirtiness = "Medium", SpinSpeed = "Normal-Sensitive", Duration = "Normal-Short", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Sensitive", Amount = "Medium", Dirtiness = "Large", SpinSpeed = "Medium", Duration = "Medium", Detergent = "High" },
            new FuzzyRule { Sensitivity = "Sensitive", Amount = "Large", Dirtiness = "Small", SpinSpeed = "Normal-Sensitive", Duration = "Normal-Short", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Sensitive", Amount = "Large", Dirtiness = "Medium", SpinSpeed = "Normal-Sensitive", Duration = "Medium", Detergent = "High" },
            new FuzzyRule { Sensitivity = "Sensitive", Amount = "Large", Dirtiness = "Large", SpinSpeed = "Medium", Duration = "Normal-Long", Detergent = "High" },
            new FuzzyRule { Sensitivity = "Medium", Amount = "Small", Dirtiness = "Small", SpinSpeed = "Normal-Sensitive", Duration = "Normal-Short", Detergent = "Low" },
            new FuzzyRule { Sensitivity = "Medium", Amount = "Small", Dirtiness = "Medium", SpinSpeed = "Medium", Duration = "Short", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Medium", Amount = "Small", Dirtiness = "Large", SpinSpeed = "Normal-Strong", Duration = "Medium", Detergent = "High" },
            new FuzzyRule { Sensitivity = "Medium", Amount = "Medium", Dirtiness = "Small", SpinSpeed = "Normal-Sensitive", Duration = "Normal-Short", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Medium", Amount = "Medium", Dirtiness = "Medium", SpinSpeed = "Medium", Duration = "Medium", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Medium", Amount = "Medium", Dirtiness = "Large", SpinSpeed = "Sensitive", Duration = "Long", Detergent = "High" },
            new FuzzyRule { Sensitivity = "Medium", Amount = "Large", Dirtiness = "Small", SpinSpeed = "Sensitive", Duration = "Medium", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Medium", Amount = "Large", Dirtiness = "Medium", SpinSpeed = "Sensitive", Duration = "Normal-Long", Detergent = "High" },
            new FuzzyRule { Sensitivity = "Medium", Amount = "Large", Dirtiness = "Large", SpinSpeed = "Sensitive", Duration = "Long", Detergent = "Very High" },
            new FuzzyRule { Sensitivity = "Strong", Amount = "Small", Dirtiness = "Small", SpinSpeed = "Medium", Duration = "Medium", Detergent = "Low" },
            new FuzzyRule { Sensitivity = "Strong", Amount = "Small", Dirtiness = "Medium", SpinSpeed = "Normal-Strong", Duration = "Medium", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Strong", Amount = "Small", Dirtiness = "Large", SpinSpeed = "Strong", Duration = "Normal-Long", Detergent = "High" },
            new FuzzyRule { Sensitivity = "Strong", Amount = "Medium", Dirtiness = "Small", SpinSpeed = "Medium", Duration = "Medium", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Strong", Amount = "Medium", Dirtiness = "Medium", SpinSpeed = "Normal-Strong", Duration = "Normal-Long", Detergent = "Medium" },
            new FuzzyRule { Sensitivity = "Strong", Amount = "Medium", Dirtiness = "Large", SpinSpeed = "Strong", Duration = "Medium", Detergent = "Very High" },
            new FuzzyRule { Sensitivity = "Strong", Amount = "Large", Dirtiness = "Small", SpinSpeed = "Normal-Strong", Duration = "Normal-Long", Detergent = "High" },
            new FuzzyRule { Sensitivity = "Strong", Amount = "Large", Dirtiness = "Medium", SpinSpeed = "Normal-Strong", Duration = "Long", Detergent = "High" },
            new FuzzyRule { Sensitivity = "Strong", Amount = "Large", Dirtiness = "Large", SpinSpeed = "Strong", Duration = "Long", Detergent = "Very High" }
        };
    }
}
