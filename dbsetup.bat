@echo off
SET PGPASSWORD=admin

echo Creating the database...
psql -U postgres -c "CREATE DATABASE infiqdb;"

echo Creating the user...
psql -U postgres -c "CREATE USER infiqadmin WITH PASSWORD 'admin';"

echo Granting privileges to the user on the database...

echo GRANT ALL PRIVILEGES ON DATABASE infiqdb TO infiqadmin
psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE infiqdb TO infiqadmin;"

echo GRANT ALL PRIVILEGES ON SCHEMA public TO infiqadmin
psql -U postgres -c "GRANT ALL PRIVILEGES ON SCHEMA public TO infiqadmin;"

echo GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO infiqadmin
psql -U postgres -c "GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO infiqadmin;"

echo ALTER USER infiqadmin
psql -U postgres -c "ALTER USER infiqadmin CREATEDB;"
psql -U postgres -c "ALTER USER infiqadmin SUPERUSER;"

echo APPLYING MIGRATIONS
python manage.py makemigrations
python manage.py migrate

echo Done.

SET PGPASSWORD=
pause
