*** For Adding ENVIRONMENT of Project To Production => dotnet run --no-launch-profile
this comman delete the launch setting file and setting ENVIRONMENT of Project to Production

For Changing Setting Envirnoment With PoweShell => $Env:ASPNETCORE_ENVIRONMENT="Staging"
OR in Command Prompt => set ASPNETCORE_ENVIRONMENT="Staging"

-------------------------------------------

*** Also Secure Way in Production adding ENVIRONMENT VARIABLES With PowerShell:
=>1- dotnet run --no-launch-profile
=>2- $Env:ParentKey__ChiledKey = "Value"

-------------------------------------------


