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

        int transactionsPerBlock = 5;
        double requiredTransactionSpeed = 10.0;
        public int difficulty = 5;

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

        // Gets all transactions waiting to be submitted
        public List<Transaction> GetPendingTransactions() 
        {
            int n = Math.Min(transactionsPerBlock, transactionsPool.Count);
            List<Transaction> transactions = transactionsPool.GetRange(0, n);

            transactionsPool.RemoveRange(0, n);

            return transactions;
        }

        // Calculates balance based on pending and saved transactions to avoid double spending
        public double CalculateBalance(String address) 
        {
            double balance = 0.0;

            foreach (Transaction t in transactionsPool) 
            {
                if (t.destinationAddress.Equals(address))
                {
                    balance += t.amount;
                }

                if (t.sourceAddress.Equals(address))
                {
                    if (t.transactionFee > 0.0)
                    {
                        balance -= t.amount + t.transactionFee;
                    }

                    else
                    {
                        balance -= t.amount;
                    }
                }
            }

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

        public bool ValidateHash(Block block)
        {
            String reHash = Block.CreateHash(block.hashCode + block.nonce.ToString());

            return reHash.Equals(block.hash);
        }

        public static bool ValidateMerkleRoot(Block block)
        {
            String reMerkle = Block.MerkleRoot(block.transactions);

            return reMerkle.Equals(block.merkleRoot);
        }

        // Adjusts mining difficulty based on time it takes to mine
        public void AdjustDifficulty(Blockchain blockchain, int chunkSize)
        {
            if ((blockchain.blocks.Count() - 1) % chunkSize == 0)
            {
                // Gets last 4 blocks of blockchain (chunk) and compares their average time to the requiredTransactionTime
                double timeMined = blockchain.blocks.Skip(Math.Max(0, blockchain.blocks.Count() - chunkSize)).Aggregate(0.0, (total, block) => total + block.mineTime) / chunkSize;
                double differenceRatio = (requiredTransactionSpeed / timeMined);

                // Taking more time than predicted therefore decreasing difficulty
                if (differenceRatio < 0.7)
                {
                    blockchain.difficulty--;
                }

                // Taking less time than predicted therefore increasing difficulty
                else if (differenceRatio > 1.3)
                {
                    blockchain.difficulty++;
                }
            }
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