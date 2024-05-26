
# Ứng Dụng Quản Lý Học Thuật

## Tổng Quan
Ứng dụng Quản Lý Học Thuật là một ứng dụng desktop phát triển bằng C# để quản lý các hoạt động học thuật một cách hiệu quả. Ứng dụng cung cấp các chức năng cho quản trị viên và người dùng để xử lý các nhiệm vụ liên quan đến quản lý học thuật.

## Tính Năng
- **Quản Lý Quản Trị**: Quản lý tài khoản người dùng, quyền hạn và vai trò.
- **Quản Lý Người Dùng**: Xử lý hồ sơ sinh viên và giáo viên, thời khóa biểu và hồ sơ học tập.
- **Views và ViewModels**: Tổ chức thành các thành phần khác nhau để tăng cường tính module và dễ bảo trì.
- **Mô Hình Dữ Liệu**: Xử lý dữ liệu bằng các mô hình mạnh mẽ.

## Cấu Trúc Dự Án
- **AdminViewmodels**: View model cho các chức năng liên quan đến quản trị viên.
- **AdminViews**: Giao diện cho quản trị viên.
- **Models**: Mô hình dữ liệu đại diện cho các thực thể cốt lõi của ứng dụng.
- **UCViewmodels**: View model cho các thành phần điều khiển người dùng.
- **UCViews**: Giao diện cho các điều khiển người dùng.
- **Viewmodels**: Các view model chung sử dụng trong toàn bộ ứng dụng.
- **Views**: Các giao diện chung sử dụng trong toàn bộ ứng dụng.

## Cài Đặt
1. Clone repository:
   ```bash
   git clone https://github.com/sammiesamm/AcademyManagementApp.git
   ```
2. Mở solution trong Visual Studio.
3. Build dự án để khôi phục các dependency.
4. Chạy ứng dụng.

## Sử Dụng
- Đăng nhập với tư cách quản trị viên để quản lý người dùng và hồ sơ học tập.
- Đăng nhập với tư cách người dùng để truy cập thông tin học tập cá nhân và thời khóa biểu.


