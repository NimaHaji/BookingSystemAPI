# Booking System API

یک API برای **سیستم رزرو نوبت چندمستاجری (Multi-Tenant)** که با **ASP.NET Core 8** پیاده‌سازی شده و قابلیت‌هایی مثل احراز هویت JWT، مدیریت کاربران و نقش‌ها، رزرو نوبت، مدیریت سرویس‌ها و اتصال به درگاه پرداخت را پوشش می‌دهد.

> این پروژه برای نمایش معماری، طراحی API و پیاده‌سازی یک سیستم رزرو قابل توسعه آماده شده است.

---

## فهرست مطالب

- [ویژگی‌ها](#ویژگیها)
- [تکنولوژی‌ها](#تکنولوژیها)
- [معماری پروژه](#معماری-پروژه)
- [ماژول‌ها](#ماژولها)
- [احراز هویت و سطح دسترسی](#احراز-هویت-و-سطح-دسترسی)
- [Multi-Tenancy](#multi-tenancy)
- [درگاه پرداخت](#درگاه-پرداخت)
- [راه‌اندازی پروژه](#راهاندازی-پروژه)
- [Endpointهای اصلی](#endpointهای-اصلی)
- [نکات امنیتی برای انتشار عمومی](#نکات-امنیتی-برای-انتشار-عمومی)
- [وضعیت پروژه](#وضعیت-پروژه)

---

## ویژگی‌ها

- پیاده‌سازی API رزرو نوبت با ASP.NET Core
- معماری چندلایه بر اساس اصول Clean Architecture
- پشتیبانی از Multi-Tenancy برای تفکیک داده‌ها بر اساس Tenant
- احراز هویت با JWT
- پشتیبانی از Refresh Token
- مدیریت نقش‌ها شامل `SuperAdmin`، `Admin`، `User` و `Customer`
- مدیریت کاربران، پروفایل و خروج از حساب کاربری
- مدیریت سرویس‌های قابل رزرو
- ایجاد، ویرایش، مشاهده و لغو نوبت
- اتصال به درگاه پرداخت با پشتیبانی از Saman و ZarinPal
- اعتبارسنجی ورودی‌ها با FluentValidation
- مستندسازی API با Swagger
- استفاده از Entity Framework Core و SQL Server

---

## تکنولوژی‌ها

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- Refresh Token
- FluentValidation
- Swagger / Swashbuckle

---

## معماری پروژه

پروژه به صورت چندلایه طراحی شده است:

```text
Booking-System
├── api
│   ├── Controllers
│   ├── Middlewares
│   └── Program.cs
├── Application
│   ├── Common
│   ├── Features
│   └── Validators
├── Domain
│   ├── Entities
│   ├── Enums
│   ├── Exceptions
│   └── Interfaces
└── Infrastructure
    ├── Persistence
    ├── Repositories
    ├── Security
    └── Services
```

### توضیح لایه‌ها

- `Domain`: شامل Entityها، Enumها، Exceptionها و قراردادهای اصلی دامنه
- `Application`: شامل Use Caseها، سرویس‌های اپلیکیشن، DTOها، Validatorها و Interfaceها
- `Infrastructure`: شامل EF Core، Repositoryها، JWT، Hashing و پیاده‌سازی سرویس‌های خارجی
- `api`: شامل Controllerها، Middlewareها، Swagger و پیکربندی اجرای برنامه

---

## ماژول‌ها

### User / Auth

- ثبت‌نام کاربر
- ورود با ایمیل و رمز عبور
- انتخاب Tenant هنگام وجود چند هویت برای یک ایمیل
- صدور Access Token و Refresh Token
- تمدید Token
- خروج از حساب کاربری
- مشاهده و ویرایش پروفایل
- مشاهده کاربران توسط `SuperAdmin`

### Tenant

- ثبت Tenant جدید
- اتصال کاربر به Tenant
- بررسی وضعیت اشتراک Tenant
- نگهداری `TenantId` در Context برنامه

### Service

- ایجاد سرویس قابل رزرو
- ویرایش سرویس
- حذف سرویس
- مشاهده سرویس‌ها

### Appointment

- ایجاد نوبت
- اتصال نوبت به یک یا چند سرویس
- ویرایش سرویس‌های نوبت
- لغو نوبت
- مشاهده نوبت‌ها
- بررسی مالکیت نوبت برای جلوگیری از دسترسی غیرمجاز

### Payment

- ایجاد پرداخت برای نوبت
- انتخاب درگاه پرداخت
- دریافت URL پرداخت
- مدیریت Callback درگاه
- ثبت وضعیت پرداخت
- ذخیره اطلاعاتی مثل `Authority`، `RefNum`، `RRN`، `TraceNo` و `SecurePan`

---

## احراز هویت و سطح دسترسی

احراز هویت با JWT انجام می‌شود. برای دسترسی به endpointهای محافظت‌شده باید Token در Header ارسال شود:

```http
Authorization: Bearer <access_token>
```

### نقش‌ها

```text
SuperAdmin
Admin
User
Customer
```

برخی عملیات مثل مدیریت سرویس‌ها، مشاهده همه کاربران، مشاهده همه نوبت‌ها و ثبت Tenant فقط برای نقش‌های مدیریتی در نظر گرفته شده‌اند.

---

## Multi-Tenancy

این پروژه با رویکرد Multi-Tenant طراحی شده است. موجودیت‌هایی مثل کاربر، سرویس، نوبت، Refresh Token و پرداخت به Tenant متصل هستند و داده‌ها بر اساس `TenantId` تفکیک می‌شوند.

در حال حاضر Tenant از طریق Context و Claimهای JWT مدیریت می‌شود و Middleware اختصاصی برای بررسی وضعیت Tenant و اعتبار اشتراک در پروژه وجود دارد.

---

## درگاه پرداخت

پروژه ساختار قابل توسعه برای اتصال به چند درگاه پرداخت دارد.

درگاه‌های فعلی:

- Saman
- ZarinPal

نمونه سناریوی پرداخت:

1. کاربر برای یک نوبت درخواست پرداخت ایجاد می‌کند.
2. سیستم بر اساس درگاه انتخاب‌شده URL پرداخت تولید می‌کند.
3. کاربر به صفحه پرداخت منتقل می‌شود.
4. درگاه، نتیجه پرداخت را به Callback API ارسال می‌کند.
5. وضعیت پرداخت در سیستم ثبت و بروزرسانی می‌شود.

---

## راه‌اندازی پروژه

### پیش‌نیازها

- .NET SDK 8
- SQL Server
- IDE یا Editor مثل Visual Studio، Rider یا VS Code

### مراحل اجرا

1. ریپازیتوری را Clone کنید:

```bash
git clone https://github.com/<your-username>/<your-repository>.git
cd <your-repository>
```

2. پکیج‌ها را Restore کنید:

```bash
dotnet restore
```

3. فایل تنظیمات نمونه را کپی کنید و مقدار Connection String، JWT و درگاه پرداخت را تنظیم کنید:

```bash
cp api/appsettings.example.json api/appsettings.json
```

برای پروژه‌های واقعی بهتر است مقادیر حساس را در Secret Manager یا Environment Variable نگهداری کنید:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=BookingDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Issuer": "https://localhost:44328",
    "Audience": "https://localhost:44328",
    "SecretKey": "CHANGE_ME_TO_A_LONG_RANDOM_SECRET",
    "AccessTokenMinutes": 10
  }
}
```

4. دیتابیس را ایجاد یا بروزرسانی کنید:

```bash
dotnet ef database update --project Infrastructure --startup-project api
```

5. پروژه API را اجرا کنید:

```bash
dotnet run --project api
```

6. Swagger را در محیط Development باز کنید:

```text
https://localhost:<port>/swagger
```

---

## Endpointهای اصلی

### User

```http
POST   /api/User/Register
POST   /api/User/login
POST   /api/User/refresh
POST   /api/User/Logout
GET    /api/User/Profile
PATCH  /api/User/Profile
GET    /api/User/ViewUsers
```

### Auth

```http
POST   /api/Auth/Select-Tenant
```

### Tenant

```http
POST   /api/Tenant
GET    /api/Tenant
```

### Service

```http
POST   /api/Service
PATCH  /api/Service/{serviceId}
DELETE /api/Service/{serviceId}
GET    /api/Service/ViewServices
```

### Appointment

```http
POST   /api/Appointment
PATCH  /api/Appointment/{appointmentId}
DELETE /api/Appointment/{appointmentId}
GET    /api/Appointment/ViewAppointments
GET    /api/Appointment/ViewMyAppointments
```

### Payment

```http
POST   /api/Payment/GetPaymentUrl
GET    /api/Payment/CallBack/{gateway}
POST   /api/Payment/CallBack/{gateway}
```

---

## نکات امنیتی برای انتشار عمومی

قبل از Public کردن ریپازیتوری در GitHub:

- مقدار واقعی `JwtSettings:SecretKey` را از فایل‌های تنظیمات حذف کنید.
- اطلاعات واقعی Super Admin، شماره موبایل، ایمیل و رمز عبور را در ریپازیتوری قرار ندهید.
- Merchant ID، Terminal ID، Callback URL و کلیدهای درگاه پرداخت را در Secret Manager، Environment Variable یا GitHub Secrets نگهداری کنید.
- اگر Secret واقعی قبلاً Commit شده، آن را Rotate کنید.
- فایل‌های `bin`، `obj`، لاگ‌ها و فایل‌های local development را Commit نکنید.
- برای Production مقدار `RequireHttpsMetadata`، تنظیمات CORS، مدیریت خطا و Logging را بازبینی کنید.

نمونه استفاده از Secret Manager:

```bash
dotnet user-secrets init --project api
dotnet user-secrets set "JwtSettings:SecretKey" "YOUR_LONG_RANDOM_SECRET" --project api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING" --project api
```

---

## وضعیت پروژه

این پروژه در حال توسعه است و ممکن است در آینده موارد زیر تغییر کند:

- بهبود ساختار Multi-Tenancy
- تکمیل Password Recovery
- بهبود Error Handling و Exception Middleware
- اضافه شدن تست‌های واحد و یکپارچه
- بهبود مستندات Swagger
- بازبینی کامل تنظیمات Production

---

## مشارکت

Pull Requestها و پیشنهادها خوشحال‌کننده‌اند. قبل از ارسال تغییرات، لطفاً پروژه را Build کنید و مطمئن شوید تنظیمات حساس در Commitها قرار نگرفته‌اند.
