

## Proje Hakkında
Bu proje, .NET ile hazırlanmış bir mikroservis mimarisi örneğidir. Amaç, modern backend geliştirme uygulamaları ve mikroservis entegrasyonunu göstermektir. Proje Onion Architecture ve CQRS prensipleriyle tasarlanmıştır.

### Mikroservisler:
- **ProductService.API**: Ürün yönetimi, CRUD operasyonları, cache mekanizması, role-based authorization.
- **AuthService.API**: JWT tabanlı authentication, refresh token yönetimi, role-based authorization.
- **LogService.API**: Centralized logging, Serilog tabanlı konsol ve dosya loglaması.

## Yapılan Geliştirmeler

### Authentication & Authorization
- JWT tabanlı authentication sistemi.
- Role-based authorization (`Admin`, `Manager`).
- Refresh token mekanizması.
- Seed edilmiş kullanıcılar: Admin ve Manager.
- ProductService endpointleri için role kontrolü.

### Database & Migrations
- Kullanıcı ve refresh token tabloları oluşturuldu.
- Gerekli migrationlar uygulanmış durumda.

### Caching
- Redis cache kullanımı.
- Cache invalidation mekanizması.

### Logging
- Serilog kullanımı ile merkezi loglama.
- Console ve file sink.

### Eksikler
- API Gateway / Rate Limiting (YARP)
- Event-driven asenkron entegrasyon (RabbitMQ/Kafka)
- Saga Pattern / Dağıtık transaction yönetimi

## Proje Kurulumu
1. **Database bağlantısı:** `appsettings.json` içerisinde `DefaultConnection` ile SQL Server bağlantısını belirtin.
2. **Migrationları uygula:**
```bash
dotnet ef database update --project ProductService.Infrastructure --startup-project ProductService.API
dotnet ef database update --project AuthService.API
