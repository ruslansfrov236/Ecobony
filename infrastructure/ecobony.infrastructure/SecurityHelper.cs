
namespace ecobony.infrastructur;

public static class SecurityHelper
{
    // Bu metod istifadəçiyə unikal açar yaradacaq
    public static string GenerateUserSecretKey(string userId)
    {
        // İstifadəçi ID ilə açar yaradılır (uniqallığı təmin edir)
        using (var sha256 = SHA256.Create())
        {
            var userKeyBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userId));
            return Convert.ToBase64String(userKeyBytes).Substring(0, 32); // Açarın uzunluğunu 32 simvolla məhdudlaşdırırıq
        }
    }

    // Verifikasiya kodunu şifrələyən metod
    public static string EncryptVerificationCode(string code, string userId)
    {
        string secretKey = GenerateUserSecretKey(userId); // Hər istifadəçiyə fərqli açar yaradılır

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(secretKey);
            aes.IV = new byte[16]; // Sabit IV istifadə edilir

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var encrypted = encryptor.TransformFinalBlock(
                Encoding.UTF8.GetBytes(code), 0, code.Length);
            return Convert.ToBase64String(encrypted);
        }
    }

    // Verifikasiya kodunu açan metod
    public static string DecryptVerificationCode(string encryptedCode, string userId)
    {
        string secretKey = GenerateUserSecretKey(userId); 

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(secretKey);
            aes.IV = new byte[16];

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var bytes = Convert.FromBase64String(encryptedCode);
            var decrypted = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}