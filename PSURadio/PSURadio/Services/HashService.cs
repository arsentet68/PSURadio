using System.Security.Cryptography;
using System.Text;

namespace PSURadio.Services
{
    public static class HashService
    {
        public static string ComputeSha256Hash(string rawData)
        {
            // Создаем SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Вычисляем хеш - возвращаем массив байтов
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Конвертируем массив байтов в строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
