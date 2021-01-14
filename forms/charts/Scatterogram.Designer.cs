
namespace pulse.forms.charts
{
    partial class Scatterogram
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Scatterogram));
            this.ScatterogramChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.angleScroll = new System.Windows.Forms.HScrollBar();
            this.angleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ScatterogramChart)).BeginInit();
            this.SuspendLayout();
            // 
            // ScatterogramChart
            // 
            chartArea1.AxisX.ScrollBar.Enabled = false;
            chartArea1.AxisY.ScrollBar.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.ScatterogramChart.ChartAreas.Add(chartArea1);
            this.ScatterogramChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScatterogramChart.Location = new System.Drawing.Point(0, 0);
            this.ScatterogramChart.Name = "ScatterogramChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Name = "Series2";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Name = "Series3";
            series3.YValuesPerPoint = 2;
            this.ScatterogramChart.Series.Add(series1);
            this.ScatterogramChart.Series.Add(series2);
            this.ScatterogramChart.Series.Add(series3);
            this.ScatterogramChart.Size = new System.Drawing.Size(768, 450);
            this.ScatterogramChart.TabIndex = 0;
            this.ScatterogramChart.Text = "chart1";
            // 
            // angleScroll
            // 
            this.angleScroll.Location = new System.Drawing.Point(0, 433);
            this.angleScroll.Maximum = 180;
            this.angleScroll.Minimum = -180;
            this.angleScroll.Name = "angleScroll";
            this.angleScroll.Size = new System.Drawing.Size(263, 17);
            this.angleScroll.TabIndex = 1;
            this.angleScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.angleScroll_Scroll);
            // 
            // angleLabel
            // 
            this.angleLabel.AutoSize = true;
            this.angleLabel.Location = new System.Drawing.Point(267, 436);
            this.angleLabel.Name = "angleLabel";
            this.angleLabel.Size = new System.Drawing.Size(35, 13);
            this.angleLabel.TabIndex = 2;
            this.angleLabel.Text = "label1";
            // 
            // Scatterogram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 450);
            this.Controls.Add(this.angleLabel);
            this.Controls.Add(this.angleScroll);
            this.Controls.Add(this.ScatterogramChart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Scatterogram";
            this.Text = "Скатерграмма";
            ((System.ComponentModel.ISupportInitialize)(this.ScatterogramChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ScatterogramChart;
        private System.Windows.Forms.HScrollBar angleScroll;
        private System.Windows.Forms.Label angleLabel;
    }
}