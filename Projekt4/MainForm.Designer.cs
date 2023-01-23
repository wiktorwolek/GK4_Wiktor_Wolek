namespace Projekt4
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.canva = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.plikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otwórzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solidColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorInterpolationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalsInterpolationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enviormentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableDisableFogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dayNightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canva)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.canva, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(774, 774);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // canva
            // 
            this.canva.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canva.Location = new System.Drawing.Point(3, 3);
            this.canva.Name = "canva";
            this.canva.Size = new System.Drawing.Size(768, 768);
            this.canva.TabIndex = 0;
            this.canva.TabStop = false;
            this.canva.Resize += new System.EventHandler(this.canva_Resize);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plikToolStripMenuItem,
            this.lightingToolStripMenuItem,
            this.enviormentToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(774, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // plikToolStripMenuItem
            // 
            this.plikToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.otwórzToolStripMenuItem});
            this.plikToolStripMenuItem.Name = "plikToolStripMenuItem";
            this.plikToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.plikToolStripMenuItem.Text = "Plik";
            // 
            // otwórzToolStripMenuItem
            // 
            this.otwórzToolStripMenuItem.Name = "otwórzToolStripMenuItem";
            this.otwórzToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.otwórzToolStripMenuItem.Text = "Otwórz";
            this.otwórzToolStripMenuItem.Click += new System.EventHandler(this.otwórzToolStripMenuItem_Click);
            // 
            // lightingToolStripMenuItem
            // 
            this.lightingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solidColorToolStripMenuItem,
            this.colorInterpolationToolStripMenuItem,
            this.normalsInterpolationToolStripMenuItem});
            this.lightingToolStripMenuItem.Name = "lightingToolStripMenuItem";
            this.lightingToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.lightingToolStripMenuItem.Text = "Lighting";
            // 
            // solidColorToolStripMenuItem
            // 
            this.solidColorToolStripMenuItem.Name = "solidColorToolStripMenuItem";
            this.solidColorToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.solidColorToolStripMenuItem.Text = "Solid Color";
            this.solidColorToolStripMenuItem.Click += new System.EventHandler(this.solidColorToolStripMenuItem_Click);
            // 
            // colorInterpolationToolStripMenuItem
            // 
            this.colorInterpolationToolStripMenuItem.Name = "colorInterpolationToolStripMenuItem";
            this.colorInterpolationToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.colorInterpolationToolStripMenuItem.Text = "Color Interpolation";
            this.colorInterpolationToolStripMenuItem.Click += new System.EventHandler(this.colorInterpolationToolStripMenuItem_Click);
            // 
            // normalsInterpolationToolStripMenuItem
            // 
            this.normalsInterpolationToolStripMenuItem.Name = "normalsInterpolationToolStripMenuItem";
            this.normalsInterpolationToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.normalsInterpolationToolStripMenuItem.Text = "Normals Interpolation";
            this.normalsInterpolationToolStripMenuItem.Click += new System.EventHandler(this.normalsInterpolationToolStripMenuItem_Click);
            // 
            // enviormentToolStripMenuItem
            // 
            this.enviormentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableDisableFogToolStripMenuItem,
            this.dayNightToolStripMenuItem});
            this.enviormentToolStripMenuItem.Name = "enviormentToolStripMenuItem";
            this.enviormentToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.enviormentToolStripMenuItem.Text = "Enviorment";
            // 
            // enableDisableFogToolStripMenuItem
            // 
            this.enableDisableFogToolStripMenuItem.Name = "enableDisableFogToolStripMenuItem";
            this.enableDisableFogToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.enableDisableFogToolStripMenuItem.Text = "Enable/Disable fog";
            this.enableDisableFogToolStripMenuItem.Click += new System.EventHandler(this.enableDisableFogToolStripMenuItem_Click);
            // 
            // dayNightToolStripMenuItem
            // 
            this.dayNightToolStripMenuItem.Name = "dayNightToolStripMenuItem";
            this.dayNightToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.dayNightToolStripMenuItem.Text = "Day/Night";
            this.dayNightToolStripMenuItem.Click += new System.EventHandler(this.dayNightToolStripMenuItem_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 774);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "GK4";
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.canva)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox canva;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem plikToolStripMenuItem;
        private ToolStripMenuItem otwórzToolStripMenuItem;
        private System.Windows.Forms.Timer timer;
        private ToolStripMenuItem lightingToolStripMenuItem;
        private ToolStripMenuItem solidColorToolStripMenuItem;
        private ToolStripMenuItem colorInterpolationToolStripMenuItem;
        private ToolStripMenuItem normalsInterpolationToolStripMenuItem;
        private ToolStripMenuItem enviormentToolStripMenuItem;
        private ToolStripMenuItem enableDisableFogToolStripMenuItem;
        private ToolStripMenuItem dayNightToolStripMenuItem;
    }
}