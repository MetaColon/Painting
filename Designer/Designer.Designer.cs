using Painting.Types.Paint;

namespace Designer
{
    partial class Designer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.SelectableShapes = new System.Windows.Forms.ComboBox();
            this.Edges = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LineWidth = new System.Windows.Forms.NumericUpDown();
            this.ColorPicker = new System.Windows.Forms.ColorDialog();
            this.LineColourBuuton = new System.Windows.Forms.Button();
            this.MainColorButton = new System.Windows.Forms.Button();
            this.Pointer = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CopyCodeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Edges)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // SelectableShapes
            // 
            this.SelectableShapes.FormattingEnabled = true;
            this.SelectableShapes.Location = new System.Drawing.Point(13, 13);
            this.SelectableShapes.Name = "SelectableShapes";
            this.SelectableShapes.Size = new System.Drawing.Size(121, 21);
            this.SelectableShapes.TabIndex = 0;
            this.SelectableShapes.SelectedIndexChanged += new System.EventHandler(this.SelectableShapes_SelectedIndexChanged);
            // 
            // Edges
            // 
            this.Edges.Enabled = false;
            this.Edges.Location = new System.Drawing.Point(189, 14);
            this.Edges.Name = "Edges";
            this.Edges.Size = new System.Drawing.Size(120, 20);
            this.Edges.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(140, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Edges:";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(492, 14);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 3;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(316, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Width";
            // 
            // LineWidth
            // 
            this.LineWidth.Location = new System.Drawing.Point(366, 16);
            this.LineWidth.Name = "LineWidth";
            this.LineWidth.Size = new System.Drawing.Size(120, 20);
            this.LineWidth.TabIndex = 5;
            // 
            // LineColourBuuton
            // 
            this.LineColourBuuton.Location = new System.Drawing.Point(574, 13);
            this.LineColourBuuton.Name = "LineColourBuuton";
            this.LineColourBuuton.Size = new System.Drawing.Size(300, 23);
            this.LineColourBuuton.TabIndex = 6;
            this.LineColourBuuton.Text = "Select Line Color";
            this.LineColourBuuton.UseVisualStyleBackColor = true;
            this.LineColourBuuton.Click += new System.EventHandler(this.LineColourButton_Click);
            // 
            // MainColorButton
            // 
            this.MainColorButton.Location = new System.Drawing.Point(880, 13);
            this.MainColorButton.Name = "MainColorButton";
            this.MainColorButton.Size = new System.Drawing.Size(300, 23);
            this.MainColorButton.TabIndex = 7;
            this.MainColorButton.Text = "Select Main Color";
            this.MainColorButton.UseVisualStyleBackColor = true;
            this.MainColorButton.Click += new System.EventHandler(this.MainColorButton_Click);
            // 
            // Pointer
            // 
            this.Pointer.AutoSize = true;
            this.Pointer.Location = new System.Drawing.Point(988, 332);
            this.Pointer.Name = "Pointer";
            this.Pointer.Size = new System.Drawing.Size(0, 13);
            this.Pointer.TabIndex = 8;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(1186, 13);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 9;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CopyCodeButton
            // 
            this.CopyCodeButton.Location = new System.Drawing.Point(1268, 13);
            this.CopyCodeButton.Name = "CopyCodeButton";
            this.CopyCodeButton.Size = new System.Drawing.Size(75, 23);
            this.CopyCodeButton.TabIndex = 10;
            this.CopyCodeButton.Text = "Copy Code";
            this.CopyCodeButton.UseVisualStyleBackColor = true;
            this.CopyCodeButton.Click += new System.EventHandler(this.CopyCodeButton_Click);
            // 
            // Designer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1452, 819);
            this.Controls.Add(this.CopyCodeButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.Pointer);
            this.Controls.Add(this.MainColorButton);
            this.Controls.Add(this.LineColourBuuton);
            this.Controls.Add(this.LineWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Edges);
            this.Controls.Add(this.SelectableShapes);
            this.DoubleBuffered = true;
            this.Name = "Designer";
            this.Text = "Designer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Designer_FormClosing);
            this.Load += new System.EventHandler(this.Designer_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Designer_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Designer_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Designer_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Designer_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Designer_MouseWheel);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Designer_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Edges)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SelectableShapes;
        private System.Windows.Forms.NumericUpDown Edges;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown LineWidth;
        private System.Windows.Forms.ColorDialog ColorPicker;
        private System.Windows.Forms.Button LineColourBuuton;
        private System.Windows.Forms.Button MainColorButton;
        private System.Windows.Forms.Label Pointer;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CopyCodeButton;
    }
}

