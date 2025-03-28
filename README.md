# Практическая работа №6 "СОЗДАНИЕ АВТОМАТИЗИРОВАННОГО UNIT-ТЕСТА" Часть 2
### Выполнил: студент группы 3ИСиП-522 Гусев А.А.
---
## Часть 1
### Таблица SQL
Листинг создания и заполнения кода таблицы SQL:
````
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Username TEXT NOT NULL UNIQUE,
    Email TEXT NOT NULL UNIQUE,
    Phone TEXT,
    PasswordHash TEXT NOT NULL,
    RegisteredAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
````
---
Скриншот содержимого таблицы:

![image](https://github.com/user-attachments/assets/6cbf2595-e86b-4537-8526-15d783eceb39)

---
## Часть 2
### Скриншот окна «Обозреватель тестов»:

![image](https://github.com/user-attachments/assets/3b9d299a-0abc-44f3-ba3f-4fddf25fcc14)


