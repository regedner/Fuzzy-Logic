using fuzzy_logic;
using System.Collections.Generic;
using System;
using System.Linq;

public class FuzzyEngine
{
    private List<FuzzyRule> rules;
    private List<FuzzySet> sensitivitySets, amountSets, dirtinessSets;

    private static readonly Dictionary<string, double> SpinSpeedFactors = new Dictionary<string, double>
    {
        {"Sensitive", 0.514},
        {"Normal-Sensitive", 2.75},
        {"Medium", 5.0},
        {"Normal-Strong", 7.25},
        {"Strong", 9.5}
    };

    private static readonly Dictionary<string, double> DurationFactors = new Dictionary<string, double>
    {
        {"Short", 22.3},
        {"Normal-Short", 39.9},
        {"Medium", 57.5},
        {"Normal-Long", 75.1},
        {"Long", 92.7}
    };

    private static readonly Dictionary<string, double> DetergentFactors = new Dictionary<string, double>
    {
        {"Very Low", 20},
        {"Low", 85},
        {"Medium", 150},
        {"High", 215},
        {"Very High", 270}
    };

    public FuzzyEngine(List<FuzzySet> sensitivitySets, List<FuzzySet> amountSets, List<FuzzySet> dirtinessSets, List<FuzzyRule> rules)
    {
        this.sensitivitySets = sensitivitySets;
        this.amountSets = amountSets;
        this.dirtinessSets = dirtinessSets;
        this.rules = rules;
    }

    public (double spinSpeed, double duration, double detergent, double weightedSpinSpeed, double weightedDuration, double weightedDetergent,
        List<(int ruleNo, double strength, string description)> activeRules,
        Dictionary<string, double> sensitivityMemberships,
        Dictionary<string, double> amountMemberships,
        Dictionary<string, double> dirtinessMemberships)
        ComputeMamdaniInference(double sensitivityValue, double amountValue, double dirtinessValue)
    {
        double[] spinSpeed = new double[5];
        double[] duration = new double[5];
        double[] detergent = new double[5];

        // Calculate clipped outputs
        var clippedSpinSpeed = GetClippedOutputs(sensitivityValue, amountValue, dirtinessValue, FuzzyData.SpinSpeedSets, rule => rule.SpinSpeed);
        var clippedDuration = GetClippedOutputs(sensitivityValue, amountValue, dirtinessValue, FuzzyData.DurationSets, rule => rule.Duration);
        var clippedDetergent = GetClippedOutputs(sensitivityValue, amountValue, dirtinessValue, FuzzyData.DetergentSets, rule => rule.Detergent);

        // Calculate membership degrees of inputs
        var sensitivityMemberships = new Dictionary<string, double>();
        foreach (var set in sensitivitySets)
        {
            double membership = GetMembership(sensitivityValue, set.Name, sensitivitySets);
            sensitivityMemberships[set.Name] = membership;
        }

        var amountMemberships = new Dictionary<string, double>();
        foreach (var set in amountSets)
        {
            double membership = GetMembership(amountValue, set.Name, amountSets);
            amountMemberships[set.Name] = membership;
        }

        var dirtinessMemberships = new Dictionary<string, double>();
        foreach (var set in dirtinessSets)
        {
            double membership = GetMembership(dirtinessValue, set.Name, dirtinessSets);
            dirtinessMemberships[set.Name] = membership;
        }

        // Calculate active rules and firing strengths
        var activeRules = new List<(int ruleNo, double strength, string description)>();
        for (int i = 0; i < rules.Count; i++)
        {
            var rule = rules[i];
            double sensMem = GetMembership(sensitivityValue, rule.Sensitivity, sensitivitySets);
            double amtMem = GetMembership(amountValue, rule.Amount, amountSets);
            double dirtMem = GetMembership(dirtinessValue, rule.Dirtiness, dirtinessSets);
            double ruleStrength = Math.Min(Math.Min(sensMem, amtMem), dirtMem);

            if (ruleStrength > 0)
            {
                string description = $"Rule {i + 1}: Sensitivity = {rule.Sensitivity} ({sensMem:F2}), " +
                                     $"Amount = {rule.Amount} ({amtMem:F2}), " +
                                     $"Dirtiness = {rule.Dirtiness} ({dirtMem:F2}), " +
                                     $"Firing Strength = {ruleStrength:F2}";
                activeRules.Add((i + 1, ruleStrength, description));
            }

            ApplyRuleToOutput(ruleStrength, rule.SpinSpeed, spinSpeed, FuzzyData.SpinSpeedSets);
            ApplyRuleToOutput(ruleStrength, rule.Duration, duration, FuzzyData.DurationSets);
            ApplyRuleToOutput(ruleStrength, rule.Detergent, detergent, FuzzyData.DetergentSets);
        }

        // Defuzzification via centroid
        double finalSpinSpeed = CentroidDefuzzify(clippedSpinSpeed, FuzzyData.SpinSpeedSets, 0, 10);
        double finalDuration = CentroidDefuzzify(clippedDuration, FuzzyData.DurationSets, 0, 130);
        double finalDetergent = CentroidDefuzzify(clippedDetergent, FuzzyData.DetergentSets, 0, 300);

        // Defuzzification via weighted average
        double weightedSpinSpeed = WeightedDefuzzify(spinSpeed, FuzzyData.SpinSpeedSets, SpinSpeedFactors);
        double weightedDuration = WeightedDefuzzify(duration, FuzzyData.DurationSets, DurationFactors);
        double weightedDetergent = WeightedDefuzzify(detergent, FuzzyData.DetergentSets, DetergentFactors);

        return (finalSpinSpeed, finalDuration, finalDetergent, weightedSpinSpeed, weightedDuration, weightedDetergent,
                activeRules, sensitivityMemberships, amountMemberships, dirtinessMemberships);
    }

    public double GetMembership(double value, string setName, List<FuzzySet> sets)
    {
        var set = sets.FirstOrDefault(s => s.Name == setName);
        return set?.GetMembership(value) ?? 0;
    }

    public void ApplyRuleToOutput(double ruleStrength, string outputSetName, double[] outputArray, List<FuzzySet> fuzzySets)
    {
        int index = fuzzySets.FindIndex(f => f.Name == outputSetName);
        if (index >= 0 && index < outputArray.Length)
        {
            outputArray[index] = Math.Max(outputArray[index], ruleStrength);
        }
    }

    public double WeightedDefuzzify(double[] outputArray, List<FuzzySet> fuzzySets, Dictionary<string, double> factors)
    {
        double weightedSum = 0;
        double totalWeight = 0;

        for (int i = 0; i < outputArray.Length; i++)
        {
            double membership = outputArray[i];
            if (membership > 0)
            {
                string setName = fuzzySets[i].Name;
                double center = factors.ContainsKey(setName) ? factors[setName] : fuzzySets[i].GetCentroid();
                weightedSum += membership * center;
                totalWeight += membership;
            }
        }

        double result = totalWeight == 0 ? 0 : weightedSum / totalWeight;
        if (fuzzySets == FuzzyData.SpinSpeedSets) return Math.Max(0, Math.Min(10, result));
        if (fuzzySets == FuzzyData.DurationSets) return Math.Max(0, Math.Min(130, result));
        if (fuzzySets == FuzzyData.DetergentSets) return Math.Max(0, Math.Min(300, result));
        return result;
    }

    public double CentroidDefuzzify(Dictionary<string, double> clipped, List<FuzzySet> sets, double minX, double maxX)
    {
        double step = 0.01;
        double numerator = 0;
        double denominator = 0;

        for (double x = minX; x <= maxX; x += step)
        {
            double y = 0;
            foreach (var kvp in clipped)
            {
                var set = sets.FirstOrDefault(s => s.Name == kvp.Key);
                if (set != null)
                {
                    y = Math.Max(y, Math.Min(set.GetMembership(x), kvp.Value));
                }
            }
            numerator += x * y;
            denominator += y;
        }

        double result = denominator == 0 ? 0 : numerator / denominator;
        if (sets == FuzzyData.SpinSpeedSets) return Math.Max(0, Math.Min(10, result));
        if (sets == FuzzyData.DurationSets) return Math.Max(0, Math.Min(130, result));
        if (sets == FuzzyData.DetergentSets) return Math.Max(0, Math.Min(300, result));
        return result;
    }

    public Dictionary<string, double> GetClippedOutputs(double sensitivityValue, double amountValue, double dirtinessValue, List<FuzzySet> outputSets, Func<FuzzyRule, string> selector)
    {
        var result = new Dictionary<string, double>();

        foreach (var rule in rules)
        {
            double s = GetMembership(sensitivityValue, rule.Sensitivity, sensitivitySets);
            double a = GetMembership(amountValue, rule.Amount, amountSets);
            double d = GetMembership(dirtinessValue, rule.Dirtiness, dirtinessSets);

            double strength = Math.Min(Math.Min(s, a), d);

            string outputName = selector(rule);

            if (result.ContainsKey(outputName))
                result[outputName] = Math.Max(result[outputName], strength);
            else
                result[outputName] = strength;
        }

        return result;
    }
}
