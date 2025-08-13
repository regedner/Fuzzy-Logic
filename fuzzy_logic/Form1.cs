using fuzzy_logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace fuzzy_logic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridViewResults.Columns.Clear();
            dataGridViewResults.Columns.Add("No", "No");
            dataGridViewResults.Columns.Add("Sensitivity", "Sensitivity");
            dataGridViewResults.Columns.Add("Amount", "Amount");
            dataGridViewResults.Columns.Add("Dirtiness", "Dirtiness");
            dataGridViewResults.Columns.Add("SpinSpeed", "Spin Speed");
            dataGridViewResults.Columns.Add("Duration", "Duration");
            dataGridViewResults.Columns.Add("Detergent", "Detergent");

            DrawFuzzySets(chartSensitivity, FuzzyData.SensitivitySets, 0, 10, 0, 1);
            DrawFuzzySets(chartAmount, FuzzyData.AmountSets, 0, 10, 0, 1);
            DrawFuzzySets(chartDirtiness, FuzzyData.DirtinessSets, 0, 10, 0, 1);
            DrawFuzzySets(chartSpinSpeed, FuzzyData.SpinSpeedSets, 0, 10, 0, 1);
            DrawFuzzySets(chartDuration, FuzzyData.DurationSets, 0, 100, 0, 1);
            DrawFuzzySets(chartDetergent, FuzzyData.DetergentSets, 0, 300, 0, 1);

            UpdateSensitivityLine(trackBarSensitivity.Value);
            UpdateAmountLine(trackBarAmount.Value);
            UpdateDirtinessLine(trackBarDirtiness.Value);

            LoadRulesIntoDataGridView();
        }

        private void DrawFuzzySets(Chart chart, List<FuzzySet> sets, double minX, double maxX, double minY, double maxY)
        {
            chart.Series.Clear();
            chart.ChartAreas[0].AxisX.Minimum = minX;
            chart.ChartAreas[0].AxisX.Maximum = maxX + 0.1;
            chart.ChartAreas[0].AxisX.Interval = (maxX - minX) / 10;
            chart.ChartAreas[0].AxisY.Minimum = minY;
            chart.ChartAreas[0].AxisY.Maximum = 1.2;
            chart.ChartAreas[0].AxisY.Interval = 1;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            chartSensitivity.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 10);
            chartAmount.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 10);
            chartDirtiness.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 10);
            chartSpinSpeed.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 10);
            chartDetergent.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 9);
            chartDuration.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 8);

            foreach (var fs in sets)
            {
                var series = new Series(fs.Name)
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 3
                };
                for (double x = minX; x <= maxX; x += 0.1)
                {
                    series.Points.AddXY(x, fs.GetMembership(x));
                }
                chart.Series.Add(series);
            }
        }

        private void UpdateResults()
        {
            double sensitivityValue = (double)numericUpDownSensitivity.Value;
            double amountValue = (double)numericUpDownAmount.Value;
            double dirtinessValue = (double)numericUpDownDirtiness.Value;

            var fuzzyEngine = new FuzzyEngine(
                FuzzyData.SensitivitySets,
                FuzzyData.AmountSets,
                FuzzyData.DirtinessSets,
                FuzzyData.rules
            );

            var (spinSpeed, duration, detergent, weightedSpinSpeed, weightedDuration, weightedDetergent,
                 activeRules, sensitivityMemberships, amountMemberships, dirtinessMemberships) =
                fuzzyEngine.ComputeMamdaniInference(sensitivityValue, amountValue, dirtinessValue);

            var clippedSpinSpeed = fuzzyEngine.GetClippedOutputs(sensitivityValue, amountValue, dirtinessValue, FuzzyData.SpinSpeedSets, rule => rule.SpinSpeed);
            var clippedDuration = fuzzyEngine.GetClippedOutputs(sensitivityValue, amountValue, dirtinessValue, FuzzyData.DurationSets, rule => rule.Duration);
            var clippedDetergent = fuzzyEngine.GetClippedOutputs(sensitivityValue, amountValue, dirtinessValue, FuzzyData.DetergentSets, rule => rule.Detergent);

            DrawOnlyClippedAreas(chartSpinSpeed, clippedSpinSpeed, FuzzyData.SpinSpeedSets, 0, 10);
            DrawOnlyClippedAreas(chartDuration, clippedDuration, FuzzyData.DurationSets, 0, 130);
            DrawOnlyClippedAreas(chartDetergent, clippedDetergent, FuzzyData.DetergentSets, 0, 300);

            label1.Text = $"Centroid = {spinSpeed:F3}";
            label6.Text = $"Centroid = {detergent:F3}";
            label9.Text = $"Centroid = {duration:F3}";
            label2.Text = $"Weighted Average = {weightedSpinSpeed:F3}";
            label5.Text = $"Weighted Average = {weightedDetergent:F3}";
            label8.Text = $"Weighted Average = {weightedDuration:F3}";

            label15.Text = string.Join("\n", sensitivityMemberships
                .Where(kvp => kvp.Value > 0)
                .Select(kvp => kvp.Key));

            label14.Text = string.Join("\n", amountMemberships
                .Where(kvp => kvp.Value > 0)
                .Select(kvp => kvp.Key));

            label13.Text = string.Join("\n", dirtinessMemberships
                .Where(kvp => kvp.Value > 0)
                .Select(kvp => kvp.Key));

            listBox1.Items.Clear();
            foreach (var rule in activeRules)
            {
                listBox1.Items.Add($"{rule.strength}");
            }

            dataGridViewResults.Rows.Clear();
            int ruleNo = 1;
            foreach (var rule in FuzzyData.rules)
            {
                int rowIndex = dataGridViewResults.Rows.Add(
                    ruleNo,
                    rule.Sensitivity,
                    rule.Amount,
                    rule.Dirtiness,
                    rule.SpinSpeed,
                    rule.Duration,
                    rule.Detergent
                );

                if (IsActiveRule(rule, sensitivityValue, amountValue, dirtinessValue))
                {
                    dataGridViewResults.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Chartreuse;
                }

                ruleNo++;
            }
        }

        private void DrawOnlyClippedAreas(Chart chart, Dictionary<string, double> clippedValues, List<FuzzySet> fuzzySets, double minX, double maxX)
        {
            for (int i = chart.Series.Count - 1; i >= 0; i--)
            {
                if (chart.Series[i].Name.EndsWith("Clipped"))
                    chart.Series.RemoveAt(i);
            }

            foreach (var clippedValue in clippedValues)
            {
                string outputName = clippedValue.Key;
                double strength = clippedValue.Value;

                var set = fuzzySets.FirstOrDefault(f => f.Name == outputName);
                if (set == null) continue;

                var clippedSeries = new Series($"{outputName} Clipped")
                {
                    ChartType = SeriesChartType.Area,
                    BorderWidth = 2,
                    Color = Color.FromArgb(120, Color.Red),
                    IsVisibleInLegend = false
                };

                for (double x = minX; x <= maxX; x += 0.1)
                {
                    double y = set.GetMembership(x);
                    clippedSeries.Points.AddXY(x, Math.Min(y, strength));
                }

                chart.Series.Add(clippedSeries);
            }
        }

        private bool IsActiveRule(FuzzyRule rule, double sensitivityValue, double amountValue, double dirtinessValue)
        {
            var matchedSensitivity = GetMatchingSetNames(sensitivityValue, FuzzyData.SensitivitySets);
            var matchedAmount = GetMatchingSetNames(amountValue, FuzzyData.AmountSets);
            var matchedDirtiness = GetMatchingSetNames(dirtinessValue, FuzzyData.DirtinessSets);

            return matchedSensitivity.Contains(rule.Sensitivity)
                && matchedAmount.Contains(rule.Amount)
                && matchedDirtiness.Contains(rule.Dirtiness);
        }

        private List<string> GetMatchingSetNames(double value, List<FuzzySet> sets)
        {
            List<string> matched = new List<string>();
            foreach (var set in sets)
            {
                if (set.GetMembership(value) > 0)
                    matched.Add(set.Name);
            }
            return matched;
        }

        private void LoadRulesIntoDataGridView()
        {
            int ruleNumber = 1;
            foreach (var rule in FuzzyData.rules)
            {
                dataGridViewResults.Rows.Add(ruleNumber, rule.Sensitivity, rule.Amount, rule.Dirtiness, rule.SpinSpeed, rule.Duration, rule.Detergent);
                ruleNumber++;
            }
        }

        private void trackBarSensitivity_Scroll(object sender, EventArgs e)
        {
            double realValue = trackBarSensitivity.Value / 10.0;
            numericUpDownSensitivity.Value = (decimal)realValue;
            UpdateSensitivityLine(realValue);
        }

        private void trackBarSensitivity_MouseUp(object sender, MouseEventArgs e)
        {
            double realValue = trackBarSensitivity.Value / 10.0;
            numericUpDownSensitivity.Value = (decimal)realValue;
            UpdateResults();
        }

        private void trackBarAmount_Scroll(object sender, EventArgs e)
        {
            double realValue = trackBarAmount.Value / 10.0;
            numericUpDownAmount.Value = (decimal)realValue;
            UpdateAmountLine(realValue);
        }

        private void trackBarAmount_MouseUp(object sender, MouseEventArgs e)
        {
            double realValue = trackBarAmount.Value / 10.0;
            numericUpDownAmount.Value = (decimal)realValue;
            UpdateResults();
        }

        private void trackBarDirtiness_Scroll(object sender, EventArgs e)
        {
            double realValue = trackBarDirtiness.Value / 10.0;
            numericUpDownDirtiness.Value = (decimal)realValue;
            UpdateDirtinessLine(realValue);
        }

        private void trackBarDirtiness_MouseUp(object sender, MouseEventArgs e)
        {
            double realValue = trackBarDirtiness.Value / 10.0;
            numericUpDownDirtiness.Value = (decimal)realValue;
            UpdateResults();
        }

        private void numericUpDownSensitivity_ValueChanged(object sender, EventArgs e)
        {
            double realValue = (double)numericUpDownSensitivity.Value;
            trackBarSensitivity.Value = (int)Math.Round(realValue * 10);
            UpdateSensitivityLine(realValue);
        }

        private void numericUpDownAmount_ValueChanged(object sender, EventArgs e)
        {
            double realValue = (double)numericUpDownAmount.Value;
            trackBarAmount.Value = (int)Math.Round(realValue * 10);
            UpdateAmountLine(realValue);
        }

        private void numericUpDownDirtiness_ValueChanged(object sender, EventArgs e)
        {
            double realValue = (double)numericUpDownDirtiness.Value;
            trackBarDirtiness.Value = (int)Math.Round(realValue * 10);
            UpdateDirtinessLine(realValue);
        }

        private void UpdateSensitivityLine(double xValue)
        {
            var area = chartSensitivity.ChartAreas[0];
            area.AxisX.StripLines.Clear();
            var strip = new StripLine
            {
                IntervalOffset = xValue + 0.0001,
                BorderColor = Color.Red,
                BorderDashStyle = ChartDashStyle.Solid,
                BorderWidth = 2
            };
            area.AxisX.StripLines.Add(strip);
        }

        private void UpdateAmountLine(double xValue)
        {
            var area = chartAmount.ChartAreas[0];
            area.AxisX.StripLines.Clear();
            var strip = new StripLine
            {
                IntervalOffset = xValue + 0.0001,
                BorderColor = Color.Red,
                BorderDashStyle = ChartDashStyle.Solid,
                BorderWidth = 2
            };
            area.AxisX.StripLines.Add(strip);
        }

        private void UpdateDirtinessLine(double xValue)
        {
            var area = chartDirtiness.ChartAreas[0];
            area.AxisX.StripLines.Clear();
            var strip = new StripLine
            {
                IntervalOffset = xValue + 0.0001,
                BorderColor = Color.Red,
                BorderDashStyle = ChartDashStyle.Solid,
                BorderWidth = 2
            };
            area.AxisX.StripLines.Add(strip);
        }

        private bool numericInputActive = false;
        private void numericUpDownSensitivity_Enter(object sender, EventArgs e)
        {
            numericInputActive = true;
        }

        private void numericUpDownSensitivity_Leave(object sender, EventArgs e)
        {
            if (numericInputActive)
            {
                double realValue = (double)numericUpDownSensitivity.Value;
                trackBarSensitivity.Value = (int)Math.Round(realValue * 10);
                UpdateSensitivityLine(realValue);
                UpdateResults();
                numericInputActive = false;
            }
        }

        private void numericUpDownSensitivity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                numericUpDownSensitivity_Leave(sender, e);
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void numericUpDownAmount_Enter(object sender, EventArgs e)
        {
            numericInputActive = true;
        }

        private void numericUpDownAmount_Leave(object sender, EventArgs e)
        {
            double realValue = (double)numericUpDownAmount.Value;
            trackBarAmount.Value = (int)Math.Round(realValue * 10);
            UpdateAmountLine(realValue);
            UpdateResults();
            numericInputActive = false;
        }

        private void numericUpDownAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                numericUpDownAmount_Leave(sender, e);
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void numericUpDownDirtiness_Enter(object sender, EventArgs e)
        {
            numericInputActive = true;
        }

        private void numericUpDownDirtiness_Leave(object sender, EventArgs e)
        {
            if (numericInputActive)
            {
                double realValue = (double)numericUpDownDirtiness.Value;
                trackBarDirtiness.Value = (int)Math.Round(realValue * 10);
                UpdateDirtinessLine(realValue);
                UpdateResults();
                numericInputActive = false;
            }
        }

        private void numericUpDownDirtiness_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                numericUpDownDirtiness_Leave(sender, e);
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }
    }
}
