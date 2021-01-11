namespace pulse
{
    partial class Form2
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
            System.Windows.Forms.DataVisualization.Charting.LineAnnotation lineAnnotation1 = new System.Windows.Forms.DataVisualization.Charting.LineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.LegendCellColumn legendCellColumn1 = new System.Windows.Forms.DataVisualization.Charting.LegendCellColumn();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.анализToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вСРToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowValuesCb = new System.Windows.Forms.ToolStripMenuItem();
            this.FocusSignalCb = new System.Windows.Forms.ToolStripMenuItem();
            this.CIV = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Signal = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CIV)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Signal)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.анализToolStripMenuItem,
            this.видToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(824, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // анализToolStripMenuItem
            // 
            this.анализToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вСРToolStripMenuItem});
            this.анализToolStripMenuItem.Name = "анализToolStripMenuItem";
            this.анализToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.анализToolStripMenuItem.Text = "Анализ";
            // 
            // вСРToolStripMenuItem
            // 
            this.вСРToolStripMenuItem.Name = "вСРToolStripMenuItem";
            this.вСРToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.вСРToolStripMenuItem.Text = "ВСР";
            this.вСРToolStripMenuItem.Click += new System.EventHandler(this.вСРToolStripMenuItem_Click);
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowValuesCb,
            this.FocusSignalCb});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // ShowValuesCb
            // 
            this.ShowValuesCb.Name = "ShowValuesCb";
            this.ShowValuesCb.Size = new System.Drawing.Size(134, 22);
            this.ShowValuesCb.Text = "Значения";
            this.ShowValuesCb.Click += new System.EventHandler(this.ShowValuesCb_Click);
            // 
            // FocusSignalCb
            // 
            this.FocusSignalCb.Checked = true;
            this.FocusSignalCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FocusSignalCb.Name = "FocusSignalCb";
            this.FocusSignalCb.Size = new System.Drawing.Size(134, 22);
            this.FocusSignalCb.Text = "Автофокус";
            this.FocusSignalCb.Click += new System.EventHandler(this.FocusSignalCb_Click);
            // 
            // CIV
            // 
            this.CIV.AllowDrop = true;
            lineAnnotation1.AllowAnchorMoving = true;
            lineAnnotation1.AllowMoving = true;
            lineAnnotation1.AllowPathEditing = true;
            lineAnnotation1.AllowResizing = true;
            lineAnnotation1.AllowSelecting = true;
            lineAnnotation1.AnchorOffsetX = 10D;
            lineAnnotation1.AnchorOffsetY = 10D;
            lineAnnotation1.AxisXName = "ChartArea1\\rX2";
            lineAnnotation1.ClipToChartArea = "ChartArea1";
            lineAnnotation1.EndCap = System.Windows.Forms.DataVisualization.Charting.LineAnchorCapStyle.Arrow;
            lineAnnotation1.LineColor = System.Drawing.Color.Red;
            lineAnnotation1.LineWidth = 2;
            lineAnnotation1.Name = "LineAnnotation1";
            lineAnnotation1.ShadowColor = System.Drawing.Color.Blue;
            lineAnnotation1.ToolTip = "Частота в минуту";
            lineAnnotation1.Visible = false;
            lineAnnotation1.YAxisName = "ChartArea1\\rY2";
            this.CIV.Annotations.Add(lineAnnotation1);
            this.CIV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CIV.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Top;
            this.CIV.BackSecondaryColor = System.Drawing.Color.Silver;
            this.CIV.BorderlineColor = System.Drawing.Color.Blue;
            this.CIV.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.CIV.BorderSkin.BackColor = System.Drawing.Color.Transparent;
            chartArea1.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
            chartArea1.AlignmentStyle = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentStyles.PlotPosition;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisX.InterlacedColor = System.Drawing.Color.White;
            chartArea1.AxisX.LabelStyle.Interval = 0D;
            chartArea1.AxisX.LabelStyle.IntervalOffset = 0D;
            chartArea1.AxisX.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.LabelStyle.IsEndLabelVisible = false;
            chartArea1.AxisX.LineWidth = 2;
            chartArea1.AxisX.MajorGrid.Interval = 0D;
            chartArea1.AxisX.MajorGrid.IntervalOffset = 0D;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Interval = 0D;
            chartArea1.AxisX.MajorTickMark.IntervalOffset = 0D;
            chartArea1.AxisX.MaximumAutoSize = 85F;
            chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.MinorTickMark.Enabled = true;
            chartArea1.AxisX.ScrollBar.BackColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.Gray;
            chartArea1.AxisX.ScrollBar.LineColor = System.Drawing.Color.Black;
            chartArea1.AxisX.ScrollBar.Size = 20D;
            chartArea1.AxisX2.ScrollBar.Enabled = false;
            chartArea1.AxisY.Crossing = -1.7976931348623157E+308D;
            chartArea1.AxisY.InterlacedColor = System.Drawing.Color.White;
            chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.LineWidth = 2;
            chartArea1.AxisY.MajorGrid.Interval = 0D;
            chartArea1.AxisY.MajorGrid.IntervalOffset = 0D;
            chartArea1.AxisY.MajorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisY.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.MajorTickMark.Interval = 0D;
            chartArea1.AxisY.MajorTickMark.IntervalOffset = 0D;
            chartArea1.AxisY.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisY.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisY.MajorTickMark.Size = 0F;
            chartArea1.AxisY.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.ScrollBar.BackColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.ScrollBar.ButtonColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.ScrollBar.LineColor = System.Drawing.Color.Black;
            chartArea1.AxisY.ScrollBar.Size = 20D;
            chartArea1.CursorX.SelectionColor = System.Drawing.Color.Silver;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 96F;
            chartArea1.Position.Width = 96F;
            chartArea1.Position.Y = 3F;
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.CIV.ChartAreas.Add(chartArea1);
            this.CIV.Dock = System.Windows.Forms.DockStyle.Fill;
            legendCellColumn1.Name = "Column2";
            legend1.CellColumns.Add(legendCellColumn1);
            legend1.Enabled = false;
            legend1.ItemColumnSpacing = 100;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.Name = "Legend1";
            this.CIV.Legends.Add(legend1);
            this.CIV.Location = new System.Drawing.Point(0, 251);
            this.CIV.Margin = new System.Windows.Forms.Padding(0);
            this.CIV.Name = "CIV";
            this.CIV.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series1.ChartArea = "ChartArea1";
            series1.CustomProperties = "IsXAxisQuantitative=False";
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.CIV.Series.Add(series1);
            this.CIV.Size = new System.Drawing.Size(824, 209);
            this.CIV.TabIndex = 13;
            this.CIV.Text = "chart2";
            this.CIV.Click += new System.EventHandler(this.CIV_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.CIV, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Signal, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 54.6697F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.3303F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(824, 460);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // Signal
            // 
            chartArea2.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
            chartArea2.AlignmentStyle = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentStyles.PlotPosition;
            chartArea2.BorderWidth = 0;
            chartArea2.InnerPlotPosition.Auto = false;
            chartArea2.InnerPlotPosition.Height = 92F;
            chartArea2.InnerPlotPosition.Width = 100F;
            chartArea2.Name = "ChartArea1";
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 100F;
            chartArea2.Position.Width = 100F;
            this.Signal.ChartAreas.Add(chartArea2);
            this.Signal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Signal.Location = new System.Drawing.Point(3, 3);
            this.Signal.Name = "Signal";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Name = "Series1";
            this.Signal.Series.Add(series2);
            this.Signal.Size = new System.Drawing.Size(818, 245);
            this.Signal.TabIndex = 14;
            this.Signal.Text = "Signal";
            this.Signal.AxisViewChanging += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ViewEventArgs>(this.Signal_AxisViewChanging);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 484);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form2";
            this.Text = "Данные";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CIV)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Signal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem анализToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вСРToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowValuesCb;
        private System.Windows.Forms.ToolStripMenuItem FocusSignalCb;
        public System.Windows.Forms.DataVisualization.Charting.Chart CIV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.DataVisualization.Charting.Chart Signal;
    }
}