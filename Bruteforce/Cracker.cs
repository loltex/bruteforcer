using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bruteforce
{
    public class Cracker
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789!@#$%^&*()-_+={}[]|\\:;“‘<>,.?/~`"; // TODO: Expand alphabet
        private Hasher _passwordHasher;

        public Cracker(Hasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string CrackPassword(string targetHash, int maxPasswordLength)
        {
            string result = null;
            CancellationTokenSource cts = new CancellationTokenSource();
            var token = cts.Token;

            Parallel.ForEach(Alphabet, (startChar, state) =>
            {
                if (token.IsCancellationRequested) return;

                char[] currentAttempt = new char[maxPasswordLength];
                currentAttempt[0] = startChar;

                if (GenerateCombinations(currentAttempt, 1, maxPasswordLength, targetHash, ref result, token))
                {
                    cts.Cancel(); // Stop other threads when the password is found
                    state.Stop(); // Break the loop
                }
            });

            return result;
        }

        private bool GenerateCombinations(char[] currentAttempt, int position, int maxLength,
                                         string targetHash, ref string foundPassword, CancellationToken token)
        {
            if (token.IsCancellationRequested) return false; // Stop if another thread found the password
            if (position == maxLength)
            {
                string attempt = new string(currentAttempt);
                string attemptHash = _passwordHasher.HashPassword(attempt);

                if (attemptHash == targetHash) {
                    foundPassword = attempt;
                    return true;
                }
                return false;
            }

            for (int i = 0; i < Alphabet.Length; i++)
            {
                currentAttempt[position] = Alphabet[i];
                if (GenerateCombinations(currentAttempt, position + 1, maxLength, targetHash, ref foundPassword, token))
                    return true;
            }

            return false;
        }
    }
}
