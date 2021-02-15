namespace pulse
{
    partial class Sphigmogram
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sphigmogram));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.анализToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вСРToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.диаграммыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.скатерграммаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HistogramDistributionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.спектрToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.welchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lombScargleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoregressiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowValuesCb = new System.Windows.Forms.ToolStripMenuItem();
            this.сбросToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workspace = new System.Windows.Forms.TableLayoutPanel();
            this.InfoBox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.workspace.SuspendLayout();
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
            this.вСРToolStripMenuItem,
            this.диаграммыToolStripMenuItem,
            this.спектрToolStripMenuItem,
            this.AllToolStripMenuItem});
            this.анализToolStripMenuItem.Name = "анализToolStripMenuItem";
            this.анализToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.анализToolStripMenuItem.Text = "Анализ";
            // 
            // вСРToolStripMenuItem
            // 
            this.вСРToolStripMenuItem.Name = "вСРToolStripMenuItem";
            this.вСРToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.вСРToolStripMenuItem.Text = "Показатели";
            this.вСРToolStripMenuItem.Click += new System.EventHandler(this.StatisticsToolStripMenuItem_Click);
            // 
            // диаграммыToolStripMenuItem
            // 
            this.диаграммыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.скатерграммаToolStripMenuItem,
            this.HistogramDistributionMenuItem});
            this.диаграммыToolStripMenuItem.Name = "диаграммыToolStripMenuItem";
            this.диаграммыToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.диаграммыToolStripMenuItem.Text = "Диаграммы";
            // 
            // скатерграммаToolStripMenuItem
            // 
            this.скатерграммаToolStripMenuItem.Name = "скатерграммаToolStripMenuItem";
            this.скатерграммаToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.скатерграммаToolStripMenuItem.Text = "Скатерграмма";
            this.скатерграммаToolStripMenuItem.Click += new System.EventHandler(this.ScattergramToolStripMenuItem_Click);
            // 
            // HistogramDistributionMenuItem
            // 
            this.HistogramDistributionMenuItem.Name = "HistogramDistributionMenuItem";
            this.HistogramDistributionMenuItem.Size = new System.Drawing.Size(234, 22);
            this.HistogramDistributionMenuItem.Text = "Гистограмма распределения";
            this.HistogramDistributionMenuItem.Click += new System.EventHandler(this.HistogramDistributionMenuItem_Click);
            // 
            // спектрToolStripMenuItem
            // 
            this.спектрToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.welchToolStripMenuItem,
            this.lombScargleToolStripMenuItem,
            this.autoregressiveToolStripMenuItem});
            this.спектрToolStripMenuItem.Name = "спектрToolStripMenuItem";
            this.спектрToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.спектрToolStripMenuItem.Text = "Спектральная плотность";
            // 
            // welchToolStripMenuItem
            // 
            this.welchToolStripMenuItem.Name = "welchToolStripMenuItem";
            this.welchToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.welchToolStripMenuItem.Text = "Welch";
            this.welchToolStripMenuItem.Click += new System.EventHandler(this.PowerSpectralHandler);
            // 
            // lombScargleToolStripMenuItem
            // 
            this.lombScargleToolStripMenuItem.Name = "lombScargleToolStripMenuItem";
            this.lombScargleToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lombScargleToolStripMenuItem.Text = "Lomb-Scargle";
            this.lombScargleToolStripMenuItem.Click += new System.EventHandler(this.PowerSpectralHandler);
            // 
            // autoregressiveToolStripMenuItem
            // 
            this.autoregressiveToolStripMenuItem.Name = "autoregressiveToolStripMenuItem";
            this.autoregressiveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.autoregressiveToolStripMenuItem.Text = "Autoregressive";
            this.autoregressiveToolStripMenuItem.Click += new System.EventHandler(this.PowerSpectralHandler);
            // 
            // AllToolStripMenuItem
            // 
            this.AllToolStripMenuItem.Name = "AllToolStripMenuItem";
            this.AllToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.AllToolStripMenuItem.Text = "Все";
            this.AllToolStripMenuItem.Click += new System.EventHandler(this.AllToolStripMenuItem_Click);
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowValuesCb,
            this.сбросToolStripMenuItem});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // ShowValuesCb
            // 
            this.ShowValuesCb.Name = "ShowValuesCb";
            this.ShowValuesCb.Size = new System.Drawing.Size(127, 22);
            this.ShowValuesCb.Text = "Значения";
            this.ShowValuesCb.Click += new System.EventHandler(this.ShowValuesCb_Click);
            // 
            // сбросToolStripMenuItem
            // 
            this.сбросToolStripMenuItem.Name = "сбросToolStripMenuItem";
            this.сбросToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.сбросToolStripMenuItem.Text = "Сброс";
            // 
            // workspace
            // 
            this.workspace.ColumnCount = 1;
            this.workspace.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.workspace.Controls.Add(this.InfoBox, 0, 0);
            this.workspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workspace.Location = new System.Drawing.Point(0, 24);
            this.workspace.Name = "workspace";
            this.workspace.RowCount = 3;
            this.workspace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.929919F));
            this.workspace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.07008F));
            this.workspace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 290F));
            this.workspace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.workspace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.workspace.Size = new System.Drawing.Size(824, 646);
            this.workspace.TabIndex = 15;
            // 
            // InfoBox
            // 
            this.InfoBox.BackColor = System.Drawing.SystemColors.Window;
            this.InfoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox.Location = new System.Drawing.Point(0, 0);
            this.InfoBox.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.ReadOnly = true;
            this.InfoBox.Size = new System.Drawing.Size(824, 20);
            this.InfoBox.TabIndex = 15;
            this.InfoBox.Text = "Время: 00:00:00    ms: 0  Y: 0";
            // 
            // Sphigmogram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 670);
            this.Controls.Add(this.workspace);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Sphigmogram";
            this.Text = "Данные";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.workspace.ResumeLayout(false);
            this.workspace.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem анализToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вСРToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowValuesCb;
        private System.Windows.Forms.TableLayoutPanel workspace;
        private System.Windows.Forms.ToolStripMenuItem сбросToolStripMenuItem;
        private System.Windows.Forms.TextBox InfoBox;
        private System.Windows.Forms.ToolStripMenuItem диаграммыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem скатерграммаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HistogramDistributionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem спектрToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem welchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lombScargleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoregressiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AllToolStripMenuItem;
    }
}