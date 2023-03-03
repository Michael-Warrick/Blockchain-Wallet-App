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
    /// <summary>
    /// Class <c>BlockchainApp</c> is where all the UI logic resides.
    /// </summary>
    public partial class BlockchainApp : Form
    {
        Blockchain blockchain;
        Wallet.Wallet wallet;

        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain();

            printerConsole.Text = "A new Blockchain has been created and initialised!\n";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Method <c>ShowMessage</c> is a helper method that creates a popup window for the given parameters.
        /// </summary>        
        public void ShowMessage(String title, String message, MessageBoxButtons buttonOptions, MessageBoxIcon type, MessageBoxDefaultButton defaultButton)
        {
            DialogResult resultWindow = MessageBox.Show(message, title, buttonOptions, type, defaultButton);
        }

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

        /// <summary>
        /// Method <c>printButton_Click</c> prints a specific block and its associated elements for a given index.
        /// </summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            int index = 0;

            if (Int32.TryParse(blockIndexTextBox.Text, out index))
            {
                printerConsole.Text = blockchain.GetBlockAsString(index);
            }
        }

        /// <summary>
        /// Method <c>showAllButton_Click</c> prints all blocks to the screen.
        /// </summary>
        private void showAllButton_Click(object sender, EventArgs e)
        {
            String output = String.Empty;

            foreach (Block block in blockchain.blocks)
            {
                output += blockchain.GetBlockAsString((int)block.index);
            }

            printerConsole.Text = output;
        }

        /// <summary>
        /// Method <c>createWalletButton_Click</c> creates a new wallet with a Public/Private Key.
        /// </summary>
        private void createWalletButton_Click(object sender, EventArgs e)
        {
            String privateKey;

            wallet = new Wallet.Wallet(out privateKey);
            publicKeyTextBox.Text = wallet.publicID;
            privateKeyTextBox.Text = privateKey;

            String message = "A wallet has been successfully created with a current balance of: " + "₦" + wallet.GetBalance();
            ShowMessage("Message", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Method <c>checkKeysButton_Click</c> checks key validity.
        /// </summary>
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

        /// <summary>
        /// Method <c>newBlockButton_Click</c> adds a new block to the blockchain through mining.
        /// </summary>
        private void newBlockButton_Click(object sender, EventArgs e)
        {
            if (wallet != null)
            {
                List<Transaction> transactions = blockchain.GetPendingTransactions();

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

        /// <summary>
        /// Method <c>newTransactionButton_Click</c> creates a new transaction with an amount and a fee, only allowing for transactions to occur if wallet balance is greater/equal to total transfer amount.
        /// </summary>
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

        // A pseudo way of introducing foreign currency into the system
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
                if (!blockchain.ValidateMerkleRoot(blockchain.blocks[0]))
                {
                    String errorMessage = "Blockchain is invalid, doesn't satisfy Merkle Root Algorithm.";
                    ShowMessage("Contiguity Check", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    String message = "Blockchain is valid.";
                    ShowMessage("Contiguity Check", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }

                return;
            }

            // Checking all blocks for previous block's hash and also verifying transactions through satisfying merkle root.
            for (int i = 1; i < blockchain.blocks.Count; i++)
            {
                String currentBlockPreviousHash = blockchain.blocks[i].previousBlockHash;
                String previousBlockHash = blockchain.blocks[i - 1].hash;
                Boolean merkleRootIsValid = blockchain.ValidateMerkleRoot(blockchain.blocks[i]);

                if (!merkleRootIsValid)
                {
                    blockchain.isValid = false;

                    String errorMessage = "Blockchain is invalid, doesn't satisfy Merkle Root Algorithm.";
                    ShowMessage("Contiguity Check", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    break;
                }

                if (currentBlockPreviousHash != previousBlockHash)
                {
                    blockchain.isValid = false;

                    String errorMessage = "Blockchain is invalid, previous hash is NOT valid.";
                    ShowMessage("Previous Hash Invalidation Test", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    break;
                }

                if (blockchain.blocks[i].hash != blockchain.blocks[i].CreateHash())
                {
                    blockchain.isValid = false;

                    String errorMessage = "Blockchain is invalid, hash is NOT valid.";
                    ShowMessage("Hash Invalidation Test", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    break;
                }
            }

            if (blockchain.isValid)
            {
                String message = "Blockchain is valid.";
                ShowMessage("Contiguity Check", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void invalidBlockHashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blockchain.GetEndBlock().hash = null;

            string message = "Hash of last block has been set to [null]. Please check the blockchain's integrity.";
            ShowMessage("Contiguity Check", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void invalidBlockPreviousHashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blockchain.GetEndBlock().previousBlockHash = null;

            string message = "Previous of last block has been set to [null]. Please check the blockchain's integrity.";
            ShowMessage("Contiguity Check", message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
    }
}