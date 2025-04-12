# Krai_Terminal.cs
![Terminal Screenshot](/img/Снимок.PNG)

Пользовательский терминал с поддержкой команд и работы с файлами, написанный на C#.

## 🌟 Особенности
- Поддержка основных команд (`cd`, `ls`, `clear`, `exit`)
- Открытие и редактирование файлов через контекстное меню
- Выделение текущего каталога и ветки Git (если доступно)
- Кроссплатформенность (Windows/Linux/macOS)
- Вывод ошибок и подсказок в цвете

![Help](/img/Снимок1.PNG)

| Команды | Описание |
| - | - |
| `help` | показать эту справку |
| `clear` | Очистить экран |
| `exit` | Выход из терминала |
| `neofetch` | Показать системную информацию |
| `ls` | список файлов |
| `dir` | список файлов |
| `cd` | передача файлов |

## ✍️ Редактировать код
![Cod](/img/Снимок2.PNG)
`Все языки`
- **Стрелки** - перемещение по строкам
- **Enter** - новая строка
- **Backspace** - удаление символа
- **Ctrl+S** - сохранение
- **Esc** - выход без сохранения

# 💸 Поддержка

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/purple_img.png)](https://buymeacoffee.com/kreofotimio)

# 🔨Сборка из исходников
```bash
git clone https://github.com/ваш-username/MyTerminal.git
cd MyTerminal
dotnet publish -c Release -r win-x64 --self-contained true
```
# 📂 Структура проекта
```
MyTerminal/
├── Команды/ # Реализация команд
├── Программа. cs # Точка входа
├── Terminal.cs # Основная логика
├── MyTerminal.csproj
└── Krai.exe # Запуск терминала
```
# 🔧Запуск терминала
```
Krai.exe
```
Если у вас не запускается Krai.exe.Перейдите в консоль и введите эту команду:
```bash
dotnet build
```
Исполняемый файл появится в папке:
```
Krai_Terminal.cs\bin\Debug\net9.0\Krai.exe
```
