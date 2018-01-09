namespace Morphing
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pic_processed = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pic_original1 = new System.Windows.Forms.PictureBox();
            this.pic_original2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_processed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_original1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_original2)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_processed
            // 
            this.pic_processed.Location = new System.Drawing.Point(511, 12);
            this.pic_processed.Name = "pic_processed";
            this.pic_processed.Size = new System.Drawing.Size(253, 366);
            this.pic_processed.TabIndex = 0;
            this.pic_processed.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 384);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(752, 52);
            this.button1.TabIndex = 1;
            this.button1.Text = "Morphing";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pic_original1
            // 
            this.pic_original1.Location = new System.Drawing.Point(6, 12);
            this.pic_original1.Name = "pic_original1";
            this.pic_original1.Size = new System.Drawing.Size(250, 366);
            this.pic_original1.TabIndex = 2;
            this.pic_original1.TabStop = false;
            // 
            // pic_original2
            // 
            this.pic_original2.Location = new System.Drawing.Point(263, 12);
            this.pic_original2.Name = "pic_original2";
            this.pic_original2.Size = new System.Drawing.Size(242, 366);
            this.pic_original2.TabIndex = 3;
            this.pic_original2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 438);
            this.Controls.Add(this.pic_original2);
            this.Controls.Add(this.pic_original1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pic_processed);
            this.Name = "Form1";
            this.Text = "Морфинг изображений";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_processed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_original1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_original2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_processed;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pic_original1;
        private System.Windows.Forms.PictureBox pic_original2;
    }
}

