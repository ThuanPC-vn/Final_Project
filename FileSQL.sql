/**
* File SQL của dự án cuối kỳ môn Phát Triển Ứng Dụng Web
* Nhóm 6 - Lớp CC23TTH - Khoa Công Nghệ Thông Tin
* MSSV: Trần 23TH2308 Nhật Hào - Huỳnh 23TH2520 Lê Minh Thuận
*/


-- Tạo cơ sở dữ liệu mới
CREATE DATABASE QuanLyQuanCafe;
GO

USE QuanLyQuanCafe;
GO

-- Tạo bảng NhanVien với NhanVienID là NVARCHAR
CREATE TABLE NhanVien (
    NhanVienID NVARCHAR(10) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh BIT,
    SoDienThoai NVARCHAR(15),
    DiaChi NVARCHAR(255),
    AnhNV NVARCHAR(50)
);

-- Tạo bảng KhachHang
CREATE TABLE KhachHang (
    KhachHangID INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100),
    SoDienThoai NVARCHAR(15),
    DiaChi NVARCHAR(255)
);

-- Tạo bảng SanPham
CREATE TABLE SanPham (
    SanPhamID INT PRIMARY KEY IDENTITY(1,1),
    TenSanPham NVARCHAR(100) NOT NULL,
    Gia DECIMAL(18, 2) NOT NULL,
    MoTa NVARCHAR(255),
    HinhAnh NVARCHAR(255)
);

-- Tạo bảng HoaDon với HoaDonID là NVARCHAR
CREATE TABLE HoaDon (
    HoaDonID NVARCHAR(10) PRIMARY KEY,
    NgayLap DATE NOT NULL,
    ThoiGianLap TIME NOT NULL DEFAULT '00:00:00',
    NhanVienID NVARCHAR(10),
    KhachHangID INT,
    TongTien DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (NhanVienID) REFERENCES NhanVien(NhanVienID),
    FOREIGN KEY (KhachHangID) REFERENCES KhachHang(KhachHangID)
);

-- Tạo bảng ChiTietHoaDon với ChiTietHoaDonID tự động nhảy số
CREATE TABLE ChiTietHoaDon (
    ChiTietHoaDonID INT IDENTITY(1,1) PRIMARY KEY,
    HoaDonID NVARCHAR(10),
    SanPhamID INT,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL,
    ThanhTien DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (HoaDonID) REFERENCES HoaDon(HoaDonID),
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(SanPhamID)
);

-- Tạo bảng Users
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    NhanVienID NVARCHAR(10),
    FOREIGN KEY (NhanVienID) REFERENCES NhanVien(NhanVienID)
);

-- Tạo bảng Permissions
CREATE TABLE Permissions (
    PermissionID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    CanAddProduct BIT DEFAULT 0,
    CanOrder BIT DEFAULT 0,
    CanCreateInvoice BIT DEFAULT 0,
    CanViewInvoiceDetails BIT DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE QuanTri
(
    Email VARCHAR(50) PRIMARY KEY,
    Admin BIT,
    HoTen NVARCHAR(50),
    Password NVARCHAR(50)
);
GO

-- Thêm dữ liệu vào bảng QuanTri
INSERT INTO QuanTri VALUES('thuan.hlm.cc23tth@ntu.edu.vn',1,N'ThuậnPC','123')
GO

-- Thêm dữ liệu vào bảng NhanVien
INSERT INTO NhanVien (NhanVienID, HoTen, NgaySinh, GioiTinh, SoDienThoai, DiaChi, AnhNV)
VALUES 
('NV0001', N'Nguyen Van A', '1990-01-01', 1, '0901234567', N'123 Đường A, Quận 1, TP.HCM', NULL),
('NV0002', N'Tran Thi B', '1992-02-02', 0, '0902345678', N'456 Đường B, Quận 2, TP.HCM', NULL),
('NV0003', N'Le Van C', '1988-03-03', 1, '0903456789', N'789 Đường C, Quận 3, TP.HCM', NULL);

-- Thêm dữ liệu vào bảng KhachHang
INSERT INTO KhachHang (HoTen, SoDienThoai, DiaChi)
VALUES 
(N'Pham Thi D', '0911234567', N'321 Đường D, Quận 4, TP.HCM'),
(N'Hoang Van E', '0912345678', N'654 Đường E, Quận 5, TP.HCM'),
(N'Vu Thi F', '0913456789', N'987 Đường F, Quận 6, TP.HCM');

-- Thêm dữ liệu vào bảng SanPham
INSERT INTO SanPham (TenSanPham, Gia, MoTa, HinhAnh)
VALUES 
(N'Cafe Đen', 20000, N'Cafe đen nguyên chất', N'cafe_den.jpg'),
(N'Cafe Sữa', 25000, N'Cafe sữa đá', N'cafe_sua.jpg'),
(N'Sinh Tố Bơ', 30000, N'Sinh tố bơ tươi', N'sinh_to_bo.jpg');

-- Thêm dữ liệu vào bảng HoaDon
INSERT INTO HoaDon (HoaDonID, NgayLap, ThoiGianLap, NhanVienID, KhachHangID, TongTien)
VALUES 
('HD0001', '2024-10-01', '12:00:00', 'NV0001', 1, 45000),
('HD0002', '2024-10-02', '13:00:00', 'NV0002', 2, 50000),
('HD0003', '2024-10-03', '14:00:00', 'NV0003', 3, 30000);

-- Thêm dữ liệu vào bảng ChiTietHoaDon với giá trị ThanhTien
INSERT INTO ChiTietHoaDon (HoaDonID, SanPhamID, SoLuong, DonGia, ThanhTien)
VALUES 
('HD0001', 1, 1, 20000, 20000 * 1),
('HD0002', 2, 1, 25000, 25000 * 1),
('HD0003', 3, 2, 50000, 50000 * 2);

-- Thêm dữ liệu vào bảng Users
INSERT INTO Users (Username, Password, Role, NhanVienID)
VALUES 
('user1', 'user1password', 'Employee', 'NV0001'),
('user2', 'user2password', 'Employee', 'NV0002');

-- Thêm dữ liệu vào bảng Permissions
INSERT INTO Permissions (UserID, CanAddProduct, CanOrder, CanCreateInvoice, CanViewInvoiceDetails)
VALUES 
(1, 0, 1, 1, 0),  -- Nhân viên 1 có thể order và tạo hóa đơn
(2, 0, 1, 0, 0);  -- Nhân viên 2 chỉ có thể order




CREATE PROCEDURE HoaDon_TimKiem
    @HoaDonID NVARCHAR(10) = NULL,
    @NgayLapFrom DATE = NULL,
    @NgayLapTo DATE = NULL,
    @ThoiGianLapFrom TIME = NULL,
    @ThoiGianLapTo TIME = NULL,
    @TongTienFrom DECIMAL(18, 2) = NULL,
    @TongTienTo DECIMAL(18, 2) = NULL
AS
BEGIN
    DECLARE @SqlStr NVARCHAR(4000)
    SELECT @SqlStr = '
       SELECT * 
       FROM HoaDon
       WHERE 1 = 1
       '
    IF @HoaDonID IS NOT NULL
        SELECT @SqlStr = @SqlStr + '
              AND (HoaDonID LIKE ''%' + @HoaDonID + '%'')
              '
    IF @NgayLapFrom IS NOT NULL
        SELECT @SqlStr = @SqlStr + '
              AND (NgayLap >= ''' + CONVERT(NVARCHAR, @NgayLapFrom, 23) + ''')
              '
    IF @NgayLapTo IS NOT NULL
        SELECT @SqlStr = @SqlStr + '
              AND (NgayLap <= ''' + CONVERT(NVARCHAR, @NgayLapTo, 23) + ''')
              '
    IF @ThoiGianLapFrom IS NOT NULL
        SELECT @SqlStr = @SqlStr + '
              AND (ThoiGianLap >= ''' + CONVERT(NVARCHAR, @ThoiGianLapFrom, 8) + ''')
              '
    IF @ThoiGianLapTo IS NOT NULL
        SELECT @SqlStr = @SqlStr + '
              AND (ThoiGianLap <= ''' + CONVERT(NVARCHAR, @ThoiGianLapTo, 8) + ''')
              '
    IF @TongTienFrom IS NOT NULL
        SELECT @SqlStr = @SqlStr + '
              AND (TongTien >= ' + CONVERT(NVARCHAR, @TongTienFrom) + ')
              '
    IF @TongTienTo IS NOT NULL
        SELECT @SqlStr = @SqlStr + '
              AND (TongTien <= ' + CONVERT(NVARCHAR, @TongTienTo) + ')
              '
    EXEC SP_EXECUTESQL @SqlStr
END

-- Thêm dữ liệu này vào để tránh lỗi khi thêm dữ liệu bên dưới
INSERT INTO KhachHang (HoTen, SoDienThoai, DiaChi)
VALUES (N'Default', NULL, NULL);
INSERT INTO SanPham (TenSanPham, Gia, MoTa, HinhAnh)
VALUES (N'Bạc xỉu', 45000, N'Bạc xỉu ngon như starbuck', N'bac_xiu.jpg');




-- Thêm dữ liệu vào bảng NhanVien
INSERT INTO NhanVien (NhanVienID, HoTen, NgaySinh, GioiTinh, SoDienThoai, DiaChi, AnhNV)
VALUES 
('NV0004', N'Nguyen Anh Binh', '1990-01-10', 1, '0901234564', N'123 Đường B, Quận 1, TP.HCM', NULL),
('NV0005', N'Le Anh Cuong', '1985-03-15', 1, '0901234565', N'456 Đường C, Quận 2, TP.HCM', NULL),
('NV0006', N'Pham Anh Duy', '1992-06-20', 1, '0901234566', N'789 Đường D, Quận 3, TP.HCM', NULL),
('NV0007', N'Tran Anh Hieu', '1987-09-25', 1, '0901234567', N'123 Đường E, Quận 4, TP.HCM', NULL),
('NV0008', N'Hoang Anh Khoa', '1990-11-30', 1, '0901234568', N'456 Đường F, Quận 5, TP.HCM', NULL),
('NV0009', N'Nguyen Anh Long', '1985-01-01', 1, '0901234569', N'789 Đường G, Quận 6, TP.HCM', NULL),
('NV0010', N'Le Anh Minh', '1992-03-05', 1, '0901234570', N'123 Đường H, Quận 7, TP.HCM', NULL),
('NV0011', N'Pham Anh Ngoc', '1987-05-10', 1, '0901234571', N'456 Đường I, Quận 8, TP.HCM', NULL),
('NV0012', N'Tran Anh Quan', '1990-07-15', 1, '0901234572', N'789 Đường J, Quận 9, TP.HCM', NULL),
('NV0013', N'Hoang Anh Son', '1985-09-20', 1, '0901234573', N'123 Đường K, Quận 10, TP.HCM', NULL),
('NV0014', N'Nguyen Anh Tam', '1992-11-25', 1, '0901234574', N'456 Đường L, Quận 11, TP.HCM', NULL),
('NV0015', N'Le Anh Uyen', '1987-02-01', 1, '0901234575', N'789 Đường M, Quận 12, TP.HCM', NULL),
('NV0016', N'Pham Anh Viet', '1990-04-05', 1, '0901234576', N'123 Đường N, Bình Thạnh, TP.HCM', NULL),
('NV0017', N'Tran Anh Xuan', '1985-06-10', 1, '0901234577', N'456 Đường O, Gò Vấp, TP.HCM', NULL),
('NV0018', N'Hoang Anh Yen', '1992-08-15', 1, '0901234578', N'789 Đường P, Thủ Đức, TP.HCM', NULL),
('NV0019', N'Nguyen Anh Phat', '1987-10-20', 1, '0901234579', N'123 Đường Q, Quận 7, TP.HCM', NULL),
('NV0020', N'Le Anh Tai', '1990-12-25', 1, '0901234580', N'456 Đường R, Quận 6, TP.HCM', NULL);



-- Thêm dữ liệu vào bảng SanPham
INSERT INTO SanPham (TenSanPham, Gia, MoTa, HinhAnh)
VALUES 
(N'Cafe Đen', 20000, N'Cafe đen nguyên chất', N'cafe_den.jpg'),
(N'Cafe Sữa', 25000, N'Cafe sữa đá', N'cafe_sua.jpg'),
(N'Sinh Tố Bơ', 30000, N'Sinh tố bơ tươi', N'sinh_to_bo.jpg'),
(N'Trá Đào', 35000, N'Trá Đào mát lạnh', N'tra_dao.jpg'),
(N'Trá Vải', 35000, N'Trá Vải thơm ngon', N'tra_vai.jpg'),
(N'Trá Xanh', 30000, N'Trá Xanh thanh mát', N'tra_xanh.jpg'),
(N'Nước Cam', 30000, N'Nước Cam nguyên chất', N'nuoc_cam.jpg'),
(N'Sinh Tố Dâu', 35000, N'Sinh Tố Dâu tươi', N'sinh_to_dau.jpg'),
(N'Soda Chanh', 25000, N'Soda Chanh tươi mát', N'soda_chanh.jpg'),
(N'Coca Cola', 20000, N'Coca Cola lon', N'coca_cola.jpg');



-- Thêm dữ liệu vào bảng HoaDon
INSERT INTO HoaDon (HoaDonID, NgayLap, ThoiGianLap, NhanVienID, KhachHangID, TongTien)
VALUES 
('HD0004', '2024-10-04', '15:00:00', 'NV0001', 4, 60000),
('HD0005', '2024-10-05', '16:00:00', 'NV0002', 4, 75000),
('HD0006', '2024-10-06', '17:00:00', 'NV0003', 4, 50000),
('HD0007', '2024-10-07', '18:00:00', 'NV0004', 4, 65000),
('HD0008', '2024-10-08', '19:00:00', 'NV0005', 4, 85000),
('HD0009', '2024-10-09', '20:00:00', 'NV0006', 4, 70000),
('HD0010', '2024-10-10', '21:00:00', 'NV0007', 4, 75000),
('HD0011', '2024-10-11', '10:00:00', 'NV0008', 4, 80000),
('HD0012', '2024-10-12', '11:00:00', 'NV0009', 4, 90000),
('HD0013', '2024-10-13', '12:00:00', 'NV0010', 4, 65000),
('HD0014', '2024-10-14', '13:00:00', 'NV0011', 4, 75000),
('HD0015', '2024-10-15', '14:00:00', 'NV0012', 4, 85000),
('HD0016', '2024-10-16', '15:00:00', 'NV0013', 4, 90000),
('HD0017', '2024-10-17', '16:00:00', 'NV0014', 4, 60000),
('HD0018', '2024-10-18', '17:00:00', 'NV0015', 4, 70000),
('HD0019', '2024-10-19', '18:00:00', 'NV0016', 4, 75000),
('HD0020', '2024-10-20', '19:00:00', 'NV0017', 4, 85000),
('HD0021', '2024-10-21', '20:00:00', 'NV0018', 4, 90000),
('HD0022', '2024-10-22', '21:00:00', 'NV0019', 4, 60000),
('HD0023', '2024-10-23', '10:00:00', 'NV0020', 4, 70000),
('HD0024', '2024-10-24', '11:00:00', 'NV0001', 4, 75000),
('HD0025', '2024-10-25', '12:00:00', 'NV0002', 4, 85000),
('HD0026', '2024-10-26', '13:00:00', 'NV0003', 4, 90000),
('HD0027', '2024-10-27', '14:00:00', 'NV0004', 4, 60000),
('HD0028', '2024-10-28', '11:00:00', 'NV0005', 4, 70000),
('HD0029', '2024-10-29', '12:00:00', 'NV0006', 4, 85000),
('HD0030', '2024-10-30', '13:00:00', 'NV0007', 4, 90000);




-- Thêm dữ liệu vào bảng ChiTietHoaDon với giá trị ThanhTien
INSERT INTO ChiTietHoaDon (HoaDonID, SanPhamID, SoLuong, DonGia, ThanhTien)
VALUES 
('HD0004', 1, 2, 20000, 20000 * 2),
('HD0004', 2, 1, 25000, 25000 * 1),
('HD0005', 3, 2, 30000, 30000 * 2),
('HD0005', 5, 1, 35000, 35000 * 1),
('HD0006', 6, 1, 30000, 30000 * 1),
('HD0006', 7, 1, 25000, 25000 * 1),
('HD0007', 1, 1, 20000, 20000 * 1),
('HD0007', 8, 2, 20000, 20000 * 2),
('HD0008', 2, 1, 25000, 25000 * 1),
('HD0008', 9, 2, 20000, 20000 * 2),
('HD0009', 3, 2, 30000, 30000 * 2),
('HD0009', 10, 1, 20000, 20000 * 1),
('HD0010', 5, 1, 35000, 35000 * 1),
('HD0010', 11, 2, 20000, 20000 * 2),
('HD0011', 6, 1, 30000, 30000 * 1),
('HD0011', 1, 2, 20000, 20000 * 2),
('HD0012', 7, 2, 25000, 25000 * 2),
('HD0012', 2, 1, 25000, 25000 * 1),
('HD0013', 8, 1, 20000, 20000 * 1),
('HD0013', 3, 2, 30000, 30000 * 2),
('HD0014', 9, 1, 20000, 20000 * 1),
('HD0014', 5, 2, 35000, 35000 * 2),
('HD0015', 10, 2, 20000, 20000 * 2),
('HD0015', 6, 1, 30000, 30000 * 1),
('HD0016', 11, 1, 20000, 20000 * 1),
('HD0016', 7, 2, 25000, 25000 * 2),
('HD0017', 1, 1, 20000, 20000 * 1),
('HD0017', 8, 2, 20000, 20000 * 2),
('HD0018', 2, 1, 25000, 25000 * 1),
('HD0018', 9, 1, 20000, 20000 * 1),
('HD0019', 3, 2, 30000, 30000 * 2),
('HD0019', 10, 2, 20000, 20000 * 2),
('HD0020', 5, 1, 35000, 35000 * 1),
('HD0020', 6, 2, 30000, 30000 * 2),
('HD0021', 7, 1, 25000, 25000 * 1),
('HD0021', 1, 2, 20000, 20000 * 2),
('HD0022', 8, 1, 20000, 20000 * 1),
('HD0022', 2, 2, 25000, 25000 * 2),
('HD0023', 9, 1, 20000, 20000 * 1),
('HD0023', 3, 2, 30000, 30000 * 2),
('HD0024', 10, 2, 20000, 20000 * 2),
('HD0024', 5, 1, 35000, 35000 * 1),
('HD0025', 6, 2, 30000, 30000 * 2),
('HD0025', 7, 1, 25000, 25000 * 1),
('HD0026', 1, 1, 20000, 20000 * 1),
('HD0026', 8, 2, 20000, 20000 * 2),
('HD0027', 2, 2, 25000, 25000 * 2),
('HD0027', 9, 1, 20000, 20000 * 1),
('HD0028', 3, 1, 30000, 30000 * 1),
('HD0028', 10, 2, 20000, 20000 * 2),
('HD0029', 5, 1, 35000, 35000 * 1),
('HD0029', 6, 2, 30000, 30000 * 2),
('HD0030', 7, 1, 25000, 25000 * 1),
('HD0030', 1, 2, 20000, 20000 * 2);
