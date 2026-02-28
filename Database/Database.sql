USE MovieBookingDB;
GO

-- =============================================
-- XÓA BẢNG MOVIES CŨ
-- =============================================
DROP TABLE IF EXISTS Movies;
GO



-- =============================================
-- KIỂM TRA BẢNG ĐÃ TẠO THÀNH CÔNG
-- =============================================
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Movies'
ORDER BY ORDINAL_POSITION;
GO


-- Kiểm tra dữ liệu đã thêm
--SELECT 
   -- Chọn dữ liệu cần xemm
--FROM --Tên bảng;
--GO



-- Xóa tất cả dữ liệu (vẫn giữ cấu trúc bảng)
DELETE FROM Bookings;
GO

-- Reset IDENTITY về 1
DBCC CHECKIDENT ('Movies', RESEED, 0);
GO

-- Kiểm tra đã xóa hết chưa
SELECT COUNT(*) AS TotalRecords FROM Movies;
GO




