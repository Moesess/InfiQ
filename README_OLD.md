# InfiQ_API

## Instalacja środowiska
Zakładam, że znajdujemy się pod następującą ścieżką Nie ma znaczenia, ale ułatwi potem:
```shell
C:\ > cd C:\_PRJ
```
W niej klonujemy sobie nasze repo. Nie wchodzimy do środka na razie.
Tworzymy virtualenv i aktywujemy go. Powinien przed znakiem zachęty pojawić się (InfiQVenv)
```shell
C:\_PRJ> python -m venv InfiQVenv
C:\_PRJ> InfiQVenv\Scripts\activate
```

Teraz możemy wejść do projektu.
I instalujemy zależności w pliku requirements.txt
```shell
(InfiQ_Venv) C:\_PRJ> cd InfiQ_API
(InfiQ_Venv) C:\_PRJ\InfiQ_API> pip install -r requirements.txt
```

## Instalacja Postgres
Pobieramy wersję 15.3:
> https://www.enterprisedb.com/downloads/postgres-postgresql-downloads

Przy instalacji wszystko raczej defaultowo zostawiamy. Hasło postgres ustawiamy na "admin".

Musimy dodać go do zmiennych środowiskowych. Musimy wskazać w PATH jego folder bin. Np:
> C:\Program Files\PostgreSQL\15\bin

Po instalacji uruchamiamy skrypt. WAŻNE, BY BYĆ W AKTYWNYM VENV.
```shell
(InfiQ_Venv) C:\_PRJ\InfiQ_API> dbsetup.bat
```

Jeśli coś się popsuło z bazą lokalną. Możemy ją zresetować za pomocą. Należy potem tez uruchomić ponownie setup.
```shell
(InfiQ_Venv) C:\_PRJ\InfiQ_API> dbreset.bat
(InfiQ_Venv) C:\_PRJ\InfiQ_API> dbsetup.bat
```