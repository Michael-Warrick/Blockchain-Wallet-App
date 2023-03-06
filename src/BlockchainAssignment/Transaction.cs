using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    internal class Transaction
    {
        public String hash;
        String signature;
        public String sourceAddress;
        public String destinationAddress;

        public DateTime timeStamp;

        public double amount;
        public double transactionFee;

        public Category transactionCategory;

        // The four horsemen of transanctions
        public enum Category
        {
            DEPOSIT,
            WITHDRAWAL,
            REWARD,
            STANDARD
        }

        public Transaction(string srcAddress, string dstAddress, double amount, double fee, string privateKey)
        {
            this.sourceAddress = srcAddress;
            this.destinationAddress = dstAddress;
            this.amount = amount;
            this.transactionFee = fee;

            this.timeStamp = DateTime.Now;

            this.hash = CreateHash();
            this.signature = Wallet.Wallet.CreateSignature(srcAddress, privateKey, this.hash);
        }

        // Reimplementation of utility function
        public String CreateHash()
        {
            String hash = String.Empty;

            SHA256 hasher = SHA256Managed.Create();
            String hashInput = timeStamp.ToString() + sourceAddress.ToString() + destinationAddress.ToString() + amount.ToString() + transactionFee.ToString();

            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(hashInput)); 

            // Converting hash from byte array to string and formatting doubles
            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }

            return hash;
        }

        public override string ToString()
        {
            return "\tCategory: " + transactionCategory + "\n" +
                    "\t\t Digital Signature: " + signature + "\n" +
                    "\t\t Timestamp: " + timeStamp.ToString() + "\n" +
                    "\t\t Hash: " + hash + "\n" +
                    "\t\t Transferred: " + "₦" + amount.ToString() + "\n" +
                    "\t\t Fees: " + transactionFee.ToString() + "\n" +
                    "\t\t Source Address: " + sourceAddress + "\n" +
                    "\t\t Destination Address: " + destinationAddress + "\n";
        }
    }
}