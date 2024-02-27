Database Migration Commands

first time
-------------
Scaffold-DbContext "Server=(localdb)\ProjectModels;Database=inventory_system;Trusted_Connection=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

update
-----------
Scaffold-DbContext "Server=(localdb)\ProjectModels;Database=inventory_system;Trusted_Connection=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force
