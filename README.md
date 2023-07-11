# InfiQ_API
## Instalacja środowiska
Pobierz dockera. https://www.docker.com/ nic specjalnego raczej nie trzeba dodatkowo konfigurować.

Uruchom skrypt docker_setup.bat, powinien przelecieć i w docker desktopie powinien pojawić się kontener. 
Jeśli z jakiegoś powodu nie odpalił się automatycznie, można go tam uruchomić.

## Wypełnienie danymi testowymi
Wystarczy wydać polecenie w terminalu usługi web
```shell
python manage.py seed_example_data
```

W celu uruchomienia scrappera wydajemy polecenie:
```shell
python manage.py fill_db
```