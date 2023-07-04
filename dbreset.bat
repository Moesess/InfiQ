@echo off
SET PGPASSWORD=admin

echo Dropping the database...
psql -U postgres -c "DROP DATABASE infiqdb;"

echo Revoking all privileges...
echo REVOKE ALL PRIVILEGES ON ALL TABLES IN SCHEMA public FROM infiqadmin
psql -U postgres -c "REVOKE ALL PRIVILEGES ON ALL TABLES IN SCHEMA public FROM infiqadmin;"

echo REVOKE ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public FROM infiqadmin
psql -U postgres -c "REVOKE ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public FROM infiqadmin;"

echo REVOKE ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public FROM infiqadmin
psql -U postgres -c "REVOKE ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public FROM infiqadmin;"

echo REVOKE ALL PRIVILEGES ON SCHEMA public FROM infiqadmin
psql -U postgres -c "REVOKE ALL PRIVILEGES ON SCHEMA public FROM infiqadmin;"

echo DROP USER
psql -U postgres -c "DROP USER infiqadmin;"
echo Done.

SET PGPASSWORD=
pause
