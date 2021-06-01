namespace sqliteDemo1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_createDataBase = new System.Windows.Forms.Button();
            this.button_connect = new System.Windows.Forms.Button();
            this.button_createTable = new System.Windows.Forms.Button();
            this.button_Index = new System.Windows.Forms.Button();
            this.button_insert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_createDataBase
            // 
            this.button_createDataBase.Location = new System.Drawing.Point(53, 40);
            this.button_createDataBase.Name = "button_createDataBase";
            this.button_createDataBase.Size = new System.Drawing.Size(75, 23);
            this.button_createDataBase.TabIndex = 0;
            this.button_createDataBase.Text = "建库";
            this.button_createDataBase.UseVisualStyleBackColor = true;
            this.button_createDataBase.Click += new System.EventHandler(this.button_createDataBase_Click);
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(53, 92);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 23);
            this.button_connect.TabIndex = 1;
            this.button_connect.Text = "连接";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // button_createTable
            // 
            this.button_createTable.Location = new System.Drawing.Point(53, 147);
            this.button_createTable.Name = "button_createTable";
            this.button_createTable.Size = new System.Drawing.Size(75, 23);
            this.button_createTable.TabIndex = 2;
            this.button_createTable.Text = "建表";
            this.button_createTable.UseVisualStyleBackColor = true;
            this.button_createTable.Click += new System.EventHandler(this.button_createTable_Click);
            // 
            // button_Index
            // 
            this.button_Index.Location = new System.Drawing.Point(53, 206);
            this.button_Index.Name = "button_Index";
            this.button_Index.Size = new System.Drawing.Size(75, 23);
            this.button_Index.TabIndex = 3;
            this.button_Index.Text = "建立索引";
            this.button_Index.UseVisualStyleBackColor = true;
            this.button_Index.Click += new System.EventHandler(this.button_Index_Click);
            // 
            // button_insert
            // 
            this.button_insert.Location = new System.Drawing.Point(53, 273);
            this.button_insert.Name = "button_insert";
            this.button_insert.Size = new System.Drawing.Size(75, 23);
            this.button_insert.TabIndex = 4;
            this.button_insert.Text = "插入数据";
            this.button_insert.UseVisualStyleBackColor = true;
            this.button_insert.Click += new System.EventHandler(this.button_insert_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 438);
            this.Controls.Add(this.button_insert);
            this.Controls.Add(this.button_Index);
            this.Controls.Add(this.button_createTable);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.button_createDataBase);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_createDataBase;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Button button_createTable;
        private System.Windows.Forms.Button button_Index;
        private System.Windows.Forms.Button button_insert;
    }
}

