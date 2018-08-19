﻿using NUnit.Framework;
using System.Text;
using Phantasma.Cryptography;
using Phantasma.Utils;
using Phantasma.VM.Types;
using System;
using System.Linq;
using System.Numerics;

namespace PhantasmaTests
{
    [TestFixture]
    public class CryptoTests
    {
        [Test]
        public void HashTests()
        {
            var bytes = new byte[32];
            var rnd = new Random();
            rnd.NextBytes(bytes);

            var hash = new Hash(bytes);

            Assert.IsTrue(hash.ToByteArray().Length == 32);
            Assert.IsTrue(hash.ToByteArray().SequenceEqual(bytes));

            bytes = new byte[10];
            rnd.NextBytes(bytes);

            var number = new BigInteger(bytes);
            hash = number;
            Assert.IsTrue(hash.ToByteArray().Length == 32);

            BigInteger other = hash;
            Assert.IsTrue(number == other);
        }

        [Test]
        public void KeyPairSign()
        {
            var keys = KeyPair.Generate();
            Assert.IsTrue(keys.PrivateKey.Length == KeyPair.PrivateKeyLength);
            Assert.IsTrue(keys.Address.PublicKey.Length == Address.PublicKeyLength);

            var msg = "Hello world";

            var msgBytes = Encoding.ASCII.GetBytes(msg);
            var signature = keys.Sign(msgBytes);

            var verified = signature.Verify(msgBytes, keys.Address);
            Assert.IsTrue(verified);

            // make sure that Verify fails for other addresses
            var otherKeys = KeyPair.Generate();
            Assert.IsFalse(otherKeys.Address == keys.Address);
            verified = signature.Verify(msgBytes, otherKeys.Address);
            Assert.IsFalse(verified);
        }
    }
}
