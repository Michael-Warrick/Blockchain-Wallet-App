using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain();
            printerConsole.Text = "A new Blockchain has been created and initialised!\n";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            int index = 0;

            if (Int32.TryParse(blockIndexTextBox.Text, out index))
            {
                printerConsole.Text = blockchain.GetBlockAsString(index);
            }
        }

        private void showAllButton_Click(object sender, EventArgs e)
        {
            String output = String.Empty;

            foreach (Block block in blockchain.blocks) 
            {
                output += blockchain.GetBlockAsString((int)block.index);
            }

            printerConsole.Text = output;
        }

        private void createWalletButton_Click(object sender, EventArgs e)
        {
            String privateKey;

            wallet = new Wallet.Wallet(out privateKey);
            publicKeyTextBox.Text = wallet.publicID;
            privateKeyTextBox.Text = privateKey;
        }

        private void checkKeysButton_Click(object sender, EventArgs e)
        {
            if (Wallet.Wallet.ValidatePrivateKey(privateKeyTextBox.Text, publicKeyTextBox.Text))
            {
                printerConsole.Text = "Keys are valid.\n";
            }
            else
            {
                printerConsole.Text = "Keys are invalid!\n";
            }
        }

        private void newBlockButton_Click(object sender, EventArgs e)
        {
            List<Transaction> transactions = blockchain.GetPendingTransactions();
            Block newBlock = new Block(blockchain.GetEndBlock(), transactions, publicKeyTextBox.Text);
            blockchain.blocks.Add(newBlock);

            printerConsole.Text = blockchain.ToString();
        }

        private void newTransactionButton_Click(object sender, EventArgs e)
        {
            Transaction transaction = new Transaction(publicKeyTextBox.Text, receiverKeyTextBox.Text, Double.Parse(amountTextBox.Text), Double.Parse(feeTextBox.Text), privateKeyTextBox.Text);
            blockchain.transactionsPool.Add(transaction);

            printerConsole.Text = transaction.ToString();
        }

        private void pendingTransactionsButton_Click(object sender, EventArgs e)
        {
            String transactionOutput = String.Empty;
            List<Transaction> transactions = blockchain.GetPendingTransactions();

            foreach (Transaction transaction in transactions) 
            {
                transactionOutput += "\n" + transaction.ToString();
            }

            printerConsole.Text = transactionOutput;
        }

        private void depositButton_Click(object sender, EventArgs e)
        {
            wallet.balance = Double.Parse(depositTextBox.Text);
        }
    }
}