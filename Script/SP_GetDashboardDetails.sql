USE [VendorManagementDB]
GO

/****** Object:  StoredProcedure [dbo].[GetDashboardDetails]    Script Date: 8/9/2024 12:47:18 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER   PROCEDURE [dbo].[GetDashboardDetails]  
    @DetailsYear NVARCHAR(10)  
AS  
BEGIN  
    SELECT
    (SELECT COUNT(DISTINCT vs.FK_VendorServiceID) 
     FROM [VendorDetails] AS vs) AS CountOfServices,
    
    (SELECT COUNT(DISTINCT v.VendorName) 
     FROM [Vendors] AS v) AS CountOfVendors,
    
    (SELECT SUM(ISNULL(vd.ServiceSantionAmount, 0)) 
     FROM [VendorDetails] AS vd 
     WHERE vd.DetailsYear = @DetailsYear) AS TotalSanctionAmount,
    
    (SELECT SUM(ISNULL(vp.VendorPaymentAmount, 0)) 
     FROM [Vendors] AS v  
     INNER JOIN [VendorServices] AS vs ON v.VendorID = vs.FK_VendorID  
     INNER JOIN [VendorDetails] AS vd ON vd.FK_VendorServiceID = vs.VendorServiceID   
     LEFT JOIN [VendorPayments] AS vp ON vp.FK_VendorDetailID = vd.VendorDetailID  
     WHERE vd.DetailsYear =@DetailsYear) AS TotalPaidAmount;
END  
GO


