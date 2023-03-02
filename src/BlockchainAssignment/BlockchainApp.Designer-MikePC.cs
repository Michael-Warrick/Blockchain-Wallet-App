namespace BlockchainAssignment
{
    partial class BlockchainApp
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
            this.printerConsole = new System.Windows.Forms.RichTextBox();
            this.printButton = new System.Windows.Forms.Button();
            this.blockIndexTextBox = new System.Windows.Forms.TextBox();
            this.createWalletButton = new System.Windows.Forms.Button();
            this.publicKeyTextBox = new System.Windows.Forms.TextBox();
            this.privateKeyTextBox = new System.Windows.Forms.TextBox();
            this.publicKeyLabel = new System.Windows.Forms.Label();
            this.privateKeyLabel = new System.Windows.Forms.Label();
            this.checkKeysButton = new System.Windows.Forms.Button();
            this.newTransactionButton = new System.Windows.Forms.Button();
            this.blockIndexLabel = new System.Windows.Forms.Label();
            this.amountTextBox = new System.Windows.Forms.TextBox();
            this.feeTextBox = new System.Windows.Forms.TextBox();
            this.amountLabel = new System.Windows.Forms.Label();
            this.feeLabel = new System.Windows.Forms.Label();
            this.receiverKeyTextBox = new System.Windows.Forms.TextBox();
            this.receiverKeyLabel = new System.Windows.Forms.Label();
            this.newBlockButton = new System.Windows.Forms.Button();
            this.showAllButton = new System.Windows.Forms.Button();
            this.pendingTransactionsButton = new System.Windows.Forms.Button();
            this.depositButton = new System.Windows.Forms.Button();
            this.depositAmountLabel = new System.Windows.Forms.Label();
            this.depositTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // printerConsole
            // 
            this.printerConsole.BackColor = System.Drawing.SystemColors.InfoText;
            this.printerConsole.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.printerConsole.Location = new System.Drawing.Point(22, 25);
            this.printerConsole.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.printerConsole.Name = "printerConsole";
            this.printerConsole.Size = new System.Drawing.Size(1509, 672);
            this.printerConsole.TabIndex = 0;
            this.printerConsole.Text = "";
            // 
            // printButton
            // 
            this.printButton.Location = new System.Drawing.Point(22, 726);
            this.printButton.Margin = new System.Windows.Forms.Padding(4);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(188, 44);
            this.printButton.TabIndex = 1;
            this.printButton.Text = "Block Info";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // blockIndexTextBox
            // 
            this.blockIndexTextBox.Location = new System.Drawing.Point(217, 731);
            this.blockIndexTextBox.Name = "blockIndexTextBox";
            this.blockIndexTextBox.Size = new System.Drawing.Size(37, 34);
            this.blockIndexTextBox.TabIndex = 2;
            // 
            // createWalletButton
            // 
            this.createWalletButton.Location = new System.Drawing.Point(851, 726);
            this.createWalletButton.Name = "createWalletButton";
            this.createWalletButton.Size = new System.Drawing.Size(150, 75);
            this.createWalletButton.TabIndex = 3;
            this.createWalletButton.Text = "Create Wallet";
            this.createWalletButton.UseVisualStyleBackColor = true;
            this.createWalletButton.Click += new System.EventHandler(this.createWalletButton_Click);
            // 
            // publicKeyTextBox
            // 
            this.publicKeyTextBox.Location = new System.Drawing.Point(1007, 726);
            this.publicKeyTextBox.Name = "publicKeyTextBox";
            this.publicKeyTextBox.Size = new System.Drawing.Size(416, 34);
            this.publicKeyTextBox.TabIndex = 4;
            // 
            // privateKeyTextBox
            // 
            this.privateKeyTextBox.Location = new System.Drawing.Point(1007, 767);
            this.privateKeyTextBox.Name = "privateKeyTextBox";
            this.privateKeyTextBox.Size = new System.Drawing.Size(416, 34);
            this.privateKeyTextBox.TabIndex = 5;
            // 
            // publicKeyLabel
            // 
            this.publicKeyLabel.AutoSize = true;
            this.publicKeyLabel.ForeColor = System.Drawing.Color.White;
            this.publicKeyLabel.Location = new System.Drawing.Point(1429, 729);
            this.publicKeyLabel.Name = "publicKeyLabel";
            this.publicKeyLabel.Size = new System.Drawing.Size(102, 28);
            this.publicKeyLabel.TabIndex = 6;
            this.publicKeyLabel.Text = "Public Key";
            // 
            // privateKeyLabel
            // 
            this.privateKeyLabel.AutoSize = true;
            this.privateKeyLabel.ForeColor = System.Drawing.Color.White;
            this.privateKeyLabel.Location = new System.Drawing.Point(1429, 770);
            this.privateKeyLabel.Name = "privateKeyLabel";
            this.privateKeyLabel.Size = new System.Drawing.Size(109, 28);
            this.privateKeyLabel.TabIndex = 7;
            this.privateKeyLabel.Text = "Private Key";
            // 
            // checkKeysButton
            // 
            this.checkKeysButton.Location = new System.Drawing.Point(851, 807);
            this.checkKeysButton.Name = "checkKeysButton";
            this.checkKeysButton.Size = new System.Drawing.Size(680, 83);
            this.checkKeysButton.TabIndex = 8;
            this.checkKeysButton.Text = "Check Key Validity";
            this.checkKeysButton.UseVisualStyleBackColor = true;
            this.checkKeysButton.Click += new System.EventHandler(this.checkKeysButton_Click);
            // 
            // newTransactionButton
            // 
            this.newTransactionButton.Location = new System.Drawing.Point(22, 940);
            this.newTransactionButton.Name = "newTransactionButton";
            this.newTransactionButton.Size = new System.Drawing.Size(188, 84);
            this.newTransactionButton.TabIndex = 10;
            this.newTransactionButton.Text = "New Transaction";
            this.newTransactionButton.UseVisualStyleBackColor = true;
            this.newTransactionButton.Click += new System.EventHandler(this.newTransactionButton_Click);
            // 
            // blockIndexLabel
            // 
            this.blockIndexLabel.AutoSize = true;
            this.blockIndexLabel.ForeColor = System.Drawing.Color.White;
            this.blockIndexLabel.Location = new System.Drawing.Point(260, 734);
            this.blockIndexLabel.Name = "blockIndexLabel";
            this.blockIndexLabel.Size = new System.Drawing.Size(59, 28);
            this.blockIndexLabel.TabIndex = 11;
            this.blockIndexLabel.Text = "Index";
            // 
            // amountTextBox
            // 
            this.amountTextBox.Location = new System.Drawing.Point(216, 940);
            this.amountTextBox.Name = "amountTextBox";
            this.amountTextBox.Size = new System.Drawing.Size(102, 34);
            this.amountTextBox.TabIndex = 12;
            // 
            // feeTextBox
            // 
            this.feeTextBox.Location = new System.Drawing.Point(216, 987);
            this.feeTextBox.Name = "feeTextBox";
            this.feeTextBox.Size = new System.Drawing.Size(102, 34);
            this.feeTextBox.TabIndex = 13;
            // 
            // amountLabel
            // 
            this.amountLabel.AutoSize = true;
            this.amountLabel.ForeColor = System.Drawing.Color.White;
            this.amountLabel.Location = new System.Drawing.Point(324, 943);
            this.amountLabel.Name = "amountLabel";
            this.amountLabel.Size = new System.Drawing.Size(83, 28);
            this.amountLabel.TabIndex = 14;
            this.amountLabel.Text = "Amount";
            // 
            // feeLabel
            // 
            this.feeLabel.AutoSize = true;
            this.feeLabel.ForeColor = System.Drawing.Color.White;
            this.feeLabel.Location = new System.Drawing.Point(324, 990);
            this.feeLabel.Name = "feeLabel";
            this.feeLabel.Size = new System.Drawing.Size(42, 28);
            this.feeLabel.TabIndex = 15;
            this.feeLabel.Text = "Fee";
            // 
            // receiverKeyTextBox
            // 
            this.receiverKeyTextBox.Location = new System.Drawing.Point(851, 987);
            this.receiverKeyTextBox.Name = "receiverKeyTextBox";
            this.receiverKeyTextBox.Size = new System.Drawing.Size(553, 34);
            this.receiverKeyTextBox.TabIndex = 16;
            // 
            // receiverKeyLabel
            // 
            this.receiverKeyLabel.AutoSize = true;
            this.receiverKeyLabel.ForeColor = System.Drawing.Color.White;
            this.receiverKeyLabel.Location = new System.Drawing.Point(1410, 990);
            this.receiverKeyLabel.Name = "receiverKeyLabel";
            this.receiverKeyLabel.Size = new System.Drawing.Size(121, 28);
            this.receiverKeyLabel.TabIndex = 17;
            this.receiverKeyLabel.Text = "Receiver Key";
            // 
            // newBlockButton
            // 
            this.newBlockButton.Location = new System.Drawing.Point(22, 860);
            this.newBlockButton.Name = "newBlockButton";
            this.newBlockButton.Size = new System.Drawing.Size(188, 74);
            this.newBlockButton.TabIndex = 9;
            this.newBlockButton.Text = "New Block";
            this.newBlockButton.UseVisualStyleBackColor = true;
            this.newBlockButton.Click += new System.EventHandler(this.newBlockButton_Click);
            // 
            // showAllButton
            // 
            this.showAllButton.Location = new System.Drawing.Point(22, 777);
            this.showAllButton.Name = "showAllButton";
            this.showAllButton.Size = new System.Drawing.Size(188, 44);
            this.showAllButton.TabIndex = 18;
            this.showAllButton.Text = "Show All";
            this.showAllButton.UseVisualStyleBackColor = true;
            this.showAllButton.Click += new System.EventHandler(this.showAllButton_Click);
            // 
            // pendingTransactionsButton
            // 
            this.pendingTransactionsButton.Location = new System.Drawing.Point(413, 940);
            this.pendingTransactionsButton.Name = "pendingTransactionsButton";
            this.pendingTransactionsButton.Size = new System.Drawing.Size(290, 84);
            this.pendingTransactionsButton.TabIndex = 19;
            this.pendingTransactionsButton.Text = "Pending Transactions";
            this.pendingTransactionsButton.UseVisualStyleBackColor = true;
            this.pendingTransactionsButton.Click += new System.EventHandler(this.pendingTransactionsButton_Click);
            // 
            // depositButton
            // 
            this.depositButton.Location = new System.Drawing.Point(851, 896);
            this.depositButton.Name = "depositButton";
            this.depositButton.Size = new System.Drawing.Size(150, 34);
            this.depositButton.TabIndex = 20;
            this.depositButton.Text = "Deposit";
            this.depositButton.UseVisualStyleBackColor = true;
            this.depositButton.Click += new System.EventHandler(this.depositButton_Click);
            // 
            // depositAmountLabel
            // 
            this.depositAmountLabel.AutoSize = true;
            this.depositAmountLabel.ForeColor = System.Drawing.Color.White;
            this.depositAmountLabel.Location = new System.Drawing.Point(1115, 899);
            this.depositAmountLabel.Name = "depositAmountLabel";
            this.depositAmountLabel.Size = new System.Drawing.Size(83, 28);
            this.depositAmountLabel.TabIndex = 22;
            this.depositAmountLabel.Text = "Amount";
            // 
            // depositTextBox
            // 
            this.depositTextBox.Location = new System.Drawing.Point(1007, 896);
            this.depositTextBox.Name = "depositTextBox";
            this.depositTextBox.Size = new System.Drawing.Size(102, 34);
            this.depositTextBox.TabIndex = 21;
            // 
            // BlockchainApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(1557, 1036);
            this.Controls.Add(this.depositAmountLabel);
            this.Controls.Add(this.depositTextBox);
            this.Controls.Add(this.depositButton);
            this.Controls.Add(this.pendingTransactionsButton);
            this.Controls.Add(this.showAllButton);
            this.Controls.Add(this.receiverKeyLabel);
            this.Controls.Add(this.receiverKeyTextBox);
            this.Controls.Add(this.feeLabel);
            this.Controls.Add(this.amountLabel);
            this.Controls.Add(this.feeTextBox);
            this.Controls.Add(this.amountTextBox);
            this.Controls.Add(this.blockIndexLabel);
            this.Controls.Add(this.newTransactionButton);
            this.Controls.Add(this.newBlockButton);
            this.Controls.Add(this.checkKeysButton);
            this.Controls.Add(this.privateKeyLabel);
            this.Controls.Add(this.publicKeyLabel);
            this.Controls.Add(this.privateKeyTextBox);
            this.Controls.Add(this.publicKeyTextBox);
            this.Controls.Add(this.createWalletButton);
            this.Controls.Add(this.blockIndexTextBox);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.printerConsole);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "BlockchainApp";
            this.Text = "Simple C# Blockchain App - 29018078";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox printerConsole;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.TextBox blockIndexTextBox;
        private System.Windows.Forms.Button createWalletButton;
        private System.Windows.Forms.TextBox publicKeyTextBox;
        private System.Windows.Forms.TextBox privateKeyTextBox;
        private System.Windows.Forms.Label publicKeyLabel;
        private System.Windows.Forms.Label privateKeyLabel;
        private System.Windows.Forms.Button checkKeysButton;
        private System.Windows.Forms.Button newTransactionButton;
        private System.Windows.Forms.Label blockIndexLabel;
        private System.Windows.Forms.TextBox amountTextBox;
        private System.Windows.Forms.TextBox feeTextBox;
        private System.Windows.Forms.Label amountLabel;
        private System.Windows.Forms.Label feeLabel;
        private System.Windows.Forms.TextBox receiverKeyTextBox;
        private System.Windows.Forms.Label receiverKeyLabel;
        private System.Windows.Forms.Button newBlockButton;
        private System.Windows.Forms.Button showAllButton;
        private System.Windows.Forms.Button pendingTransactionsButton;
        private System.Windows.Forms.Button depositButton;
        private System.Windows.Forms.Label depositAmountLabel;
        private System.Windows.Forms.TextBox depositTextBox;
    }
}

