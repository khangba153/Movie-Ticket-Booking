# MovieBooking System

Hệ thống đặt vé xem phim trực tuyến được xây dựng trong khuôn khổ đồ án môn Công nghệ phần mềm. Ứng dụng hỗ trợ người dùng tra cứu phim, xem lịch chiếu, chọn ghế, đặt vé và hỗ trợ quản trị các dữ liệu chính của hệ thống như phim, rạp chiếu, phòng chiếu và suất chiếu.

## Công nghệ sử dụng

- **Backend:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Frontend:** HTML, CSS, JavaScript
- **Kiến trúc:** 3-Layer Architecture

## Chức năng chính

### Người dùng
- Xem danh sách phim
- Xem chi tiết phim
- Xem lịch chiếu
- Chọn ghế và đặt vé
- Xem vé đã đặt

### Quản trị viên
- Quản lý phim
- Quản lý rạp chiếu
- Quản lý suất chiếu
- Quản lý thông tin đặt vé

## Cấu trúc dự án

- `Controllers/` - Xử lý request API
- `Services/` - Xử lý nghiệp vụ
- `Data/` - Cấu hình DbContext
- `Models/` - Entity và DTO
- `Migrations/` - Migration của Entity Framework Core
- `wwwroot/` - Giao diện HTML, CSS, JavaScript

## Yêu cầu môi trường

Trước khi chạy hệ thống, cần cài đặt:

- .NET SDK
- SQL Server hoặc SQL Server Express
- Visual Studio 2022 hoặc Visual Studio Code

## Hướng dẫn cài đặt và khởi chạy

### 1. Clone repository

```bash
git clone <repository-link>
cd MovieBooking
```
### 2. Restore package
```bash
dotnet restore
```
### 3. Cấu hình cơ sở dữ liệu

Mở file appsettings.json hoặc appsettings.Development.json và chỉnh lại connection string cho phù hợp với máy:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=MovieBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
### 4. Chạy ứng dụng
```bash
dotnet run
```
Hoặc mở project bằng Visual Studio 2022 và nhấn F5 để chạy.

### 5. Truy cập hệ thống

Sau khi ứng dụng khởi động thành công, mở trình duyệt và truy cập địa chỉ localhost được hiển thị trên terminal hoặc cửa sổ Output.

## Ghi chú
Hệ thống sử dụng Entity Framework Core để quản lý cơ sở dữ liệu.
Migration được áp dụng tự động khi ứng dụng khởi động.
Dữ liệu mẫu có thể được seed tự động ở lần chạy đầu tiên.
Nếu không kết nối được cơ sở dữ liệu, cần kiểm tra lại SQL Server instance và connection string.
## Thành viên nhóm
[Tên thành viên 1]
[Tên thành viên 2]
[Tên thành viên 3]
[Tên thành viên 4]
