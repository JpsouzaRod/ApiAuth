using System.Security.Cryptography;
using System.Text;

namespace ApiAuth.Application.Services
{
    public class HashService
    {
        private HashAlgorithm _algoritmo;

        public HashService(HashAlgorithm algoritmo)
        {
            _algoritmo = algoritmo;
        }

        public string CriptografarSenha(string senha)
        {
            var encodedValue = Encoding.UTF8.GetBytes(senha);
            var encryptedPassword = _algoritmo.ComputeHash(encodedValue);


            var sb = new StringBuilder();

            foreach (var caracter in encryptedPassword)
            {
                sb.Append(caracter.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
