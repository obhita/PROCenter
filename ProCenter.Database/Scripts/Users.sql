CREATE USER [$(UserLogin)] FOR LOGIN [$(UserLogin)]
CREATE USER [ProcenterSqlLogin] FOR LOGIN [ProcenterSqlLogin]

exec sp_addrolemember 'db_owner', [$(UserLogin)]
exec sp_addrolemember 'db_owner', [ProcenterSqlLogin]