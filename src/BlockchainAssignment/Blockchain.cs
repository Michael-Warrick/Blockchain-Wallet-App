using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    internal class Blockchain
    {
        public List<Block> blocks = new List<Block>();
        public List<Transaction> transactionsPool = new List<Transaction>();
        public bool isValid = true;

        int transactionsPerBlock = 5;

        public Blockchain()
        {
            blocks.Add(new Block());
        }

        // Block index error checking (else will cause app to crash)
        public String GetBlockAsString(int index)
        {
            if (index <= (blocks.Count - 1))
            {
                return blocks[index].ToString();
            }
            else 
            {
                return "ERROR::OUT_OF_RANGE:: Index exceeds block count!";
            }
        }

        public Block GetEndBlock()
        {
            return blocks[blocks.Count - 1];
        }

        public List<Transaction> GetPendingTransactions() 
        {
            int n = Math.Min(transactionsPerBlock, transactionsPool.Count);
            List<Transaction> transactions = transactionsPool.GetRange(0, n);

            transactionsPool.RemoveRange(0, n);

            return transactions;
        }

        public double CalculateBalance(String address) 
        {
            double balance = 0.0;
            foreach (Block block in blocks)
            {
                foreach (Transaction transaction in block.transactions)
                {
                    if (transaction.destinationAddress.Equals(address))
                    {
                        balance += transaction.amount;
                    }
                    if (transaction.sourceAddress.Equals(address))
                    {
                        balance -= transaction.amount + transaction.transactionFee;
                    }
                }
            }
            
            return balance;
        }

        public bool ValidateMerkleRoot(Block block) 
        {
            String reMerkle = Block.MerkleRoot(block.transactions);

            return reMerkle.Equals(block.merkleRoot);
        }

        public override string ToString()
        {
            String output = String.Empty;
            foreach (Block block in blocks)
            {
                output += (block.ToString() + "\n");
            }

            return output;
        }
    }
}