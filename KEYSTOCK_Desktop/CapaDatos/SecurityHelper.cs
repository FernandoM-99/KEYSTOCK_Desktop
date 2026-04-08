using System.Security.Cryptography;
using System.Text;

public static class SecurityHelper
{
    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password)) return string.Empty;

        using (SHA256 sha256Hash = SHA256.Create())
        {
            // Convertir la cadena en un array de bytes y computar el hash
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convertir el array de bytes en una cadena hexadecimal
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}