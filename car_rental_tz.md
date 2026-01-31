# Car Rental System - Backend (Technical Specification)

## 🎯 Мақсад
Системаи идоракунии иҷораи мошинҳо барои истифодабарандагон ва маъмурон (Admin). Истифодабарандагон метавонанд мошинҳоро бубинанд, иҷора гиранд ва профили худро таҳрир кунанд. Admin метавонад мошинҳо, истифодабарандагон ва иҷораҳоро идора кунад.

## ⚙️ Технологияҳо
- Backend: ASP.NET Core Web API
- ORM: Entity Framework Core
- Database: PostgreSQL
- Auth: JWT
- Hashing: BCrypt.Net
- Logging: ILogger<T>
- File Upload: IFormFile (/wwwroot/images/)
- Pagination & Filtering: Query Parameters
- Email Sender: SMTP (MailKit ё System.Net.Mail)
- Documentation: Swagger / Swashbuckle

## 👥 Нақшҳо
- Admin: Идоракунии истифодабарандагон, мошинҳо ва иҷораҳо
- User: Дидани мошинҳо, иҷора гирифтан ва таҳрири профил

## 🧱 Ҷадвалҳо (Entities)

### User
| Property | Type | Description |
|-----------|------|-------------|
| Id | Guid | Идентификатор |
| FullName | string | Ном ва насаб |
| Email | string | Почтаи истифодабаранда |
| PasswordHash | string | Пароли ҳашшуда (BCrypt) |
| Role | string | "Admin" ё "User" |
| ProfileImagePath | string | Расми профили истифодабаранда |
| CreatedAt | DateTime | Вақти сабт |

### Car
| Property | Type | Description |
|-----------|------|-------------|
| Id | Guid | Идентификатор |
| Brand | string | Бренд |
| Model | string | Модел |
| Year | int | Сол |
| DailyPrice | decimal | Нархи иҷора барои як рӯз |
| ImagePath | string | Расми мошин |
| IsAvailable | bool | Дастрас ё не |
| CreatedAt | DateTime | Вақти илова |

### Rental
| Property | Type | Description |
|-----------|------|-------------|
| Id | Guid | Идентификатор |
| UserId | Guid | Истифодабаранда |
| CarId | Guid | Мошин |
| StartDate | DateTime | Санаи оғоз |
| EndDate | DateTime | Санаи анҷом |
| TotalPrice | decimal | Маблағи умумии иҷора |
| Status | string | Active, Completed, Cancelled |

## 🧮 Қоидаҳои иҷора
```csharp
TotalPrice = DailyPrice * (EndDate - StartDate).TotalDays
```
- Агар фарқи рӯзҳо = 0 → 1 рӯз ҳисоб карда мешавад
- Ҳангоми иҷора гирифтан `IsAvailable = false`
- Ҳангоми Completed ё Cancelled → `IsAvailable = true`

## 🗂️ Папкаҳои расмҳо
- Машинҳо: /wwwroot/images/cars/
- Профил: /wwwroot/images/users/
- Номи файл: Guid + extension

## 🔐 JWT Claims
```json
{
  "id": "guid",
  "email": "user@example.com",
  "role": "Admin"
}
```

## 🧾 Logging
- Ҳар як амалиёт бо ILogger<T>
```
[INFO] 2025-10-18 14:05 | User: admin@site.tj | Action: AddCar | Car: BMW X5
```

## 📡 Endpoints

### AuthenticationController
| Method | Route | Role | Description |
|--------|-------|------|-------------|
| POST | /api/auth/register | Admin | Сабти корбари нав, генератсияи парол ва фиристодан ба email |
| GET | /api/auth/login | All | Воридшавӣ ва гирифтани JWT Token |

### UserController
| Method | Route | Role | Description |
|--------|-------|------|-------------|
| GET | /api/users | Admin | Рӯйхати истифодабарандагон бо пагинатсия |
| GET | /api/users/{id} | Admin | Маълумоти як истифодабаранда |
| PUT | /api/users/profile | User | Тағйир додани профил, номи корбар ва расм |
| PUT | /api/users/profile-image | User | Навсозии расми профил |
| GET | /api/users/me | User | Дидани маълумоти профил бо расм |
| DELETE | /api/users/{id} | Admin | Нест кардани истифодабаранда |

### CarController
| Method | Route | Role | Description |
|--------|-------|------|-------------|
| GET | /api/cars | All | Рӯйхати мошинҳо бо пагинатсия ва филтратсия |
| GET | /api/cars/{id} | All | Маълумоти мошини мушаххас |
| POST | /api/cars | Admin | Илова кардани мошин бо расм |
| PUT | /api/cars/{id} | Admin | Тағйири маълумот ва расм |
| DELETE | /api/cars/{id} | Admin | Нест кардани мошин ва расмаш |
| GET | /api/cars/search | All | Ҷустуҷӯ аз рӯи бренд ва модел |
| GET | /api/cars/available | All | Рӯйхати мошинҳои дастрас (филтр бо санаҳо мумкин) |

### RentalController
| Method | Route | Role | Description |
|--------|-------|------|-------------|
| GET | /api/rentals | Admin | Рӯйхати иҷораҳо бо пагинатсия |
| GET | /api/rentals/my | User | Рӯйхати иҷораҳои шахсӣ |
| POST | /api/rentals | User | Иҷора гирифтан (TotalPrice ҳисоб мешавад) |
| PUT | /api/rentals/{id}/complete | Admin | Статус ба Completed |
| PUT | /api/rentals/{id}/cancel | Admin/User | Бекор кардани иҷора ва баргардонидани мошин |
| GET | /api/rentals/filter | Admin/User | Филтратсия бо санаи оғоз, санаи анҷом, status |

### StatisticsController
| Method | Route | Role | Description |
|--------|-------|------|-------------|
| GET | /api/statistics/total-users | Admin | Гирифтани шумораи умумии истифодабарандагон |
| GET | /api/statistics/total-cars | Admin | Гирифтани шумораи умумии мошинҳо |
| GET | /api/statistics/total-rentals | Admin | Гирифтани шумораи умумии иҷораҳо |
| GET | /api/statistics/active-rentals | Admin | Рӯйхати иҷораҳои актив |
| GET | /api/statistics/revenue | Admin | Ҳисоби фоида аз иҷораҳо |

## 📤 Email Service
- Сабти корбар → фиристодани email бо пароли тавлидшуда
- Генератсияи пароли нав ва фиристодан барои фаромӯшкунии парол

## 🔍 Filtering & Pagination
- Cars & Rentals: filter by brand, model, year, price, status, availability
- Pagination: pageNumber, pageSize
- Response includes: totalCount, pageNumber, pageSize, items

## 🧩 Extension Features
- Dashboard барои Admin (статистикаи мошинҳо, иҷораҳо, фоида)
- Ҷустуҷӯ ва филтратсияи пешрафта бо дата ва нарх
- Истфода аз IQueryable барои филтратсия динамикӣ

---

