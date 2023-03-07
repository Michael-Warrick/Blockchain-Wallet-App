using BlockchainAssignment.Wallet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {
        Blockchain blockchain;
        Wallet.Wallet wallet;

        // App Constructor
        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain();

            printerConsole.Text = "A new Blockchain has been created and initialised!\n";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        // Helper method that creates a popup window for the given parameters 
        public void ShowMessage(String title, String message, MessageBoxButtons buttonOptions, MessageBoxIcon type, MessageBoxDefaultButton defaultButton)
        {
            DialogResult resultWindow = MessageBox.Show(message, title, buttonOptions, type, defaultButton);
        }

        // Helper Function
        public bool isPositiveDouble(String text)
        {
            bool isEmpty = string.IsNullOrEmpty(text);
            bool isDouble = text.Contains(".");
            bool isNegative = text.Contains("-");

            if (isEmpty || !isDouble || isNegative)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        // Prints a specific block and its associated elements for a given index
        private void printButton_Click(object sender, EventArgs e)
        {
            int index = 0;

            if (Int32.TryParse(blockIndexTextBox.Text, out index))
            {
                printerConsole.Text = blockchain.GetBlockAsString(index);
            }
        }

        // Prints all blocks to the screen
        private void showAllButton_Click(object sender, EventArgs e)
        {
            String output = String.Empty;

            foreach (Block block in blockchain.blocks)
            {
                output += blockchain.GetBlockAsString((int)block.index);
            }

            printerConsole.Text = output;
        }

        // Creates a new wallet with a Public/Private Key
        private void createWalletButton_Click(object sender, EventArgs e)
        {
            String privateKey;

            wallet = new Wallet.Wallet(out privateKey);
            publicKeyTextBox.Text = wallet.publicID;
            privateKeyTextBox.Text = privateKey;

            String message = "A wallet has been successfully created with a current balance of: " + "₦" + wallet.GetBalance();
            ShowMessage("Message", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        // Checks validity of keys
        private void checkKeysButton_Click(object sender, EventArgs e)
        {
            if (Wallet.Wallet.ValidatePrivateKey(privateKeyTextBox.Text, publicKeyTextBox.Text))
            {
                String message = "Keys are valid.";
                ShowMessage("Message", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                String message = "Keys are invalid.";
                ShowMessage("Error", message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        // Adds a new block to the blockchain through mining
        private void newBlockButton_Click(object sender, EventArgs e)
        {
            if (wallet != null)
            {
                List<Transaction> transactions = blockchain.GetPendingTransactions();

                SortTransactionsByPreference(transactions);

                Stopwatch stopwatch = new Stopwatch();
                double elapsedTime;

                stopwatch.Start();
                Block newBlock = new Block(blockchain.GetEndBlock(), transactions, blockchain.difficulty, publicKeyTextBox.Text);
                stopwatch.Stop();
                elapsedTime = stopwatch.Elapsed.TotalSeconds;

                newBlock.mineTime = elapsedTime;

                blockchain.blocks.Add(newBlock);
                printerConsole.Text = blockchain.ToString();

                blockchain.AdjustDifficulty(blockchain, 4);
            }

            else
            {
                String message = "Wallet impossible to locate, please create one before mining for rewards.";
                ShowMessage("Error", message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        // Creates a new transaction with an amount and a fee, only allowing for transactions to occur if wallet balance is greater/equal to total transfer amount
        private void newTransactionButton_Click(object sender, EventArgs e)
        {
            bool amountIsEmpty = string.IsNullOrEmpty(amountTextBox.Text);
            bool feeIsEmtpy = string.IsNullOrEmpty(feeTextBox.Text);
            bool amountIsDouble = amountTextBox.Text.Contains(".");
            bool feeIsDouble = feeTextBox.Text.Contains(".");

            if ((amountIsEmpty || feeIsEmtpy) && (!amountIsDouble || !feeIsDouble))
            {
                printerConsole.Text = "ERROR::TRANSACTION_FAILURE: Transactions require both a transferrable amount and a fee value, and to be in the correct format (Double)!";
            }
            else
            {
                Double currentBalance = blockchain.CalculateBalance(publicKeyTextBox.Text);
                Double transactionCost = Double.Parse(amountTextBox.Text) + Double.Parse(feeTextBox.Text);

                if (transactionCost <= currentBalance)
                {
                    Transaction transaction = new Transaction(publicKeyTextBox.Text, receiverKeyTextBox.Text, Double.Parse(amountTextBox.Text), Double.Parse(feeTextBox.Text), privateKeyTextBox.Text);
                    transaction.transactionCategory = Transaction.Category.STANDARD;
                    blockchain.transactionsPool.Add(transaction);

                    printerConsole.Text = transaction.ToString();
                }

                else
                {
                    printerConsole.Text = "ERROR::TRANSACTION_FAILURE: Insufficient funds!";
                }
            }
        }

        // Prints all transactions waiting to be submitted
        private void pendingTransactionsButton_Click(object sender, EventArgs e)
        {
            String transactionOutput = String.Empty;
            List<Transaction> transactions = blockchain.GetPendingTransactions();

            foreach (Transaction transaction in transactions)
            {
                transactionOutput += transaction.ToString() + "\n";
            }

            printerConsole.Text = transactionOutput;
            blockchain.transactionsPool = transactions;
        }

        // Simulating introducing foreign currency into the system
        private void depositButton_Click(object sender, EventArgs e)
        {
            bool depositAmountIsEmpty = string.IsNullOrEmpty(depositTextBox.Text);
            bool depositIsDouble = depositTextBox.Text.Contains(".");
            bool depositIsNegative = depositTextBox.Text.Contains("-");

            // Data sanitization
            if (depositAmountIsEmpty || !depositIsDouble || depositIsNegative)
            {
                String message = "Deposit amount is invalid! Please enter a valid amount (Positive Double)";
                ShowMessage("Error", message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            else
            {
                Double currentDeposit = Double.Parse(depositTextBox.Text);

                if (currentDeposit > 0.0 && wallet != null)
                {
                    String message = "An amount of " + "₦" + depositTextBox.Text + " has been deposited into your account.";
                    ShowMessage("Success", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    Transaction transaction = new Transaction("Bank : :API->DEPOSIT", publicKeyTextBox.Text, currentDeposit, 0.0, privateKeyTextBox.Text);
                    transaction.transactionCategory = Transaction.Category.DEPOSIT;
                    blockchain.transactionsPool.Add(transaction);
                }

                else if (currentDeposit == 0.0 || wallet == null)
                {
                    String message = "Either deposit is zero, or no associated wallet was located.";
                    ShowMessage("Error", message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }

        // Allows users to simulate withdrawing from their "crypto wallet"
        private void withdrawalButton_Click(object sender, EventArgs e)
        {
            Double currentWithdrawal = Double.Parse(withdrawalTextBox.Text);
            Double availableBalance = blockchain.CalculateBalance(publicKeyTextBox.Text);

            if (currentWithdrawal > 0.0 && wallet != null && currentWithdrawal <= availableBalance)
            {
                String message = "An amount of " + "₦" + withdrawalTextBox.Text + " has been withdrawn from your account.";
                ShowMessage("Success", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                Transaction transaction = new Transaction(publicKeyTextBox.Text, "Bank : : API->Credit", currentWithdrawal, 0.0, privateKeyTextBox.Text);
                transaction.transactionCategory = Transaction.Category.WITHDRAWAL;
                blockchain.transactionsPool.Add(transaction);
            }

            else if (currentWithdrawal > availableBalance)
            {
                String message = "Withdrawal exceeds available currency in balance.";
                ShowMessage("Error", message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            else if (!isPositiveDouble(withdrawalTextBox.Text) || currentWithdrawal == 0.0 || wallet == null)
            {
                String errorMessage = "Amount must be a non-zero, positive double and an associated Wallet created.";
                ShowMessage("Error", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        // Checks the wallet balance through traversing all the nodes and making note of all credits/debits
        private void checkBalanceButton_Click(object sender, EventArgs e)
        {
            if (wallet == null)
            {
                String message = "No associated wallet was located.";
                ShowMessage("Error", message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            else
            {
                String message = "₦" + blockchain.CalculateBalance(publicKeyTextBox.Text).ToString() + ".";
                ShowMessage("Balance", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        // Ensures that the blockchain is integral, able to point from block to block all the way to the genesis block
        private void checkIntegrityButton_Click(object sender, EventArgs e)
        {
            if (blockchain.blocks.Count == 1)
            {
                if (!blockchain.ValidateHash(blockchain.blocks[0]))
                {
                    String message = "Blockchain is invalid";
                    ShowMessage("Contiguity Check", message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    String message = "Blockchain is valid.";
                    ShowMessage("Contiguity Check", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                return;
            }

            for (int i = 1; i < blockchain.blocks.Count; i++)
            {
                if (
                    blockchain.blocks[i].previousBlockHash != blockchain.blocks[i - 1].hash || // Check hash "chain"
                    !blockchain.ValidateHash(blockchain.blocks[i]) ||  // Check each blocks hash
                    !Blockchain.ValidateMerkleRoot(blockchain.blocks[i]) // Check transaction integrity using Merkle Root
                )
                {
                    String message = "Blockchain is invalid.\n\n";
                    message += "\nValidates Hash: " + blockchain.ValidateHash(blockchain.blocks[i]).ToString();
                    message += "\nValidates Previous Hash: " + (blockchain.blocks[i].previousBlockHash == blockchain.blocks[i - 1].hash).ToString();
                    message += "\nValidates Merkle Root: " + Blockchain.ValidateMerkleRoot(blockchain.blocks[i]).ToString();
                    ShowMessage("Contiguity Check", message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return;
                }
            }

            String windowMessage = "Blockchain is valid.";
            ShowMessage("Contiguity Check", windowMessage, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        // Enum for setting the style of mining
        public enum MiningPreference
        {
            AddressPreference,
            Altruistic,
            Greedy,
            Random
        }

        public MiningPreference currentPreference = MiningPreference.Altruistic;

        private bool isAltruisticEnabled = true;
        private bool isGreedyEnabled = false;
        private bool isPreferredAddressEnabled = false;
        private bool isRandomEnabled = false;

        private string preferredAddress = "";

        // Helper Function to set the preference easily
        private void SetPreference(MiningPreference miningPreference)
        {
            currentPreference = miningPreference;

            isAltruisticEnabled = miningPreference == MiningPreference.Altruistic;
            isGreedyEnabled = miningPreference == MiningPreference.Greedy;
            isPreferredAddressEnabled = miningPreference == MiningPreference.AddressPreference;
            isRandomEnabled = miningPreference == MiningPreference.Random;

            UpdateMenuItems();
        }

        // Updates the menuItems to display correctly selected item
        private void UpdateMenuItems()
        {
            altruisticToolStripMenuItem1.Checked = isAltruisticEnabled ? true : false;
            greedyToolStripMenuItem1.Checked = isGreedyEnabled ? true : false;
            preferredAddressToolStripMenuItem.Checked = isPreferredAddressEnabled ? true : false;
            randomToolStripMenuItem1.Checked = isRandomEnabled ? true : false;
        }

        private readonly Random random = new Random();

        // Simple shuffle based on random generator
        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // General Sorting Function
        private void SortTransactionsByPreference(List<Transaction> transactions)
        {
            // Altruistic
            if (currentPreference == MiningPreference.Altruistic)
            {
                transactions.Sort((x, y) => x.timeStamp.CompareTo(y.timeStamp));
            }

            // Greedy
            if (currentPreference == MiningPreference.Greedy)
            {
                transactions.Sort((x, y) => y.transactionFee.CompareTo(x.transactionFee));
            }

            // Preferred Address
            if (currentPreference == MiningPreference.AddressPreference)
            {
                // Sorts with preferred address transactions leading and then by timestamp
                transactions.Sort((x, y) =>
                {
                    if (x.destinationAddress == preferredAddress && y.destinationAddress != preferredAddress)
                    {
                        return -1;
                    }
                    if (x.destinationAddress != preferredAddress && y.destinationAddress == preferredAddress)
                    {
                        return 1;
                    }

                    return x.timeStamp.CompareTo(y.timeStamp);
                });
            }

            // Random
            if (currentPreference == MiningPreference.Random)
            {
                Shuffle(transactions);
            }
        }

        private void altruisticToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetPreference(MiningPreference.Altruistic);
        }

        private void greedyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetPreference(MiningPreference.Greedy);
        }

        private void preferredAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            preferredAddress = ShowDialog("Enter a string: ");
            SetPreference(MiningPreference.AddressPreference);
        }

        private void randomToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetPreference(MiningPreference.Random);
        }

        // Helper function for opening a popup with a text prompt for the preferred address test case
        private string ShowDialog(string prompt)
        {
            Form inputBox = new Form();
            inputBox.Text = "Preferred Address";
            inputBox.Size = new System.Drawing.Size(300, 150);
            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.ControlBox = false;

            Label promptLabel = new Label();
            promptLabel.Text = prompt;
            promptLabel.AutoSize = true;
            promptLabel.Location = new System.Drawing.Point(10, 20);
            inputBox.Controls.Add(promptLabel);

            TextBox inputTextBox = new TextBox();
            inputTextBox.Location = new System.Drawing.Point(10, 50);
            inputTextBox.Width = 270;
            inputBox.Controls.Add(inputTextBox);

            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Enabled = false;
            okButton.Location = new System.Drawing.Point(205, 90);
            okButton.Click += (s, e) => inputBox.Close();
            inputBox.Controls.Add(okButton);

            inputTextBox.TextChanged += (s, e) => okButton.Enabled = !string.IsNullOrEmpty(inputTextBox.Text);

            inputBox.ShowDialog();
            return inputTextBox.Text;
        }

        private void invalidateHashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blockchain.GetEndBlock().hash = null;

            string message = "Hash of last block has been set to [null]. Please check the blockchain's integrity.";
            ShowMessage("Contiguity Check", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void invalidatePreviousHashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blockchain.GetEndBlock().previousBlockHash = null;

            string message = "Previous of last block has been set to [null]. Please check the blockchain's integrity.";
            ShowMessage("Contiguity Check", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Please go to: https://github.com/Michael-Warrick/UoR-Final-Year-Blockchain-Assignment";
            ShowMessage("Documentation", message, MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Version 0.0.1 Development Build - Created by Student: 29018078";
            ShowMessage("Credits", message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }
    }
}