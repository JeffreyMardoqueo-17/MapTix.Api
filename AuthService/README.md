## Company

- Id (Guid)
- Name (string)
- RUC (string?)
- Address (string?)
- Email (string?)
- PhoneNumber (string?)
- CreatedAt (DateTime)
- IsActive (bool)
- UpdateAt (Date Time)
-- Ready

## Role

- Id (Guid)
- Name (string) // Ej: "AdminEmpresa", "Encargado", "Empleado"
- Description (string?)
- CreatedAt (DateTime)
-- Ready

## User

- Id (Guid)
- FirstName (string)
- LastName (string)
- Email (string)
- PasswordHash (string)
- PhoneNumber (string?)
- RoleId (Guid) // FK → Role
- CompanyId (Guid) // FK → Company
- IsActive (bool)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)

## RefreshToken

- Id (Guid)
- UserId (Guid) // FK → User
- Token (string)
- ExpiresAt (DateTime)
- CreatedAt (DateTime)
- RevokedAt (DateTime?)
- IsRevoked (bool)

## UserSession

- Id (Guid)
- UserId (Guid) // FK → User

LoginTime (DateTime)

LogoutTime (DateTime?)

IpAddress (string?)

DeviceInfo (string?)

IsActive (bool)

Company ───< User ───< RefreshToken
             │
             ├──< UserSession
             │
             └──> Role ───< Permission (opcional)

# Orden de creacion de cada cosa
Modelos → DbContext → DTOs → Interfaces → Services → Controllers → Middleware
