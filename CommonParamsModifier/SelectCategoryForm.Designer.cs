namespace CommonParamsModifier
{
    partial class ModifierXForm
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
            this.selectCategotyLabel = new System.Windows.Forms.Label();
            this.parametersComboBox = new System.Windows.Forms.ComboBox();
            this.selectParameterLabel = new System.Windows.Forms.Label();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.lessthanUnit = new System.Windows.Forms.Label();
            this.morethanUnit = new System.Windows.Forms.Label();
            this.StorageTypeLabel = new System.Windows.Forms.Label();
            this.selectParameterConfirmButton = new System.Windows.Forms.Button();
            this.filterContainsLabel = new System.Windows.Forms.Label();
            this.stringFilterTextBox = new System.Windows.Forms.TextBox();
            this.lessthanTextBox = new System.Windows.Forms.TextBox();
            this.morethanTextBox = new System.Windows.Forms.TextBox();
            this.lessthanComboBox = new System.Windows.Forms.ComboBox();
            this.morethanComboBox = new System.Windows.Forms.ComboBox();
            this.ViewElementHost = new System.Windows.Forms.Integration.ElementHost();
            this.modifyPanel = new System.Windows.Forms.Panel();
            this.modifyLabel = new System.Windows.Forms.Label();
            this.modifyButton = new System.Windows.Forms.Button();
            this.modifyTextBox = new System.Windows.Forms.TextBox();
            this.categoryCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.filterPanel.SuspendLayout();
            this.modifyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectCategotyLabel
            // 
            this.selectCategotyLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectCategotyLabel.Location = new System.Drawing.Point(12, 382);
            this.selectCategotyLabel.Name = "selectCategotyLabel";
            this.selectCategotyLabel.Size = new System.Drawing.Size(249, 33);
            this.selectCategotyLabel.TabIndex = 1;
            this.selectCategotyLabel.Text = "The model has the following categories, please select tw or more categories:";
            // 
            // parametersComboBox
            // 
            this.parametersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parametersComboBox.FormattingEnabled = true;
            this.parametersComboBox.Location = new System.Drawing.Point(10, 50);
            this.parametersComboBox.Name = "parametersComboBox";
            this.parametersComboBox.Size = new System.Drawing.Size(253, 21);
            this.parametersComboBox.TabIndex = 1;
            this.parametersComboBox.TextChanged += new System.EventHandler(this.parametersComboBoxTextChanged);
            // 
            // selectParameterLabel
            // 
            this.selectParameterLabel.Font = new System.Drawing.Font("Arial", 9.75F);
            this.selectParameterLabel.Location = new System.Drawing.Point(10, 9);
            this.selectParameterLabel.Name = "selectParameterLabel";
            this.selectParameterLabel.Size = new System.Drawing.Size(250, 33);
            this.selectParameterLabel.TabIndex = 0;
            this.selectParameterLabel.Text = "Select a common parameter:";
            // 
            // filterPanel
            // 
            this.filterPanel.Controls.Add(this.lessthanUnit);
            this.filterPanel.Controls.Add(this.morethanUnit);
            this.filterPanel.Controls.Add(this.StorageTypeLabel);
            this.filterPanel.Controls.Add(this.selectParameterConfirmButton);
            this.filterPanel.Controls.Add(this.filterContainsLabel);
            this.filterPanel.Controls.Add(this.stringFilterTextBox);
            this.filterPanel.Controls.Add(this.lessthanTextBox);
            this.filterPanel.Controls.Add(this.selectParameterLabel);
            this.filterPanel.Controls.Add(this.morethanTextBox);
            this.filterPanel.Controls.Add(this.parametersComboBox);
            this.filterPanel.Controls.Add(this.lessthanComboBox);
            this.filterPanel.Controls.Add(this.morethanComboBox);
            this.filterPanel.Location = new System.Drawing.Point(268, 373);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(273, 248);
            this.filterPanel.TabIndex = 3;
            // 
            // lessthanUnit
            // 
            this.lessthanUnit.Location = new System.Drawing.Point(235, 126);
            this.lessthanUnit.Name = "lessthanUnit";
            this.lessthanUnit.Size = new System.Drawing.Size(32, 15);
            this.lessthanUnit.TabIndex = 11;
            this.lessthanUnit.Text = "(mm)";
            this.lessthanUnit.Visible = false;
            // 
            // morethanUnit
            // 
            this.morethanUnit.Location = new System.Drawing.Point(235, 99);
            this.morethanUnit.Name = "morethanUnit";
            this.morethanUnit.Size = new System.Drawing.Size(32, 15);
            this.morethanUnit.TabIndex = 10;
            this.morethanUnit.Text = "(mm)";
            this.morethanUnit.Visible = false;
            // 
            // StorageTypeLabel
            // 
            this.StorageTypeLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StorageTypeLabel.Location = new System.Drawing.Point(11, 74);
            this.StorageTypeLabel.Name = "StorageTypeLabel";
            this.StorageTypeLabel.Size = new System.Drawing.Size(252, 22);
            this.StorageTypeLabel.TabIndex = 9;
            this.StorageTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // selectParameterConfirmButton
            // 
            this.selectParameterConfirmButton.Location = new System.Drawing.Point(180, 170);
            this.selectParameterConfirmButton.Name = "selectParameterConfirmButton";
            this.selectParameterConfirmButton.Size = new System.Drawing.Size(80, 25);
            this.selectParameterConfirmButton.TabIndex = 8;
            this.selectParameterConfirmButton.Text = "Confirm";
            this.selectParameterConfirmButton.UseVisualStyleBackColor = true;
            this.selectParameterConfirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // filterContainsLabel
            // 
            this.filterContainsLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterContainsLabel.Location = new System.Drawing.Point(11, 96);
            this.filterContainsLabel.Name = "filterContainsLabel";
            this.filterContainsLabel.Size = new System.Drawing.Size(65, 21);
            this.filterContainsLabel.TabIndex = 2;
            this.filterContainsLabel.Text = "Contains:";
            this.filterContainsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.filterContainsLabel.Visible = false;
            // 
            // stringFilterTextBox
            // 
            this.stringFilterTextBox.Location = new System.Drawing.Point(82, 96);
            this.stringFilterTextBox.Name = "stringFilterTextBox";
            this.stringFilterTextBox.Size = new System.Drawing.Size(178, 20);
            this.stringFilterTextBox.TabIndex = 3;
            this.stringFilterTextBox.Visible = false;
            // 
            // lessthanTextBox
            // 
            this.lessthanTextBox.Location = new System.Drawing.Point(82, 123);
            this.lessthanTextBox.Name = "lessthanTextBox";
            this.lessthanTextBox.Size = new System.Drawing.Size(151, 20);
            this.lessthanTextBox.TabIndex = 7;
            this.lessthanTextBox.Visible = false;
            // 
            // morethanTextBox
            // 
            this.morethanTextBox.Location = new System.Drawing.Point(82, 96);
            this.morethanTextBox.Name = "morethanTextBox";
            this.morethanTextBox.Size = new System.Drawing.Size(151, 20);
            this.morethanTextBox.TabIndex = 5;
            this.morethanTextBox.Visible = false;
            // 
            // lessthanComboBox
            // 
            this.lessthanComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lessthanComboBox.FormattingEnabled = true;
            this.lessthanComboBox.Items.AddRange(new object[] {
            "<",
            "<="});
            this.lessthanComboBox.Location = new System.Drawing.Point(11, 123);
            this.lessthanComboBox.Name = "lessthanComboBox";
            this.lessthanComboBox.Size = new System.Drawing.Size(56, 21);
            this.lessthanComboBox.TabIndex = 6;
            this.lessthanComboBox.Visible = false;
            // 
            // morethanComboBox
            // 
            this.morethanComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.morethanComboBox.FormattingEnabled = true;
            this.morethanComboBox.Items.AddRange(new object[] {
            ">",
            ">="});
            this.morethanComboBox.Location = new System.Drawing.Point(11, 96);
            this.morethanComboBox.Name = "morethanComboBox";
            this.morethanComboBox.Size = new System.Drawing.Size(56, 21);
            this.morethanComboBox.TabIndex = 4;
            this.morethanComboBox.Visible = false;
            // 
            // ViewElementHost
            // 
            this.ViewElementHost.Location = new System.Drawing.Point(12, 17);
            this.ViewElementHost.Name = "ViewElementHost";
            this.ViewElementHost.Size = new System.Drawing.Size(735, 350);
            this.ViewElementHost.TabIndex = 0;
            this.ViewElementHost.Text = "elementHost1";
            this.ViewElementHost.Child = null;
            // 
            // modifyPanel
            // 
            this.modifyPanel.Controls.Add(this.modifyLabel);
            this.modifyPanel.Controls.Add(this.modifyButton);
            this.modifyPanel.Controls.Add(this.modifyTextBox);
            this.modifyPanel.Location = new System.Drawing.Point(547, 373);
            this.modifyPanel.Name = "modifyPanel";
            this.modifyPanel.Size = new System.Drawing.Size(200, 248);
            this.modifyPanel.TabIndex = 4;
            // 
            // modifyLabel
            // 
            this.modifyLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modifyLabel.Location = new System.Drawing.Point(0, 9);
            this.modifyLabel.Name = "modifyLabel";
            this.modifyLabel.Size = new System.Drawing.Size(200, 33);
            this.modifyLabel.TabIndex = 0;
            this.modifyLabel.Text = "Type in the new value:";
            // 
            // modifyButton
            // 
            this.modifyButton.Location = new System.Drawing.Point(120, 81);
            this.modifyButton.Name = "modifyButton";
            this.modifyButton.Size = new System.Drawing.Size(80, 25);
            this.modifyButton.TabIndex = 2;
            this.modifyButton.Text = "Modify";
            this.modifyButton.UseVisualStyleBackColor = true;
            this.modifyButton.Click += new System.EventHandler(this.modifyButton_Click);
            // 
            // modifyTextBox
            // 
            this.modifyTextBox.Location = new System.Drawing.Point(0, 50);
            this.modifyTextBox.Name = "modifyTextBox";
            this.modifyTextBox.Size = new System.Drawing.Size(200, 20);
            this.modifyTextBox.TabIndex = 1;
            // 
            // categoryCheckedListBox
            // 
            this.categoryCheckedListBox.CheckOnClick = true;
            this.categoryCheckedListBox.FormattingEnabled = true;
            this.categoryCheckedListBox.Location = new System.Drawing.Point(12, 423);
            this.categoryCheckedListBox.Name = "categoryCheckedListBox";
            this.categoryCheckedListBox.ScrollAlwaysVisible = true;
            this.categoryCheckedListBox.Size = new System.Drawing.Size(249, 199);
            this.categoryCheckedListBox.TabIndex = 2;
            this.categoryCheckedListBox.SelectedValueChanged += new System.EventHandler(this.CheckedListBoxCheckedItemsChanged);
            // 
            // ModifierXForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 633);
            this.Controls.Add(this.categoryCheckedListBox);
            this.Controls.Add(this.modifyPanel);
            this.Controls.Add(this.ViewElementHost);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(this.selectCategotyLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ModifierXForm";
            this.Text = " ModifierX";
            this.filterPanel.ResumeLayout(false);
            this.filterPanel.PerformLayout();
            this.modifyPanel.ResumeLayout(false);
            this.modifyPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label selectCategotyLabel;
        private System.Windows.Forms.ComboBox parametersComboBox;
        private System.Windows.Forms.Label selectParameterLabel;
        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.TextBox lessthanTextBox;
        private System.Windows.Forms.TextBox morethanTextBox;
        private System.Windows.Forms.ComboBox lessthanComboBox;
        private System.Windows.Forms.ComboBox morethanComboBox;
        private System.Windows.Forms.Label filterContainsLabel;
        private System.Windows.Forms.TextBox stringFilterTextBox;
        private System.Windows.Forms.Button selectParameterConfirmButton;
        private System.Windows.Forms.Integration.ElementHost ViewElementHost;
        private System.Windows.Forms.Panel modifyPanel;
        private System.Windows.Forms.TextBox modifyTextBox;
        private System.Windows.Forms.Button modifyButton;
        private System.Windows.Forms.CheckedListBox categoryCheckedListBox;
        private System.Windows.Forms.Label modifyLabel;
        private System.Windows.Forms.Label StorageTypeLabel;
        private System.Windows.Forms.Label lessthanUnit;
        private System.Windows.Forms.Label morethanUnit;
    }
}