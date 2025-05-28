using System.Collections.Concurrent;

namespace ecobony.application.Validator
{
    public class CustomerManager : LanguageManager
    {
        protected readonly ConcurrentDictionary<(string Locale, string Key), string> _languages = new();

      
        public CustomerManager()
        {
            // === AZ (Azərbaycan dili) ===
            _languages.TryAdd(("az-AZ", "RequiredValidator"), "'{PropertyName}' sahəsi boş ola bilməz, bu sahə mütləq doldurulmalıdır.");
            _languages.TryAdd(("az-AZ", "NotEmptyValidator"), "'{PropertyName}' sahəsi boş olmamalıdır.");
            _languages.TryAdd(("az-AZ", "MaximumLengthValidator"), "'{PropertyName}' sahəsi {MaxLength} simvoldan çox olmamalıdır.");
            _languages.TryAdd(("az-AZ", "GreaterThanValidator"), "'{PropertyName}' 0-dan böyük olmalıdır.");
            _languages.TryAdd(("az-AZ", "ISOCodeCannotBeNull"), "ISO kodu boş ola bilməz.");
            _languages.TryAdd(("az-AZ", "ISOCodeCannotBeEmpty"), "ISO kodu boş ola bilməz.");
            _languages.TryAdd(("az-AZ", "ISOCodeLength"), "ISO kodu 5 ilə 7 simvol arasında olmalıdır. Məsələn: 'az-AZ', 'en-US', 'ru-RU'.");
            _languages.TryAdd(("az-AZ", "InvalidISOCode"), "Yanlış ISO kodu. Doğru kodlar: 'az-AZ', 'en-US', 'ru-RU'.");
            _languages.TryAdd(("az-AZ", "CultureIsRequired"), "Mədəniyyət tələb olunur.");
            _languages.TryAdd(("az-AZ", "CultureCannotBeEmpty"), "Mədəniyyət boş ola bilməz.");
            _languages.TryAdd(("az-AZ", "CultureLength"), "Mədəniyyət 5 ilə 20 simvol arasında olmalıdır.");
            _languages.TryAdd(("az-AZ", "KeyCannotBeNull"), "Açar boş ola bilməz.");
            _languages.TryAdd(("az-AZ", "KeyMaxLength"), "Açar maksimum 3 simvoldan ibarət olmalıdır.");
            _languages.TryAdd(("az-AZ", "FileIsRequired"), "Fayl tələb olunur.");
            _languages.TryAdd(("az-AZ", "FileCannotBeEmpty"), "Fayl boş ola bilməz.");
            _languages.TryAdd(("az-AZ", "FileFormatNotAllowed"), "Yalnız .jpg, .jpeg, .png, .gif formatları icazəlidir.");
            _languages.TryAdd(("az-AZ", "FileSizeExceeded"), "Fayl ölçüsü 5MB-dan az olmalıdır.");

            // === RU (Rus dili) ===
            _languages.TryAdd(("ru-RU", "RequiredValidator"), "Поле '{PropertyName}' не может быть пустым, это обязательное поле.");
            _languages.TryAdd(("ru-RU", "NotEmptyValidator"), "Поле '{PropertyName}' не должно быть пустым.");
            _languages.TryAdd(("ru-RU", "MaximumLengthValidator"), "Длина поля '{PropertyName}' не должна превышать {MaxLength} символов.");
            _languages.TryAdd(("ru-RU", "GreaterThanValidator"), "'{PropertyName}' должно быть больше 0.");
            _languages.TryAdd(("ru-RU", "ISOCodeCannotBeNull"), "ISO код не может быть пустым.");
            _languages.TryAdd(("ru-RU", "ISOCodeCannotBeEmpty"), "ISO код не может быть пустым.");
            _languages.TryAdd(("ru-RU", "ISOCodeLength"), "ISO код должен содержать от 5 до 7 символов. Пример: 'az-AZ', 'en-US', 'ru-RU'.");
            _languages.TryAdd(("ru-RU", "InvalidISOCode"), "Неверный ISO код. Примеры правильных кодов: 'az-AZ', 'en-US', 'ru-RU'.");
            _languages.TryAdd(("ru-RU", "CultureIsRequired"), "Культура обязательна.");
            _languages.TryAdd(("ru-RU", "CultureCannotBeEmpty"), "Культура не может быть пустой.");
            _languages.TryAdd(("ru-RU", "CultureLength"), "Культура должна содержать от 5 до 20 символов.");
            _languages.TryAdd(("ru-RU", "KeyCannotBeNull"), "Ключ не может быть пустым.");
            _languages.TryAdd(("ru-RU", "KeyMaxLength"), "Ключ должен содержать не более 3 символов.");
            _languages.TryAdd(("ru-RU", "FileIsRequired"), "Файл требуется.");
            _languages.TryAdd(("ru-RU", "FileCannotBeEmpty"), "Файл не может быть пустым.");
            _languages.TryAdd(("ru-RU", "FileFormatNotAllowed"), "Разрешены только форматы .jpg, .jpeg, .png, .gif.");
            _languages.TryAdd(("ru-RU", "FileSizeExceeded"), "Размер файла должен быть меньше 5 МБ.");

            // === EN (İngilis dili) ===
            _languages.TryAdd(("en-US", "RequiredValidator"), "'{PropertyName}' cannot be empty; it is a required field.");
            _languages.TryAdd(("en-US", "NotEmptyValidator"), "'{PropertyName}' is required.");
            _languages.TryAdd(("en-US", "MaximumLengthValidator"), "'{PropertyName}' must not exceed {MaxLength} characters.");
            _languages.TryAdd(("en-US", "GreaterThanValidator"), "'{PropertyName}' must be greater than 0.");
            _languages.TryAdd(("en-US", "ISOCodeCannotBeNull"), "ISO code cannot be null.");
            _languages.TryAdd(("en-US", "ISOCodeCannotBeEmpty"), "ISO code cannot be empty.");
            _languages.TryAdd(("en-US", "ISOCodeLength"), "ISO code must be between 5 and 7 characters. Example: 'az-AZ', 'en-US', 'ru-RU'.");
            _languages.TryAdd(("en-US", "InvalidISOCode"), "Invalid ISO code. Example of valid codes: 'az-AZ', 'en-US', 'ru-RU'.");
            _languages.TryAdd(("en-US", "CultureIsRequired"), "Culture is required.");
            _languages.TryAdd(("en-US", "CultureCannotBeEmpty"), "Culture cannot be empty.");
            _languages.TryAdd(("en-US", "CultureLength"), "Culture must be between 5 and 20 characters.");
            _languages.TryAdd(("en-US", "KeyCannotBeNull"), "Key cannot be null.");
            _languages.TryAdd(("en-US", "KeyMaxLength"), "Key must be a maximum of 3 characters.");
            _languages.TryAdd(("en-US", "FileIsRequired"), "File is required.");
            _languages.TryAdd(("en-US", "FileCannotBeEmpty"), "File cannot be empty.");
            _languages.TryAdd(("en-US", "FileFormatNotAllowed"), "Only .jpg, .jpeg, .png, .gif formats are allowed.");
            _languages.TryAdd(("en-US", "FileSizeExceeded"), "File size must be less than 5MB.");

          
            _languages.TryAdd(("en-US", "NotNullValidator"), "'{PropertyName}' is required.");
            _languages.TryAdd(("en-US", "LengthValidator"), "'{PropertyName}' must be between {MinLength} and {MaxLength} characters.");
        }
    }
}
