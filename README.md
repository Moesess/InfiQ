# InfiQ_API

## Uruchomienie
Zakładam, że znajdujemy się pod następującą ścieżką (Dysk nie ma znaczenia):
```shell
cd C:\_PRJ
```
W niej klonujemy sobie nasze repo. Nie wchodzimy do środka na razie.
Tworzymy virtualenv i aktywujemy go. Powinien przed znakiem zachęty pojawić się (InfiQVenv)
```shell
python -m venv InfiQVenv
InfiQVenv\Scripts\activate
```

Teraz możemy wejść do projektu.
I instalujemy zależności w pliku requirements.txt
```shell
cd InfiQ_API
pip install -r requirements.txt
```

Po skończeniu instalacji wpisujemy następującą komendę w celu utworzenia bazy danych.

```shell
python manage.py migrate
```

W celu uruchomienia serwera z API wpisujemy
```shell
python manage.py runserver {opcjonalne_ip}
```