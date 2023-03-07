using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    internal class Block
    {
        // Public Members //
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

        public String hashCode;

        // Private Members //
        private static int processorCount = 4;
        private static String[] hashList = new String[processorCount];
        private static long[] nonceList = new long[processorCount];

        // Genesis Block Constructor //
        public Block()
        {
            this.timeStamp = DateTime.Now;
            this.index = 0;
            this.previousBlockHash = "GENESIS BLOCK";
            this.reward = 0.0;
            this.hashCode = index.ToString() + timeStamp.ToString() + previousBlockHash + merkleRoot;

            (this.hash, this.nonce) = Mine();
        }

        public Block(String hash, uint index)
        {
            this.timeStamp = DateTime.Now;
            this.index = index + 1;
            this.previousBlockHash = hash;
            this.hashCode = index.ToString() + timeStamp.ToString() + previousBlockHash + merkleRoot;

            (this.hash, this.nonce) = Mine();
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
            this.hashCode = index.ToString() + timeStamp.ToString() + previousBlockHash + merkleRoot;

            (this.hash, this.nonce) = Mine();
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

        // NEW HASHER //
        public static String CreateHash(String hashInput)
        {
            String hash = String.Empty;
            SHA256 hasher = SHA256.Create();

            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(hashInput));

            // Converting hash from byte array to string and formatting doubles
            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }

            return hash;
        }

        public static void HashSolver(String inputHash, int index, int difficulty, CancellationToken token)
        {
            String hash = CreateHash(inputHash + nonceList[index].ToString());
            String difficultyCriteria = new String('0', difficulty);

            while (hash == String.Empty || !hash.StartsWith(difficultyCriteria))
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                nonceList[index]++;
                hash = CreateHash(inputHash + nonceList[index].ToString());
            }

            hashList[index] = hash;
        }

        public (String Hash, long Nonce) Mine()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < processorCount; i++)
            {
                Task task = Task.Factory.StartNew((index) =>
                {
                    try
                    {
                        HashSolver(hashCode, (int)index, difficulty, cancelToken);
                    }
                    catch (OperationCanceledException)
                    {

                        Console.WriteLine("Task " + i.ToString() + " has been terminated.");
                    }
                }, i, cancelToken);
                tasks.Add(task);
            }

            int winner = Task.WaitAny(tasks.ToArray());
            cancelTokenSource.Cancel();

            String winningHash = hashList[winner];
            long winningNonce = nonceList[winner];

            nonceList = new long[processorCount];

            return (winningHash, winningNonce);
        }

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