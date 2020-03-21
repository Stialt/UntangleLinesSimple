namespace UntangleLines
{
    partial class Form1
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownN = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.labelInitState = new System.Windows.Forms.Label();
            this.buttonGiveUp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownN)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(114, 72);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Choose Difficulty (5-30)";
            // 
            // numericUpDownN
            // 
            this.numericUpDownN.Location = new System.Drawing.Point(93, 44);
            this.numericUpDownN.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownN.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownN.Name = "numericUpDownN";
            this.numericUpDownN.Size = new System.Drawing.Size(120, 22);
            this.numericUpDownN.TabIndex = 3;
            this.numericUpDownN.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(378, 44);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(132, 21);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Show initial state";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // labelInitState
            // 
            this.labelInitState.AutoSize = true;
            this.labelInitState.Location = new System.Drawing.Point(218, 135);
            this.labelInitState.Name = "labelInitState";
            this.labelInitState.Size = new System.Drawing.Size(77, 17);
            this.labelInitState.TabIndex = 5;
            this.labelInitState.Text = "Initial State";
            this.labelInitState.Visible = false;
            // 
            // buttonGiveUp
            // 
            this.buttonGiveUp.Enabled = false;
            this.buttonGiveUp.Location = new System.Drawing.Point(233, 618);
            this.buttonGiveUp.Name = "buttonGiveUp";
            this.buttonGiveUp.Size = new System.Drawing.Size(75, 23);
            this.buttonGiveUp.TabIndex = 6;
            this.buttonGiveUp.Text = "GIVE UP";
            this.buttonGiveUp.UseVisualStyleBackColor = true;
            this.buttonGiveUp.Click += new System.EventHandler(this.buttonGiveUp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 675);
            this.Controls.Add(this.buttonGiveUp);
            this.Controls.Add(this.labelInitState);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.numericUpDownN);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStart);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownN;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label labelInitState;
        private System.Windows.Forms.Button buttonGiveUp;
    }
}

