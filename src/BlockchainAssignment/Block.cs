using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    internal class Block
    {
        public uint index;
        public String hash;
        DateTime timeStamp;
        public String previousBlockHash;

        public List<Transaction> transactions = new List<Transaction>();
        public String merkleRoot;

        // Proof of Work (PoW)
        public long nonce = 0;
        public int difficulty = 5;

        public double mineTime = 0;

        // Rewards/Fees
        public Double reward = 1.0;
        public Double fees = 0.0;

        public String minerAddress = String.Empty;

        // Genesis Block Constructor //
        public Block()
        {
            this.timeStamp = DateTime.Now;
            this.index = 0;
            this.previousBlockHash = "GENESIS BLOCK";
            this.reward = 0.0;
            this.hash = CreateHash();
        }

        public Block(String hash, uint index)
        {
            this.timeStamp = DateTime.Now;
            this.index = index + 1;
            this.previousBlockHash = hash;
            this.hash = Mine();
        }

        public Block(Block endBlock, List<Transaction> transactions, int difficulty, String address = "")
        {
            this.timeStamp = DateTime.Now;
            this.index = endBlock.index + 1;
            this.previousBlockHash = endBlock.hash;

            this.minerAddress = address;
            this.difficulty = difficulty;

            transactions.Add(CreateRewardTransaction(transactions));
            this.transactions = transactions;

            this.merkleRoot = MerkleRoot(transactions);

            this.hash = Mine();
        }

        public Transaction CreateRewardTransaction(List<Transaction> transactions)
        {
            // Sum of all fees in trasaction list of current mined block
            fees = transactions.Aggregate(0.0, (acc, t) => acc + t.transactionFee);

            // Creates a reward transaction (fees + reward) => transfered to miner's wallet
            Transaction rewardTransaction = new Transaction("Mining Rewards", minerAddress, (reward + fees), 0, "");
            rewardTransaction.transactionCategory = Transaction.Category.REWARD;

            return rewardTransaction;
        }

        public String CreateHash()
        {
            String hash = String.Empty;
            SHA256 hasher = SHA256.Create();

            String hashInput = index.ToString() + timeStamp.ToString() + previousBlockHash + nonce.ToString() + merkleRoot;

            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(hashInput));

            // Converting hash from byte array to string and formatting doubles
            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }

            return hash;
        }

        public string Mine()
        {
            // Defining mining difficulty
            string regexDefinition = new string('0', difficulty);

            string result = null;

            Parallel.For(0, Environment.ProcessorCount, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                (i, state) =>
                {
                    while (true)
                    {
                        string hash = CreateHash();

                        if (hash.StartsWith(regexDefinition))
                        {
                            // A hash that meets the difficulty level has been found
                            if (Interlocked.CompareExchange(ref result, hash, null) == null)
                            {
                                // This thread's hash is the winner
                                state.Break();
                                break;
                            }
                            else
                            {
                                // Another thread has already found the winner hash
                                break;
                            }
                        }

                        // Increment nonce and try again
                        Interlocked.Increment(ref nonce);
                    }
                });

            return result;
        }

        //public string Mine()
        //{
        //    // Defining mining difficulty
        //    string regexDefinition = new string('0', difficulty);

        //    string result = null;

        //    Parallel.For(0, Environment.ProcessorCount, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
        //        (i, state) =>
        //        {
        //            while (true)
        //            {
        //                // Increment nonce and create hash
        //                long currentNonce = Interlocked.Increment(ref nonce);
        //                string hash = CreateHash(currentNonce);

        //                if (hash.StartsWith(regexDefinition))
        //                {
        //                    // A hash that meets the difficulty level has been found
        //                    if (Interlocked.CompareExchange(ref result, hash, null) == null)
        //                    {
        //                        // This thread's hash is the winner
        //                        state.Break();
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        // Another thread has already found the winner hash
        //                        break;
        //                    }
        //                }
        //            }
        //        });

        //    return result;
        //}


        public static String MerkleRoot(List<Transaction> transactions)
        {
            List<String> hashes = transactions.Select(t => t.hash).ToList();

            if (hashes.Count == 0)
            {
                return String.Empty;
            }

            if (hashes.Count == 1)
            {
                return HashCode.HashTools.CombineHash(hashes[0], hashes[0]);
            }

            while (hashes.Count > 1)
            {
                List<String> merkleLeaves = new List<String>();
                for (int i = 0; i < hashes.Count; i += 2)
                {
                    if (i == hashes.Count - 1)
                    {
                        merkleLeaves.Add(HashCode.HashTools.CombineHash(hashes[i], hashes[i]));
                    }
                    else
                    {
                        merkleLeaves.Add(HashCode.HashTools.CombineHash(hashes[i], hashes[i + 1]));
                    }
                }

                hashes = merkleLeaves;
            }

            return hashes[0];
        }

        public override string ToString()
        {
            String output = String.Empty;

            foreach (Transaction tx in transactions)
            {
                output += tx.ToString() + "\n";
            }

            return
                "Index: " + index.ToString() +
                "\nTimeStamp: " + timeStamp.ToString() +
                "\nPrevious Block's Hash: " + previousBlockHash +
                "\nHash: " + hash +
                "\nNonce: " + nonce.ToString() +
                "\nMining Difficulty: " + difficulty.ToString() +
                "\nMine Time: " + mineTime.ToString() +
                "\nRewards: " + reward.ToString() +
                "\nFees: " + fees.ToString() +
                "\nMiner Address: " + minerAddress +
                "\n\nTransactions: \n" + output;
        }
    }
}