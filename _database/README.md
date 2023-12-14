# SetUp


## Database (Localhost)

If you have MSFT SQL Server installed locally (or have a sandbox instance avaialble) - the "Exercise-DB_Init.sql" file in the --SQL-Scripts folder will initialize the Exercise database for this project.

# Docker
## Build the image 
Build with `docker build`:
```
cd Patients.CSV.Upload/_database
docker build -t mssql-custom .
```

## Run the container

Then spin up a new container using `docker run`
```
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=(Another.Demo)(001)' -p 1433:1433 --name sqldev001 -d mssql-custom
```

Log in with the 'sa' credentials specified above and execute the contents of "Exercise-DB_Init.sql" file in the --SQL-Scripts folder to initialize the database.

The API expects that the SQL Server will be available on localhost (127.1.0.1) on port 1433.

To connect to this docker image via MSFT SSMS, the Server Name value should be "127.0.0.1,1443"

### SQL Server Accounts/Credentials
2 accounts are available after DB initialization:
- 'sa' (password definied in the docker run command)
- 'svc_patients_api' - the credentials that the API will access the  DB with.  PSWD: (Api-2023.SomeWord.Thing@)