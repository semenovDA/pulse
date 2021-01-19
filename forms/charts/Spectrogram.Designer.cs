
namespace pulse.forms.charts
{
    partial class Spectrogram
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Spectrogram));
            this.Spectogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.infoBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Spectogram)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Spectogram
            // 
            chartArea1.Name = "ChartArea1";
            this.Spectogram.ChartAreas.Add(chartArea1);
            this.Spectogram.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.Spectogram.Legends.Add(legend1);
            this.Spectogram.Location = new System.Drawing.Point(3, 27);
            this.Spectogram.Name = "Spectogram";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Black;
            series1.Legend = "Legend1";
            series1.LegendText = "ULF";
            series1.Name = "ulf";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Green;
            series2.Legend = "Legend1";
            series2.LegendText = "VLF";
            series2.Name = "vlf";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Red;
            series3.Legend = "Legend1";
            series3.LegendText = "LF";
            series3.Name = "lf";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.Blue;
            series4.Legend = "Legend1";
            series4.LegendText = "HF";
            series4.Name = "hf";
            this.Spectogram.Series.Add(series1);
            this.Spectogram.Series.Add(series2);
            this.Spectogram.Series.Add(series3);
            this.Spectogram.Series.Add(series4);
            this.Spectogram.Size = new System.Drawing.Size(662, 420);
            this.Spectogram.TabIndex = 0;
            this.Spectogram.Text = "chart1";
            this.Spectogram.CursorPositionChanging += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CursorEventArgs>(this.Spectogram_CursorPositionChanging);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Spectogram, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.infoBox, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.333333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.66666F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(668, 450);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // infoBox
            // 
            this.infoBox.BackColor = System.Drawing.SystemColors.Window;
            this.infoBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.infoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoBox.Location = new System.Drawing.Point(3, 3);
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.Size = new System.Drawing.Size(662, 20);
            this.infoBox.TabIndex = 1;
            this.infoBox.Text = "X: 0    Y:0";
            // 
            // Spectrogram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Spectrogram";
            this.Text = "Spectrogram";
            ((System.ComponentModel.ISupportInitialize)(this.Spectogram)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Spectogram;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox infoBox;
    }
}