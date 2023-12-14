# Patients.CSV.Upload exercise Set-Up

### Clone this repo
```
git clone https://github.com/brianackley001/Patients.CSV.Upload.git
```
### Setup the database
[Please see the _database folder README.md](_database/README.md)

### Launch localhost API and Client App
- API:
  - open `Patients.CSV.Upload.sln` in Visual Studio (_note_: VS 2022 was used for this project)
  - Ensure that Web.Api.Patients is selected as the  Startup project
  - Launch the API in DEBUG mode
  - The API should default to being available at https://localhost:7171/  (Swagger:  https://localhost:7171/swagger/index.html)
- Client App
  - Open the folder `./src/Web/ClientApp` in VS Code to view the client application
  - Run `ng serve` to launch the Angular application at http://localhost:4200/
> [!NOTE]
> - The client app's `environments` folder is defining the API endpoint as  https://localhost:7171/ 
> - Likewise, the SQL Connection string defined in `appsettings.json` in the API is expecting `server=127.0.0.1`
> - Should ports be altered, please update the localhost endpoints configuration accordingly

#### Framework Version Notes
- API is targeting .NET 6.0
- Client App was created with:
  - Angular CLI: 17.0.6
  - Node: 20.9.0
  - Package Manager: npm 10.2.5
  - OS: win32 x64
 
 
### Project Assumptions and Considerations
#### Architecture
The API structure is a nod to [Clean Architecture with .NET Core](https://jasontaylor.dev/clean-architecture-getting-started/).  The layers of Domain, Application, Infrastructure, and Web are implemented here.  The scope of the exercise does not provide for a wealth of Domain business logic to enter into play, and the unit tests for the inner 2 layers reflect that.

For this MVP, Angular template-driven forms were chosen.  Only the boilerplate Jasmine tests are currently present - the _Edit Patient_ form would benefit from attention directed at fleshing out front end unit tests (and the consideration of pivoting to reactive forms).

The Bootstrap library does adequately provide responsive design, but long strings in the _Patients_ table columns is not ideal. Given the use case of uploading patient CSV files, the assumption was made that the user base would skew towards administrative staff versus end-user consumers/customers. The smaller breakpoints for the phone form could use iterative attention to be truly mobile-first (whereas the assumption of office staff assumes more desktop experiences).

#### Security Considerations
 - The MVP parsing of the CSV is executed on the client app for simplistic validation.  Moving forward with this approach would warrant CSV file data inspection for malicious data
 - This demo is not secured by authentication.  Iterating forward on this project, an Identity Provider would need to be chosen to authenticate users and thereby secure the API with JWT restricted access.
 - The DB Connection string should be pulled from a Key Vault or other secrets management solution versus living in config files.

### Next Up
- This project has not yet integrated a Github Actions CI/CD Pipeline for automated testing and deployments
- Ideally a docker compose file would be available to spin up containers for the DB, API, and client
